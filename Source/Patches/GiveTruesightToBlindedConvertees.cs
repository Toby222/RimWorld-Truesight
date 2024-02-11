using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

// Handle conversion of already blind pawns
[HarmonyPatch(typeof(Pawn_IdeoTracker), nameof(Pawn_IdeoTracker.SetIdeo))]
public static class GiveTruesightToBlindedConvertees
{
    public static void Postfix(Pawn ___pawn)
    {
        if (___pawn.ShouldGetTruesight())
        {
            ___pawn.health.AddHediff(Truesight_DefOf.Truesight, ___pawn.health.hediffSet.GetBrain());
        }
    }
}