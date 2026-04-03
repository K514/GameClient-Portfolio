#if !SERVER_DRIVE

using Cysharp.Threading.Tasks;
using System.Threading;

namespace k514.Mono.Common
{
    public partial class UIxControlRoot : SceneChangeEventReceiveAsyncSingleton<UIxControlRoot>, ISceneFaderContainer
    {
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            Priority = 25;
            _Dependencies.Add(typeof(UIxObjectRoot));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            OnCreateControl();
            OnCreateSceneFader();
        }
 
        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }
        
        #endregion
    }
}

#endif