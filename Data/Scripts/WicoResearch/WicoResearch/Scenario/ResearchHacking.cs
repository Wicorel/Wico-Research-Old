using System.Collections.Generic;
using System.Linq;
using Duckroll;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.ModAPI;
using VRageMath;
using Draygo.API;
using System.Text;

namespace WicoResearch
{
	public class ResearchHacking : ModSystemUpdatable
	{
		public const string SeTextColor = "<color=202,228,241>";
        HudAPIv2 TextAPI;
        HudAPIv2.HUDMessage hackBarV2;
        HudAPIv2.HUDMessage hackInterruptedV2;

        //TODO: change hacking range based on per-location basis.
        private const int HackingRangeSquared = 5*5; // 5 meters
		private const int HackingBarTicks = 26;
		private readonly ResearchControl researchControl;
		private readonly NetworkComms networkComms;
		private readonly InterruptingAudioSystem audioSystem = new InterruptingAudioSystem();
		private readonly List<HackingLocation> hackingLocations = new List<HackingLocation>();
		private bool wasHackingLastUpdate;
		private int hackInterruptCooldown = 6;

		internal ResearchHacking(ResearchControl researchControl, HudAPIv2 hudTextApi, NetworkComms networkComms)
		{
            this.researchControl = researchControl;

            TextAPI = hudTextApi;

            this.networkComms = networkComms;

        }
        void textHudCallback()
        {
            StringBuilder sb = new StringBuilder(SeTextColor + "CONNECTION LOST");
            hackInterruptedV2 = new HudAPIv2.HUDMessage(sb, new Vector2D(-0.5, 0.5));

            sb.Clear();
            hackBarV2 = new HudAPIv2.HUDMessage(sb, new Vector2D(-0.5, 0.5));
        }

        class UnlockLocation
        {
            public int techGroup;
            public Vector3D location;
            // int unlockradiusSQ;
        }
        List<UnlockLocation> unlockLocationList = new List<UnlockLocation>();

        public override void GridInitialising(IMyCubeGrid grid)
        {
//            ModLog.Info("Checking grid: " + grid.CustomName);

            // TODO: Limit to certain owners?  
            // TODO: prevent player from pasting or building and then game loading something with unlock codes.

            // find unlock points
            var slimBlocks = new List<IMySlimBlock>();

            // limit to PBs for now..  could do it from ANYTHING, really...
            grid.GetBlocks(slimBlocks, b => b.FatBlock is IMyProgrammableBlock);
            foreach (var slim in slimBlocks)
            {
                var tb = slim.FatBlock as IMyTerminalBlock;

//                ModLog.Info("Checking PB:" + pb.CustomName);

                string data = tb.CustomData;
                if (data.Contains("[WICORESEARCH]"))
                {
                    int techgroup = -1;
                    Vector3D location = tb.GetPosition();
                    //                    ModLog.Info("Found our section");
                    // get an array of the all of lines
                    string[] aLines = data.Split('\n');

                    // walk through all of the lines
                    for (int iLine = 0; iLine < aLines.Count(); iLine++)
                    {
                        if (aLines[iLine].Contains("[WICORESEARCH]"))
                        {
  //                          ModLog.Info("Found our section  2");
                            int foundCount = 0;
                            // we found our section
                            int iSectionLine = iLine + 1;
                            for (; iSectionLine < aLines.Count(); iSectionLine++)
                            {
                                if (aLines[iSectionLine].StartsWith("["))
                                {
                                    iSectionLine--;
                                    break;
                                }
                                string sLine = aLines[iSectionLine].Trim();
                                string[] aKeyValue = sLine.Split('=');
                                if (aKeyValue.Count() > 1)
                                {
                                    // key is 0 value is 1
                                    if (aKeyValue[0] == "Group")
                                    {
//                                        ModLog.Info("Found Group:"+aLines[iSectionLine]);
                                        int iTest;
                                        bool bOK = int.TryParse(aKeyValue[1], out iTest);
                                        if (bOK)
                                        {
                                            techgroup = iTest;
                                            foundCount++;
                                        }
                                        else
                                        {
                                            ModLog.Info("group parse fail");
                                        }
                                    }
                                    if (aKeyValue[0] == "Location")
                                    {
                                        Vector3D testLocation;
                                        bool bOK = Vector3D.TryParse(aKeyValue[1], out testLocation);
                                        if (bOK)
                                            location = testLocation;
                                        foundCount++;
                                    }

                                }
                            }
                            iLine = iSectionLine;
//                            ModLog.Info("Foundcount=" + foundCount.ToString());
                            if (foundCount > 0)
                            {
                                UnlockLocation ul=new UnlockLocation();
                                ul.location = location;
                                ul.techGroup = techgroup;
//                                ModLog.Info("Adding research unlock from: " + pb.CustomName);
                                unlockLocationList.Add(ul);
                            }
                        }
                    }
                }
            }
        }

        public override void AllGridsInitialised()
        {
            // add unlock points
            ModLog.Info("Found " + unlockLocationList.Count().ToString() + " Unlock locations");
            foreach( var unlockLocation in unlockLocationList)
            {
                AddHackingLocation((TechGroup)unlockLocation.techGroup, unlockLocation.location);

                if (researchControl.bDebugLocations)
                {
                    string name = "PBUnlock " + unlockLocation.techGroup.ToString();
                    DuckUtils.AddGpsToAllPlayers(name, "PB Unlock location", unlockLocation.location);
                }
            }
            // all grids have been loaded during game load.
            researchControl.AllowUnlockedTechs();


            // 
            if (researchControl.bDebugLocations)
            {
                foreach (var unlockLocation in hackingLocations)
                {
                    string name = "Unlock " + unlockLocation.TechGroup.ToString();
                    DuckUtils.AddGpsToAllPlayers(name, "Unlock location", unlockLocation.Coords);
                }
            }

        }

        internal void InitHackingLocations()
		{
            //GPS:toddlerself #1:53569.19:-26676.81:11932.84:
            //GPS:toddlerself #2:53558.6:-26668.48:11986.06:
            AddHackingLocation(TechGroup.LgLightArmor, new Vector3D(53569.19, -26676.81, 11932.84));
            AddHackingLocation(TechGroup.AtmosphericEngines, new Vector3D(53558.6, -26668.48, 11986.06));

            /*
			AddHackingLocation(TechGroup.AtmosphericEngines, new Vector3D(1854774.5,-2005846.88,1325410.5));
			AddHackingLocation(TechGroup.GasStorage, new Vector3D(1869167.75,-2004920.12,1316376.38));
			AddHackingLocation(TechGroup.Rockets, new Vector3D(1843300.12,-1996436.5,1324474.12));
			AddHackingLocation(TechGroup.OxygenFarm, new Vector3D(1851936.75,-2001115.25,1324439.75));
            AddHackingLocation(TechGroup.OxygenGenerators, new Vector3D(1869136.62, -2004926.38, 1316339.62));

            AddHackingLocation(TechGroup.BasicWeapons, new Vector3D(1868130.53, -2003476.19, 1316621.55));
            */
        }

        private void AddHackingLocation(TechGroup techGroup, Vector3D coords)
		{
			if (!researchControl.UnlockedTechs.Contains(techGroup))
			{
				hackingLocations.Add(new HackingLocation(techGroup, coords));
			}
		}

        public new void Close()
        {
            if (TextAPI != null)
                TextAPI.Close();
        }

        public override void Update30()
		{
            if (hackingLocations.Count == 0)
			{
				return;
			}

			var players = new List<IMyPlayer>();
			MyAPIGateway.Players.GetPlayers(players);

			foreach (var player in players)
			{
				var controlled = player.Controller.ControlledEntity;
				if (controlled == null) continue;
				var position = controlled.Entity.GetPosition();

				foreach (var hack in hackingLocations.Reverse<HackingLocation>())
				{
					var distSq = Vector3D.DistanceSquared(hack.Coords, position);

					if (distSq <= HackingRangeSquared)
					{
                        if (!wasHackingLastUpdate)
                        {
                            hackInterruptCooldown = 6;// reset
                            InitHudMesages(true);
                        }
						wasHackingLastUpdate = true;
						hack.CompletionTicks++;
						ShowLocalHackingProgress(hack.CompletionTicks);
						networkComms.ShowHackingProgressOnAllClients(hack.CompletionTicks);

						if (hack.CompletionTicks >= HackingBarTicks)
						{
							ShowLocalHackingSuccess();
							networkComms.ShowHackingSuccessOnAllClients();
							researchControl.UnlockTechGroupForAllPlayers(hack.TechGroup);
							hackingLocations.Remove(hack);
							wasHackingLastUpdate = false;
						}
						return; // Only one hack allowed at a time by one player
					}
				}
			}

			if (wasHackingLastUpdate)
			{
				if (hackInterruptCooldown == 0)
				{
					ShowLocalHackingInterruptStopped();
                    if (hackInterruptedV2 != null) hackInterruptedV2.Visible = false;
					wasHackingLastUpdate = false;
					hackInterruptCooldown = 6;
					networkComms.ShowHackingInterruptStoppedOnAllClients();
				}
				else
				{
					ShowLocalHackingInterrupted();
					hackInterruptCooldown--;
					networkComms.ShowHackingInterruptedOnAllClients();
				}
			}

            
		}

        StringBuilder sbInterruptedMessage;
        StringBuilder sbHackBarMessage;

        internal void InitHudMesages(bool bForce = false)
        {
            if (TextAPI == null)
            {
                ModLog.Error("Text HUD API not loaded");
                return;
            }
            if (TextAPI.Heartbeat)
            {
//                ModLog.Info("Have Heartbeat");

                if (hackInterruptedV2 == null || bForce )
                {
//                    ModLog.Info("Creating Interrupted HUD");
                    sbInterruptedMessage = new StringBuilder(SeTextColor + "CONNECTION LOST");
                    hackInterruptedV2 = new HudAPIv2.HUDMessage(sbInterruptedMessage, new Vector2D(-0.5, 0.5));
                    if (hackInterruptedV2 != null)
                    {
                        hackInterruptedV2.Message = sbInterruptedMessage;
                        hackInterruptedV2.Scale = 2;
                        hackInterruptedV2.Options = HudAPIv2.Options.Shadowing;
                        hackInterruptedV2.Options |= HudAPIv2.Options.HideHud;
                        //                        hackInterruptedV2.TimeToLive = 45;
                        hackInterruptedV2.Visible = false;

                    }
                    else ModLog.Info("Could not create Interrupted HUD");
                }
                if (hackBarV2 == null || bForce)
                {
//                    ModLog.Info("Creating Hacking HUD");
                    if(sbHackBarMessage==null) sbHackBarMessage = new StringBuilder("Initial Hack Bar");
                    hackBarV2 = new HudAPIv2.HUDMessage(sbHackBarMessage, new Vector2D(-0.5, 0.5));
                    if (hackBarV2 != null)
                    {
                        hackBarV2.Message = sbHackBarMessage;
                        hackBarV2.Scale = 2;
                        hackBarV2.Options = HudAPIv2.Options.Shadowing;
                        hackBarV2.Options |= HudAPIv2.Options.HideHud;
                        hackBarV2.Visible = false;
//                        hackBarV2.TimeToLive = 45;
                    }
                    else ModLog.Info("Could not create Hacking HUD");
                }
            }
            else ModLog.Info("NO TextHud HEARTBEAT");
        }

        internal void ShowLocalHackingProgress(int ticks)
		{

           audioSystem.EnsurePlaying(AudioClip.HackingSound);
			var hackbarStr = SeTextColor + "Hack in progress: ";
			var percent = ticks * 100 / HackingBarTicks;
			hackbarStr += percent + "%\n\n";
			for (var i = 0; i < ticks; i++)
			{
				hackbarStr += "|";
			}
            if (sbHackBarMessage == null)
            {
                sbHackBarMessage = new StringBuilder(hackbarStr);
            }
            else
            {
                sbHackBarMessage.Clear();
                sbHackBarMessage.Append(hackbarStr);
            }

            InitHudMesages();
            if(hackInterruptedV2 != null) hackInterruptedV2.Visible = false;
            if (hackBarV2 != null) hackBarV2.Visible = true;

        }

        internal void ShowLocalHackingSuccess()
		{
            if(hackBarV2!=null) hackBarV2.Visible = false;
            if (hackInterruptedV2 != null) hackInterruptedV2.Visible = false;
			audioSystem.EnsurePlaying(AudioClip.HackFinished);
		}

		internal void ShowLocalHackingInterruptStopped()
		{
			audioSystem.Stop();
            if (hackBarV2 != null) hackBarV2.Visible = false;
            if (hackInterruptedV2 != null) hackInterruptedV2.Visible = false;
        }

        internal void ShowLocalHackingInterrupted()
		{
            InitHudMesages();
			audioSystem.EnsurePlaying(AudioClip.ConnectionLostSound);

            if (hackBarV2 != null) hackBarV2.Visible = false;
            if (hackInterruptedV2 != null) hackInterruptedV2.Visible = true;
        }

        //		private void SendToHud(HUDTextAPI.HUDMessage hudMessage)
        private void SendToHud(HudAPIv2.HUDMessage hudMessage)
        {
            if (TextAPI.Heartbeat)
// V1			if (hudTextApi.Heartbeat)
			{

//				hudTextApi.Send(hudMessage);
			}
			else
			{
				MyAPIGateway.Utilities.ShowNotification("Error: You need to install the Text HUD API Mod!", 300, MyFontEnum.Red);
			}
		}

		internal List<HackingSaveData> GetSaveData()
		{
			var saveData = new List<HackingSaveData>();
			foreach (var hackingLocation in hackingLocations)
			{
				if (hackingLocation.CompletionTicks > 0)
				{
					saveData.Add(new HackingSaveData {
						Completion = hackingLocation.CompletionTicks, TechGroup 
						= hackingLocation.TechGroup
					});
				}
			}

			return saveData;
		}

		internal void RestoreSaveData(List<HackingSaveData> saveData)
		{
			if (saveData == null)
			{
				return;
			}
			foreach (var hackingSaveData in saveData)
			{
				foreach (var hackingLocation in hackingLocations)
				{
					if (hackingLocation.TechGroup == hackingSaveData.TechGroup)
					{
						hackingLocation.CompletionTicks = hackingSaveData.Completion;
					}
				}
			}
		}

		internal class HackingLocation
		{
			internal readonly TechGroup TechGroup;
			internal readonly Vector3D Coords;
			internal int CompletionTicks;

			public HackingLocation(TechGroup techGroup, Vector3D coords)
			{
				TechGroup = techGroup;
				Coords = coords;
			}
		}

		public class HackingSaveData
		{
			public int Completion { get; set; }
			public TechGroup TechGroup { get; set; }
		}
	}

}