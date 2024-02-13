using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace Truesight;

public static class TruesightHediffUtils
{
    public static bool ShouldGetTruesight(this Pawn pawn)
    {
        return !pawn.HasTruesight() && pawn.ShouldHaveTruesight();
    }

    public static bool SightSourcesNonFunctional(this Pawn pawn)
    {
        List<BodyPartRecord> sightSourceParts = pawn.RaceProps.body.GetPartsWithTag(
            BodyPartTagDefOf.SightSource
        );
        return sightSourceParts.All(part =>
            PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, part) <= 0.0f
        );
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

    private static readonly Dictionary<int, HediffStage> stageCache = new();

    public static void ClearHediffStageCache()
    {
        stageCache.Clear();
    }

    public static HediffStage GetHediffStage(int severity)
    {
        var stage = stageCache.TryGetValue<int, HediffStage>(severity);
        if (stage is not null)
        {
            return stage;
        }
        var newStage = GenerateHediffStage(severity);
        stageCache.Add(severity, newStage);
        return newStage;
    }

    private static HediffStage GenerateHediffStage(int severity)
    {
        return new()
        {
            capMods = new()
            {
                new()
                {
                    capacity = PawnCapacityDefOf.Hearing,
                    offset = Mathf.Lerp(0.0f, 0.5f, severity / TruesightMod.MaxLevel)
                },
                new()
                {
                    capacity = PawnCapacityDefOf.Sight,
                    offset = Mathf.Lerp(-1.0f, 0.5f, severity / TruesightMod.MaxLevel)
                },
            },
            overrideTooltip =
                "This person has successfully completed the ritual of Truesight and is now on their way to unlocking their true vision. Acquire psylink levels to improve their stats.",
        };
    }
}
