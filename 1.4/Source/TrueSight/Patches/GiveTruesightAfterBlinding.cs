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
            Hediff hediff = HediffMaker.MakeHediff(Truesight_DefOf.Truesight, pawn);
            pawn.health.AddHediff(hediff);
        }
    }
}