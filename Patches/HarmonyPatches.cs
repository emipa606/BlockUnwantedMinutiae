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
        
        internal static bool ContainsLetter(string text)
        {
            List<string> labels = LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().GetActiveLetterPatches();

            foreach (string l in labels)
            {
                string targetMsg = ReplaceTags(l.Translate());
                Regex regex = new Regex(@"" + targetMsg);
                
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
    
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            new Harmony("BlockUnwantedMinutiae").PatchAll();
        }
    }

    [HarmonyPatch(typeof(Alert_ColonistsIdle))]
    [HarmonyPatch("IdleColonists", MethodType.Getter)]
    static class IdleColonistsPatch
    {
        static void Postfix(ref List<Pawn> __result)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().idleColonistsPatch == false) return;
            
            List<Pawn> nonGuests = new List<Pawn>();
            foreach (Pawn pawn in __result)
            {
                if (!pawn.IsQuestLodger()) nonGuests.Add(pawn);
            }
            
            __result = nonGuests;
        }
    }

    [HarmonyPatch(typeof(Dialog_FormCaravan))]
    [HarmonyPatch("DrawAutoSelectCheckbox")]
    static class DrawAutoSelectCheckboxPatch
    {
        static bool Prefix(Dialog_FormCaravan __instance)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().drawAutoSelectCheckboxPatch == false) return true;
            
            Traverse.Create(__instance).Field("autoSelectFoodAndMedicine").SetValue(false);
            return false;
        }
    }
}