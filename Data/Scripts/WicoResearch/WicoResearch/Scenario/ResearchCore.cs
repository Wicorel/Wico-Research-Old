using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using Duckroll;
using Sandbox.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRage.Game.ModAPI;
using Draygo.API;
using System;

namespace WicoResearch
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class ResearchCore : AbstractCore<SaveData>
	{
        private readonly NetworkComms networkComms = new NetworkComms();
        // https://steamcommunity.com/sharedfiles/filedetails/?id=1803307275

        // Current mod version, increased each time before workshop publish
        private const int CurrentModVersion = 1;

		private ResearchControl researchControl;
        private ResearchHacking researchHacking;

        private HudAPIv2 TextAPI;
        private readonly QueuedAudioSystem audioSystem = new QueuedAudioSystem();

        private int modBuildWhenGameStarted;
        private int modBuildWhenLastSaved;

        MyDefinitionId oxygenDefId = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Oxygen");
        MyDefinitionId hydrogenDefId = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Hydrogen");

        protected override void InitCommon(IModSystemRegistry modSystemRegistry)
		{
            string sInit = "Initialising Wico Research. Version: " + CurrentModVersion;

            MyAPIGateway.Utilities.ShowNotification(sInit, 5000, MyFontEnum.DarkBlue);
            ModLog.Info(sInit);

            if (MyAPIGateway.Session.IsServer)
                MyVisualScriptLogicProvider.SendChatMessage(sInit, "Wicorel", 0, MyFontEnum.DarkBlue);

            bool bResearch = Session.SessionSettings.EnableResearch;

            // This works to change the setting.
            Session.SessionSettings.EnableResearch = true;

            if (!bResearch)
            {
                ModLog.Info("Research was not turned on");
            }

            TextAPI = new HudAPIv2();
            researchControl = new ResearchControl(audioSystem);
			researchControl.InitResearchRestrictions();
            researchHacking = new ResearchHacking(researchControl, TextAPI, networkComms);
            networkComms.Init(audioSystem, researchControl, researchHacking);
			modSystemRegistry.AddCloseableModSystem(networkComms);
			modSystemRegistry.AddUpatableModSystem(audioSystem);

            MyAPIGateway.Utilities.MessageEntered += MessageEntered;

        }

        public override void Close()
        {
            ModLog.Info("Close Called");
            base.Close();
            MyAPIGateway.Utilities.MessageEntered -= MessageEntered;
           TextAPI.Close();
        }


        private void MessageEntered(string msg, ref bool visible)
        {
        }
        protected override void InitHostPreLoading()
		{
			if (MyAPIGateway.Session == null)
				return;
		}
		
        // after loading of saved data
		protected override void InitHostPostLoading(IModSystemRegistry modSystemRegistry)
		{
            ModLog.Info("Original world was loaded by Version:" + modBuildWhenGameStarted.ToString());
            ModLog.Info("Loaded world was saved by Version:" + modBuildWhenLastSaved.ToString());
            researchHacking.InitHackingLocations(); // Uses research restrictions and save data
			audioSystem.AudioRelay = networkComms;
//			networkComms.StartWipeHostToolbar();
			modSystemRegistry.AddUpatableModSystem(researchHacking);
			
		}
		

		protected override void InitClient(IModSystemRegistry modSystemRegistry)
		{
			var player = MyAPIGateway.Session.Player;
			if (player != null)
			{
				networkComms.NotifyServerClientJoined(player);
			}
		}

		private const string SaveFileName = "WicoResearch-SaveData.xml";

		public override string GetSaveDataFileName()
		{
			return SaveFileName;
		}

		public override SaveData GetSaveData()
		{
			if (modBuildWhenGameStarted == 0)
			{
				modBuildWhenGameStarted = CurrentModVersion;
			}

            if (researchControl == null)
                ModLog.Info("RC NULL!");
            if (networkComms == null)
                ModLog.Info("NC NULL!");
            if (researchHacking == null)
                ModLog.Info("RH NULL");

            var saveData = new SaveData
            {
                UnlockedTechs = researchControl.UnlockedTechs
                ,RegisteredPlayers = networkComms.RegisteredPlayers
                ,HackingData = researchHacking.GetSaveData()
                ,BuildWhenGameStarted = modBuildWhenGameStarted
                ,BuildWhenSaved = CurrentModVersion
			};
			return saveData;
		}

		public override void LoadPreviousGame(SaveData saveData)
		{
			networkComms.RegisteredPlayers = saveData.RegisteredPlayers;
			researchControl.UnlockedTechs = saveData.UnlockedTechs;
			researchHacking.RestoreSaveData(saveData.HackingData);
			modBuildWhenGameStarted = saveData.BuildWhenGameStarted;
            modBuildWhenLastSaved = saveData.BuildWhenSaved;
		}

		public override void StartedNewGame()
		{
			modBuildWhenGameStarted = CurrentModVersion;
		}
	}

	public class SaveData
	{
		public HashSet<TechGroup> UnlockedTechs { get; set; }
		public HashSet<long> RegisteredPlayers { get; set; }
		public List<ResearchHacking.HackingSaveData> HackingData { get; set; }
		public int BuildWhenGameStarted { get; set; }
        public int BuildWhenSaved { get; set; }

    }
}