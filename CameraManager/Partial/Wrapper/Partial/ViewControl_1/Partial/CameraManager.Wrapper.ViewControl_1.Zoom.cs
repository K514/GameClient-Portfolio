#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Method/Zoom>
        
        /// <summary>
        /// 카메라의 줌 거리를 지정한 값으로 세트하는 메서드
        /// </summary>
        public void SetCameraDistanceZoom(CameraTool.CameraWrapperType p_Type, float p_Distance)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.SetValue(p_Distance);
            }
        }

        /// <summary>
        /// 카메라의 줌 거리를 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddCameraDistanceZoom(CameraTool.CameraWrapperType p_Type, float p_Distance, float p_DeltaTime)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.AddValue(p_Distance, p_DeltaTime);
            }
        }
         
        /// <summary>
        /// 카메라의 거리를 원본 값으로 초기화 시키는 메서드
        /// </summary>
        public void ResetCameraDistanceZoom(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.ResetValue();
            }
        }
        
        #endregion
        
        #region <Method/Zoom/Lerp>
        
        /// <summary>
        /// 카메라의 거리를 지정한 값으로 러프하는 메서드
        /// </summary>
        public void SetCameraDistanceZoomLerp(CameraTool.CameraWrapperType p_Type, float p_Distance, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.SetValueLerp(p_Distance, p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
                
        /// <summary>
        /// 카메라의 거리를 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddCameraDistanceZoomLerp(CameraTool.CameraWrapperType p_Type, float p_Distance, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.AddValueLerp(p_Distance, p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
                        
        /// <summary>
        /// 카메라의 거리를 원본 값으로 초기화 시키는 메서드
        /// </summary>
        public void ResetCameraDistanceZoomLerp(CameraTool.CameraWrapperType p_Type, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.ResetValueLerp(p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
                      
        /// <summary>
        /// 카메라의 거리 변환 러프 이벤트를 캔슬시키는 메서드
        /// </summary>
        public void CancelCameraDistanceZoomLerp(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType() && !IsCameraDistanceZoomLerpValid(p_Type))
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                targetHandler.Terminate();
            }
        }
        
        #endregion
    }
}

#endif