namespace k514.Mono.Common
{
    /// <summary>
    /// 애니메이션 모듈로 넣어버리고 싶은데, 해당 콜백함수들은 유니티 컴포넌트 스크립트로만 동작하기 때문에 이쪽에 모아둠
    /// </summary>
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        /// <summary>
        /// 모션에 등록된 Start 클립으로부터 호출되는 콜백
        /// </summary>
        public void OnAnimationMotionStartCue()
        {
            if (IsFunctional)
            {
                ActionModule.OnAnimationMotionStartCue();
            }
        }

        /// <summary>
        /// 모션에 등록된 Cue 클립으로부터 호출되는 콜백
        /// </summary>
        public void OnAnimationMotionEventCue()
        {
            if (IsFunctional)
            {
                ActionModule.OnAnimationMotionEventCue();
            }
        }

        /// <summary>
        /// 모션에 등록된 Cancel 클립으로부터 호출되는 콜백
        /// </summary>
        public void OnAnimationMotionCancelCue()
        {
            if (IsFunctional)
            {
                ActionModule.OnAnimationMotionCancelCue();
            }
        }

        /// <summary>
        /// 모션에 등록된 Stop 클립으로부터 호출되는 콜백
        /// </summary>
        public void OnAnimationMotionStopCue()
        {
            if (IsFunctional)
            {
                ActionModule.OnAnimationMotionStopCue();
            }
        }
        
        /// <summary>
        /// 모션에 등록된 MoveSound 클립으로부터 호출되는 콜백
        /// </summary>
        public void OnAnimationMotionSoundCue()
        {
            if(IsFunctional)
            {
                ActionModule.OnAnimationMotionSoundCue();
            }
        }

        /// <summary>
        /// 모션에 등록된 End 클립으로부터 호출되는 콜백
        /// </summary>
        public void OnAnimationMotionEndCue()
        {
            if (IsFunctional)
            {
                ActionModule.OnAnimationMotionEndCue();
            }
        }

        /// <summary>
        /// 모션에 루트모션이 적용되야 하는 경우 Animator 컴포넌트로부터 호출되는 콜백
        /// </summary>
        public void OnAnimatorMove()
        {
            if (IsFunctional)
            {
                AnimationModule.OnAnimatorMove();
            }
        }
    }
}