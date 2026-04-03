using System;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 씬 설정이 변경된 경우, 해당 이벤트를 수신받는 오브젝트
        /// </summary>
        private SceneEventReceiver _SceneEventReceiver;

        #endregion

        #region <Callbakcs>

        private void OnCreateSceneEvent()
        {
            _SceneEventReceiver = 
                new SceneEventReceiver
                (
                    SceneTool.SceneEventType.OnSceneEnvironmentPreload, 
                    OnHandleEvent
                );
        }

        private void OnDisposeSceneEvent()
        {
            _SceneEventReceiver?.Dispose();
            _SceneEventReceiver = null;
        }

        private void OnHandleEvent(SceneTool.SceneEventType p_EventType, SceneEventParams p_Params)
        {
            switch (p_EventType)
            {
                case SceneTool.SceneEventType.OnSceneEnvironmentPreload:
                    var sceneEnv = p_Params.SceneEnvironment;
                    SetCameraConfig(sceneEnv.GetCameraConstantDataRecord(), sceneEnv.GetCameraVariableDataRecord(), sceneEnv.SkyBoxIndex);
                    break;
            }
        }
        
        #endregion
    }
}

#endif