using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// 해당 스크립트를 Entry Point로 하여, 특정한 싱글톤 및 테이블 데이터 클래스들이
    /// Awake에서 초기화된다.
    ///
    /// 실제 플랫폼에서는 초기 싱글톤 중에서 가장 먼저 초기화되어야 다른 싱글톤들도 초기화가 가능하므로
    /// Script Execution Order가 -50으로 설정되어 있다.
    /// </summary>
    [ExecutionOrder(-50)]
    public class SystemEntry : UnityAsyncSingleton<SystemEntry>
    {
        #region <Callbacks>

        /// <summary>
        /// 실제 플랫폼에서는 SystemBoot의 InitializeOnLoadMethod 어트리뷰트가 동작하지 않으므로
        /// 해당 콜백이 시스템 진입점이 된다.
        ///
        /// 반대로 에디터 모드에서는 SystemBoot가 진입점이 된다.
        /// </summary>
        protected override async void Awake()
        {
            if (await SystemBoot.StartSystem())
            {
                base.Awake();
            }
        }
        
        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            // StartSystem에 묶여있는 busywaiting을 풀어주기 위해 1프레임 쉰다.
            await UniTask.Delay(1, cancellationToken: p_CancellationToken);
            
            SystemBoot.EnterPlayMode();
        }
        
        #endregion
    }
}