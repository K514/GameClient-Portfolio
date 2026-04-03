using System.Linq;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityExtraOptionStorage
    {
        #region <Callbacks>

        private void OnCreateOptionC()
        {
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.ManaStoneGamble, new ManaStoneGamble());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.GoldGamble, new GoldGamble());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.SkillGamble, new SkillGamble());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.StatusGamble, new StatusGamble());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.ConsumeGamble, new ConsumeGamble());
        }

        #endregion

        #region <Classess>
     
        public class ManaStoneGamble : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var cost = p_Handler.CommonParams.Count;
                var randomGem = Random.Range(0, 2 * cost);
                
                GameManager.GetInstanceUnsafe.AddGold(randomGem);
                
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, 2 * p_Preset.Count);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class GoldGamble : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var cost = p_Handler.CommonParams.Count;
                var randomGold = Random.Range(0, 2 * cost);
                
                // AccountManager.GetInstanceUnsafe.AddGold(randomGold, false);
      
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, 2 * p_Preset.Count);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class SkillGamble : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var actionModule = caster.ActionModule;
                // var passiveTable = actionModule.GetPassiveActionTable();
                // var passiveCount = passiveTable.Count;
                
                /*switch (passiveCount)
                {
                    case 0 :
                        break;
                    case 1:
                    {
                        var randomAddPassiveIndex = passiveTable.GetRandomElement().Key;
                        actionModule.BindAction(randomAddPassiveIndex);
              
                        break;
                    }
                    default:
                    {
                        var randomSubtractPassiveIndex = passiveTable.GetRandomElement().Key;
                        actionModule.ReleaseAction(randomSubtractPassiveIndex);

                        var randomAddPassiveIndex = passiveTable.GetRandomElement().Key;
                        actionModule.BindAction(new ActionTool.ActionBindPreset(randomAddPassiveIndex, 2));
           
                        break;
                    }
                }*/
                
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class StatusGamble : GameEntityExtraOptionBase
        {
            private readonly BattleStatusTool.BattleStatusType[]
                _RandomizeStatusTypeSet
                    = new[]
                    {
                        BattleStatusTool.BattleStatusType.HP_Base, 
                        BattleStatusTool.BattleStatusType.MP_Base,
                        BattleStatusTool.BattleStatusType.Attack_Melee,
                        BattleStatusTool.BattleStatusType.CriticalRate_Melee,
                        BattleStatusTool.BattleStatusType.MoveSpeedRate,
                        BattleStatusTool.BattleStatusType.AntiDamageRate
                    };
            
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var randomSubtractStatusType = _RandomizeStatusTypeSet.GetRandomElement();
                var randomAddStatusType = _RandomizeStatusTypeSet.GetRandomElement();
                
                caster.AddStatus(StatusTool.BattleStatusGroupType.SimpleMul, new BattleStatusPreset(randomSubtractStatusType, -0.1f));
                caster.AddStatus(StatusTool.BattleStatusGroupType.SimpleMul, new BattleStatusPreset(randomAddStatusType, 0.2f));

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class ConsumeGamble : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var inventory = caster.GetInventory();
                var slotCount = inventory.Count;
                
                switch (slotCount)
                {
                    case 0 :
                        break;
                    case 1:
                    {
                        var slot = inventory.First().Value;
                        slot.AddNumber(1);
         
                        break;
                    }
                    default:
                    {
                        var randomSubtractItemSlot = inventory.GetRandomElement().Value;
                        var randomAddItemSlot = inventory.GetRandomElement().Value;
                
                        randomSubtractItemSlot.AddNumber(-1);
                        randomAddItemSlot.AddNumber(2);
     
                        break;
                    }
                }
                
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
       
        #endregion
    }
}