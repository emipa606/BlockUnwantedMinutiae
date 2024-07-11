using System.Collections.Generic;
using System.Text.RegularExpressions;
using Verse;

namespace BlockUnwantedMinutiae.Patches;

internal static class GenericMessagePatchHelper
{
    private static readonly Dictionary<string, bool> messagePatchesLookup = new Dictionary<string, bool>();
    private static readonly Dictionary<string, bool> letterPatchesLookup = new Dictionary<string, bool>();

    public static void ResetPatches()
    {
        messagePatchesLookup.Clear();
        letterPatchesLookup.Clear();
    }

    internal static bool ContainsMessage(string text)
    {
        if (messagePatchesLookup.TryGetValue(text, out var result))
        {
            return result;
        }

        foreach (var l in BUMMod.Instance.settings.ActiveMessagePatches)
        {
            var targetMsg = ReplaceTags(l.Translate());
            var regex = new Regex($".*{targetMsg}");

            if (regex.Match(text).Length <= 0)
            {
                continue;
            }

            messagePatchesLookup[text] = false;
            return false;
        }

        messagePatchesLookup[text] = true;
        return true;
    }

    internal static bool ContainsLetter(string text)
    {
        if (letterPatchesLookup.TryGetValue(text, out var result))
        {
            return result;
        }

        foreach (var l in BUMMod.Instance.settings.ActiveLetterPatches)
        {
            var targetMsg = ReplaceTags(l.Translate());
            var regex = new Regex($"{targetMsg}");

            if (regex.Match(text).Length <= 0)
            {
                continue;
            }

            letterPatchesLookup[text] = false;
            return false;
        }

        letterPatchesLookup[text] = true;
        return true;
    }

    private static string ReplaceTags(string text)
    {
        var regex = new Regex(@"{\S*}");
        return regex.Replace(text, ".*");
    }
}