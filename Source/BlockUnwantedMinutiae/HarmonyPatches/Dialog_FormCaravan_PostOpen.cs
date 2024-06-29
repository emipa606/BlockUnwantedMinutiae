using HarmonyLib;
using RimWorld;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(Dialog_FormCaravan), nameof(Dialog_FormCaravan.PostOpen))]
internal static class Dialog_FormCaravan_PostOpen
{
    private static bool Prefix(Dialog_FormCaravan __instance)
    {
        if (BUMMod.Instance.settings.drawAutoSelectCheckboxPatch)
        {
            Traverse.Create(__instance).Field("autoSelectTravelSupplies").SetValue(false);
        }

        return true;
    }
}