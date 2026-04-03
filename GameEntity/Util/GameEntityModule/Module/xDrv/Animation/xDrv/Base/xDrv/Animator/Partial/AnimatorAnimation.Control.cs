using UnityEngine;

namespace k514.Mono.Common
{
    public partial class AnimatorAnimation
    {
        /// <summary>
        /// 진행중인 모션이 있는데, 로직상에서 애니메이션 모션 전이가 일어나는 경우
        /// 로직상 전이 직전에 호출되는 콜백
        /// </summary>
        public override void LoadMotionAffine()
        {
            // 현재 진행중인 모션이 공격모션인 경우에만 방향을 이어준다.
            var currentMotionType = _CurrentClipPreset.MotionType;
            switch (currentMotionType)
            {
                case AnimationTool.MotionType.Punch:
                case AnimationTool.MotionType.Kick:
                    base.LoadMotionAffine();
                    break;
            }
        }
        
        public override void SetAnimationPause()
        {
            _Animator.enabled = false;
        }
        
        public override void SetAnimationResume()
        {
            _Animator.enabled = true;
        }
        
        protected override void UpdateAnimationSpeed()
        {
            _Animator.speed = Mathf.Clamp(_MotionSpeedFactor * _AnimationSpeedFactor, AnimationTool.AnimationSpeedLowerBound, AnimationTool.AnimationSpeedUpperBound);
        }
        
        /// <summary>
        /// 루트 모션 사용여부를 지정하는 메서드
        /// </summary>
        public void SetRootMotionEnable(bool p_Flag)
        {
            _Animator.applyRootMotion = p_Flag;
        }
        
        /// <summary>
        /// 특정 애니메이션 재생 속도를 세트하는 메서드
        /// </summary>
        public void SetAnimationFloat(string p_Name, float p_Float)
        {
            _Animator.SetFloat(p_Name, p_Float);
        }
    }
}