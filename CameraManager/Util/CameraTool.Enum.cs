using System;

namespace k514.Mono.Common
{
    public static partial class CameraTool
    {
        #region <Enum>

        public enum CameraMode
        {
            /// <summary>
            /// 카메라가 특별히 움직이지 않는 경우
            /// </summary>
            None,
            
            /// <summary>
            /// 카메라가 특정 오브젝트를 추적하는 경우
            /// </summary>
            ObjectTracing,
            
            /// <summary>
            /// 카메라가 특정 오브젝트를 추적하는데, 보간을 이용하여 부드럽게 이동하는 경우
            /// </summary>
            ObjectTracingSmoothLerp,
            
            /// <summary>
            /// 카메라가 특정 오브젝트를 기준으로 1인칭 시점 연출을 한다.
            /// </summary>
            FirstPersonTracing,
        }

        [Flags]
        public enum CameraEventType
        {
            /// <summary>
            /// 이벤트 없음
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 카메라 BaseWrapper 위치가 변경된 경우
            /// </summary>
            CameraPositionChanged = 1 << 0,
            
            /// <summary>
            /// 카메라 줌 거리가 변경된 경우
            /// </summary>
            Zoom = 1 << 1,
            
            /// <summary>
            /// 카메라가 회전된 경우
            /// </summary>
            Rotate = 1 << 2,
            
            /// <summary>
            /// 카메라 컬링 상한값이 변경된 경우
            /// </summary>
            FarCullingDistanceChanged = 1 << 3,

            /// <summary>
            /// 카메라 포커스가 변경된 경우
            /// </summary>
            TraceTargetChanged = 1 << 4,
            
            WholeAffine = CameraPositionChanged | Zoom | Rotate,
        }
        
        public static readonly CameraEventType[] CameraEventTypeEnumerator;

        [Flags]
        public enum CameraCullingState
        {
            /// <summary>
            /// 기본값
            /// </summary>
            None = 0,
            
            /// <summary>
            /// 오브젝트가 카메라에 근접컬링 하한거리 보다 가까운 상태
            /// </summary>
            NearCulling = 1 << 0,
            
            /// <summary>
            /// 오브젝트가 카메라에 근접컬링 상한거리 보다 먼 상태
            /// </summary>
            FarCulling = 1 << 1,
            
            /// <summary>
            /// 오브젝트가 카메라로부터 적정거리 내에 있지만 Terrain 혹은 Obstacle 레이어 타입 오브젝트에 의해 가려진 상태
            /// </summary>
            Blocked = 1 << 2,
            
            /// <summary>
            /// 카메라 뷰포트 밖으로 벗어난 상태
            /// </summary>
            OutOfScreen = 1 << 3,
            
            /// <summary>
            /// 컬링되지 않은 상태
            /// </summary>
            Visible = 1 << 4,
        }

        public static readonly CameraCullingState[] CameraCullingStateEnumerator;
        
        public enum CameraRenderProcessType
        {
            /// <summary>
            /// 모든 유닛의 UI를 표시하는 모드
            /// </summary>
            Musashi,
            
            /// <summary>
            /// 플레이어 유닛 및 플레이어 유닛의 포커스 유닛의 UI를 표시하는 메서드
            /// </summary>
            Kojiro,
            
            /// <summary>
            /// 미정
            /// </summary>
            Nyaasu,
            
            /// <summary>
            /// 미정
            /// </summary>
            Sonansu
        }

        /// <summary>
        /// 메인 카메라 연출을 위해 카메라를 감싸는 래퍼 Transform 타입
        /// </summary>
        public enum CameraWrapperType
        {
            /// <summary>
            /// 카메라의 시선 끝에 위치한 Transform, 가장 최상단 Transform
            /// </summary>
            Root,
            
            /// <summary>
            /// 카메라를 직접 감싸고 있는 뷰 컨트롤용 Transform 0
            /// 
            /// 장기지속되는 뷰 컨트롤에 사용된다.
            /// </summary>
            ViewControl_0,
            
            /// <summary>
            /// 카메라를 직접 감싸고 있는 뷰 컨트롤용 Transform 1.
            ///
            /// 일시적인 뷰 컨트롤에 사용된다.
            /// </summary>
            ViewControl_1,
                       
            /// <summary>
            /// 카메라를 직접 감싸고 있는 뷰 컨트롤용 Transform 2.
            ///
            /// 카메라 흔들림 연출에 사용된다.
            /// </summary>
            ViewControl_Shake,
        }

        public static readonly CameraWrapperType[] CameraWrapperTypeEnumerator;

        /// <summary>
        /// 카메라 연출을 위한 아핀 변환 타입
        /// </summary>
        public enum ViewControlType
        {
            /// <summary>
            /// 카메라 초점 평행이동
            /// </summary>
            Focus,

            /// <summary>
            /// 카메라를 중심으로 하는 회전
            /// </summary>
            Rotate,
            
            /// <summary>
            /// 줌 인, 줌 아웃
            /// </summary>
            Zoom,
        }
        
        public static ViewControlType[] ViewControlEnumerator;

        [Flags]
        public enum CameraStateFlag
        {
            None = 0,

            /// <summary>
            /// 카메라 동작 플래그
            /// </summary>
            CameraBlock = 1 << 0,
            
            /// <summary>
            /// 입력 장치를 통한 이벤트를 발생시키는 기능을 막는다.
            /// </summary>
            BlockManualControl = 1 << 4,
            
            /// <summary>
            /// 충돌 검증 로직(CheckViewControlZoomAgainstTerrain) 종료 후, 로직상 거리를 실제 적용중인 거리와 동기화 시킨다.
            /// </summary>
            SyncLogicZoomDistanceToCurrentZoomDistance = 1 << 5,
            
            /// <summary>
            /// 1인칭 촬영 모드에서 블록되어야할 플래그 마스크
            /// </summary>
            FirstPersonFocusModeFlagMask = BlockManualControl | SyncLogicZoomDistanceToCurrentZoomDistance,
        }

        #endregion
    }
}