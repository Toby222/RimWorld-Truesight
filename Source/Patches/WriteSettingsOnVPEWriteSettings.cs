using System;
using HarmonyLib;

namespace Truesight.Patches;

[HarmonyPatch(
    typeof(VanillaPsycastsExpanded.PsycastsMod),
    nameof(VanillaPsycastsExpanded.PsycastsMod.WriteSettings)
)]
public static class WriteSettingsOnVPEWriteSettings
{
    public static Delegate ApplySettingsDelegate;

    [HarmonyPostfix]
    public static void Postfix()
    {
        ApplySettingsDelegate?.DynamicInvoke();
    }
}
