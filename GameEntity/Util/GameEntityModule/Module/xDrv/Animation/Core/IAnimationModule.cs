using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 애니메이터 및 애니메이션 컨트롤러나 아핀변환 등을 통해 동작하는 게임 오브젝트의 기능을 제공하는 인터페이스
    /// </summary>
    public interface IAnimationModule : IGameEntityModule, IAnimatorEventReceiver
    {
        /* Default */
        AnimationModuleDataTableQuery.TableLabel GetAnimationModuleType();
        
        /* Motion */
        bool IsReservedMotion(AnimationTool.MotionType p_Type);
        bool IsCurrentMotion(AnimationTool.MotionType p_Type);
        bool IsCurrentMotionProgressing();
        bool SwitchMotion(AnimationTool.MotionType p_Type, AnimationTool.MotionTransitionType p_TransitionFlag);
        bool SwitchMotion(AnimationTool.MotionType p_Type, int p_Index, AnimationTool.MotionTransitionType p_TransitionFlag);
        
        /* Idle Motion */
        bool IsIdleState(AnimationTool.IdleMotionType p_Type);
        void SetIdleState(AnimationTool.IdleMotionType p_Type);
        AnimationTool.IdleMotionType GetIdleState();
        void SwitchToIdleMotion(AnimationTool.MotionTransitionType p_MotionTransitionType);
        void SwitchToIdleMotion(AnimationTool.IdleMotionType p_Type, AnimationTool.MotionTransitionType p_MotionTransitionType);
        
        /* Move Motion */
        bool IsMoveState(AnimationTool.MoveMotionType p_Type);
        void SetMoveState(AnimationTool.MoveMotionType p_Type);
        void SetMoveRunOrWalk(bool p_Flag);

        AnimationTool.MoveMotionType GetMoveState();
        void SwitchToMoveMotion(AnimationTool.MotionTransitionType p_MotionTransitionType);
        void SwitchToMoveMotion(AnimationTool.MoveMotionType p_Type, AnimationTool.MotionTransitionType p_MotionTransitionType);
        
        /* Hit Motion */
        void SwitchHitMotion();
        void TryHitMotionResume();
        
        /* Control */
        /// <summary>
        /// 모션이 진행되는 동안 회전도 등의 개체의 아핀값이 바뀌는데 모션 실행 전, 혹은 특정 모션의 아핀값을 저장해놓는 메서드
        /// </summary>
        void SaveMotionAffine();
        void LoadMotionAffine();
        void SetAnimationPause();
        void SetAnimationResume();
        void SetAnimationSpeed(float p_Factor);
        void ResetAnimationSpeed();

        /* Event */
        void OnAnimationStart();
        void OnAnimationStop();
        void OnAnimationEnd();
    }
}