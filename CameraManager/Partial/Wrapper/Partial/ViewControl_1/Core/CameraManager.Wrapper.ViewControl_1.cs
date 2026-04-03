#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Methods>

        /// <summary>
        /// 카메라의 회전 변환 러프 이벤트가 진행중인지 여부를 검증하는 메서드
        /// </summary>
        public bool IsCameraDegreeLerpValid(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                return _CameraAffineWrapperControllerSet[p_Type].RotateController.ValidFlag;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 카메라의 거리 변환 러프 이벤트가 진행중인지 여부를 검증하는 메서드
        /// </summary>
        public bool IsCameraDistanceZoomLerpValid(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].ZoomController;
                return targetHandler.ValidFlag;
            }
            else
            {
                return false;
            }
        }
                                
        /// <summary>
        /// 카메라의 초점 변환 러프 이벤트가 진행중인지 여부를 검증하는 메서드
        /// </summary>
        public bool IsCameraFocusOffsetLerpValid(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].FocusController;
                return targetHandler.ValidFlag;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// 지정한 래퍼의 변환을 전부 초기화시키는 메서드
        /// </summary>
        public void ResetAllAffine(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                _CameraAffineWrapperControllerSet[p_Type].ResetAllAffineTransform();
            }
        }
        
        /// <summary>
        /// 모든 래퍼의 변환을 모두 초기화시키는 메서드
        /// </summary>
        public void ResetAllLerp()
        {
            foreach (var controllerKV in _CameraAffineWrapperControllerSet)
            {
                var controller = controllerKV.Value;
                controller.ResetAllAffineTransform();
            }
        }

        /// <summary>
        /// 지정한 래퍼의 러프 이벤트를 전부 캔슬시키는 메서드
        /// </summary>
        public void CancelAllLerp(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                CancelDegreeLerp(p_Type);
                CancelCameraDistanceZoomLerp(p_Type);
                CancelCameraFocusOffsetLerp(p_Type);
            }
        }
                
        /// <summary>
        /// 모든 래퍼의 러프 이벤트를 전부 캔슬시키는 메서드
        /// </summary>
        public void CancelAllLerp()
        {
            foreach (var controllerKV in _CameraAffineWrapperControllerSet)
            {
                var controller = controllerKV.Value;
                controller.CancelAllCameraEvent();
            }
        }
        
        #endregion
    }
}

#endif