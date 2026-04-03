using System;

namespace k514.Mono.Common
{
    public partial class AnimationTool
    {
        public enum MotionSetType
        {
            Test = 0, // Test@**
        }
        
        public enum MotionType
        {
            None,
            
            IdleRelax, IdleCombat, IdleAerial,
            MoveWalk, MoveRun,
            JumpUp, JumpDown,
            Punch, Kick, Cast, Dash, Interact,
            Hit, Groggy, Dead,
        }
        
        [Flags]
        public enum MotionTransitionType
        {
            None = 0,
            
            /// <summary>
            /// 각 모션의 상관 관계에 따라 전이
            /// 예를 들어, 점프 모션 중에 달리기 모션은 할 수 없다던가
            /// </summary>
            Bypass_StateMachine = 1 << 0,
            
            /// <summary>
            /// Bypass_StateMachine 반대로 동작
            /// </summary>
            Bypass_InverseStateMachine = 1 << 1,
            
            /// <summary>
            /// 지정한 모션이 현재 모션과 같은 타입인 경우에만 전이
            /// </summary>
            WhenSameMotion = 1 << 10,
            
            /// <summary>
            /// 지정한 모션이 현재 모션과 같은 타입인 다른 경우에만 전이
            /// </summary>
            WhenDifferentMotion = 1 << 11,
                        
            /// <summary>
            /// 지정한 모션으로 무조건 전이
            /// </summary>
            Restrict = 1 << 14,
            
            /// <summary>
            /// 해당 플래그를 포함시키는 전이의 경우, 현재 애니메이션 정보를 지우고 모션 진입을 시도한다.
            /// </summary>
            ErasePrevMotion = 1 << 30,
            
            /// <summary>
            /// 마스크를 논리곱으로 변환시키는 플래그
            /// </summary>
            AndMask = 1 << 31,
        }

        public enum MoveMotionType
        {
            Idle,
            Walk,
            Run
        }
 
        public enum IdleMotionType
        {
            Relax,
            Combat,
        }
        
        public enum ClipEventType
        {
            StartCue,
            EventCue,
            CancelCue,
            StopCue,
            SoundCue,
            EndCue,
        }
        
        public enum MotionPlaceType
        {
            /// <summary>
            /// 루트모션의 영향을 받지 않음
            /// </summary>
            None,
            
            /// <summary>
            /// 루트모션 노드(애니메이터 컴포넌트)가 모션에 의해 아핀변환을 적용받는 모션
            /// </summary>
            InPlaceRootMotion,
            
            /// <summary>
            /// 모션의 Y 평행이동 아핀변환을 제외한 나머지 변환을 적용받는 모션 
            /// </summary>
            InPlaceRootMotionExceptY,
            
            /// <summary>
            /// 루트모션 노드(애니메이터 컴포넌트)가 모션에 의해 평행이동 변환만을 적용받는 모션
            /// </summary>
            InPlaceRootMotionPositionOnly,
                        
            /// <summary>
            /// 루트모션 노드(애니메이터 컴포넌트)가 모션에 의해 회전 변환만을 적용받는 모션
            /// </summary>
            InPlaceRootMotionRotationOnly,
        }
        
        /// <summary>
        /// 지정한 모션이 없어서 FallBack 모션을 재생해야 할 때, 유닛의 상태에 따라 처리해야할 이벤트를
        /// 기술하는 열거형 상수
        /// </summary>
        public enum FallBackFailHandleType
        {
            /// <summary>
            /// 그냥 FallBackMotion을 재생한다.
            /// </summary>
            JustPlay,
                
            /// <summary>
            /// 만약 유닛이 이동중이라면, FallBack으로 전이하지 않고 이전 모션을 계속 재생한다.
            /// </summary>
            KeepPrevMotionWhenMoving,
        }
    }
}