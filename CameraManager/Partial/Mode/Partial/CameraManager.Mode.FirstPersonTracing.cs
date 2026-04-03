#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Consts>

        private const int __FirstPersonFocus_ViewControl_Msec = 100;

        #endregion
        
        #region <Callbacks>

        /// <summary>
        /// 1인칭 즉시 추적 모드
        /// </summary>
        public void OnUpdateFirstPersonTracing(float p_DeltaTime)
        {
            if (_CurrentFocusPreset.IsFocusableValid())
            {
                var focusable = _CurrentFocusPreset.FocusObject;
                var lookVector = focusable.GetLookUV();
                SetRootPosition(focusable.GetBottomPosition());

                // 방향을 즉시 동기화 시키면, 화면이 갑자기 움직여서 어지럽기 때문에 러프시킨다.
                SetDegreeLerp(CameraTool.CameraWrapperType.ViewControl_0, lookVector, 0, __FirstPersonFocus_ViewControl_Msec);
                SetCameraDistanceZoomLerp(CameraTool.CameraWrapperType.ViewControl_0, 0f, 0, __FirstPersonFocus_ViewControl_Msec);
                OnCheckTargetMove();
            }
        }
        
        #endregion
    }
}
#endif