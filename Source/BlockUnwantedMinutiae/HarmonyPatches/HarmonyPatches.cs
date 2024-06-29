using System.Reflection;
using HarmonyLib;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("BlockUnwantedMinutiae").PatchAll(Assembly.GetExecutingAssembly());
    }
}