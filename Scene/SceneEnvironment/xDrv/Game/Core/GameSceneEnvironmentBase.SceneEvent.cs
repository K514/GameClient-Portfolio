using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase
    {
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);

            OnCreateGameSceneEnvironment();
            SpawnPlayer();
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await base.OnSceneStart(p_CancellationToken);
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await base.OnSceneTerminate(p_CancellationToken);

            TerminateSceneDependency();
        }
    }
}