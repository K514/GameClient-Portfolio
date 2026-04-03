using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class AnimatorAnimation
    {
        #region <Callbacks>

        /// <summary>
        /// Animator 컴포넌트의 play함수가 호출되기 직전에 호출되는 콜백
        /// </summary>
        private void OnAnimationPlayRequested()
        {
            var motionType = _ReservedClipPreset.MotionType;
            switch (motionType)
            {
                case AnimationTool.MotionType.MoveWalk:
                case AnimationTool.MotionType.MoveRun:
                {
                    // 이동 모션은 MoveSpeedRate만 적용
                    var moveSpeed = Entity[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.MoveSpeedRate, AnimationTool.MoveSpeedStatusAdaptFactor];
                    SetMotionSpeed(moveSpeed);   
                    break;
                }
                case AnimationTool.MotionType.Kick:
                case AnimationTool.MotionType.Punch:
                {
                    // 공격 모션은 AttackSpeedBasis * AttackSpeedRate 적용
                    var attackSpeed = Entity[StatusTool.BattleStatusGroupType.Total].GetScaledAttackSpeed() * AnimationTool.AttackSpeedStatusAdaptFactor;
                    SetMotionSpeed(attackSpeed);
                    break;   
                }
                default:
                {
                    SetMotionSpeed(1f);
                    break;
                }
            }
        }

        /// <summary>
        /// 애니메이션이 시작되는 경우 호출되는 콜백
        /// </summary>
        public override void OnAnimationStart()
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
            {
                CustomDebug.Log((this, $"Motion Started [{_ReservedClipPreset}]"));
            }
#endif
            // 진행중인 다른 모션이 없는 경우, 예약된 모션이 있는지 확인하고 갱신해준다.
            if (IsCurrentMotion(AnimationTool.MotionType.None))
            {
                if (IsReserveClipValid())
                {
                    _CurrentClipPreset = _ReservedClipPreset;
                    _ReservedClipPreset = default;
                    _OnProgressingFlag = _CurrentClipPreset.ValidFlag;
                }
            }
            // 다른 모션이 진행중이었던 경우, 예약된 모션이 있는지 확인하고 있다면 진행중인 모션을 취소하고 갱신해준다.
            else
            {
                if (IsReserveClipValid())
                {
                    OnAnimationEnd();
                    
                    _CurrentClipPreset = _ReservedClipPreset;
                    _ReservedClipPreset = default;
                    _OnProgressingFlag = _CurrentClipPreset.ValidFlag;
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
                    {
                        CustomDebug.Log((this, $"Motion Transition Detected [{_CurrentClipPreset}] => [{_ReservedClipPreset}]"));
                    }
#endif
                }
            }
        }

        public override void OnAnimationStop()
        {
            if (Entity.IsAvailableStopHitMotion())
            {
                SetAnimationPause();
            }
        }

        /// <summary>
        /// 애니메이션이 종료되는 경우 호출되는 콜백
        /// </summary>
        public override void OnAnimationEnd()
        {
            var currentMotionTypeOnAnimator = _CurrentClipPreset.MotionType;
            if (currentMotionTypeOnAnimator == AnimationTool.MotionType.None)
            {
                // 현재 모션이 초기화 되지 않은 경우
                return;
            }

            // 예약된 모션이 있는 경우
            if (IsReserveClipValid())
            {
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
                {
                    CustomDebug.Log((this, $"Motion Ended [{_CurrentClipPreset}] (reserve to [{_ReservedClipPreset}])"));
                }
#endif
                OnMotionOver(currentMotionTypeOnAnimator);
            }
            // 예약된 모션이 없는 경우
            else
            {
                // 루프 모션이었던 경우, 현재 모션 정보를 유지시키기 위해 현재 모션을 다시 예약시킨다.
                if (_CurrentClipPreset.Clip.isLooping)
                {
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
                    {
                        CustomDebug.Log((this, $"Motion Loop Detected [{_CurrentClipPreset}]"));
                    }
#endif
                    _ReservedClipPreset = _CurrentClipPreset;
                    SaveMotionAffine();
                    OnMotionOver(currentMotionTypeOnAnimator);
                }
                // 루프 모션이 아니었던 경우, 기본 모션을 재생시켜준다.
                else
                {
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
                    {
                        CustomDebug.Log((this, $"Motion Ended [{_CurrentClipPreset}]"));
                    }
#endif
                    OnMotionOver(currentMotionTypeOnAnimator);
                    SwitchToIdleMotion(AnimationTool.MotionTransitionType.Bypass_StateMachine);
                }
            }
        }
        
        /// <summary>
        /// 모션이 끝난 이후에 호출되는 콜백
        /// </summary>
        private void OnMotionOver(AnimationTool.MotionType p_Type)
        {
            _OnProgressingFlag = false;
            
            switch (p_Type)
            {
                default:
                    _CurrentClipPreset = default;
                    break;
                case AnimationTool.MotionType.Hit:
                    _CurrentClipPreset = default;
                    Entity.OnModule_HitMotion_Over();
                    break;
                case AnimationTool.MotionType.Dead:
                    Entity.ReservePooling();
                    break;
            }
        }
        
        public override void OnAnimatorMove()
        {
            var currentPlaceType = _CurrentClipPreset.MotionPlaceType;

            switch (currentPlaceType)
            {
                case AnimationTool.MotionPlaceType.None:
                    break;
                case AnimationTool.MotionPlaceType.InPlaceRootMotion:
                {
                    var deltaPos = _Animator.deltaPosition;
                    var deltaRot = _Animator.deltaRotation;
                    var targetTransform = Entity.Affine;
                    var deltaTime = Time.deltaTime;

                    targetTransform.forward = deltaRot * targetTransform.forward;
                    // _Entity.MoveTo(deltaPos, deltaTime);
                }
                    break;
                case AnimationTool.MotionPlaceType.InPlaceRootMotionExceptY:
                {
                    var deltaPos = _Animator.deltaPosition.XZVector();
                    var deltaRot = _Animator.deltaRotation;
                    var targetTransform = Entity.Affine;
                    var deltaTime = Time.deltaTime;

                    targetTransform.forward = deltaRot * targetTransform.forward;
                    // _Entity.MoveTo(deltaPos, deltaTime);
                }
                    break;
                case AnimationTool.MotionPlaceType.InPlaceRootMotionPositionOnly:
                {
                    var deltaPos = _Animator.deltaPosition;
                    var deltaTime = Time.deltaTime;
                    // _Entity.MoveTo(deltaPos, deltaTime);
                }
                    break;
                case AnimationTool.MotionPlaceType.InPlaceRootMotionRotationOnly:
                {
                    var deltaRot = _Animator.deltaRotation;
                    var targetTransform = Entity.Affine;
                    targetTransform.forward = deltaRot * targetTransform.forward;
                }
                    break;
            }
        }

        #endregion
    }
}