using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;
using System.Reflection;

namespace Truesight;

public class TruesightMod : Mod
{
    public TruesightMod(ModContentPack content) : base(content)
    {
        ApplySettingsDelegate = () => ApplySettings();

        Harmony harmony = new("dev.toby.truesight");
        if (ModsConfig.IsActive("VanillaExpanded.VPsycastsE"))
        {
            var original = typeof(VanillaPsycastsExpanded.PsycastsMod).GetMethod(nameof(VanillaPsycastsExpanded.PsycastsMod.WriteSettings), BindingFlags.Public | BindingFlags.Instance);
            var postfix = typeof(TruesightMod).GetMethod(nameof(ApplySettingsStatic), BindingFlags.NonPublic | BindingFlags.Static);
            if (original is null) throw new NullReferenceException("original WriteSettings method is null");
            if (postfix is null) throw new NullReferenceException("new ApplySettings method is null");
            harmony.Patch(original, postfix: new HarmonyMethod(postfix));
        }

        harmony.PatchAll();
        LongEventHandler.ExecuteWhenFinished(ApplySettings);
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        ApplySettings();
    }

    public static float MaxLevel;

    private static Delegate ApplySettingsDelegate;

    private static void ApplySettingsStatic()
    {
        ApplySettingsDelegate.DynamicInvoke();
    }

    private void ApplySettings()
    {
        TruesightModSettings truesightModSettings = GetSettings<TruesightModSettings>();
        MaxLevel = truesightModSettings.Source switch
        {
            TruesightModSettings.MaxLevelSource.Psylink when ModsConfig.IsActive("VanillaExpanded.VPsycastsE") => VanillaPsycastsExpanded.PsycastsMod.Settings.maxLevel,
            // Fall back to Ideology if VPE not installed
            TruesightModSettings.MaxLevelSource.Psylink => HediffDefOf.PsychicAmplifier.maxSeverity,
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
        listing.LabelDouble("Truesight.MaxLevel".Translate(), Truesight_DefOf.Truesight.maxSeverity.ToString());
        if (listing.ButtonTextLabeled("Truesight.MaxLevelSource".Translate(), $"Truesight.MaxLevelSource.{settings.Source}".Translate()))
        {
            Find.WindowStack.Add(new FloatMenu(
                Enum.GetValues(typeof(TruesightModSettings.MaxLevelSource))
              .Cast<TruesightModSettings.MaxLevelSource>()
              .Select(source => new FloatMenuOption(source.ToString(), () =>
              {
                  GetSettings<TruesightModSettings>().Source = source;
                  ApplySettings();
              }))
              .ToList()));
        }
        float textHeight = Text.CalcHeight("Truesight.MaxLevel.Custom".Translate(), listing.ColumnWidth / 2f);
        var rect = listing.GetRect(textHeight);
        Widgets.Label(rect.LeftHalf(), "Truesight.MaxLevel.Custom".Translate());
        rect.height = 30f;
        Widgets.IntEntry(rect.RightHalf(), ref settings.CustomLevel, ref TruesightModSettings.CustomLevelString);
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