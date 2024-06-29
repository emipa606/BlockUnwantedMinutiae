using System.Text.RegularExpressions;
using Verse;

namespace BlockUnwantedMinutiae.Patches;

internal static class GenericMessagePatchHelper
{
    internal static bool ContainsMessage(string text)
    {
        var labels = BUMMod.Instance.settings.ActiveGenericMessagePatches();

        foreach (var l in labels)
        {
            var targetMsg = ReplaceTags(l.Translate());
            var regex = new Regex($".*{targetMsg}");

            if (regex.Match(text).Length > 0)
            {
                return false;
            }
        }

        return true;
    }

    internal static bool ContainsLetter(string text)
    {
        var labels = BUMMod.Instance.settings.ActiveGenericLetterPatches();

        foreach (var l in labels)
        {
            var targetMsg = ReplaceTags(l.Translate());
            var regex = new Regex($"{targetMsg}");

            if (regex.Match(text).Length > 0)
            {
                return false;
            }
        }

        return true;
    }

    private static string ReplaceTags(string text)
    {
        var regex = new Regex(@"{\S*}");
        return regex.Replace(text, ".*");
    }
}