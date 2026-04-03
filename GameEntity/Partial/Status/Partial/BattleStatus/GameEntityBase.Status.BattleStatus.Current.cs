using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>
        
        /// <summary>
        /// 현재 값을 가지는 전투능력치 반복자
        /// </summary>
        private List<BattleStatusTool.BattleStatusType> _CurrentStatusTypeEnumerator;

        /// <summary>
        /// 현재 값을 가지는 전투능력치 타입마스크
        /// </summary>
        private BattleStatusTool.BattleStatusFlag _CurrentStatusTypeFlagMask;

        #endregion

        #region <Callbacks>

        private void OnCreateBattleStatusCurrent()
        {
            _CurrentStatusTypeEnumerator = new List<BattleStatusTool.BattleStatusType>();
            
            SetCurrentValueType(BattleStatusTool.BattleStatusType.HP_Base);
            SetCurrentValueType(BattleStatusTool.BattleStatusType.MP_Base);
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 지정한 타입 능력치의 현재값을 추적할 수 있도록 표시하는 메서드
        /// </summary>
        private void SetCurrentValueType(BattleStatusTool.BattleStatusType p_Type)
        {
            if (!_CurrentStatusTypeFlagMask.HasFlag(p_Type))
            {
                _CurrentStatusTypeFlagMask.AddFlag(p_Type);
                _CurrentStatusTypeEnumerator.Add(p_Type);

                _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetProperty(p_Type, _BattleStatusTable[StatusTool.BattleStatusGroupType.Total].GetProperty(p_Type));
            }
        }

        /// <summary>
        /// 현재값을 추적을 해제하는 메서드
        /// </summary>
        private void RemoveCurrentValueType(BattleStatusTool.BattleStatusType p_Type)
        {
            if (_CurrentStatusTypeFlagMask.HasFlag(p_Type))
            {
                _CurrentStatusTypeFlagMask.RemoveFlag(p_Type);
                _CurrentStatusTypeEnumerator.Remove(p_Type);
                
                _BattleStatusTable[StatusTool.BattleStatusGroupType.Current].SetProperty(p_Type, BattleStatusTool.BasisBattleStatusPreset.GetProperty(p_Type));
            }
        }

        public void SyncCurrentStatusToTotal()
        {
            foreach (var battleStatusType in _CurrentStatusTypeEnumerator)
            {
                AddStatusRate(StatusTool.BattleStatusGroupType.Current, battleStatusType, 1f);
            }
        }

        #endregion
    }
}