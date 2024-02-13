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
            __instance.health.AddHediff(
                Truesight_DefOf.Truesight,
                __instance.health.hediffSet.GetBrain()
            );
        }
    }
}
