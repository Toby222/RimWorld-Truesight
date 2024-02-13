using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(
    typeof(PawnCapacityWorker_Sight),
    nameof(PawnCapacityWorker_Sight.CalculateCapacityLevel)
)]
public static class TruesightedPawnsHaveSightCapacity
{
    [HarmonyPostfix]
    public static void Postfix(ref float __result, HediffSet diffSet)
    {
        if (diffSet.HasHediff(Truesight_DefOf.Truesight))
        {
            // Sets sight to 100% for truesighted pawns
            __result = 1.0f;
        }
    }
}
