using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;
using UnityEngine.UI;

#if !SERVER_DRIVE

namespace k514.Mono.Feature
{
    public class UIxStatusPanel : UIxPanelBase
    {
        #region <Consts>

        private static readonly BattleStatusTool.BattleStatusType[] _CharacterInfoPropertyGroup
            = new[]
            {
                /* Col 0 */
                BattleStatusTool.BattleStatusType.Attack_Melee, 
                BattleStatusTool.BattleStatusType.HP_Base, 
                BattleStatusTool.BattleStatusType.HP_Fix_Recovery, 
                BattleStatusTool.BattleStatusType.AttackSpeedRate, 
                BattleStatusTool.BattleStatusType.CriticalRate_Melee, 

                /* Col 1 */
                BattleStatusTool.BattleStatusType.AntiDamageRate_Melee,
                BattleStatusTool.BattleStatusType.MP_Base,
                BattleStatusTool.BattleStatusType.MP_Fix_Recovery,
                BattleStatusTool.BattleStatusType.MoveSpeedRate,
                BattleStatusTool.BattleStatusType.Absorb,
            };
  
        #endregion
        
        #region <Fields>
        
        private Dictionary<BattleStatusTool.BattleStatusType, (Text, Text)> _PropertyTextElementTable;

        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            var index = 0;
            _PropertyTextElementTable = new Dictionary<BattleStatusTool.BattleStatusType, (Text, Text)>();
            {
                var (valid, statusCol0) = RectTransform.FindRecursive("StatusColumn0");
                if (valid)
                {
                    var propertyLabelSet = statusCol0.GetComponentsInChildren<Image>();
                    foreach (var propertyLabel in propertyLabelSet)
                    {
                        var propertyTextSet = propertyLabel.GetComponentsInChildren<Text>();
                        var tryBattleStatusType = _CharacterInfoPropertyGroup[index++];
                        var propertyName = propertyTextSet[0];
                        var propertyValue = propertyTextSet[1];
                        _PropertyTextElementTable.Add(tryBattleStatusType, (propertyName, propertyValue));
                        propertyName.text = tryBattleStatusType.GetPropertyName();
                    }
                }
            }
            {
                var (valid, statusCol0) = RectTransform.FindRecursive("StatusColumn1");
                if (valid)
                {
                    var propertyLabelSet = statusCol0.GetComponentsInChildren<Image>();
                    foreach (var propertyLabel in propertyLabelSet)
                    {
                        var propertyTextSet = propertyLabel.GetComponentsInChildren<Text>();
                        var tryBattleStatusType = _CharacterInfoPropertyGroup[index++];
                        var propertyName = propertyTextSet[0];
                        var propertyValue = propertyTextSet[1];
                        _PropertyTextElementTable.Add(tryBattleStatusType, (propertyName, propertyValue));
                        propertyName.text = tryBattleStatusType.GetPropertyName();
                    }
                }
            } 
        }

        #endregion

        #region <Methods>

        public void UpdateStatusInfo(BattleStatusTool.BattleStatusType p_Type, BattleStatusPreset p_Status)
        {
            foreach (var battleStatusType in _CharacterInfoPropertyGroup)
            {
                if (p_Type.HasAnyFlagExceptNone(battleStatusType))
                {
                    _PropertyTextElementTable[battleStatusType].Item2.text = p_Status.GetPropertyText(battleStatusType);
                }
            }
        }
        
        public void UpdateStatusInfo(BattleStatusPreset p_Status)
        {
            foreach (var battleStatusType in _CharacterInfoPropertyGroup)
            {
                _PropertyTextElementTable[battleStatusType].Item2.text = p_Status.GetPropertyText(battleStatusType);
            }
        }

        #endregion
    }
}
#endif