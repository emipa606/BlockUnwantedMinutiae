using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.Patches
{
    [HarmonyPatch]
    class AlertPatches
    {
        private static readonly ReadOnlyDictionary<string, string> classMap = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()
        {
            {"Alert_ActivatorCountdown", "ActivatorCountdown"},
            {"Alert_AwaitingMedicalOperation", "PatientsAwaitingMedicalOperation"},
            {"Alert_BestowerWaiting", "BestowerWaitingAlert"},
            {"Alert_BilliardsTableOnWall", "BilliardsNeedsSpace"},
            {"Alert_Boredom", "Boredom"},
            {"Alert_BrawlerHasRangedWeapon", "BrawlerHasRangedWeapon"},
            {"Alert_CannotBeUsedRoofed", "BuildingCantBeUsedRoofed"},
            {"Alert_CaravanIdle", "CaravanIdle"},
            {"Alert_ColonistLeftUnburied", "AlertColonistLeftUnburied"},
            {"Alert_ColonistNeedsRescuing", "ColonistNeedsRescue"},
            {"Alert_ColonistNeedsTend", "ColonistNeedsTreatment"},
            {"Alert_ColonistsIdle", "ColonistsIdle"},
            {"Alert_DisallowedBuildingInsideMonument", "DisallowedBuildingInsideMonument"},
            {"Alert_Exhaustion", "Exhaustion"},
            {"Alert_FireInHomeArea", "FireInHomeArea"},
            {"Alert_Heatstroke", "AlertHeatstroke"},
            {"Alert_HunterHasShieldAndRangedWeapon", "HunterHasShieldAndRangedWeapon"},
            {"Alert_HunterLacksRangedWeapon", "HunterLacksWeapon"},
            {"Alert_Hypothermia", "AlertHypothermia"},
            {"Alert_ImmobileCaravan", "ImmobileCaravan"},
            {"Alert_LifeThreateningHediff", "PawnsWithLifeThreateningDisease"},
            {"Alert_LowFood", "LowFood"},
            {"Alert_LowMedicine", "LowMedicine"},
            {"Alert_MonumentMarkerMissingBlueprints", "MonumentMarkerMissingBlueprints"},
            {"Alert_NeedBatteries", "NeedBatteries"},
            {"Alert_NeedColonistBeds", "NeedColonistBeds"},
            {"Alert_NeedDefenses", "NeedDefenses"},
            {"Alert_NeedDoctor", "NeedDoctor"},
            {"Alert_NeedJoySources", "NeedJoySource"},
            {"Alert_NeedMealSource", "NeedMealSource"},
            {"Alert_NeedMeditationSpot", "NeedMeditationSpotAlert"},
            {"Alert_NeedMiner", "NeedMiner"},
            {"Alert_NeedResearchProject", "NeedResearchProject"},
            {"Alert_NeedWarden", "NeedWarden"},
            {"Alert_NeedWarmClothes", "NeedWarmClothes"},
            {"Alert_PasteDispenserNeedsHopper", "NeedFoodHopper"},
            {"Alert_PermitAvailable", "PermitChoiceReadyAlert"},
            {"Alert_QuestExpiresSoon", "QuestExpiresSoon"},
            {"Alert_RoyalNoAcceptableFood", "RoyalNoAcceptableFood"},
            {"Alert_RoyalNoThroneAssigned", "NeedThroneAssigned"},
            {"Alert_ShieldUserHasRangedWeapon", "ShieldUserHasRangedWeapon"},
            {"Alert_ShuttleLandingBeaconUnusable", "ShipLandingBeaconUnusable"},
            {"Alert_StarvationAnimals", "StarvationAnimals"},
            {"Alert_StarvationColonists", "Starvation"},
            {"Alert_ThroneroomInvalidConfiguration", "ThroneroomInvalidConfiguration"},
            {"Alert_TitleRequiresBedroom", "NeedBedroomAssigned"},
            {"Alert_UndignifiedBedroom", "UndignifiedBedroom"},
            {"Alert_UndignifiedThroneroom", "UndignifiedThroneroom"},
            {"Alert_UnusableMeditationFocus", "UnusableMeditationFocusAlert"},
            {"QuestPart_MoodBelow", "QuestPartMoodBelowThreshold"},
            {"QuestPart_ShuttleDelay", "QuestPartShuttleArriveDelay"},
            {"QuestPart_ShuttleLeaveDelay", "QuestPartShuttleLeaveDelay"},
        });
        
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return AccessTools.GetTypesFromAssembly(Assembly.Load("Assembly-CSharp"))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.ReturnType == typeof(AlertReport) &&
                                 !method.IsAbstract &&
                                 method.ReflectedType != null &&
                                 classMap.ContainsKey(method.ReflectedType.Name))
                .Cast<MethodBase>();
        }

        public static bool Prefix(MethodBase __originalMethod)
        {
            if (__originalMethod.ReflectedType == null) return true;
            return !LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue(classMap[__originalMethod.ReflectedType.Name]);
        }
    }
    

    [HarmonyAfter(nameof(AlertPatches))]
    [HarmonyPatch(typeof(Alert_ColonistsIdle))]
    [HarmonyPatch("IdleColonists", MethodType.Getter)] // can't use alternate annotation because member is private
    static class IdleColonistsPatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().idleColonistsPatch == false) return;
            
            List<Pawn> nonGuests = new List<Pawn>();
            foreach (Pawn pawn in __result)
            {
                if (!pawn.IsQuestLodger()) nonGuests.Add(pawn);
            }
            
            __result = nonGuests;
        }
    }

    [HarmonyPatch(typeof(BreakRiskAlertUtility), nameof(BreakRiskAlertUtility.PawnsAtRiskMinor), MethodType.Getter)]
    static class RiskMinorPatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("BreakRiskMinor"))
                __result = new List<Pawn>();
        }
    }

    [HarmonyPatch(typeof(BreakRiskAlertUtility), nameof(BreakRiskAlertUtility.PawnsAtRiskMajor), MethodType.Getter)]
    static class RiskMajorPatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("BreakRiskMajor"))
                __result = new List<Pawn>();
        }
    }

    [HarmonyPatch(typeof(BreakRiskAlertUtility), nameof(BreakRiskAlertUtility.PawnsAtRiskExtreme), MethodType.Getter)]
    static class RiskExtremePatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("BreakRiskExtreme"))
                __result = new List<Pawn>();
        }
    }

    [HarmonyPatch(typeof(Alert_Thought), nameof(Alert_Thought.GetReport))]
    static class AlertThoughtPatch
    {
        static bool Prefix(Alert_Thought __instance)
        {
            switch (__instance)
            {
                case Alert_TatteredApparel _:
                    return !LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("AlertTatteredApparel");
                case Alert_UnhappyNudity _:
                    return !LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("AlertUnhappyNudity");
                default:
                    return true;
            }
        }
    }
}