using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 현재 레벨 값
        /// </summary>
        protected int _CurrentLevel;

        /// <summary>
        /// 최대 레벨 값
        /// </summary>
        private int _MaxLevel => _EnhanceRecord.MaxLevel;
        
        #endregion

        #region <Callbacks>

        private void OnCreateLevel()
        {
        }
        
        private void OnActivateLevel()
        {
            SetLevel(ComponentDataRecord.Level);
        }

        private void OnRetrieveLevel()
        {
        }

        private void OnLevelChanged(int p_PrevLevel, int p_TargetLevel)
        {
            var levelUpBonusRate = _EnhanceRecord.GetLevelUpStatusBonusRateDelta(p_TargetLevel - p_PrevLevel);
            AddStatus(StatusTool.BattleStatusGroupType.CompoundMul, levelUpBonusRate * BattleStatusTool.LevelUpBonusBattleStatusPreset);
            
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Level_Change, default);
        }
        
        #endregion

        #region <Methods>

        public bool IsLevelCounterStop()
        {
            return _CurrentLevel >= _MaxLevel;
        }
        
        public void SetLevel(int p_Level)
        {
            p_Level = Mathf.Clamp(p_Level, 1, _MaxLevel);

            var prevLevel = _CurrentLevel;
            _CurrentLevel = p_Level;

            OnLevelChanged(prevLevel, _CurrentLevel);
        }
        
        public void AddLevel(int p_Value)
        {
            SetLevel(_CurrentLevel + p_Value);
        }

        public int GetLevel()
        {
            return _CurrentLevel;
        }

        #endregion
    }
}