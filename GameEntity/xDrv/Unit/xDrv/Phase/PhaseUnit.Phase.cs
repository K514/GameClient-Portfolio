using System.Collections.Generic;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class PhaseUnit
    {
        #region <Fields>

        private List<UnitPhase> _PhaseList;
        private int _CurrentIndex;
        private UnitPhase _CurrentPhase;
        private ProgressTimer _PhaseDuration;
        private int _PhaseHitCount;
        private bool HasDuration => _CurrentPhase?.AttributeMask.HasAnyFlagExceptNone(UnitTool.UnitPhaseAttribute.HasDuration) ?? false;
        private bool HasHitCount => _CurrentPhase?.AttributeMask.HasAnyFlagExceptNone(UnitTool.UnitPhaseAttribute.HasHitCount) ?? false;
        private bool HasHpRate => _CurrentPhase?.AttributeMask.HasAnyFlagExceptNone(UnitTool.UnitPhaseAttribute.HasHpRate) ?? false;
        private bool HasSuccessEvent => _CurrentPhase?.AttributeMask.HasAnyFlagExceptNone(UnitTool.UnitPhaseAttribute.HasPhaseSuccessEvent) ?? false;
        private bool HasFailEvent => _CurrentPhase?.AttributeMask.HasAnyFlagExceptNone(UnitTool.UnitPhaseAttribute.HasPhaseFailEvent) ?? false;
        
        #endregion

        #region <Callbacks>

        private void OnCreatePhase()
        {
            _PhaseList = ComponentDataRecord.PhaseList;
        }

        private void OnActivatePhase()
        {
            _CurrentIndex = 0;
            UpdatePhase();
        }

        private void OnUpdatePhase(float p_DeltaTime)
        {
            if (ReferenceEquals(null, _CurrentPhase))
            {
                if (HasState_Only(GameEntityTool.EntityStateType.AllowStateGroupMask))
                {
                    UpdatePhase();
                }
            }
            else
            {
                if (HasDuration)
                {
                    if (_PhaseDuration.IsOver())
                    {
                        TerminatePhase(true);
                        return;
                    }
                    else
                    {
                        _PhaseDuration.Progress(p_DeltaTime);
                    }
                }

                if (HasHitCount)
                {
                    if (_PhaseHitCount >= _CurrentPhase[UnitTool.UnitPhaseTerminateConditionType.HitCount])
                    {
                        TerminatePhase(false);
                        return;
                    }
                }
                
                if (HasHpRate)
                {
                    if (GetCurrentStatusRate(BattleStatusTool.BattleStatusType.HP_Base) < _CurrentPhase[UnitTool.UnitPhaseTerminateConditionType.HpRate])
                    {
                        TerminatePhase(true);
                        return;
                    }
                }
            }
        }

        public override void OnStrikeEntity(IGameEntityBridge p_Entity, StatusTool.StatusChangeParams p_Params)
        {
            base.OnStrikeEntity(p_Entity, p_Params);

            if (p_Entity?.IsPlayer ?? false)
            {
                _PhaseHitCount++;
                Debug.LogError($"피격 횟수 {_PhaseHitCount}");
            }
        }
        
        #endregion

        #region <Methdos>

        private void ResetPhase()
        {
            _CurrentPhase = default;
            _PhaseDuration = default;
            _PhaseHitCount = default;
        }

        private void TerminatePhase(bool p_Success)
        {
            if (p_Success && HasSuccessEvent)
            {
                Debug.LogError($"페이즈 파훼 {_CurrentPhase.PhaseSuccessEventIndex}");
                
                // TriggerDeployEvent(_CurrentPhase.PhaseSuccessEventIndex); 
            }
            
            if (!p_Success && HasFailEvent)
            {
                Debug.LogError($"페이즈 실패 {_CurrentPhase.PhaseFailEventIndex}");
                
                // TriggerDeployEvent(_CurrentPhase.PhaseFailEventIndex); 
            }
            
            ResetPhase();
            
            _CurrentIndex++;
        }
        
        private void UpdatePhase()
        {
            if (_PhaseList.TryGetElementSafe(_CurrentIndex, out _CurrentPhase))
            {
                Debug.LogError($"페이즈 시작");

                if (HasDuration)
                {
                    _PhaseDuration = _CurrentPhase[UnitTool.UnitPhaseTerminateConditionType.Duration];
                    SwitchActionModule(_CurrentPhase.PhaseNormalActionModuleIndex);
                }
            }
        }

        #endregion
    }
}