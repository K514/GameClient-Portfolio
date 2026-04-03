#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Method/Focus>

        /// <summary>
        /// 카메라의 초점을 지정한 값으로 세트하는 메서드
        /// </summary>
        public void SetCameraFocusOffset(CameraTool.CameraWrapperType p_Type, Vector3 p_Offset)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.SetValue(p_Offset);
            }
        }

        /// <summary>
        /// 카메라의 초점을 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddCameraFocusOffset(CameraTool.CameraWrapperType p_Type, Vector3 p_Offset, float p_DeltaTime)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.AddValue(p_Offset, p_DeltaTime);
            }
        }
 
        /// <summary>
        /// 카메라의 초점을 원본 값으로 초기화 시키는 메서드
        /// </summary>
        public void ResetCameraFocusOffset(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.ResetValue();
            }
        }
        
        #endregion

        #region <Method/Focus/Lerp>
        
        /// <summary>
        /// 카메라의 초점을 지정한 값으로 러프하는 메서드
        /// </summary>
        public void SetCameraFocusOffsetLerp(CameraTool.CameraWrapperType p_Type, Vector3 p_Offset, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.SetValueLerp(p_Offset, p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
                
        /// <summary>
        /// 카메라의 초점을 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddCameraFocusOffsetLerp(CameraTool.CameraWrapperType p_Type, Vector3 p_Offset, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.AddValueLerp(p_Offset, p_PreDelayMsec, p_LerpDurationMsec);
            }
        }

        /// <summary>
        /// 카메라의 초점을 원본 값으로 초기화 시키는 메서드
        /// </summary>
        public void ResetCameraFocusOffsetLerp(CameraTool.CameraWrapperType p_Type, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.ResetValueLerp(p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
  
        /// <summary>
        /// 카메라의 초점 변환 러프 이벤트를 캔슬시키는 메서드
        /// </summary>
        public void CancelCameraFocusOffsetLerp(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType() && !IsCameraFocusOffsetLerpValid(p_Type))
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                targetHandler.Terminate();
            }
        }
        
        #endregion
    }
}

#endif