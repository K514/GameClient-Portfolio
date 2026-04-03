using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Consts>

        private const int _BattleStatusRegenCountPerSec = 10;
        private const float _BattleStatusRegenInterval = 1f / _BattleStatusRegenCountPerSec;

        #endregion

        #region <Fields>

        /// <summary>
        /// 전투 능력치 갱신 타이머
        /// </summary>
        private ProgressTimer _BattleStatusRegenTimer;

        #endregion
        
        #region <Callbacks>

        private void OnCreateBattleStatusRegen()
        {
            _BattleStatusRegenTimer = _BattleStatusRegenInterval;
        }
        
        private void OnActivateBattleStatusRegen()
        {
            _BattleStatusRegenTimer.Reset();
        }
        
        private void OnUpdateBattleStatusRegenerate(float p_DeltaTime)
        {
            if (IsAlive)
            {
                if (_BattleStatusRegenTimer.IsOver())
                {
                    _BattleStatusRegenTimer.Reset();
                
                    var hpRegenValue = this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.HP_Fix_Recovery, _BattleStatusRegenInterval];
                    switch (hpRegenValue)
                    {
                        case > CustomMath.Epsilon:
                        {
                            var hpRate = GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base);
                            if (!hpRate.IsReachedOne())
                            {
                                AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, hpRegenValue);
                            }
                            break;
                        }
                        case < CustomMath.NegativeEpsilon:
                        {
                            var hpRate = GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base);
                            if (!hpRate.IsReachedZero())
                            {
                                var currentHp = this[StatusTool.BattleStatusGroupType.Current][BattleStatusTool.BattleStatusType3.HP_Base];
                                // hp리젠으로 사망하면 안되므로 체력이 0이 남도록 리젠값을 보정해준다.
                                if (currentHp + hpRegenValue <= 0f)
                                {
                                    hpRegenValue = -currentHp;
                                }
                                
                                AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, hpRegenValue);
                            }
                            break;
                        }
                    }
     
                    var mpRegenValue = this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.MP_Fix_Recovery, _BattleStatusRegenInterval];
                    switch (mpRegenValue)
                    {
                        case > CustomMath.Epsilon:
                        {
                            var mpRate = GetCurrentStatusRate(BattleStatusTool.BattleStatusType.MP_Base);
                            if (!mpRate.IsReachedOne())
                            {
                                AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.MP_Base, mpRegenValue);
                            }
                            break;
                        }
                        case < CustomMath.Epsilon:
                        {
                            var mpRate = GetCurrentStatusRate(BattleStatusTool.BattleStatusType.MP_Base);
                            if (!mpRate.IsReachedZero())
                            {
                                AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.MP_Base, mpRegenValue);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    _BattleStatusRegenTimer.Progress(p_DeltaTime);
                }
            }
        }
        
        #endregion
    }
}