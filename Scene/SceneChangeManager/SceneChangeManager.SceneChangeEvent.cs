using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514.Mono.Common
{
    public partial class SceneChangeManager : ISceneChangeEvent
    {
        public async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            // 다른 씬으로의 전이를 막는다.
            _CurrentPhase = SceneControlPhase.Reserved;
#if UNITY_EDITOR
            CustomDebug.LogError($"[{SceneManager.GetActiveScene().name}] OnScenePreload");
#endif
            await SceneChangeEventManager.GetInstanceUnsafe.OnBroadCastSceneChangeEvent(SceneTool.SceneChangeEventType.ScenePreload, p_CancellationToken);
        }
        
        public async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            // 다른 씬으로의 전이를 허용한다.
            _CurrentPhase = SceneControlPhase.None;
#if UNITY_EDITOR
            CustomDebug.LogError($"[{SceneManager.GetActiveScene().name}] OnSceneStart");
#endif
            await SceneChangeEventManager.GetInstanceUnsafe.OnBroadCastSceneChangeEvent(SceneTool.SceneChangeEventType.SceneStart, p_CancellationToken);
        }

        public async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
#if UNITY_EDITOR
            CustomDebug.LogError($"[{SceneManager.GetActiveScene().name}] OnSceneTerminate");
#endif
            await SceneChangeEventManager.GetInstanceUnsafe.OnBroadCastSceneChangeEvent(SceneTool.SceneChangeEventType.SceneTerminate, p_CancellationToken);
        }
        
        public async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
#if UNITY_EDITOR
            CustomDebug.LogError($"[{SceneManager.GetActiveScene().name}] OnSceneTransition");
#endif
            await SceneChangeEventManager.GetInstanceUnsafe.OnBroadCastSceneChangeEvent(SceneTool.SceneChangeEventType.SceneTransition, p_CancellationToken);
        }
    }
}