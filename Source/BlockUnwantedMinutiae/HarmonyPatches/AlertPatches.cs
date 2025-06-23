using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch]
internal class AlertPatches
{
    private static readonly ReadOnlyDictionary<string, string> classMap = new(
        new Dictionary<string, string>
        {
            // CORE
            { "Alert_ActivatorCountdown", "ActivatorCountdown" },
            { "Alert_AnimalFilth", "AlertAnimalFilth" },
            { "Alert_AnimalPenNeeded", "AlertAnimalPenNeeded" },
            { "Alert_AnimalPenNotEnclosed", "AlertAnimalPenNotEnclosed" },
            { "Alert_AnimalRoaming", "AlertAnimalIsRoaming" },
            { "Alert_AwaitingMedicalOperation", "PatientsAwaitingMedicalOperation" },
            { "Alert_BilliardsTableOnWall", "BilliardsTableOnWall" },
            { "Alert_Boredom", "Boredom" },
            { "Alert_BrawlerHasRangedWeapon", "BrawlerHasRangedWeapon" },
            { "Alert_CannotBeUsedRoofed", "BuildingCantBeUsedRoofed" },
            { "Alert_CaravanIdle", "CaravanIdle" },
            { "Alert_CasketOpening", "AlertCasketOpening" },
            { "Alert_ColonistLeftUnburied", "ColonistLeftUnburied" },
            { "Alert_ColonistNeedsRescuing", "ColonistNeedsRescuing" },
            { "Alert_ColonistNeedsTend", "ColonistNeedsTend" },
            { "Alert_ColonistsIdle", "ColonistsIdle" },
            { "Alert_DormanyWakeUpDelay", "DormancyWakeUpDelay" },
            { "Alert_Exhaustion", "Exhaustion" },
            { "Alert_FireInHomeArea", "FireInHomeArea" },
            { "Alert_FuelNodeIgnition", "FuelNodeIgnition" },
            { "Alert_Heatstroke", "Heatstroke" },
            { "Alert_HitchedAnimalHungryNoFood", "HitchedAnimalHungryNoFood" },
            { "Alert_HunterHasShieldAndRangedWeapon", "HunterHasShieldAndRangedWeapon" },
            { "Alert_HunterLacksRangedWeapon", "HunterLacksRangedWeapon" },
            { "Alert_Hypothermia", "Hypothermia" },
            { "Alert_HypothermicAnimals", "HypothermicAnimals" },
            { "Alert_ImmobileCaravan", "ImmobileCaravan" },
            { "Alert_InfestationDelay", "InfestationDelay" },
            { "Alert_JoyBuildingNoChairs", "JoyBuildingNoChairs" },
            { "Alert_LifeThreateningHediff", "LifeThreateningHediff" },
            { "Alert_LowFood", "LowFood" },
            { "Alert_LowMedicine", "LowMedicine" },
            { "Alert_MajorOrExtremeBreakRisk", "MajorOrExtremeBreakRisk" },
            { "Alert_MinifiedTreeAboutToDie", "MinifiedTreeAboutToDie" },
            { "Alert_MinorBreakRisk", "MinorBreakRisk" },
            { "Alert_NeedBatteries", "NeedBatteries" },
            { "Alert_NeedColonistBeds", "NeedColonistBeds" },
            { "Alert_NeedDefenses", "NeedDefenses" },
            { "Alert_NeedDoctor", "NeedDoctor" },
            { "Alert_NeedJoySources", "NeedJoySources" },
            { "Alert_NeedMealSource", "NeedMealSource" },
            { "Alert_NeedMiner", "NeedMiner" },
            { "Alert_NeedResearchBench", "NeedResearchBench" },
            { "Alert_NeedResearchProject", "NeedResearchProject" },
            { "Alert_NeedWarden", "NeedWarden" },
            { "Alert_NeedWarmClothes", "NeedWarmClothes" },
            { "Alert_PasteDispenserNeedsHopper", "PasteDispenserNeedsHopper" },
            { "Alert_PennedAnimalHungry", "PennedAnimalHungry" },
            { "Alert_PredatorInPen", "PredatorInPen" },
            { "Alert_QuestExpiresSoon", "QuestExpiresSoon" },
            { "Alert_ShieldUserHasRangedWeapon", "ShieldUserHasRangedWeapon" },
            { "Alert_StarvationAnimals", "StarvationAnimals" },
            { "Alert_StarvationColonists", "StarvationColonists" },
            { "Alert_Thought", "Thought" },
            { "QuestPart_Alert", "Alert" },
            { "QuestPart_Delay", "Delay" },
            { "QuestPart_MoodBelow", "MoodBelow" },
            { "QuestPart_ShuttleDelay", "ShuttleDelay" },
            { "QuestPart_ShuttleLeaveDelay", "ShuttleLeaveDelay" },
            // ROYALTY
            { "Alert_AnimaLinkingReady", "AnimaLinkingReady" },
            { "Alert_BestowerWaiting", "BestowerWaiting" },
            { "Alert_DisallowedBuildingInsideMonument", "DisallowedBuildingInsideMonument" },
            { "Alert_MonumentMarkerMissingBlueprints", "MonumentMarkerMissingBlueprints" },
            { "Alert_NeedMeditationSpot", "NeedMeditationSpot" },
            { "Alert_RoyalNoAcceptableFood", "RoyalNoAcceptableFood" },
            { "Alert_RoyalNoThroneAssigned", "RoyalNoThroneAssigned" },
            { "Alert_ShuttleLandingBeaconUnusable", "ShuttleLandingBeaconUnusable" },
            { "Alert_ThroneroomInvalidConfiguration", "ThroneroomInvalidConfiguration" },
            { "Alert_TimedMakeFactionHostile", "TimedMakeFactionHostile" },
            { "Alert_TimedRaidsArriving", "TimedRaidsArriving" },
            { "Alert_TitleRequiresBedroom", "TitleRequiresBedroom" },
            { "Alert_UndignifiedBedroom", "UndignifiedBedroom" },
            { "Alert_UndignifiedThroneroom", "UndignifiedThroneroom" },
            { "Alert_UnusableMeditationFocus", "UnusableMeditationFocus" },
            { "Alert_PermitAvailable", "PermitAvailable" },
            // IDEOLOGY
            { "Alert_AgeReversalDemandNear", "AgeReversalDemandNear" },
            { "Alert_ConnectedPawnNotAssignedToPlantCutting", "ConnectedPawnNotAssignedToPlantCutting" },
            { "Alert_DateRitualComing", "DateRitualComing" },
            { "Alert_GauranlenTreeWithoutProductionMode", "GauranlenTreeWithoutProductionMode" },
            { "Alert_IdeoBuildingDisrespected", "IdeoBuildingDisrespected" },
            { "Alert_IdeoBuildingMissing", "IdeoBuildingMissing" },
            { "Alert_NeedSlaveBeds", "NeedSlaveBeds" },
            { "Alert_RolesEmpty", "RolesEmpty" },
            { "Alert_SlaveRebellionLikely", "SlaveRebellionLikely" },
            { "Alert_SlavesUnattended", "SlavesUnattended" },
            { "Alert_SlavesUnsuppressed", "SlavesUnsuppressed" },
            // BIOTECH
            { "Alert_Biostarvation", "Biostarvation" },
            { "Alert_GenebankUnpowered", "GenebankUnpowered" },
            { "Alert_LowBabyFood", "LowBabyFood" },
            { "Alert_LowDeathrest", "LowDeathrest" },
            { "Alert_LowHemogen", "LowHemogen" },
            { "Alert_MechChargerFull", "MechChargerFull" },
            { "Alert_MechDamaged", "MechDamaged" },
            { "Alert_MechMissingBodyPart", "MechMissingBodyPart" },
            { "Alert_NeedBabyCribs", "NeedBabyCribs" },
            { "Alert_NeedMechChargers", "NeedMechChargers" },
            { "Alert_NeedSlaveCribs", "NeedSlaveCribs" },
            { "Alert_NoBabyFeeders", "NoBabyFeeders" },
            { "Alert_NoBabyFoodCaravan", "NoBabyFoodCaravan" },
            { "Alert_PollutedTerrain", "PollutedTerrain" },
            { "Alert_PsychicBondedSeparated", "PsychicBondedSeparated" },
            { "Alert_ReimplantationAvailable", "ReimplantationAvailable" },
            { "Alert_SubjectHasNowOverseer", "SubjectHasNowOverseer" },
            { "Alert_ToxicBuildup", "ToxicBuildup" },
            { "Alert_ToxifierGeneratorStopped", "ToxifierGeneratorStopped" },
            { "Alert_WarqueenHasLowResources", "WarqueenHasLowResources" },
            // ANOMALY
            { "Alert_UndercaveUnstable", "Alert_UndercaveUnstable" },
            { "Alert_CultistPsychicRitual", "Alert_CultistPsychicRitual" },
            { "Alert_InsufficientContainment", "Alert_InsufficientContainment" },
            { "Alert_MeatHunger", "AlertMeatHunger" },
            { "Alert_NeedHoldingPlatform", "AlertHoldingPlatform" },
            { "Alert_CubeWithdrawal", "AlertCubeWithdrawal" },
            { "Alert_Digesting", "AlertDigestion" },
            { "Alert_InhibitorBlocked", "AlertInhibitorBlocked" },
            { "Alert_CreepJoinerTimeout", "CreepJoinerTimeout" },
            { "Alert_NeedAnomalyProject", "NeedAnomalyProject" },
            { "Alert_DangerousActivity", "ActivityMultipleDangerous" },
            { "Alert_EntityNeedsTend", "EntityNeedsTreatment" },
            { "Alert_GhoulHypothermia", "AlertGhoulHypothermia" },
            { "Alert_Analyzable", "AlertAnalyzable" }
        });

    public static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.GetTypesFromAssembly(Assembly.Load("Assembly-CSharp"))
            .SelectMany(type => type.GetMethods())
            .Where(method => method.ReturnType == typeof(AlertReport) &&
                             !method.IsAbstract &&
                             method.ReflectedType != null &&
                             classMap.ContainsKey(method.ReflectedType.Name));
    }

    public static bool Prefix(MethodBase __originalMethod)
    {
        if (__originalMethod.ReflectedType == null)
        {
            return true;
        }

        if (classMap.TryGetValue(__originalMethod.ReflectedType.Name, out var value))
        {
            return !BUMMod.Instance.Settings.GetGenericAlertPatchValue(value);
        }

        Log.ErrorOnce($"Failed to find alert type for {__originalMethod.ReflectedType.Name}",
            __originalMethod.ReflectedType.Name.GetHashCode());
        return true;
    }
}