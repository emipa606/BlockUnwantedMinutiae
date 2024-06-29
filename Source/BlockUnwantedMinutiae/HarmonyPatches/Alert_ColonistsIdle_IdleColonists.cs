using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BlockUnwantedMinutiae.HarmonyPatches;

[HarmonyAfter(nameof(AlertPatches))]
[HarmonyPatch(typeof(Alert_ColonistsIdle), "IdleColonists",
    MethodType.Getter)] // can't use alternate annotation because member is private
internal static class Alert_ColonistsIdle_IdleColonists
{
    private static void Postfix(ref List<Pawn> __result)
    {
        if (BUMMod.Instance.settings.idleColonistsPatch == false)
        {
            return;
        }

        var nonGuests = new List<Pawn>();
        foreach (var pawn in __result)
        {
            if (!pawn.IsQuestLodger())
            {
                nonGuests.Add(pawn);
            }
        }

        __result = nonGuests;
    }
}