using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using xk514;

namespace k514.Mono.Common
{
    public partial class SceneChangeManager
    {
        public async UniTask UpdateSceneBundle(CancellationToken p_CancellationToken)
        {
            if (PrevSceneControlPreset.TryGetSceneName(out var o_PrevSceneName))
            {
                if (CurrentSceneControlPreset.TryGetSceneName(out var o_CurrentSceneName))
                {
                    if (o_PrevSceneName == o_CurrentSceneName)
                    {
                        
                    }
                    else
                    {
                        var prevSceneUnloadKey = new AssetLoadKey(o_PrevSceneName, ResourceLifeCycleType.ManualUnload);
                        var currentSceneLoadKey = new AssetLoadKey(o_CurrentSceneName, ResourceLifeCycleType.ManualUnload);
                        
                        AssetLoaderManager.GetInstanceUnsafe.UnloadAsset(prevSceneUnloadKey);
                        await AssetLoaderManager.GetInstanceUnsafe.LoadBundleSceneAsync(currentSceneLoadKey, p_CancellationToken);
                    }
                }
                else
                {
                    var prevSceneUnloadKey = new AssetLoadKey(o_PrevSceneName, ResourceLifeCycleType.ManualUnload);
                    AssetLoaderManager.GetInstanceUnsafe.UnloadAsset(prevSceneUnloadKey);
                }
            }
            else
            {
                if (CurrentSceneControlPreset.TryGetSceneName(out var o_CurrentSceneName))
                {
                    var currentSceneLoadKey = new AssetLoadKey(o_CurrentSceneName, ResourceLifeCycleType.ManualUnload);
                    await AssetLoaderManager.GetInstanceUnsafe.LoadBundleSceneAsync(currentSceneLoadKey, p_CancellationToken);
                }
            }
        }
    }
}