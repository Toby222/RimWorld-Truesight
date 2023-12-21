using HarmonyLib;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.SpawnSetup))]
public static class GiveTruesightOnSpawn
{
    public static void Postfix(Pawn __instance)
    {
        if (__instance.ShouldGetTruesight())
        {
            Hediff hediff = HediffMaker.MakeHediff(Truesight_DefOf.Truesight, __instance);
            __instance.health.AddHediff(hediff);
        }
    }
}