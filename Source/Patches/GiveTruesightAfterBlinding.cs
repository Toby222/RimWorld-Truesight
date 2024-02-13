using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(JobDriver_Blind), nameof(JobDriver_Blind.Blind), typeof(Pawn), typeof(Pawn))]
public static class GiveTruesightAfterBlinding
{
    public static void Postfix(Pawn pawn)
    {
        if (pawn.ShouldGetTruesight())
        {
            pawn.health.AddHediff(Truesight_DefOf.Truesight, pawn.health.hediffSet.GetBrain());
        }
    }
}
