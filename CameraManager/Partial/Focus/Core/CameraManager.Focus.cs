#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>
        
        /// <summary>
        /// 현재 선정된 추적 대상 프리셋
        ///
        /// TODO<k514> : 추후 리스트로 변경하여 다수의 포커스를 가질 수 있도록 개선해야 한다.
        /// TODO<k514> : 각 오브젝트는 GameEntityEvent를 통해 제어되어야만 한다.
        /// </summary>
        private CameraFocusPreset _CurrentFocusPreset;

        #endregion

        #region <Callbacks>
        
        private void OnUpdateTraceTargetConfig()
        {
            _CurrentFocusPreset = new CameraFocusPreset(_CurrentCameraConstantDataRecord.TraceRadiusRate);
        }
        
        private void OnTracingTargetChanged(ICameraFocusable p_PrevFocus, ICameraFocusable p_CurrentFocus)
        {
            if (!ReferenceEquals(p_PrevFocus, p_CurrentFocus))
            {
                if (!ReferenceEquals(null, p_PrevFocus))
                {
                    p_PrevFocus.OnCameraFocusTerminated();
                }

                if (!ReferenceEquals(null, p_CurrentFocus))
                {
                    p_CurrentFocus.OnCameraFocused(_CurrentCameraMode);
                }
                
                CameraEventSenderManager.GetInstanceUnsafe.SendEvent(CameraTool.CameraEventType.TraceTargetChanged, new CameraEventParams());
            }
        }

        private void OnCheckTargetMove()
        {
            if (_CurrentFocusPreset.IsPositionChanged())
            {
                CheckViewControlZoomAgainstTerrain();
                CameraEventSenderManager.GetInstanceUnsafe.SendEvent(CameraTool.CameraEventType.CameraPositionChanged, new CameraEventParams());
            }
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 지정한 오브젝트가 현재 카메라 매니저가 추적하는 오브젝트와 일치하는지 검증하는 논리 메서드
        /// </summary>
        public bool IsTracingTarget(ICameraFocusable p_Focus)
        {
            return ReferenceEquals(_CurrentFocusPreset.FocusObject, p_Focus);
        }

        #endregion
    }
}

#endif