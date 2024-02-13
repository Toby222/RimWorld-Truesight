using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(
    typeof(ThoughtWorker_Precept_HalfBlind),
    nameof(ThoughtWorker_Precept_HalfBlind.IsHalfBlind)
)]
public static class TruesightedPawnsAreNotHalfBlind
{
    public static void Postfix(ref bool __result, Pawn p)
    {
        if (p.HasTruesight())
        {
            __result = false;
        }
    }
}
