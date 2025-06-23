using HarmonyLib;
using RimWorld;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(Alert_Thought), nameof(Alert_Thought.GetReport))]
internal static class Alert_Thought_GetReport
{
    private static bool Prefix(Alert_Thought __instance)
    {
        switch (__instance)
        {
            case Alert_TatteredApparel:
                return !BUMMod.Instance.Settings
                    .GetGenericAlertPatchValue("AlertTatteredApparel");
            case Alert_UnhappyNudity:
                return !BUMMod.Instance.Settings
                    .GetGenericAlertPatchValue("AlertUnhappyNudity");
            default:
                return true;
        }
    }
}