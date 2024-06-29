using BlockUnwantedMinutiae.Patches;
using HarmonyLib;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(LetterStack), nameof(LetterStack.ReceiveLetter), typeof(TaggedString), typeof(TaggedString),
    typeof(LetterDef), typeof(string), typeof(int), typeof(bool))]
internal static class LetterStack_ReceiveLetter_2
{
    private static bool Prefix(TaggedString label)
    {
        return GenericMessagePatchHelper.ContainsLetter(label.ToString());
    }
}