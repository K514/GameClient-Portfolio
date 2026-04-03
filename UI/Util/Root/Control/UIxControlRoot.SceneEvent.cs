#if !SERVER_DRIVE

using Cysharp.Threading.Tasks;
using System.Threading;

namespace k514.Mono.Common
{
    public partial class UIxControlRoot
    {
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            RegisterSceneFader();
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            ClearControlStack();
            
            await UniTask.CompletedTask;
        }
    }
}

#endif