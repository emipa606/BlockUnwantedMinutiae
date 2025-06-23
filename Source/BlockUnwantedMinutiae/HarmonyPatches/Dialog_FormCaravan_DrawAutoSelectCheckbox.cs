using HarmonyLib;
using RimWorld;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(Dialog_FormCaravan), nameof(Dialog_FormCaravan.DrawAutoSelectCheckbox))]
internal static class Dialog_FormCaravan_DrawAutoSelectCheckbox
{
    private static bool Prefix(Dialog_FormCaravan __instance)
    {
        if (!BUMMod.Instance.Settings.DrawAutoSelectCheckboxPatch)
        {
            return true;
        }

        Traverse.Create(__instance).Field("autoSelectTravelSupplies").SetValue(false);
        return false;
    }
}