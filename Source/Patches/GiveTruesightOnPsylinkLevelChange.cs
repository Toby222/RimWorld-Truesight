using HarmonyLib;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(Hediff_Psylink), nameof(Hediff_Psylink.ChangeLevel), typeof(int), typeof(bool))]
public static class GiveTruesightOnPsylinkLevelChange
{
    public static void Postfix(Hediff_Psylink __instance)
    {
        if (__instance.pawn.ShouldGetTruesight())
        {
            __instance.pawn.health.AddHediff(Truesight_DefOf.Truesight, __instance.pawn.health.hediffSet.GetBrain());
        }
    }
}