namespace k514.Mono.Common
{
    public abstract partial class AnimationBase
    {
        #region <Fields>

        /// <summary>
        /// 모션 실행 전 아핀 상태 프리셋
        /// </summary>
        public AffinePreset CachedMotionAffine { get; protected set; }
        
        /// <summary>
        /// 모션 재생 속도 배율
        /// </summary>
        protected float _AnimationSpeedFactor;

        /// <summary>
        /// 모션 재생 속도 배율
        ///
        /// 모듈 내부에서만 사용된다.
        /// </summary>
        protected float _MotionSpeedFactor;

        /// <summary>
        /// 현재 애니메이션이 실행중인지 표시하는 플래그
        /// </summary>
        protected bool _OnProgressingFlag;
        
        #endregion
        
        #region <Methods>

        public void SaveMotionAffine()
        {
            var masterAffine = Entity.Affine;
            CachedMotionAffine = masterAffine;
        }
        
        public virtual void LoadMotionAffine()
        {
            Entity.Affine.forward = CachedMotionAffine.Forward;
        }
        
        public abstract void SetAnimationPause();
        public abstract void SetAnimationResume();

        public void SetAnimationSpeed(float p_Factor)
        {
            _AnimationSpeedFactor = p_Factor;
            UpdateAnimationSpeed();
        }

        protected void SetMotionSpeed(float p_Factor)
        {
            _MotionSpeedFactor = p_Factor;
            UpdateAnimationSpeed();
        }
        
        protected abstract void UpdateAnimationSpeed();
        
        public void ResetAnimationSpeed()
        {
            _MotionSpeedFactor = 1f;
            _AnimationSpeedFactor = 1f;
            UpdateAnimationSpeed();
        }
        
        #endregion
    }
}