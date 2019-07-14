using System.Collections.Generic;
using Duckroll;
using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.ModAPI;

namespace WicoResearch
{
    internal class ResearchControl
    {
        readonly bool bNewResearch = true;
        public readonly bool bDebugLocations = false;

        private readonly MyDefinitionId refinery = MyVisualScriptLogicProvider.GetDefinitionId(
            "Refinery", "LargeRefinery");

        private readonly MyDefinitionId blastFurnace = MyVisualScriptLogicProvider.GetDefinitionId(
            "Refinery", "Blast Furnace");

        private readonly MyDefinitionId jumpDrive = MyVisualScriptLogicProvider.GetDefinitionId(
            "JumpDrive", "LargeJumpDrive");

        private readonly MyDefinitionId radioAntennaLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "RadioAntenna", "LargeBlockRadioAntenna");

        private readonly MyDefinitionId radioAntennaSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "RadioAntenna", "SmallBlockRadioAntenna");

        private readonly MyDefinitionId largeMissileTurret = MyVisualScriptLogicProvider.GetDefinitionId(
            "LargeMissileTurret", null);

        private readonly MyDefinitionId smallMissileTurret = MyVisualScriptLogicProvider.GetDefinitionId(
            "LargeMissileTurret", "SmallMissileTurret");

        private readonly MyDefinitionId rocketLauncher = MyVisualScriptLogicProvider.GetDefinitionId(
            "SmallMissileLauncher", null);

        private readonly MyDefinitionId largeRocketLauncher = MyVisualScriptLogicProvider.GetDefinitionId(
            "SmallMissileLauncher", "LargeMissileLauncher");

        private readonly MyDefinitionId smallReloadableRocketLauncher =
            MyVisualScriptLogicProvider.GetDefinitionId("SmallMissileLauncherReload", "SmallRocketLauncherReload");


        private readonly MyDefinitionId LargeGatlingTurret = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_LargeGatlingTurret", null);

        private readonly MyDefinitionId InteriorTurret = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_InteriorTurret", "LargeInteriorTurret");

        private readonly MyDefinitionId SmallRocketLauncherReload = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_SmallMissileLauncherReload", "SmallRocketLauncherReload");

        private readonly MyDefinitionId SmallGatlingTurret = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_LargeGatlingTurret", "SmallGatlingTurret");


        private readonly MyDefinitionId ionThrusterSmallShipSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "SmallBlockSmallThrust");

        private readonly MyDefinitionId ionThrusterSmallShipLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "SmallBlockLargeThrust");

        private readonly MyDefinitionId ionThrusterLargeShipSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "LargeBlockSmallThrust");

        private readonly MyDefinitionId ionThrusterLargeShipLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "LargeBlockLargeThrust");

        private readonly MyDefinitionId hydroThrusterSmallShipSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "SmallBlockSmallHydrogenThrust");

        private readonly MyDefinitionId hydroThrusterSmallShipLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "SmallBlockLargeHydrogenThrust");

        private readonly MyDefinitionId hydroThrusterLargeShipSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "LargeBlockSmallHydrogenThrust");

        private readonly MyDefinitionId hydroThrusterLargeShipLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "LargeBlockLargeHydrogenThrust");

        private readonly MyDefinitionId atmoThrusterSmallShipSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "SmallBlockSmallAtmosphericThrust");

        private readonly MyDefinitionId atmoThrusterSmallShipLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "SmallBlockLargeAtmosphericThrust");

        private readonly MyDefinitionId atmoThrusterLargeShipSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "Thrust", "LargeBlockSmallAtmosphericThrust");

        private readonly MyDefinitionId atmoThrusterLargeShipLarge = MyVisualScriptLogicProvider.GetDefinitionId("Thrust",
            "LargeBlockLargeAtmosphericThrust");

        private readonly MyDefinitionId oxygenFarm = MyVisualScriptLogicProvider.GetDefinitionId(
            "OxygenFarm", "LargeBlockOxygenFarm");

        private readonly MyDefinitionId oxygenGeneratorLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_OxygenGenerator", null);

        private readonly MyDefinitionId oxygenGeneratorSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_OxygenGenerator", "OxygenGeneratorSmall");

        private readonly MyDefinitionId oxygenTankLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "OxygenTank", null);

        private readonly MyDefinitionId oxygenTankSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "OxygenTank", "OxygenTankSmall");

        private readonly MyDefinitionId hydrogenTankLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "OxygenTank", "LargeHydrogenTank");

        private readonly MyDefinitionId hydrogenTankSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "OxygenTank", "SmallHydrogenTank");

        private readonly MyDefinitionId projectorLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_Projector", "LargeProjector");

        private readonly MyDefinitionId projectorSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_Projector", "SmallProjector");

        private readonly MyDefinitionId EngineLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_HydrogenEngine", "LargeHydrogenEngine");

        private readonly MyDefinitionId WindTurbineLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_WindTurbine", "LargeBlockWindTurbine");

        private readonly MyDefinitionId SkLarge = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_SurvivalKit", "SurvivalKitLarge");

        private readonly MyDefinitionId BasicAssembler = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_Assembler", "BasicAssembler");

        private readonly MyDefinitionId SmallBattery = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_BatteryBlock", "SmallBlockSmallBatteryBlock");

        private readonly MyDefinitionId EngineSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_HydrogenEngine", "SmallHydrogenEngine");

        private readonly MyDefinitionId SkSmall = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_SurvivalKit", "SurvivalKit");


        // Armor
        private readonly MyDefinitionId LargeBlockArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorBlock");

// variants
//          <SubtypeId>LargeBlockArmorSlope</SubtypeId>
//          <SubtypeId>LargeBlockArmorCorner</SubtypeId>
//          <SubtypeId>LargeBlockArmorCornerInv</SubtypeId>
//          <SubtypeId>LargeHalfArmorBlock</SubtypeId>
//          <SubtypeId>LargeHalfSlopeArmorBlock</SubtypeId>

        private readonly MyDefinitionId LargeBlockArmorSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorSlope");

        private readonly MyDefinitionId LargeBlockArmorCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorCorner");

        private readonly MyDefinitionId LargeBlockArmorCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorCornerInv");

        private readonly MyDefinitionId LargeRoundArmor_Slope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeRoundArmor_Slope");

        private readonly MyDefinitionId LargeRoundArmor_Corner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeRoundArmor_Corner");

        private readonly MyDefinitionId LargeRoundArmor_CornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeRoundArmor_CornerInv");

        private readonly MyDefinitionId LargeHalfArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHalfArmorBlock");

        private readonly MyDefinitionId LargeHalfSlopeArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHalfSlopeArmorBlock");



        private readonly MyDefinitionId LargeHeavyBlockArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorBlock");
        // variants
        //          <SubtypeId>LargeHeavyBlockArmorSlope</SubtypeId>
        //          <SubtypeId>LargeHeavyBlockArmorCorner</SubtypeId>
        //          <SubtypeId>LargeHeavyBlockArmorCornerInv</SubtypeId>
        //          <SubtypeId>LargeHeavyHalfArmorBlock</SubtypeId>
        //          <SubtypeId>LargeHeavyHalfSlopeArmorBlock</SubtypeId>


        private readonly MyDefinitionId LargeHeavyBlockArmorSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorSlope");

        private readonly MyDefinitionId LargeHeavyBlockArmorCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorCorner");

        private readonly MyDefinitionId LargeHeavyBlockArmorCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorCornerInv");

        private readonly MyDefinitionId LargeHeavyHalfArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyHalfArmorBlock");

        private readonly MyDefinitionId LargeHeavyHalfSlopeArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyHalfSlopeArmorBlock");


        // SMALL GRID BLOCKS

        private readonly MyDefinitionId SmallBlockArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorBlock");
        // variants
        //          <SubtypeId>SmallBlockArmorSlope</SubtypeId>
        //          <SubtypeId>SmallBlockArmorCorner</SubtypeId>
        //          <SubtypeId>SmallBlockArmorCornerInv</SubtypeId>
        //          <SubtypeId>HalfArmorBlock</SubtypeId>
        //          <SubtypeId>HalfSlopeArmorBlock</SubtypeId>

        private readonly MyDefinitionId SmallBlockArmorSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorSlope");

        private readonly MyDefinitionId SmallBlockArmorCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorCorner");

        private readonly MyDefinitionId SmallBlockArmorCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorCorner");

        private readonly MyDefinitionId SmallHeavyBlockArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallHeavyBlockArmorBlock");
        // block variants
//          <SubtypeId>SmallHeavyBlockArmorSlope</SubtypeId>
//          <SubtypeId>SmallHeavyBlockArmorCorner</SubtypeId>
//          <SubtypeId>SmallHeavyBlockArmorCornerInv</SubtypeId>
//          <SubtypeId>HeavyHalfArmorBlock</SubtypeId>
//          <SubtypeId>HeavyHalfSlopeArmorBlock</SubtypeId>


        private readonly MyDefinitionId SmallHeavyBlockArmorSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallHeavyBlockArmorSlope");

        private readonly MyDefinitionId SmallHeavyBlockArmorCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallHeavyBlockArmorCorner");

        private readonly MyDefinitionId SmallHeavyBlockArmorCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallHeavyBlockArmorCornerInv");




        // SMALL GRID HALF ARMOR
        private readonly MyDefinitionId HalfArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "HalfArmorBlock");

        private readonly MyDefinitionId HeavyHalfArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "HeavyHalfArmorBlock");

        private readonly MyDefinitionId HalfSlopeArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "HalfSlopeArmorBlock");

        private readonly MyDefinitionId HeavyHalfSlopeArmorBlock = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "HeavyHalfSlopeArmorBlock");


        // BEGIN ROUND ARMOR
        private readonly MyDefinitionId LargeBlockArmorRoundSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorRoundSlope");
        //block variants
//        <SubtypeId>LargeBlockArmorRoundCorner</SubtypeId>
//        <SubtypeId>LargeBlockArmorRoundCornerInv</SubtypeId>

        private readonly MyDefinitionId LargeBlockArmorRoundCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorRoundCorner");

        private readonly MyDefinitionId LargeBlockArmorRoundCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorRoundCornerInv");

        // large heavy
        private readonly MyDefinitionId LargeHeavyBlockArmorRoundSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorRoundSlope");
        // block variants
        //          <SubtypeId>LargeHeavyBlockArmorRoundCorner</SubtypeId>
        //          <SubtypeId>LargeHeavyBlockArmorRoundCornerInv</SubtypeId>


        private readonly MyDefinitionId LargeHeavyBlockArmorRoundCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorRoundCorner");

        private readonly MyDefinitionId LargeHeavyBlockArmorRoundCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorRoundCornerInv");

        // small light

        private readonly MyDefinitionId SmallBlockArmorRoundSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorRoundSlope");
        // block variants
        //          <SubtypeId>SmallBlockArmorRoundCorner</SubtypeId>
        //          <SubtypeId>SmallBlockArmorRoundCornerInv</SubtypeId>


        private readonly MyDefinitionId SmallBlockArmorRoundCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeHeavyBlockArmorRoundCorner");

        private readonly MyDefinitionId SmallBlockArmorRoundCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorRoundCornerInv");

        // small heavy
        private readonly MyDefinitionId SmallHeavyBlockArmorRoundSlope = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallHeavyBlockArmorRoundSlope");
        // block variants
//        <SubtypeId>SmallHeavyBlockArmorRoundCorner</SubtypeId>
//          <SubtypeId>SmallHeavyBlockArmorRoundCornerInv</SubtypeId>

        private readonly MyDefinitionId SmallHeavyBlockArmorRoundCorner = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallHeavyBlockArmorRoundCorner");

        private readonly MyDefinitionId SmallHeavyBlockArmorRoundCornerInv = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "SmallBlockArmorRoundCornerInv");


        // non-deforming sloped armor
        private readonly MyDefinitionId LargeBlockArmorSlope2Base = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorSlope2Base");
        //          <SubtypeId>LargeBlockArmorSlope2Tip</SubtypeId>


        private readonly MyDefinitionId LargeBlockArmorSlope2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorSlope2Tip");

        // 2x1x1 corner
        private readonly MyDefinitionId LargeBlockArmorCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorCorner2Base");
        //           <SubtypeId>LargeBlockArmorCorner2Tip</SubtypeId>


        private readonly MyDefinitionId LargeBlockArmorCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
           "CubeBlock", "LargeBlockArmorCorner2Tip");

        private readonly MyDefinitionId LargeBlockArmorInvCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeBlockArmorCorner2Base");
        //           <SubtypeId>LargeBlockArmorInvCorner2Tip</SubtypeId>


        private readonly MyDefinitionId LargeBlockArmorInvCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeBlockArmorInvCorner2Tip");


        private readonly MyDefinitionId LargeHeavyBlockArmorSlope2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeHeavyBlockArmorSlope2Base");
        //           <SubtypeId>LargeHeavyBlockArmorSlope2Tip</SubtypeId>


        private readonly MyDefinitionId LargeHeavyBlockArmorSlope2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeHeavyBlockArmorSlope2Tip");

        private readonly MyDefinitionId LargeHeavyBlockArmorCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeHeavyBlockArmorCorner2Base");
        //           <SubtypeId>LargeHeavyBlockArmorCorner2Tip</SubtypeId>


        private readonly MyDefinitionId LargeHeavyBlockArmorCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeHeavyBlockArmorCorner2Tip");

        private readonly MyDefinitionId LargeHeavyBlockArmorInvCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeHeavyBlockArmorInvCorner2Base");
        //          <SubtypeId>LargeHeavyBlockArmorInvCorner2Tip</SubtypeId>

        private readonly MyDefinitionId LargeHeavyBlockArmorInvCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "LargeHeavyBlockArmorInvCorner2Tip");


        //

        private readonly MyDefinitionId SmallBlockArmorSlope2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallBlockArmorSlope2Base");

        private readonly MyDefinitionId SmallBlockArmorSlope2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallBlockArmorSlope2Tip");

        private readonly MyDefinitionId SmallBlockArmorCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallBlockArmorCorner2Base");

        private readonly MyDefinitionId SmallBlockArmorCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallBlockArmorCorner2Tip");

        private readonly MyDefinitionId SmallBlockArmorInvCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallBlockArmorInvCorner2Base");

        private readonly MyDefinitionId SmallBlockArmorInvCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallBlockArmorInvCorner2Tip");

        private readonly MyDefinitionId SmallHeavyBlockArmorSlope2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallHeavyBlockArmorSlope2Base");

        private readonly MyDefinitionId SmallHeavyBlockArmorSlope2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallHeavyBlockArmorSlope2Tip");

        private readonly MyDefinitionId SmallHeavyBlockArmorCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallHeavyBlockArmorCorner2Base");

        private readonly MyDefinitionId SmallHeavyBlockArmorCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallHeavyBlockArmorCorner2Tip");

        private readonly MyDefinitionId SmallHeavyBlockArmorInvCorner2Base = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallHeavyBlockArmorInvCorner2Base");

        private readonly MyDefinitionId SmallHeavyBlockArmorInvCorner2Tip = MyVisualScriptLogicProvider.GetDefinitionId(
            "CubeBlock", "SmallHeavyBlockArmorInvCorner2Tip");

        /// //////////////////////////////////

        // Economy.  V1.192
        private readonly MyDefinitionId SafeZoneBlock = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_SafeZoneBlock", "SafeZoneBlock");

        private readonly MyDefinitionId StoreBlock = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_StoreBlock", "StoreBlock");

        private readonly MyDefinitionId ContractBlock = MyVisualScriptLogicProvider.GetDefinitionId(
            "MyObjectBuilder_ContractBlock", "ContractBlock");

        private readonly Dictionary<TechGroup, HashSet<MyDefinitionId>> techsForGroup =
        new Dictionary<TechGroup, HashSet<MyDefinitionId>>();

        private readonly QueuedAudioSystem audioSystem;

        internal ResearchControl(QueuedAudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        internal HashSet<TechGroup> UnlockedTechs { get; set; } = new HashSet<TechGroup>();

        internal void InitResearchRestrictions()
        {
            if (bNewResearch)
            {
                MyVisualScriptLogicProvider.ResearchListClear();
            }

            NeedsResearch(largeMissileTurret, TechGroup.Rockets);
            NeedsResearch(smallMissileTurret, TechGroup.Rockets);
            NeedsResearch(rocketLauncher, TechGroup.Rockets);
            NeedsResearch(largeRocketLauncher, TechGroup.Rockets);
            NeedsResearch(smallReloadableRocketLauncher, TechGroup.Rockets);


            NeedsResearch(atmoThrusterSmallShipSmall, TechGroup.AtmosphericEngines);
            NeedsResearch(atmoThrusterSmallShipLarge, TechGroup.AtmosphericEngines);
            NeedsResearch(atmoThrusterLargeShipSmall, TechGroup.AtmosphericEngines);
            NeedsResearch(atmoThrusterLargeShipLarge, TechGroup.AtmosphericEngines);
            NeedsResearch(WindTurbineLarge, TechGroup.AtmosphericEngines);

            NeedsResearch(oxygenFarm, TechGroup.OxygenFarm);

            NeedsResearch(oxygenGeneratorLarge, TechGroup.OxygenGenerators);
            NeedsResearch(oxygenGeneratorSmall, TechGroup.OxygenGenerators);

            NeedsResearch(oxygenTankLarge, TechGroup.GasStorage);
            NeedsResearch(oxygenTankSmall, TechGroup.GasStorage);
            NeedsResearch(hydrogenTankLarge, TechGroup.GasStorage);
            NeedsResearch(hydrogenTankSmall, TechGroup.GasStorage);
            NeedsResearch(EngineLarge, TechGroup.GasStorage);
            NeedsResearch(EngineSmall, TechGroup.GasStorage);


            NeedsResearch(SmallGatlingTurret, TechGroup.BasicWeapons);
            NeedsResearch(SmallRocketLauncherReload, TechGroup.BasicWeapons);
            NeedsResearch(InteriorTurret, TechGroup.BasicWeapons);
            NeedsResearch(LargeGatlingTurret, TechGroup.BasicWeapons);

            NeedsResearch(LargeBlockArmorBlock, TechGroup.LgLightArmor);
            NeedsResearch(LargeBlockArmorCorner, TechGroup.LgLightArmor);
            NeedsResearch(LargeBlockArmorCornerInv, TechGroup.LgLightArmor);
            NeedsResearch(LargeRoundArmor_Slope, TechGroup.LgLightArmor);
            NeedsResearch(LargeRoundArmor_Corner, TechGroup.LgLightArmor);
            NeedsResearch(LargeRoundArmor_CornerInv, TechGroup.LgLightArmor);
            NeedsResearch(LargeHalfArmorBlock, TechGroup.LgLightArmor);
            NeedsResearch(LargeHalfSlopeArmorBlock, TechGroup.LgLightArmor);

            NeedsResearch(LargeHeavyBlockArmorBlock, TechGroup.LgHeavyArmor);
            NeedsResearch(LargeHeavyBlockArmorSlope, TechGroup.LgHeavyArmor);
            NeedsResearch(LargeHeavyBlockArmorCorner, TechGroup.LgHeavyArmor);
            NeedsResearch(LargeHeavyBlockArmorCornerInv, TechGroup.LgHeavyArmor);
            NeedsResearch(LargeHeavyHalfArmorBlock, TechGroup.LgHeavyArmor);
            NeedsResearch(LargeHeavyHalfSlopeArmorBlock, TechGroup.LgHeavyArmor);


            NeedsResearch(ionThrusterSmallShipSmall, TechGroup.Permabanned);
            NeedsResearch(ionThrusterSmallShipLarge, TechGroup.Permabanned);
            NeedsResearch(ionThrusterLargeShipSmall, TechGroup.Permabanned);
            NeedsResearch(ionThrusterLargeShipLarge, TechGroup.Permabanned);
            NeedsResearch(hydroThrusterSmallShipSmall, TechGroup.Permabanned);
            NeedsResearch(hydroThrusterSmallShipLarge, TechGroup.Permabanned);
            NeedsResearch(hydroThrusterLargeShipSmall, TechGroup.Permabanned);
            NeedsResearch(hydroThrusterLargeShipLarge, TechGroup.Permabanned);

            NeedsResearch(refinery, TechGroup.Permabanned);
            NeedsResearch(blastFurnace, TechGroup.Permabanned);
            NeedsResearch(blastFurnace, TechGroup.Permabanned);
            NeedsResearch(jumpDrive, TechGroup.Permabanned);
            NeedsResearch(projectorLarge, TechGroup.Permabanned);
            NeedsResearch(projectorSmall, TechGroup.Permabanned);

            NeedsResearch(SkLarge, TechGroup.Permabanned);
            NeedsResearch(SkSmall, TechGroup.Permabanned);
            NeedsResearch(BasicAssembler, TechGroup.Permabanned);

            NeedsResearch(SafeZoneBlock, TechGroup.Permabanned);
            NeedsResearch(StoreBlock, TechGroup.Permabanned);
            NeedsResearch(ContractBlock, TechGroup.Permabanned);

        }
        public void AllowUnlockedTechs()
        {
            UnlockTechsSilently(0, UnlockedTechs);
        }

        private void NeedsResearch(MyDefinitionId techDef, TechGroup techgroup)
        {
            if (techDef == null) return;

            MyVisualScriptLogicProvider.ResearchListAddItem(techDef);

            HashSet<MyDefinitionId> techsInGroup;
            if (!techsForGroup.TryGetValue(techgroup, out techsInGroup))
            {
                techsInGroup = new HashSet<MyDefinitionId>();
                techsForGroup.Add(techgroup, techsInGroup);
            }
            techsInGroup.Add(techDef);
        }

        // Untested
        public void KeepTechsLocked()
        {
//            ModLog.Info("KeepTechsLocked()");

            foreach (var techGroup in techsForGroup)
            {
                var group = techGroup.Key;
//                ModLog.Info("KTL: Group=" + group.ToString());
                if (UnlockedTechs.Contains(group))
                {
//                    ModLog.Info(" UNLOCKED");
                    // OK to unlock
                    var technologies = techsForGroup[group];
                    foreach (var technology in technologies)
                    {
                        MyVisualScriptLogicProvider.ResearchListRemoveItem(technology); 
                    }
                }
                else
                {
//                    ModLog.Info(" LOCKED");
                    // block should be locked
                    var technologies = techsForGroup[group];
                    if (technologies == null)
                    {
                        ModLog.Error("No technologies for group: " + techGroup);
                        continue;
                    }
//                    ModLog.Info(" # blocks=" + technologies.Count.ToString());
                    foreach (var technology in technologies)
                    {
                        MyVisualScriptLogicProvider.ResearchListAddItem(technology);
                    }
                }
            }
        }

        internal void UnlockTechGroupForAllPlayers(TechGroup techGroup)
        {
            if (UnlockedTechs.Contains(techGroup))
            {
                return; // Already unlocked
            }

            HashSet<MyDefinitionId> technologies;
            if (!techsForGroup.TryGetValue(techGroup, out technologies))
            {
                ModLog.Error("No technologies for group: " + techGroup);
                return;
            }
            var players = new List<IMyPlayer>();
            MyAPIGateway.Players.GetPlayers(players);
            foreach (var player in players)
            {
                foreach (var technology in technologies)
                {
                    if (bNewResearch)
                        MyVisualScriptLogicProvider.ResearchListRemoveItem(technology); // SE 1.189
                    else
                        MyVisualScriptLogicProvider.PlayerResearchUnlock(player.IdentityId, technology);
                }
            }
            UnlockedTechs.Add(techGroup);
            audioSystem.PlayAudio(GetAudioClipForTechGroup(techGroup));
        }

        private static AudioClip GetAudioClipForTechGroup(TechGroup techGroup)
        {
            switch (techGroup)
            {
                case TechGroup.Permabanned:
                    return AudioClip.AllTechUnlocked;
                case TechGroup.AtmosphericEngines:
                    return AudioClip.UnlockAtmospherics;
                case TechGroup.Rockets:
                    return AudioClip.UnlockedMissiles;
                case TechGroup.OxygenGenerators:
                    return AudioClip.OxygenGeneratorUnlocked;
                case TechGroup.OxygenFarm:
                    return AudioClip.OxygenFarmUnlocked;
                case TechGroup.GasStorage:
                    return AudioClip.GasStorageUnlocked;
                case TechGroup.BasicWeapons:
                    return AudioClip.BasicWeaponsUnlocked;
                default:
                    return AudioClip.TechUnlocked;
            }
        }

        public void UnlockTechsSilently(long playerId, HashSet<TechGroup> techGroups)
        {
            foreach (var techGroup in techGroups)
            {
                var technologies = techsForGroup[techGroup];
                if (technologies == null)
                {
                    ModLog.Error("No technologies for group: " + techGroup);
                    return;
                }

                foreach (var technology in technologies)
                {

                    if (bNewResearch)
                        // unknown: does this work for ALL players?
                        MyVisualScriptLogicProvider.ResearchListRemoveItem(technology); // SE 1.189
                    else
                        MyVisualScriptLogicProvider.PlayerResearchUnlock(playerId, technology);
                }
            }
        }

        public void UnlockTechForJoiningPlayer(long playerId)
        {
            foreach (var techGroup in UnlockedTechs)
            {
                var technologies = techsForGroup[techGroup];
                if (technologies == null)
                {
                    ModLog.Error("No technologies for group: " + techGroup);
                    return;
                }

                foreach (var technology in technologies)
                {
                    if (bNewResearch)
                        MyVisualScriptLogicProvider.ResearchListRemoveItem(technology); // SE 1.189
                    else
                        MyVisualScriptLogicProvider.PlayerResearchUnlock(playerId, technology);
                }
            }
        }
    }
}

