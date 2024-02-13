using HarmonyLib;
using RimWorld;
using Verse;

namespace Truesight.Patches;

[HarmonyPatch(typeof(ThoughtWorker_Precept_NonBlind_Social), "ShouldHaveThought")]
public static class TruesightedPawnsAreNotNonBlind_Social
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Roslynator",
        "RCS1163:Unused parameter",
        Justification = "Required for patching"
    )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Required for patching"
    )]
    public static void Postfix(ref ThoughtState __result, Pawn p, Pawn otherPawn)
    {
        if (otherPawn.HasTruesight())
        {
            __result = ThoughtState.Inactive;
        }
    }
}
