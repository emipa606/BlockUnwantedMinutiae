using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using System.Text.RegularExpressions;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae
{
    
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            new Harmony("BlockUnwantedMinutiae").PatchAll();

            
        }
    }

    [HarmonyPatch(typeof(Messages))]
    [HarmonyPatch("Message")]
    [HarmonyPatch(new Type[] { typeof(string), typeof(LookTargets), typeof(MessageTypeDef), typeof(bool)})]
    static class ApparelMessagePatch
    {
        static bool Prefix(string text)
        {
            string targetMsg = "MessageDeterioratedAway".Translate(""); // blank arg so we don't have {0}
            string pattern = @".*\)\s*" + targetMsg;

            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().apparelMessagePatch == false)
            {
                pattern = @".*T\)\s*" + targetMsg;
                if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().taintedMessagePatch == false)
                    return true;
            }
            
            Regex regex = new Regex(pattern);

            if (regex.Match(text).Length > 0) return false;
            
            return true;
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

    [HarmonyPatch(typeof(LetterStack))]
    [HarmonyPatch("ReceiveLetter")]
    [HarmonyPatch(new Type[] { typeof(TaggedString), typeof(TaggedString), typeof(LetterDef), typeof(LookTargets), typeof(Faction), typeof(Quest), typeof(List<ThingDef>), typeof(string)})]
    static class RoofCollapsePatch
    {
        static bool Prefix(TaggedString label)
        {
            if (LoadedModManager.GetMod<BUMMod>().GetSettings<BUMSettings>().taintedMessagePatch == false) return true;
            
            if (label == "LetterLabelRoofCollapsed".Translate()) return false;
            
            return true;
        }
    }
}