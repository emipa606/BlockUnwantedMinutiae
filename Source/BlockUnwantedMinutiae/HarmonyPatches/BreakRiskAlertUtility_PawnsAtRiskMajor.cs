using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(BreakRiskAlertUtility), nameof(BreakRiskAlertUtility.PawnsAtRiskMajor), MethodType.Getter)]
internal static class BreakRiskAlertUtility_PawnsAtRiskMajor
{
    public static void Postfix(ref List<Pawn> __result)
    {
        if (BUMMod.Instance.Settings.GetGenericAlertPatchValue("BreakRiskMajor"))
        {
            __result = [];
        }
    }
}