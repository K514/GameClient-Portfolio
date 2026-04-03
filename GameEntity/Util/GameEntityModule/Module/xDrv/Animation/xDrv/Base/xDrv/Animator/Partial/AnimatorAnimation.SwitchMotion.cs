using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class AnimatorAnimation
    {
        public override void SwitchToIdleMotion(AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
            switch (_IdleMode)
            {
                case var _ when _MoveMode == AnimationTool.MoveMotionType.Walk:
                    SwitchMotion(AnimationTool.MotionType.MoveWalk, p_MotionTransitionType);
                    break;
                case var _ when _MoveMode == AnimationTool.MoveMotionType.Run:
                    SwitchMotion(AnimationTool.MotionType.MoveRun, p_MotionTransitionType);
                    break;
                case AnimationTool.IdleMotionType.Relax:
                    SwitchMotion(AnimationTool.MotionType.IdleRelax, p_MotionTransitionType);
                    break;
                case AnimationTool.IdleMotionType.Combat:
                    SwitchMotion(AnimationTool.MotionType.IdleCombat, p_MotionTransitionType);
                    break;
            }
        }
                
        public override void SwitchToMoveMotion(AnimationTool.MotionTransitionType p_MotionTransitionType)
        {
            switch (_MoveMode)
            {
                case AnimationTool.MoveMotionType.Idle when _IdleMode == AnimationTool.IdleMotionType.Relax:
                    SwitchMotion(AnimationTool.MotionType.IdleRelax, p_MotionTransitionType);
                    break;
                case AnimationTool.MoveMotionType.Idle when _IdleMode == AnimationTool.IdleMotionType.Combat:
                    SwitchMotion(AnimationTool.MotionType.IdleCombat, p_MotionTransitionType);
                    break;
                case AnimationTool.MoveMotionType.Walk:
                    SwitchMotion(AnimationTool.MotionType.MoveWalk, p_MotionTransitionType);
                    break;
                case AnimationTool.MoveMotionType.Run:
                    SwitchMotion(AnimationTool.MotionType.MoveRun, p_MotionTransitionType);
                    break;
            }
        }

        /// <summary>
        /// 지정한 타입의 애니메이션을 재생시킨다. 새로운 모션이 실행되거나, 혹은 현재 애니메이션과 동일한 타입의 애니메이션이 선정된 경우 true를 리턴한다.
        /// </summary>
        public override bool SwitchMotion(AnimationTool.MotionType p_Type, AnimationTool.MotionTransitionType p_TransitionFlag)
        {
            if (_AnimatorPreset.TryGetRandomMotionIndex(p_Type, out var o_Index))
            {
                return SwitchMotion(p_Type, o_Index, p_TransitionFlag);
            }
            else
            {
                return false;
            }
        }
         
        /// <summary>
        /// 지정한 타입의 애니메이션을 재생시킨다. 새로운 모션이 실행되거나, 혹은 현재 애니메이션과 동일한 타입의 애니메이션이 선정된 경우 true를 리턴한다.
        /// 2번째 파라미터에 의해 여러 애니메이션 중 지정한 인덱스의 애니메이션을 재생할 수 있다.
        /// 3번째 파라미터로 전이 타입을 정할 수 있다.
        /// </summary>
        public override bool SwitchMotion(AnimationTool.MotionType p_Type, int p_Index, AnimationTool.MotionTransitionType p_TransitionFlag)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
            {
                CustomDebug.Log((this, $"[{Entity.GetName()}] : [{p_Type} / {p_Index} / {p_TransitionFlag}] Req (current : {_CurrentClipPreset.MotionType})"));
            }
#endif
            // 현재 진행중인 모션 정보를 지워준다.
            if (p_TransitionFlag.HasAnyFlagExceptNone(AnimationTool.MotionTransitionType.Restrict | AnimationTool.MotionTransitionType.ErasePrevMotion))
            {
                _CurrentClipPreset = default;
            }

            var targetMotion = GetValidTargetMotion(p_Type);
            
            // 1. 타겟 모션을 보유한 경우
            if (_AnimatorPreset.HasMotion(targetMotion))
            {
                var isEnterable = false;
                // 2. 현재 재생중인 모션이 없는 경우
                if (IsPlayedNoneMotion())
                {
                    // p_TransitionFlag에 따라 가능한 전이인지 체크한다.
                    isEnterable = IsAvailableTransition(p_TransitionFlag, _CurrentClipPreset.MotionType, targetMotion, p_Index);
                }
                else
                {
                    // 3-1. 예약된 모션이 존재하는 경우
                    if (IsReserveClipValid())
                    {
                        // 예약된 모션을 임시로 현재 모션으로 가정하고, 타겟 모션에 진입할 수 있는지 검증한다.
                        var cachedCurrentPreset = _CurrentClipPreset;
                        _CurrentClipPreset = _ReservedClipPreset;
                        isEnterable = IsAvailableTransition(p_TransitionFlag, _CurrentClipPreset.MotionType, targetMotion, p_Index);
                        
                        // 진입할 수 없었던 경우, 현재 모션을 원래대로 돌려놓는다.
                        if (!isEnterable)
                        {
                            _CurrentClipPreset = cachedCurrentPreset;
                        }
                    }
                    // 3-2. 예약된 모션이 없는 경우
                    else
                    {
                        // 전이 타입 p_TransitionFlag에 따라 가능한 전이인지 체크한다.
                        isEnterable = IsAvailableTransition(p_TransitionFlag, _CurrentClipPreset.MotionType, targetMotion, p_Index);
                    }
                }

                // 4. 진입 가능한 모션인 경우
                if(isEnterable)
                {
                    var motionTable = _AnimatorPreset.MotionTable;
                    var clipList = motionTable[targetMotion];
                    
                    _ReservedClipPreset = clipList[Mathf.Clamp(p_Index, 0, clipList.Count - 1)];
                    
                    LoadMotionAffine();
                    SaveMotionAffine();
                    SetAnimationResume();
                    OnAnimationPlayRequested();
                    
                    _Animator.Play(_ReservedClipPreset.ClipName, -1, 0f);

                    // TODO<K514>
                    // CrossFade 사용시에, Fade되는 특이점의 위치에 따라서
                    // OnAnimationStart 및 OnAnimationTerminate가 호출되지 않는 경우가 있어서
                    // 모션의 시작과 끝 이벤트를 전달받지 못하고 로직에서 애니메이션을 제어할 수 없는 경우가 생긴다.
                    // _Animator.CrossFade(targetAnimationClip.name, 0.05f, 0);
                            
                    return true;
                }
            }
            
            return _CurrentClipPreset.MotionType == targetMotion;

            #region <Nest>

            /// <summary>
            /// 전이하려는 모션을 현재 개체 상태에 맞게 변환하는 메서드
            /// </summary>
            AnimationTool.MotionType GetValidTargetMotion(AnimationTool.MotionType p_Type)
            {
                switch (Entity)
                {
                    // 비활성 상태에서
                    case var _ when Entity.IsDisable :
                    {
                        return AnimationTool.MotionType.None;
                    }
                    // 사망 상태에서
                    case var _ when Entity.IsDead :
                    {
                        return AnimationTool.MotionType.Dead;
                    }
                    // 경직 상태에서
                    case var _ when Entity.IsStuck :
                    {
                        switch (p_Type)
                        {
                            case AnimationTool.MotionType.Dead:
                                break;
                            default:
                                return AnimationTool.MotionType.Hit;
                        }
                        break;
                    }
                    // 그로기 상태에서
                    case var _ when Entity.IsGroggy :
                    {
                        switch (p_Type)
                        {
                            case AnimationTool.MotionType.Dead:
                                break;
                            default:
                                return AnimationTool.MotionType.Groggy;
                        }
                        break;
                    }
                    // 부유 상태에서
                    case var _ when Entity.IsFloat :
                    {
                        switch (p_Type)
                        {
                            case AnimationTool.MotionType.IdleRelax:
                            case AnimationTool.MotionType.IdleCombat:
                            case AnimationTool.MotionType.MoveWalk:
                            case AnimationTool.MotionType.MoveRun:
                            case AnimationTool.MotionType.JumpDown:
                                return AnimationTool.MotionType.IdleAerial;
                        }
                        break;
                    }
                }
 
                return p_Type;
            }
 
            /// <summary>
            /// 지정한 전이타입에 따라 현재 애니메이션이 지정한 모션의 지정한 인덱스로 전이할 수 있는지 체크하는 논리 메서드
            /// </summary>
            bool IsAvailableTransition(AnimationTool.MotionTransitionType p_TransitionFlag, AnimationTool.MotionType p_FromMotion, AnimationTool.MotionType p_Type, int p_Index)
            {
                var hasAndMask = p_TransitionFlag.HasAnyFlagExceptNone(AnimationTool.MotionTransitionType.AndMask);
                foreach (var motionTransitionType in AnimationTool._MotionTransitionType_Enumerator)
                {
                    if (p_TransitionFlag.HasAnyFlagExceptNone(motionTransitionType))
                    {
                        var result = hasAndMask;
                        switch (motionTransitionType)
                        {
                            case AnimationTool.MotionTransitionType.Bypass_StateMachine:
                                result = _IsAvailableTransition(p_FromMotion, p_Type, p_Index);
                                break;
                            case AnimationTool.MotionTransitionType.Bypass_InverseStateMachine:
                                result = !_IsAvailableTransition(p_FromMotion, p_Type, p_Index);
                                break;
                            case AnimationTool.MotionTransitionType.Restrict:
                                result = true;
                                break;
                            case AnimationTool.MotionTransitionType.WhenSameMotion:
                                result = _CurrentClipPreset.MotionType == p_Type;
                                break;
                            case AnimationTool.MotionTransitionType.WhenDifferentMotion:
                                result = _CurrentClipPreset.MotionType != p_Type;
                                break;
                        }

                        if (hasAndMask != result)
                        {
                            return result;
                        }
                    }
                }
            
                return hasAndMask;
            }
            
            /// <summary>
            /// 어떤 모션에서 다른 모션으로 전이가 가능한지에 대한 여부를 리턴하는 메서드
            /// </summary>
            bool _IsAvailableTransition(AnimationTool.MotionType p_FromMotion, AnimationTool.MotionType p_ToMotion, int p_Index)
            {
                switch (p_FromMotion)
                {
                    // None 상태는 어떤 모션으로도 전이할 수 있다.
                    case AnimationTool.MotionType.None:
                    {
                        return true;
                    }
                    // 그로기 모션
                    case AnimationTool.MotionType.Groggy:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.Dead:
                                return true;
                            default :
                                return !Entity.IsGroggy;
                        }
                    }
                    // 대기 모션
                    case AnimationTool.MotionType.IdleRelax:
                    case AnimationTool.MotionType.IdleCombat:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.IdleRelax:
                            case AnimationTool.MotionType.IdleCombat:
                                return p_FromMotion != p_ToMotion;
                            case AnimationTool.MotionType.JumpUp:
                                return PhysicsModule.Current_Y_VelocityType == CustomMath.Significant.Plus;
                            case AnimationTool.MotionType.JumpDown:
                                return false;
                            default:
                                return true;
                        }
                    }
                    case AnimationTool.MotionType.IdleAerial:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.JumpUp:
                                return PhysicsModule.Current_Y_VelocityType == CustomMath.Significant.Plus;
                            default:
                                return true;
                        }
                    }
                    // 이동 모션
                    case AnimationTool.MotionType.MoveWalk:
                    case AnimationTool.MotionType.MoveRun:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.MoveWalk:
                            case AnimationTool.MotionType.MoveRun:
                                return p_FromMotion != p_ToMotion;
                            case AnimationTool.MotionType.JumpUp:
                                return PhysicsModule.Current_Y_VelocityType == CustomMath.Significant.Plus;
                            case AnimationTool.MotionType.JumpDown:
                                return false;
                            default:
                                return true;
                        }
                    }
                    // 점프 모션
                    case AnimationTool.MotionType.JumpUp:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.MoveWalk:
                            case AnimationTool.MotionType.MoveRun:
                                return false;
                            default:
                                return true;
                        }
                    }
                    // 착지 모션
                    case AnimationTool.MotionType.JumpDown:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.JumpUp:
                            case AnimationTool.MotionType.Punch:
                            case AnimationTool.MotionType.Kick:
                            case AnimationTool.MotionType.Cast:
                            case AnimationTool.MotionType.Dash:
                            case AnimationTool.MotionType.Interact:
                            case AnimationTool.MotionType.Groggy:
                            case AnimationTool.MotionType.Hit:
                            case AnimationTool.MotionType.Dead:
                                return true;
                            default :
                                return false;
                        }
                    }
                    // 공격 모션
                    case AnimationTool.MotionType.Punch:
                    case AnimationTool.MotionType.Kick:
                    case AnimationTool.MotionType.Cast:
                    case AnimationTool.MotionType.Dash:
                    case AnimationTool.MotionType.Interact:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.MoveWalk:
                            case AnimationTool.MotionType.MoveRun:
                                return false;
                            default:
                                return true;
                        }
                    }
                    // 피격 모션
                    case AnimationTool.MotionType.Hit:
                    {
                        switch (p_ToMotion)
                        {
                            case AnimationTool.MotionType.Hit:
                                return _CurrentClipPreset.MotionIndex != p_Index;
                            default:
                                return true;
                        }
                    }
                    // 사망 모션에서는 어떤 모션으로도 전이할 수 없다.
                    case AnimationTool.MotionType.Dead:
                    {
                        return false;
                    }
                    // 정의되지 않는 모션은 다른 모션으로 전이할 수 없다.
                    default:
                        return false;
                }
            }
            
            #endregion
        }
    }
}