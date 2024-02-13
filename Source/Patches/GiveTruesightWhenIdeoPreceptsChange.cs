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
        if (!__instance.IdeoApprovesOfBlindness())
            return;

        foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
        {
            if (pawn.ShouldGetTruesight())
            {
                pawn.health.AddHediff(Truesight_DefOf.Truesight, pawn.health.hediffSet.GetBrain());
            }
        }
    }
}
