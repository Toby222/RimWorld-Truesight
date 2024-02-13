using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace Truesight;

public class TruesightMod : Mod
{
    public TruesightMod(ModContentPack content)
        : base(content)
    {
        Patches.WriteSettingsOnVPEWriteSettings.ApplySettingsDelegate = () => ApplySettings();

        Harmony harmony = new("dev.toby.truesight");

        harmony.PatchAll();
        LongEventHandler.ExecuteWhenFinished(ApplySettings);
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        ApplySettings();
    }

    public static float MaxLevel;

    private void ApplySettings()
    {
        TruesightModSettings truesightModSettings = GetSettings<TruesightModSettings>();
        MaxLevel = truesightModSettings.Source switch
        {
            TruesightModSettings.MaxLevelSource.Psylink
                => VanillaPsycastsExpanded.PsycastsMod.Settings.maxLevel,
            TruesightModSettings.MaxLevelSource.Custom => truesightModSettings.CustomLevel,

            _ => throw new Exception("Unexpected value for TruesightModSettings.Source"),
        };

        if (Truesight_DefOf.Truesight.maxSeverity != MaxLevel)
        {
            Truesight_DefOf.Truesight.maxSeverity = MaxLevel;
            TruesightHediffUtils.ClearHediffStageCache();
            // TODO(Maybe) Update all hediffs to scale to new max level?
        }
    }

    public override string SettingsCategory()
    {
        return "Truesight.SettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        var settings = GetSettings<TruesightModSettings>();
        Listing_Standard listing = new();
        listing.Begin(inRect);
        listing.LabelDouble(
            "Truesight.MaxLevel".Translate(),
            Truesight_DefOf.Truesight.maxSeverity.ToString()
        );
        if (
            listing.ButtonTextLabeled(
                "Truesight.MaxLevelSource".Translate(),
                $"Truesight.MaxLevelSource.{settings.Source}".Translate()
            )
        )
        {
            Find.WindowStack.Add(
                new FloatMenu(
                    Enum.GetValues(typeof(TruesightModSettings.MaxLevelSource))
                        .Cast<TruesightModSettings.MaxLevelSource>()
                        .Select(source => new FloatMenuOption(
                            source.ToString(),
                            () =>
                            {
                                GetSettings<TruesightModSettings>().Source = source;
                                ApplySettings();
                            }
                        ))
                        .ToList()
                )
            );
        }
        float textHeight = Text.CalcHeight(
            "Truesight.MaxLevel.Custom".Translate(),
            listing.ColumnWidth / 2f
        );
        var rect = listing.GetRect(textHeight);
        Widgets.Label(rect.LeftHalf(), "Truesight.MaxLevel.Custom".Translate());
        rect.height = 30f;
        Widgets.IntEntry(
            rect.RightHalf(),
            ref settings.CustomLevel,
            ref TruesightModSettings.CustomLevelString
        );
        listing.End();
    }
}

public class TruesightModSettings : ModSettings
{
    public MaxLevelSource Source;
    public int CustomLevel;
    public static string CustomLevelString;

    public enum MaxLevelSource
    {
        Psylink,
        Custom,
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Source, nameof(Source), MaxLevelSource.Psylink);
        Scribe_Values.Look(ref CustomLevel, nameof(CustomLevel), 6);
    }
}
