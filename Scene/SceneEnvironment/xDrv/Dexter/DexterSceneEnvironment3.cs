using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public class DexterSceneEnvironment3 : SceneEnvironment
    {
        #region <Fields>

        private InputLayerEventReceiver _InputEventReceiver;
        
        #endregion
        
        #region <Callbacks>

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);

            Debug.LogError($"DexterSceneEnvironment3 preload");
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await base.OnSceneStart(p_CancellationToken);
            
            Debug.LogError($"DexterSceneEnvironment3 start");

            var createParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(1007);
            var spawned = UnitPoolManager.GetInstanceUnsafe.Pop(createParams, default);
            spawned.SwitchPersona(BoundedModuleDataTableQuery.TableLabel.Puppet);
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await base.OnSceneTerminate(p_CancellationToken);
                 
            Debug.LogError("DexterSceneEnvironment3 Terminate");
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            await base.OnSceneTransition(p_CancellationToken);
                
            Debug.LogError("DexterSceneEnvironment3 Transition");
        }

        #endregion
    }
}