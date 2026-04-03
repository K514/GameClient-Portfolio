#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public interface ICameraFocusEventReceiver
    {
        void OnCameraFocused(CameraTool.CameraMode p_ModeType);
        void OnCameraModeChanged(CameraTool.CameraMode p_PrevCameraMode, CameraTool.CameraMode p_CurrentCameraMode);
        void OnCameraFocusTerminated();
    }
}

#endif
