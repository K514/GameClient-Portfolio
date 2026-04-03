#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Callbacks>

        /// <summary>
        /// 즉시 추적 모드
        /// </summary>
        public void OnUpdateTracing(float p_DeltaTime)
        {
            if (_CurrentFocusPreset.IsFocusableValid())
            {
                SetRootPosition(_CurrentFocusPreset.FocusObject.GetBottomPosition());
            }

            OnCheckTargetMove();
        }

        #endregion
    }
}
#endif