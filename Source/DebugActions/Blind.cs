namespace Truesight.DebugActions;

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

public static class TruesightActions
{
    [DebugAction(
        "Truesight",
        "Blind pawn",
        requiresIdeology: true,
        actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap
    )]
    private static void BlindPawn(Pawn pawn)
    {
        foreach (
            BodyPartRecord bodypart in from bodyPart in pawn.health.hediffSet.GetNotMissingParts()
            where bodyPart.def == BodyPartDefOf.Eye
            select bodyPart
        )
        {
            foreach (
                var hediff in pawn.TakeDamage(
                    new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, bodypart)
                )
                    .hediffs.Where(hediff => hediff is Hediff_MissingPart)
                    .Cast<Hediff_MissingPart>()
            )
            {
                hediff.IsFresh = false;
            }
        }
    }
}
