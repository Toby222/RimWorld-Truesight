using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(ThoughtWorker_Precept_NonBlind), "ShouldHaveThought")]
public static class TruesightedPawnsAreNotNonBlind
{
    public static void Postfix(ref ThoughtState __result, Pawn p)
    {
        if (p.HasTruesight())
        {
            __result = ThoughtState.Inactive;
        }
    }
}