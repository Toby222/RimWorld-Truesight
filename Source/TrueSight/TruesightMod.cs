using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Truesight;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("dev.toby.truesight").PatchAll();
    }

    public static bool ShouldGetTruesight(this Pawn pawn)
    {
        return !pawn.HasTruesight() && pawn.ShouldHaveTruesight();
    }

    public static bool SightSourcesNonFunctional(this Pawn pawn)
    {
        List<BodyPartRecord> sightSourceParts = pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource);
        return sightSourceParts.All(part => PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part) <= 0.0f);
    }

    public static bool ShouldHaveTruesight(this Pawn pawn)
    {
        return pawn is Pawn { Dead: false, Ideo: not null, health: not null }
            && pawn.Ideo.IdeoApprovesOfBlindness()
            && pawn.SightSourcesNonFunctional();
    }

    public static bool HasTruesight(this Pawn pawn)
    {
        return pawn.health.hediffSet.HasHediff(Truesight_DefOf.Truesight);
    }
}