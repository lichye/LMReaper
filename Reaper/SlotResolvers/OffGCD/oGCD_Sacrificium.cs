﻿using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using AEAssist.Extension;
using static System.Windows.Forms.Design.AxImporter;
using AEAssist.JobApi;
using LM.Reaper.Setting;

namespace LM.Reaper.SlotResolvers.OffGCD;

public class oGCD_Sacrificium : ISlotResolver
{
    public int Check()
    {
        // if we can't use GCD, return -2
        if (GCDHelper.GetGCDCooldown() < ReaperSettings.Instance.AnimationLock)
            return -2;
        // if we don't have Oblatio, return -5
        if (!Core.Me.HasAura(AurasDefine.Oblatio))
            return -5;
        // if we don't have Sacrificium, return -1
        if (!SpellsDefine.Sacrificium.IsReady())
            return -1;

        // if we don't have Deaths Design, return -3
        if (!Core.Me.GetCurrTarget().HasAura(AurasDefine.DeathsDesign))
            return -3;
        
        // if we are near the ArcaneCircle, keep for the ArcaneCircle
        if (SpellsDefine.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds < 10000)
            return -4;

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.Sacrificium.GetSpell());
    }
}