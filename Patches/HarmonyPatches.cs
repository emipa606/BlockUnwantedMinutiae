using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.Patches
{
    
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