using AEAssist.CombatRoutine;
using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using AEAssist.Extension;
using AEAssist.JobApi;
using static System.Windows.Forms.Design.AxImporter;
using System.CodeDom;
using LM.Reaper.Setting;
namespace LM.Reaper.SlotResolvers.GCD;

public class GCD_ShadowofDeath : ISlotResolver
{
    //In this slot resolver，we will use one of the following skills:
    // WhorlOfDeath
    // ShadowOfDeath Lv.35

    private Spell GetSpell()
    {   
        //if in aoe mode
        if (ReaperRotationEntry.QT.GetQt(QTKey.AOE))
        {
            var aoeCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 5, 5);
            
            //if there are more than 2 enemies around us and we are at least level 35
            if (aoeCount >= 2&& Core.Me.Level >= 35)
                return SpellsDefine.WhorlOfDeath.GetSpell();
        }

        if(ReaperBattleData.Instance.AutoDoubleEnshroud){
            //if the target will not have the DeathsDesign debuff in the next X seconds, we will use the ShadowOfDeath skill
            if (!Core.Me.GetCurrTarget().HasMyAuraWithTimeleft(AurasDefine.DeathsDesign, ReaperSettings.Instance.ShadowofDeath_time))
                return SpellsDefine.ShadowOfDeath.GetSpell();

            //if an ShadowOfDeath is not enough, then we do a VoidReaping
            if(ReaperBattleData.Instance.DoOneReaping){
                ReaperBattleData.Instance.DoOneReaping = false;
                return SpellsDefine.VoidReaping.GetSpell();
            }
                

            //The second usage of ShadowOfDeath,turn off the automatic use of ShadowOfDeath
            ReaperBattleData.Instance.AutoDoubleEnshroud = false;
            return SpellsDefine.ShadowOfDeath.GetSpell();
        }

        return SpellsDefine.ShadowOfDeath.GetSpell();
    }

    public int Check()
    {   
        //Level Check
        if (Core.Me.Level < 10)
            return -1;

        //Target touchable check  
        if (Core.Me.Distance(Core.Me.GetCurrTarget()) > SettingMgr.GetSetting<GeneralSettings>().AttackRange)
            return -2;
            
        //Buff confiction Check
        if (Core.Me.HasAura(AurasDefine.SoulReaver) || 
            Core.Me.HasAura(AurasDefine.Executioner))
            return -3;

        //AutoDoubleEnshroud Check
        if(ReaperBattleData.Instance.AutoDoubleEnshroud){
            return 1;
        }

        // In Double Enshroud Trigger
        if (ReaperSettings.Instance.DoubleEnshroud){
            if(SpellsDefine.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds < 10000){
                return -1;
            }       
        }

                
        if (!Core.Me.GetCurrTarget().HasMyAuraWithTimeleft(AurasDefine.DeathsDesign, ReaperSettings.Instance.ShadowofDeath_time))
            return 1;

        //Normally not use
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(GetSpell());
    }

}