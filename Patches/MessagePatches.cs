using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.Patches
{
    internal static class GenericMessagePatchHelper
    {
        internal static bool ContainsMessage(string text)
        {
            List<string> labels = LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetActiveMessagePatches();

            foreach (string l in labels)
            {
                string targetMsg = ReplaceTags(l.Translate());
                Regex regex = new Regex(@".*" + targetMsg);
                
                if (regex.Match(text).Length > 0) return false;
            }
            
            return true;
        }

        private static string ReplaceTags(string text)
        {
            Regex regex = new Regex(@"{\S*}");
            return regex.Replace(text, ".*");
        }
    }
    
    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(LookTargets), typeof(MessageTypeDef), typeof(Quest), typeof(bool)})]
    static class GenericMessagePatch_1
    {
        static bool Prefix(string text)
        {
            Log.Message("GenericMessagePatch_1: " + text);
            return GenericMessagePatchHelper.ContainsMessage(text);
        }
    }
    
    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(LookTargets), typeof(MessageTypeDef), typeof(bool)})]
    static class GenericMessagePatch_2
    {
        static bool Prefix(string text)
        {
            Log.Message("GenericMessagePatch_2: " + text);
            return GenericMessagePatchHelper.ContainsMessage(text);
        }
    }
    
    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(MessageTypeDef), typeof(bool)})]
    static class GenericMessagePatch_3
    {
        static bool Prefix(string text)
        {
            Log.Message("GenericMessagePatch_3: " + text);
            return GenericMessagePatchHelper.ContainsMessage(text);
        }
    }

    [HarmonyAfter("GenericMessagePatch_2")]
    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(LookTargets), typeof(MessageTypeDef), typeof(bool)})]
    static class TaintedMessagePatch
    {
        static bool Prefix(string text)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().taintedMessagePatch == false) return true;
            
            string targetMsg = "MessageDeterioratedAway".Translate(""); // blank arg so we don't have {0}
            string pattern = @".*T\)\s*" + targetMsg;
            
            Regex regex = new Regex(pattern);

            if (regex.Match(text).Length > 0) return false;
            
            return true;
        }
    }
}