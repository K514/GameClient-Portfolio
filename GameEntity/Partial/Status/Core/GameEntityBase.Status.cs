using xk514;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Callbacks>

        protected virtual void OnCreateStatus()
        {
            OnCreateName();
            OnCreateEnhance();
            OnCreateBaseStatus();
            OnCreateBattleStatus();
            OnCreateShotStatus();
            OnCreateGroup();
        }

        protected virtual void OnActivateStatus(ActivateParams p_ActivateParams)
        {
            OnActivateName(p_ActivateParams);
            OnActivateEnhance();
            OnActivateBaseStatus();
            OnActivateBattleStatus();
            OnActivateShotStatus();
            OnActivateGroup();
        }

        protected virtual void OnRetrieveStatus()
        {
            OnRetrieveGroup();
            OnRetrieveShotStatus();
            OnRetrieveBattleStatus();
            OnRetrieveBaseStatus();
            OnRetrieveEnhance();
            OnRetrieveName();
        }

        private void OnDisposeStatus()
        {
            OnDisposePortrait();
        }

        #endregion

        #region <Methods>

#if APPLY_PRINT_LOG
        public virtual void PrintStatusInfo()
        {
            CustomDebug.LogError($"** 이름 : {name}");
            CustomDebug.LogError($"** 레벨 : {_CurrentLevel}");
            CustomDebug.LogError("** 종합 스탯");
            CustomDebug.LogError(_BaseStatusTable[StatusTool.BaseStatusGroupType.Total].ToString());
            CustomDebug.LogError("** 기본 스탯");
            CustomDebug.LogError(_BaseStatusTable[StatusTool.BaseStatusGroupType.Main].ToString());
            CustomDebug.LogError("** 종합 전투 스탯");
            CustomDebug.LogError(_BattleStatusTable[StatusTool.BattleStatusGroupType.Total].ToString());
            CustomDebug.LogError("** 기본 전투 스탯");
            CustomDebug.LogError(_BattleStatusTable[StatusTool.BattleStatusGroupType.Main].ToString());
            CustomDebug.LogError("** 종합 샷 스탯");
            CustomDebug.LogError(_ShotStatusTable[StatusTool.ShotStatusGroupType.Total].ToString());
            CustomDebug.LogError("** 기본 샷 스탯");
            CustomDebug.LogError(_ShotStatusTable[StatusTool.ShotStatusGroupType.Main].ToString());
            CustomDebug.LogError($"** 레벨 [{_CurrentLevel}]");
            CustomDebug.LogError($"** 경험치 [{_CurrentExp} / {GetNeededExp()}]({100f * GetCurrentExpRate()}%)");
        }
#endif
        
        #endregion
    }
}