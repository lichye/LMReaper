using AEAssist.CombatRoutine;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using AEAssist.Extension;

namespace LM.Reaper.SlotResolvers.GCD;

public class GCD_PlentifulHarvest : ISlotResolver
{
    
    public int Check()
    {
        //Level Check
        if (Core.Me.Level < 88)
            return -1;
        
        //Buff confiction Check
        if (Core.Me.HasAura(AurasDefine.SoulReaver) || 
            Core.Me.HasAura(AurasDefine.Executioner)||
            !Core.Me.HasAura(AurasDefine.ImmortalSacrifice) ||
            Core.Me.HasAura(AurasDefine.Enshrouded)||
            Core.Me.HasMyAuraWithTimeleft(AurasDefine.BloodsownCircle, 1500)
            )
            return -2;

        // if (Core.Me.HasMyAuraWithTimeleft(AurasDefine.ArcaneCircle, 16000))
        //     return 1;

        //Target Distance Check
        if (Core.Me.Distance(Core.Me.GetCurrTarget()) > 15)
            return -4;

        return 0;
    }
    
    public void Build(Slot slot)
    {
        slot.Add(SpellsDefine.PlentifulHarvest.GetSpell());
    }
}