#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Callbacks>

        private void OnCreateEvent()
        {
            OnCreatePlayerEvent();
            OnCreateSceneEvent();
            OnCreateTouchEvent();
        }

        private void OnDisposeEvent()
        {
            OnDisposeTouchEvent();
            OnDisposeSceneEvent();
            OnDisposePlayerEvent();
        }

        #endregion
    }
}

#endif