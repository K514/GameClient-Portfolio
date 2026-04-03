using System;
using k514.Mono.Feature;
using UnityEngine;
using Random = UnityEngine.Random;

namespace k514.Mono.Common
{
    public partial class GameEntityExtraOptionStorage
    {
        #region <Callbacks>

        private void OnCreateOptionA()
        {
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.None, new NotImplemented());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddAdditiveBattleStatus, new AddAdditiveBattleStatus());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddSimpleMultiplyBattleStatus, new AddSimpleMultiplyBattleStatus());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddCompoundMultiplyBattleStatus, new AddCompoundMultiplyBattleStatus());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddAdditiveShotStatus, new AddAdditiveShotStatus());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddSimpleMultiplyShotStatus, new AddSimpleMultiplyShotStatus());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddCompoundMultiplyShotStatus, new AddCompoundMultiplyShotStatus());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddActionLevel, new AddActionLevel());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.CastEnchantToSelf, new CastEnchantToSelf());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.CastEnchantToHit, new CastEnchantToHit()); 
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.CastEnchantToStrike, new CastEnchantToStrike());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.UseItemWhenStartStage, new UseItemWhenStartStage());
        }

        #endregion

        #region <Classess>
      
        public class NotImplemented : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
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
        
        public class AddAdditiveBattleStatus : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var battleStatus = record.BattleStatusPreset + (param.Count - 1) * record.BattleStatusPreset2;
                p_Handler.AddAdditiveStatus(battleStatus);

                return true;
            }

            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var type = p_Record.BattleStatusType;
                    var battleStatus = p_Record.BattleStatusPreset + (p_Params.Count - 1) * p_Record.BattleStatusPreset2;
                    var value = battleStatus[type];
                    
                    if (value < 0f)
                    {
                        return string.Format(o_Record.Description2, type.GetPropertyName(), battleStatus.GetPropertyText(type, -1f));
                    }
                    else
                    {
                        return string.Format(o_Record.Description1, type.GetPropertyName(), battleStatus.GetPropertyText(type));
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class AddSimpleMultiplyBattleStatus : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var battleStatus = record.BattleStatusPreset + (param.Count - 1) * record.BattleStatusPreset2;
                p_Handler.AddSimpleMultiplyStatus(battleStatus);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var type = p_Record.BattleStatusType;
                    var battleStatus = p_Record.BattleStatusPreset + (p_Params.Count - 1) * p_Record.BattleStatusPreset2;
                    var value = 100f * battleStatus[type];
  
                    if (value < 0f)
                    {
                        return string.Format(o_Record.Description2, type.GetPropertyName(), -value);
                    }
                    else
                    {
                        return string.Format(o_Record.Description1, type.GetPropertyName(), value);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class AddCompoundMultiplyBattleStatus : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var battleStatus = record.BattleStatusPreset + (param.Count - 1) * record.BattleStatusPreset2;
                p_Handler.AddCompoundMultiplyStatus(battleStatus);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var type = p_Record.BattleStatusType;
                    var battleStatus = p_Record.BattleStatusPreset + (p_Params.Count - 1) * p_Record.BattleStatusPreset2;
                    var value = battleStatus[type];
                    
                    return string.Format(o_Record.Description1, type.GetPropertyName(), 1f + value);
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class AddAdditiveShotStatus : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var shotStatus = record.ShotStatusPreset + (param.Count - 1) * record.ShotStatusPreset2;
                p_Handler.AddAdditiveStatus(shotStatus);

                return true;
            }

            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var type = p_Record.ShotStatusType;
                    var shotStatus = p_Record.ShotStatusPreset + (p_Params.Count - 1) * p_Record.ShotStatusPreset2;
                    var value = shotStatus[type];
                    
                    if (value < 0f)
                    {
                        return string.Format(o_Record.Description2, type.GetPropertyName(), shotStatus.GetPropertyText(type, -1f));
                    }
                    else
                    {
                        return string.Format(o_Record.Description1, type.GetPropertyName(), shotStatus.GetPropertyText(type));
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class AddSimpleMultiplyShotStatus : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var shotStatus = record.ShotStatusPreset + (param.Count - 1) * record.ShotStatusPreset2;
                p_Handler.AddSimpleMultiplyStatus(shotStatus);
                
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var type = p_Record.ShotStatusType;
                    var shotStatus = p_Record.ShotStatusPreset + (p_Params.Count - 1) * p_Record.ShotStatusPreset2;
                    var value = 100f * shotStatus[type];
                    
                    if (value < 0f)
                    {
                        return string.Format(o_Record.Description2, type.GetPropertyName(), -value);
                    }
                    else
                    {
                        return string.Format(o_Record.Description1, type.GetPropertyName(), value);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class AddCompoundMultiplyShotStatus : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var shotStatus = record.ShotStatusPreset + (param.Count - 1) * record.ShotStatusPreset2;
                p_Handler.AddCompoundMultiplyStatus(shotStatus);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var type = p_Record.ShotStatusType;
                    var shotStatus = p_Record.ShotStatusPreset + (p_Params.Count - 1) * p_Record.ShotStatusPreset2;
                    var value = shotStatus[type];
                    
                    return string.Format(o_Record.Description1, type.GetPropertyName(), 1f + value);
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class AddActionLevel : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var actionIndex = record.Index;
                var addCount = record.Count;
                p_Handler.AddLeveling(actionIndex, addCount);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                /*if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, SkillDataTableQuery.GetInstanceUnsafe.GetRecord(p_Record.Index).GetActionLanguageRecord().Text, p_Record.Count);
                }
                else*/
                {
                    return string.Empty;
                }
            }
        }  
        
        public class CastEnchantToSelf : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                if (Random.value <= p_Handler.Record.Value)
                {
                    var caster = p_Handler.Caster;
                    var record = p_Handler.Record;
                    var enchantIndex = record.Index;
                    // caster.Enchant(enchantIndex, new GameEntityEventCommonParams(caster));

                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var enchantIndex = p_Record.Index;
                    var enchantRecord = EnchantDataTableQuery.GetInstanceUnsafe.GetRecord(enchantIndex);

                    return enchantRecord.CacheDescription;
                    //return string.Format(o_Record.Description, enchantRecord.CacheDescription);
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class CastEnchantToHit : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                if (Random.value <= p_Handler.Record.Value)
                {
                    p_Handler.AddEvent
                    (
                        GameEntityTool.GameEntityBaseEventType.Strike,
                        (handler, baseEventParam, extraOptionParam) =>
                        {
                            if (baseEventParam.StatusChangeParams.EventType == StatusTool.StatusChangeEventType.Combat)
                            {
                                // baseEventParam.Target.Enchant(handler.Record.Index, new GameEntityEventCommonParams(handler.Caster));
                            }
                        }
                    );
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var enchantIndex = p_Record.Index;
                    var enchantRecord = EnchantDataTableQuery.GetInstanceUnsafe.GetRecord(enchantIndex);

                    return enchantRecord.CacheDescription;
                   //return string.Format(o_Record.Description, enchantRecord.CacheDescription);
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class CastEnchantToStrike : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                if (Random.value <= p_Handler.Record.Value)
                {
                    p_Handler.AddEvent
                    (
                       GameEntityTool.GameEntityBaseEventType.Hit,
                       (handler, baseEventParam, extraOptionParam) =>
                       {
                           if (baseEventParam.StatusChangeParams.EventType == StatusTool.StatusChangeEventType.Combat)
                           {
                               // baseEventParam.Trigger?.Enchant(handler.Record.Index, new GameEntityEventCommonParams(handler.Caster));
                           }
                       }
                    );

                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var enchantIndex = p_Record.Index;
                    var enchantRecord = EnchantDataTableQuery.GetInstanceUnsafe.GetRecord(enchantIndex);

                    return enchantRecord.CacheDescription;
                    //return string.Format(o_Record.Description, enchantRecord.Duration, enchantRecord.CacheDescription);
                }
                else
                {
                    return string.Empty;
                }
            }
        }  
        
        public class UseItemWhenStartStage : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, ItemLanguageDataTable.GetInstanceUnsafe.GetRecord(p_Record.Index));
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