using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(PawnUtility), nameof(PawnUtility.IsArtificiallyBlind))]
public static class TruesightedPawnsAreNotArtificiallyBlind
{
    public static void Postfix(ref bool __result, Pawn p)
    {
        if (p.HasTruesight())
        {
            __result = false;
        }
    }
}