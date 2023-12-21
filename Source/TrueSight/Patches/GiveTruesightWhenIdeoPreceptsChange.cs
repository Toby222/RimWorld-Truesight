using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

// Handle ideological reform
[HarmonyPatch(typeof(Ideo), nameof(Ideo.RecachePrecepts))]
public static class GiveTruesightWhenIdeoPreceptsChange
{
    public static void Postfix(Ideo __instance)
    {
        if (!__instance.IdeoApprovesOfBlindness()) return;

        foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
        {
            if (pawn.ShouldGetTruesight())
            {
                Hediff hediff = HediffMaker.MakeHediff(Truesight_DefOf.Truesight, pawn);
                pawn.health.AddHediff(hediff);
            }
        }
    }
}