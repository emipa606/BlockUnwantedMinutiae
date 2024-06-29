using BlockUnwantedMinutiae.Patches;
using HarmonyLib;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(Messages), nameof(Messages.Message), typeof(string), typeof(LookTargets), typeof(MessageTypeDef),
    typeof(bool))]
internal static class Messages_Message_2
{
    private static bool Prefix(string text)
    {
        return GenericMessagePatchHelper.ContainsMessage(text);
    }
}