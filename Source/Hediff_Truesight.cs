#nullable enable

using RimWorld;
using Verse;

namespace Truesight;

public class Hediff_Truesight : Hediff_Level
{
    public override void PostAdd(DamageInfo? dinfo)
    {
        base.PostAdd(dinfo);
        UpdateLevel();
    }

    public override bool ShouldRemove => !pawn.ShouldHaveTruesight();

    public override void Tick()
    {
        base.Tick();
        if (Find.TickManager.TicksGame % 60 == 0)
        {
            UpdateLevel();
        }
    }

    public void UpdateLevel()
    {
        SetLevelTo(pawn.HasPsylink ? pawn.GetPsylinkLevel() : 0);
    }

    public override HediffStage CurStage => TruesightHediffUtils.GetHediffStage((int)severityInt);
}
