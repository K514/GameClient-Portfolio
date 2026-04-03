using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514.Mono.Common
{
    public partial class LoadingScene
    {
        #region <Callbacks>

        protected override void _OnEntryPhaseLoop()
        {
        }
        
        protected override async UniTask _OnTerminatePhaseLoop(CancellationToken p_CancellationToken)
        {
            await SceneChangeManager.GetInstanceUnsafe.OnSceneStart(p_CancellationToken);
        }

        #endregion

        #region <Methods>

        private async UniTask Release_LifeCycle_Asset(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 수명단위 에셋 릴리스를 시작합니다.");
#endif
            await UniTask.CompletedTask;
            // 에셋번들의 언로드는 비동기 테스크를 지원하지 않음.
            AssetLoaderManager.GetInstanceUnsafe.UnloadAsset_SceneLifeCycle();
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 수명단위 에셋 릴리스에 성공했습니다.");
#endif
        }
        
        private async UniTask Release_LifeCycle_Singleton(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 수명단위 싱글톤 릴리스를 시작합니다.");
#endif
            await TableManager.GetInstanceUnsafe.Clear_SceneLifeCycle_TableSingleton();
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 수명단위 싱글톤 릴리스에 성공했습니다.");
#endif
        }
        
        private async UniTask CheckSceneBundle(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("전이할 씬의 번들을 로드합니다.");
#endif
            await SceneChangeManager.GetInstanceUnsafe.UpdateSceneBundle(p_CancellationToken);
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("번들 로드에 성공했습니다.");
#endif
        }
        
        private async UniTask<AsyncOperation> AsyncLoadScene(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬을 로드합니다.");
#endif
            await UniTask.CompletedTask;
            if (SceneChangeManager.GetInstanceUnsafe.CurrentSceneControlPreset.TryGetSceneFullPath(out var o_SceneFullPath))
            {
                return SceneManager.LoadSceneAsync(o_SceneFullPath, LoadSceneMode.Additive);
            }
            else
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError("씬 이름을 가져오는데 실패했습니다.");
#endif
                return null;
            }
        }
        
        private async UniTask PreloadScene(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("해당 씬에서 수행할 초기 로드 작업을 수행합니다.");
#endif
            await SceneChangeManager.GetInstanceUnsafe.OnScenePreload(p_CancellationToken);
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("해당 씬에서 수행할 초기 로드 작업에 성공했습니다.");
#endif
        }
        
        private async UniTask MergeScene(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬을 병합합니다.");
#endif
            await UniTask.CompletedTask;
            if (SceneChangeManager.GetInstanceUnsafe.CurrentSceneControlPreset.TryGetSceneFullPath(out var o_SceneFullPath))
            {
                var loadScene = SceneManager.GetActiveScene();
                var loadedScene = SceneManager.GetSceneByPath(o_SceneFullPath);
                
                // 씬 매니저의 씬 병합은 비동기 테스크를 지원하지 않음.
                SceneManager.MergeScenes(loadScene, loadedScene);
#if APPLY_PRINT_LOG
                CustomDebug.LogWarning("씬이 병합되었습니다.");
#endif
            }
            else
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError("씬 이름을 가져오는데 실패했습니다.");
#endif 
            }
        }
        
        private async UniTask SceneLoadOver(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 로드 완료 작업을 시작합니다.");
#endif
            await UniTask.CompletedTask;

#if APPLY_PRINT_LOG
            AssetLoaderManager.GetInstanceUnsafe.AssetBundleLoader?.PrintLoadedAssetBundleList();
            AssetLoaderManager.GetInstanceUnsafe.AssetBundleLoader?.PrintLoadedAssetBundleResourceList();
#endif
            
#if APPLY_PRINT_LOG
            CustomDebug.LogWarning("씬 로드 완료 작업이 성공했습니다.");
#endif
        }
        
        #endregion
    }
}