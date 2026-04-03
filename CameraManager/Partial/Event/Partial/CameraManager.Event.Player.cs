#if !SERVER_DRIVE

using System;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 플레이어 이벤트를 수신받는 오브젝트
        /// </summary>
        private PlayerEventReceiver _PlayerEventReceiver;

        #endregion

        #region <Callbakcs>

        private void OnCreatePlayerEvent()
        {
            _PlayerEventReceiver = 
                new PlayerEventReceiver
                (
                    PlayerTool.PlayerEventType.PlayerChanged | PlayerTool.PlayerEventType.SceneStart,
                    OnHandleEvent
                );
        }

        private void OnDisposePlayerEvent()
        {
            _PlayerEventReceiver?.Dispose();
            _PlayerEventReceiver = null;
        }

        private void OnHandleEvent(PlayerTool.PlayerEventType p_EventType, PlayerEventParams p_EventParams)
        {
            switch (p_EventType)
            {
                case PlayerTool.PlayerEventType.PlayerChanged:
                case PlayerTool.PlayerEventType.SceneStart:
                {
                    switch (_CurrentCameraMode)
                    {
                        case CameraTool.CameraMode.None:
                        case CameraTool.CameraMode.ObjectTracing:
                            SetCameraModeTracingObject(p_EventParams.Player);
                            break;
                        case CameraTool.CameraMode.ObjectTracingSmoothLerp:
                            SetCameraModeSmoothTracingObject(p_EventParams.Player);
                            break;
                        case CameraTool.CameraMode.FirstPersonTracing:
                            break;
                    }   
                    break;
                }
            }
        }
        
        #endregion
    }
}

#endif