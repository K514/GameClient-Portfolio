#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>
        
        /// <summary>
        /// 현재 카메라 상태
        /// </summary>
        private CameraTool.CameraStateFlag _CameraState;
        
        #endregion

        #region <Callback>

        private void OnCreateState()
        {
            ResetCameraState();
        }

        private void OnResetState()
        {
            ResetCameraState();
        }

        #endregion
        
        #region <Methods>

        private void SetCameraValid(bool p_Flag)
        {
            if (p_Flag)
            {
                _CameraState.RemoveFlag(CameraTool.CameraStateFlag.CameraBlock);
            }
            else
            {
                _CameraState.AddFlag(CameraTool.CameraStateFlag.CameraBlock);
            }
        }

        public void SetCameraViewControlBlock(bool p_Flag)
        {
            if (p_Flag)
            {
                _CameraState.AddFlag(CameraTool.CameraStateFlag.BlockManualControl);
            }
            else
            {
                _CameraState.RemoveFlag(CameraTool.CameraStateFlag.BlockManualControl);
            }
        }
        
        /// <summary>
        /// 현재 카메라 상태를 초기화 시키는 메서드
        /// </summary>
        private void ResetCameraState()
        {
            _CameraState = CameraTool.CameraStateFlag.None;
        }

        #endregion
    }
}

#endif