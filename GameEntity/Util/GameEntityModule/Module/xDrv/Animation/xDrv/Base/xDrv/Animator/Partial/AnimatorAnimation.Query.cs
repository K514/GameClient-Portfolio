namespace k514.Mono.Common
{
    public partial class AnimatorAnimation
    {
        /// <summary>
        /// Animator.Play 으로 실행 요청한 모션이 아직 실행되지 않은 경우 참을 리턴하는 논리 메서드
        /// </summary>
        private bool IsReserveClipValid()
        {
            return _ReservedClipPreset.ValidFlag;
        }
        
        /// <summary>
        /// 예약된 모션이 지정된 타입인지 검증하는 메서드
        /// </summary>
        public override bool IsReservedMotion(AnimationTool.MotionType p_Type)
        {
            return _ReservedClipPreset.MotionType == p_Type;
        }
        
        /// <summary>
        /// 현재 실행중인 모션이 지정한 타입인지 검증하는 메서드
        /// </summary>
        public override bool IsCurrentMotion(AnimationTool.MotionType p_Type)
        {
            return _CurrentClipPreset.MotionType == p_Type;
        }

        /// <summary>
        /// 현재 실행중인 모션이 지정한 타입의 지정한 인덱스 모션인지 검증하는 메서드
        /// </summary>
        private bool IsCurrentMotion(AnimationTool.MotionType p_Type, int p_Index)
        {
            return _CurrentClipPreset.MotionType == p_Type && _CurrentClipPreset.MotionIndex == p_Index;
        }

        /// <summary>
        /// 현재 실행중인 모션이 진행중인지 검증하는 메서드
        /// </summary>
        public override bool IsCurrentMotionProgressing()
        {
            return _OnProgressingFlag;
        }

        /// <summary>
        /// 지정한 애니메이션 컨트롤러가 현재 재생중인 모션이 없는지 검증하는 메서드
        /// </summary>
        public bool IsPlayedNoneMotion()
        {
            return IsCurrentMotion(AnimationTool.MotionType.None);
        }
 
        /// <summary>
        /// 현재 실행중인 모션이 루프 모션인지 검증하는 메서드
        /// </summary>
        private bool IsCurrentMotionLooping()
        {
            return _CurrentClipPreset.Clip.isLooping;
        }
    }
}