using RimWorld;
using Verse;

namespace Truesight;

public class Hediff_Truesight : HediffWithComps
{
    public override void PostAdd(DamageInfo? dinfo)
    {
        base.PostAdd(dinfo);
        UpdateSeverity();
    }

    public override bool ShouldRemove => !pawn.ShouldHaveTruesight();

    public override void Tick()
    {
        base.Tick();
        if (Find.TickManager.TicksGame % 60 == 0)
        {
            UpdateSeverity();
        }
    }

    public void UpdateSeverity()
    {
        severityInt = pawn.HasPsylink
            ? pawn.GetPsylinkLevel()
            : 0;
    }
}