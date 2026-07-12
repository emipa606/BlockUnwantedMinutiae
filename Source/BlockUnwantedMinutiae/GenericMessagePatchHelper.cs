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
            var regex = CreateComparer(l.Translate());

            if (!regex.IsMatch(text))
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
            var regex = CreateComparer(l.Translate());

            if (!regex.IsMatch(text))
            {
                continue;
            }

            letterPatchesLookup[text] = false;
            return false;
        }

        if (BUMMod.Instance.Settings.ShouldBlock(text))
        {
            letterPatchesLookup[text] = false;
            return false;
        }

        BUMMod.Instance.Settings.TryAddSeenText(text);
        letterPatchesLookup[text] = true;
        return true;
    }

    private static Regex CreateComparer(string target)
    {
        var pattern = Regex.Escape(target);

        if (LanguageDatabase.activeLanguage.FriendlyNameEnglish is "Korean")
        {
            pattern = Regex.Replace(pattern, @"\\\(은\\\)는", "(은|는)");
            pattern = Regex.Replace(pattern, @"\\\(이\\\)가", "(이|가)");
            pattern = Regex.Replace(pattern, @"\\\(을\\\)를", "(을|를)");
            pattern = Regex.Replace(pattern, @"\\\(와\\\)과", "(와|과)");
            pattern = Regex.Replace(pattern, @"\\\(으\\\)로", "(으로|로)");
        }

        pattern = Regex.Replace(pattern, @"\\{\S*?}", ".*");
        pattern = Regex.Replace(pattern, @"\.(\*)+", ".*");

        return new Regex($"{pattern}", RegexOptions.IgnoreCase);
    }
}