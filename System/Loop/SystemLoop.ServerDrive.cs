#if SERVER_DRIVE
using UnityEngine;

namespace k514
{
    public partial class SystemLoop
    {
        #region <Consts>

        private const float __LOG_INTERVAL = 20f;

        #endregion
        
        #region <Fields>

        private float _AccumulatedTime;

        #endregion

        #region <Callbacks>

        private void OnCreated_ServerDrive()
        {
        }

        private void OnUpdate_ServerDrive(float p_DeltaTime)
        {
            if (NetworkManager.GetInstanceUnsafe.IsConnected)
            {
                if (_AccumulatedTime > __LOG_INTERVAL)
                {
#if APPLY_PRINT_LOG
                    CustomDebug.Log($"k514, [Log] Fps : {1f / Time.unscaledDeltaTime}]");
#endif
                    _AccumulatedTime = 0f;
                }
                else
                {
                    _AccumulatedTime += p_DeltaTime;
                }
            }
        }

        #endregion
    }
}
#endif