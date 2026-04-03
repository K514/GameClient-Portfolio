#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        /// <summary>
        /// 카메라가 해당 유닛을 포커싱하기 시작한 경우 호출되는 콜백
        /// </summary>
        public void OnCameraFocused(CameraTool.CameraMode p_ModeType)
        {
            OnModule_CameraFocused(p_ModeType);
        }

        /// <summary>
        /// 카메라의 촬영 모드가 변경된 경우 호출되는 콜백
        /// </summary>
        public void OnCameraModeChanged(CameraTool.CameraMode p_PrevCameraMode, CameraTool.CameraMode p_CurrentCameraMode)
        {
            OnModule_CameraModeChanged(p_PrevCameraMode, p_CurrentCameraMode);
        }

        /// <summary>
        /// 카메라가 해당 유닛으로부터 포커싱을 해제한 경우 호출되는 콜백
        /// </summary>
        public void OnCameraFocusTerminated()
        {
            OnModule_CameraFocusTerminated();
        }
    }
}

#endif 