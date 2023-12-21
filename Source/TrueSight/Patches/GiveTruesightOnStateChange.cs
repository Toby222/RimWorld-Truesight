using HarmonyLib;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.CheckForStateChange))]
public static class GiveTruesightOnStateChange
{
    public static void Postfix(Pawn ___pawn)
    {
        if (___pawn.ShouldGetTruesight())
        {
            Hediff hediff = HediffMaker.MakeHediff(Truesight_DefOf.Truesight, ___pawn);
            ___pawn.health.AddHediff(hediff);
        }
    }
}