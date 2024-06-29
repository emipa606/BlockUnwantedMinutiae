using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(BreakRiskAlertUtility), nameof(BreakRiskAlertUtility.PawnsAtRiskMinor), MethodType.Getter)]
internal static class BreakRiskAlertUtility_PawnsAtRiskMinor
{
    private static void Postfix(ref List<Pawn> __result)
    {
        if (BUMMod.Instance.settings.GetGenericAlertPatchValue("BreakRiskMinor"))
        {
            __result = [];
        }
    }
}