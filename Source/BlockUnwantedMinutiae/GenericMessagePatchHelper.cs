using System.Collections.Generic;
using System.Text.RegularExpressions;
using Verse;

namespace BlockUnwantedMinutiae.Patches;

internal static class GenericMessagePatchHelper
{
    private static readonly Dictionary<string, bool> messagePatchesLookup = new();
    private static readonly Dictionary<string, bool> letterPatchesLookup = new();

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

        foreach (var l in BUMMod.Instance.Settings.ActiveMessagePatches)
        {
            var targetMsg = replaceTags(l.Translate());
            var regex = new Regex($".*{targetMsg}");

            if (regex.Match(text).Length <= 0)
            {
                continue;
            }

            messagePatchesLookup[text] = false;
            return false;
        }

        if (BUMMod.Instance.Settings.ShouldBlock(text))
        {
            messagePatchesLookup[text] = false;
            return false;
        }

        BUMMod.Instance.Settings.TryAddSeenText(text);
        messagePatchesLookup[text] = true;
        return true;
    }

    internal static bool ContainsLetter(string text)
    {
        if (letterPatchesLookup.TryGetValue(text, out var result))
        {
            return result;
        }

        foreach (var l in BUMMod.Instance.Settings.ActiveLetterPatches)
        {
            var targetMsg = replaceTags(l.Translate());
            var regex = new Regex($"{targetMsg}");

            if (regex.Match(text).Length <= 0)
            {
                continue;
            }

            letterPatchesLookup[text] = false;
            return false;
        }

        if (BUMMod.Instance.Settings.ShouldBlock(text))
        {
            messagePatchesLookup[text] = false;
            return false;
        }

        BUMMod.Instance.Settings.TryAddSeenText(text);
        letterPatchesLookup[text] = true;
        return true;
    }

    private static string replaceTags(string text)
    {
        var regex = new Regex(@"{\S*}");
        return regex.Replace(text, ".*");
    }
}