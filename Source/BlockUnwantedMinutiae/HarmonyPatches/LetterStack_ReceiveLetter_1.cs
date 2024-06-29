using System.Collections.Generic;
using BlockUnwantedMinutiae.Patches;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyPatch(typeof(LetterStack), nameof(LetterStack.ReceiveLetter), typeof(TaggedString), typeof(TaggedString),
    typeof(LetterDef), typeof(LookTargets), typeof(Faction), typeof(Quest), typeof(List<ThingDef>), typeof(string),
    typeof(int), typeof(bool))]
internal static class LetterStack_ReceiveLetter_1
{
    private static bool Prefix(TaggedString label)
    {
        return GenericMessagePatchHelper.ContainsLetter(label.ToString());
    }
}