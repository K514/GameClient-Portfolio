using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityExtraOptionStorage
    {
        #region <Callbacks>

        private void OnCreateOptionB()
        {
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.HpRecoveryFix, new HpRecoveryFix());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.HpRecoveryRate, new HpRecoveryRate());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.MpRecoveryFix, new MpRecoveryFix());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.MpRecoveryRate, new MpRecoveryRate());
        }

        #endregion

        #region <Classess>

        public class HpRecoveryFix : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var value = record.Value;
                var value2 = record.Value2 * caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.HP_Base];
                var heal = Mathf.Max(value, value2);
                caster.HealHP(heal);

                return true;
            }

            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, p_Record.Value, 100f * p_Record.Value2);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class HpRecoveryRate : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var value = record.Value;
                caster.HealRateHP(value);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, p_Record.Value);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class MpRecoveryFix : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var value = record.Value;
                caster.HealMP(value);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, p_Record.Value);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class MpRecoveryRate : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                var value = record.Value;
                caster.HealRateMP(value);

                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, p_Record.Value);
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