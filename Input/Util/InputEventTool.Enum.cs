using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class InputEventTool
    {
        /// <summary>
        /// 입력 장치 타입
        /// </summary>
        [Flags]
        public enum InputDeviceType
        {
            None = 0,
            
            /// <summary>
            /// UI 입력(마우스 or 터치)
            /// </summary>
            UI = 1 << 0, 
            
            /// <summary>
            /// 키보드 입력
            /// </summary>
            Keyboard = 1 << 1, 
        }

        /// <summary>
        /// 입력 이벤트 레이어 타입
        ///
        /// 각 레이어 타입은 독립적으로 입력 이벤트를 처리한다.
        /// </summary>
        [Flags]
        public enum InputLayerType
        {
            None = 0,
            
            /// <summary>
            /// 기본 입력 타입
            /// </summary>
            ControlUnit = 1 << 0,
            
            /// <summary>
            /// 카메라 조작
            /// </summary>
            ControlView = 1 << 1,
                  
            /// <summary>
            /// UI 컨트롤
            /// </summary>
            ControlUI = 1 << 2,

#if APPLY_TEST_MANAGER
            /// <summary>
            /// 디버그 기능
            /// </summary>
            Debug = 1 << 31,
            
            /// <summary>
            ///기본 키보드 레이어
            /// </summary>
            DefaultKeyboard = ControlUnit | ControlUI | Debug,

            /// <summary>
            /// 모든 타입 플래그 마스크
            /// </summary>
            WholeMask = ControlUnit | ControlView | ControlUI | Debug,
#else
            /// <summary>
            ///기본 키보드 레이어
            /// </summary>
            DefaultKeyboard = ControlUnit | ControlUI,

            /// <summary>
            /// 모든 타입 플래그 마스크
            /// </summary>
            WholeMask = ControlUnit | ControlView | ControlUI,
#endif
        }

        /// <summary>
        /// 입력 키 타입
        /// </summary>
        [Flags]
        public enum InputKeyType
        {
            None = 0,
            
            /// <summary>
            /// 방향키
            /// </summary>
            ArrowKey = 1 << 0,

            /// <summary>
            /// 트리거키
            /// </summary>
            TriggerKey = 1 << 1,
            
            /// <summary>
            /// 기능키
            /// </summary>
            FunctionKey = 1 << 2,
            
            Whole = ArrowKey | TriggerKey | FunctionKey,
        }

        /// <summary>
        /// 트리거 키 코드 타입
        /// </summary>
        [Flags]
        public enum TriggerKeyType
        {
            None = 0,
            
            UpArrow = 1 << 0,
            LeftArrow = 1 << 1,
            DownArrow = 1 << 2,
            RightArrow = 1 << 3,
            
            Z = 1 << 4,
            X = 1 << 5,
            C = 1 << 6,
            V = 1 << 7,
            B = 1 << 8,
            Space = 1 << 9,
            
            A = 1 << 10,
            S = 1 << 11,
            D = 1 << 12,
            F = 1 << 13,
            G = 1 << 14,
            H = 1 << 15,
            
            Q = 1 << 16,
            W = 1 << 17,
            E = 1 << 18,
            R = 1 << 19,
            T = 1 << 20,
            
            LeftControl = 1 << 21,
            LeftShift = 1 << 22,
            LeftAlt = 1 << 23,
            
            RightControl = 1 << 24,
            RightShift = 1 << 25,

            Delete = 1 << 26,
            End = 1 << 27,
            PageDown = 1 << 28,
            
            Move = UpArrow | LeftArrow | DownArrow | RightArrow,
        }

        /// <summary>
        /// 입력 상태 타입
        /// </summary>
        public enum InputStateType
        {
            /// <summary>
            /// 특정 키를 뗀 경우
            /// </summary>
            Release,
            
            /// <summary>
            /// 특정 키를 누른 경우
            /// </summary>
            Press,
            
            /// <summary>
            /// 특정 키를 계속 누르고 있는 경우
            /// 정확히는 Press ~ Release의 사이
            /// </summary>
            Holding,
        }

        /// <summary>
        /// 입력 제스쳐 타입
        /// </summary>
        public enum InputGestureType
        {
            /// <summary>
            /// 입력 없음
            /// </summary>
            None, 

            /// <summary>
            /// 손가락 두 개로 터치 지점을 모으는 경우
            /// 혹은 마우스 휠을 바깥쪽으로 민 경우
            /// </summary>
            Gather, 
            
            /// <summary>
            /// 손가락 두 개로 터치 지점을 흩어지게 하는 경우
            /// 혹은 마우스 휠을 안쪽으로 민 경우
            /// </summary>
            Scatter, 
            
            /// <summary>
            /// 터치 혹은 클릭이 지속되는 경우
            /// </summary>
            Stable
        }
    }
}