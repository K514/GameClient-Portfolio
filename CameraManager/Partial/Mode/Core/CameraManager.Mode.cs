#if !SERVER_DRIVE

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 현재 카메라 촬영 모드
        /// </summary>
        private CameraTool.CameraMode _CurrentCameraMode;
        
        #endregion

        #region <Callbacks>

        private void OnCreateMode()
        {
            SetCameraSmoothTraceRadius(Default_Smooth_Trace_Radius);
        }

        private void OnUpdateCameraModeConfig()
        {
            SetCameraMode(_CurrentCameraMode, _CurrentFocusPreset.FocusObject);
        }

        private void OnResetMode()
        {
            SetCameraModeNone();
            SetCameraSmoothTraceRadius(Default_Smooth_Trace_Radius);
        }

        private void OnCameraModeChanged(CameraTool.CameraMode p_PrevCameraMode, CameraTool.CameraMode p_CurrentCameraMode)
        {
            switch (p_PrevCameraMode)
            {
                case CameraTool.CameraMode.None:
                    break;
                case CameraTool.CameraMode.ObjectTracing:
                    break;
                case CameraTool.CameraMode.ObjectTracingSmoothLerp:
                    break;
                case CameraTool.CameraMode.FirstPersonTracing:
                    // 1인칭 모드가 해제된 경우, 뷰 컨트롤 기능을 활성화 시킨다.
                    _CameraState.RemoveFlag(CameraTool.CameraStateFlag.FirstPersonFocusModeFlagMask);
                    break;
            }

            var isTraceTargetValid = _CurrentFocusPreset.IsFocusableValid();
            switch (p_CurrentCameraMode)
            {
                case CameraTool.CameraMode.None:
                    break;
                case CameraTool.CameraMode.ObjectTracing:
                case CameraTool.CameraMode.ObjectTracingSmoothLerp:
                    if (isTraceTargetValid)
                    {
                        ResetViewControl();
                        _CurrentFocusPreset.FocusObject.OnCameraModeChanged(p_PrevCameraMode, p_CurrentCameraMode);
                    }
                    else
                    {
                        ResetViewControl();
                    }
                    break;
                case CameraTool.CameraMode.FirstPersonTracing:
                    if (isTraceTargetValid)
                    {
                        var focusable = _CurrentFocusPreset.FocusObject;

                        // 1인칭 모드에서는 뷰 컨트롤 기능이 동작하지 않음
                        _CameraState.AddFlag(CameraTool.CameraStateFlag.FirstPersonFocusModeFlagMask);
                        
                        // 카메라가 포커스에게 초근접할 수 있도록, 근접 컬링 거리 및
                        // 현재 포커싱 거리를 0으로 세팅해준다.
                        _CurrentFocusPreset.SetNearBlockRadius(0f);
              
                        // * 여기서 Root 타입은 뷰 컨트롤에 사용되는 아핀 래퍼임.
                        // 1인칭 시점으로 보이도록 카메라 아핀값을 200ms에 걸쳐 조작한다.
                        SetCameraFocusOffsetLerp(CameraTool.CameraWrapperType.ViewControl_0, focusable.GetHeightVector(1f), 0, 200);
                        focusable.OnCameraModeChanged(p_PrevCameraMode, p_CurrentCameraMode);
                    }
                    else
                    {
                        ResetViewControl();
                    }
                    break;
            }
        }

        public void OnFocusDead(ICameraFocusable p_Focus)
        {
            switch (_CurrentCameraMode)
            {
                case CameraTool.CameraMode.None:
                    break;
                case CameraTool.CameraMode.ObjectTracing:
                case CameraTool.CameraMode.ObjectTracingSmoothLerp:
                    SetCameraModeNone();
                    break;
                case CameraTool.CameraMode.FirstPersonTracing:
                    SetCameraModeNone();
                    ResetViewControl();
                    break;
            }
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 카메라를 초기화 시키는 메서드
        /// </summary>
        private void SetCameraMode(CameraTool.CameraMode p_Type, ICameraFocusable p_Focus)
        {
            var prevCameraMode = _CurrentCameraMode;
            var prevTarget = _CurrentFocusPreset.FocusObject;
            
            if (ReferenceEquals(null, p_Focus))
            {
                _CurrentCameraMode = CameraTool.CameraMode.None;
                _CurrentFocusPreset.SetFocusable(null);
            }
            else
            {
                _CurrentCameraMode = p_Type;
                _CurrentFocusPreset.SetFocusable(p_Focus);
                SetRootPosition(_CurrentFocusPreset.FocusObject.GetBottomPosition());
            }

            OnTracingTargetChanged(prevTarget, _CurrentFocusPreset.FocusObject);
            OnCameraModeChanged(prevCameraMode, _CurrentCameraMode);
        }

        /// <summary>
        /// 현재 카메라 모드를 None으로 전이시키는 메서드
        /// </summary>
        public void SetCameraModeNone()
        {
            SetCameraMode(CameraTool.CameraMode.None, null);
        }
        
        /// <summary>
        /// 현재 카메라 모드를 오브젝트 추적 모드로 전이시키는 메서드
        /// </summary>
        public void SetCameraModeTracingObject(ICameraFocusable p_Focus)
        {
            SetCameraMode(CameraTool.CameraMode.ObjectTracing, p_Focus);
        }

        /// <summary>
        /// 현재 카메라 모드를 오브젝트 Smooth 추적 모드로 전이시키는 메서드
        /// 2번째 파라미터 p_SmoothRadius는 지연 추적을 위해 유닛 초점과 최대로 멀어지는 반경을 의미한다.
        /// </summary>
        public void SetCameraModeSmoothTracingObject(ICameraFocusable p_Focus)
        {
            _SmoothTracingCameraOffset = Vector3.zero;
            SetCameraMode(CameraTool.CameraMode.ObjectTracingSmoothLerp, p_Focus);
        }

        /// <summary>
        /// 현재 카메라 모드를 1인칭 시점으로 전이시키는 메서드
        /// </summary>
        public void SetCameraModeFirstPersonTracing(ICameraFocusable p_Focus)
        {
            SetCameraMode(CameraTool.CameraMode.FirstPersonTracing, p_Focus);
        }

        /// <summary>
        /// ObjectTracingSmoothLerp 모드에서 사용할 추적 시작 반경 
        /// </summary>
        public void SetCameraSmoothTraceRadius(float p_Radius)
        {
            _TracingSmoothRadius = p_Radius;
        }

        #endregion
    }
}

#endif