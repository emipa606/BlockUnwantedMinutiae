using System.Reflection;
using BlockUnwantedMinutiae.Patches;
using HarmonyLib;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("BlockUnwantedMinutiae").PatchAll(Assembly.GetExecutingAssembly());
        BUMMod.Instance.Settings.ResetPatches();
        GenericMessagePatchHelper.ResetPatches();
    }
}