using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(BreakRiskAlertUtility), nameof(BreakRiskAlertUtility.PawnsAtRiskExtreme), MethodType.Getter)]
internal static class BreakRiskAlertUtility_PawnsAtRiskExtreme
{
    public static void Postfix(ref List<Pawn> __result)
    {
        if (BUMMod.Instance.Settings.GetGenericAlertPatchValue("BreakRiskExtreme"))
        {
            __result = [];
        }
    }
}