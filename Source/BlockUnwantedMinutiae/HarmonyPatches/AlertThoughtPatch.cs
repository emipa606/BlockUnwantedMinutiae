using HarmonyLib;
using RimWorld;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(Alert_Thought), nameof(Alert_Thought.GetReport))]
internal static class AlertThoughtPatch
{
    private static bool Prefix(Alert_Thought __instance)
    {
        switch (__instance)
        {
            case Alert_TatteredApparel:
                return !BUMMod.Instance.settings
                    .GetGenericAlertPatchValue("AlertTatteredApparel");
            case Alert_UnhappyNudity:
                return !BUMMod.Instance.settings
                    .GetGenericAlertPatchValue("AlertUnhappyNudity");
            default:
                return true;
        }
    }
}