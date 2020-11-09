using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.Patches
{

    [HarmonyPatch(typeof(Alert_ColonistsIdle))]
    [HarmonyPatch("IdleColonists", MethodType.Getter)]
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

    [HarmonyPatch(typeof(BreakRiskAlertUtility))]
    [HarmonyPatch("PawnsAtRiskMinor", MethodType.Getter)]
    static class RiskMinorPatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("BreakRiskMinor"))
                __result = new List<Pawn>();
        }
    }

    [HarmonyPatch(typeof(BreakRiskAlertUtility))]
    [HarmonyPatch("PawnsAtRiskMajor", MethodType.Getter)]
    static class RiskMajorPatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("BreakRiskMajor"))
                __result = new List<Pawn>();
        }
    }

    [HarmonyPatch(typeof(BreakRiskAlertUtility))]
    [HarmonyPatch("PawnsAtRiskExtreme", MethodType.Getter)]
    static class RiskExtremePatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetGenericAlertPatchValue("BreakRiskExtreme"))
                __result = new List<Pawn>();
        }
    }
}