using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(PawnUtility), nameof(PawnUtility.IsBiologicallyBlind))]
public static class TruesightedPawnsAreNotBiologicallyBlind
{
    public static void Postfix(ref bool __result, Pawn pawn)
    {
        if (pawn.HasTruesight())
        {
            __result = false;
        }
    }
}
