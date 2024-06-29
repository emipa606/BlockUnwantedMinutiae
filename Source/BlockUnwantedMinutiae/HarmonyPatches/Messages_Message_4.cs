using System.Text.RegularExpressions;
using HarmonyLib;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyAfter(nameof(Messages_Message_2))]
[HarmonyPatch(typeof(Messages), nameof(Messages.Message), typeof(string), typeof(LookTargets), typeof(MessageTypeDef),
    typeof(bool))]
internal static class Messages_Message_4
{
    private static bool Prefix(string text)
    {
        if (BUMMod.Instance.settings.taintedMessagePatch == false)
        {
            return true;
        }

        string targetMsg = "MessageDeterioratedAway".Translate(""); // blank arg so we don't have {0}
        var pattern = $@".*T\)\s*{targetMsg}";

        var regex = new Regex(pattern);

        return regex.Match(text).Length <= 0;
    }
}