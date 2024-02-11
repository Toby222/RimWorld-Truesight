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
            ___pawn.health.AddHediff(Truesight_DefOf.Truesight, ___pawn.health.hediffSet.GetBrain());
        }
    }
}