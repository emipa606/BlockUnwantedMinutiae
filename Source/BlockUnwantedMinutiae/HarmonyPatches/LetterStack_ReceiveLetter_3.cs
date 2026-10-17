using BlockUnwantedMinutiae.Patches;
using HarmonyLib;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(LetterStack), nameof(LetterStack.ReceiveLetter), typeof(Letter), typeof(string), typeof(int),
    typeof(bool))]
internal static class LetterStack_ReceiveLetter_3
{
    public static bool Prefix(Letter let)
    {
        return GenericMessagePatchHelper.ContainsLetter(let.Label.ToString());
    }
}