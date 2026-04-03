using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class AnimatorAnimation : AnimationBase
    {
        #region <Consts>

        public static (bool, AnimationModuleDataTableQuery.TableLabel, AnimatorAnimation) CreateModule(IAnimationModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        { 
            var animationControllerPreset = AnimationManager.GetInstanceUnsafe.LoadAnimationController(p_Entity.GetAnimatorIndex());
            if (animationControllerPreset.ValidFlag)
            {
                var result = new AnimatorAnimation(p_ModuleRecord, p_Entity);
                result._AnimatorPreset = animationControllerPreset;
                result._Animator.runtimeAnimatorController = animationControllerPreset.Animator.Asset;
                
                return AnimationBase.CreateModule(result);
            }
            else
            {
                return AnimationBase.CreateModule(default(AnimatorAnimation));
            }
        }
        
        public static async UniTask<(bool, AnimationModuleDataTableQuery.TableLabel, AnimatorAnimation)> CreateModule(IAnimationModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            var animationControllerPreset = AnimationManager.GetInstanceUnsafe.LoadAnimationController(p_Entity.GetAnimatorIndex());
            if (animationControllerPreset.ValidFlag)
            {
                var result = new AnimatorAnimation(p_ModuleRecord, p_Entity);
                result._AnimatorPreset = animationControllerPreset;
                result._Animator.runtimeAnimatorController = animationControllerPreset.Animator.Asset;
                
                return await AnimationBase.CreateModule(result, p_CancellationToken);
            }
            else
            {
                return await AnimationBase.CreateModule(default(AnimatorAnimation), p_CancellationToken);
            }
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 애니메이터 컴포넌트 레퍼런스
        /// </summary>
        private Animator _Animator;
         
        /// <summary>
        /// 해당 유닛이 사용할 모션 프리셋
        /// </summary>
        private AnimatorPreset _AnimatorPreset;

        /// <summary>
        /// 현재 진행중인 클립 프리셋
        /// </summary>
        private ClipPreset _CurrentClipPreset;
        
        /// <summary>
        /// 현재 예약중인 클립 프리셋
        /// </summary>
        private ClipPreset _ReservedClipPreset;
     
        #endregion
     
        #region <Constructor>

        private AnimatorAnimation(IAnimationModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(AnimationModuleDataTableQuery.TableLabel.AnimatorAnimation, p_ModuleRecord, p_Entity)
        {
            Affine.GetSafeComponent(ref _Animator);
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();

            SetIdleState(AnimationTool.IdleMotionType.Relax);
            SetMoveState(AnimationTool.MoveMotionType.Idle);
            SwitchToIdleMotion(AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }

        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }
        
        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();

            _Animator.runtimeAnimatorController = null;
            _AnimatorPreset = default;
        }
        
        #endregion
   
        #region <Methods>

#if APPLY_PRINT_LOG
        public void PrintAnimationClip()
        {
            if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
            {
                CustomDebug.Log((this, $" * Unit [{Entity.GetName()} Animation Controller State *"));
                foreach (var motionType in AnimationTool._MotionTypeEnumerator)
                {
                    var clipList = _AnimatorPreset.MotionTable[motionType];
                    foreach (var clipPreset in clipList)
                    {
                        CustomDebug.Log((this, $"  ** MotionType [{motionType}] \n{clipPreset}"));
                    }
                }
            }
        }
#endif
 
        #endregion
    }
}