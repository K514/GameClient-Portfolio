using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Fields>

        /// <summary>
        /// 전체 비동기 테스크 취소용 토큰
        /// </summary>
        private static CancellationTokenSource _SystemTaskCancellationTokenSource;

        #endregion

        #region <Callbacks>

        private static void OnInitSystemToken()
        {
            AsyncTaskTool.Reset(ref _SystemTaskCancellationTokenSource);
        }

        private static void OnReleaseSystemToken()
        {
            AsyncTaskTool.Dispose(ref _SystemTaskCancellationTokenSource);
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 시스템 전체의 비동기 작업을 취소할 수 있는 토큰을 리턴하는 메서드
        /// </summary>
        public static CancellationToken GetSystemCancellationToken()
        {
            return _SystemTaskCancellationTokenSource.Token;
        }

        /// <summary>
        /// 지정한 파라미터에 시스템 취소 토큰과 연결된 CancellationTokenSource를 생성하여 참조시키는 메서드
        /// </summary>
        public static void GetSystemLinkedCancellationTokenSource(ref CancellationTokenSource r_Token)
        {
            if (r_Token.IsValid())
            {
                r_Token.Cancel();
            }

            r_Token = CancellationTokenSource.CreateLinkedTokenSource(_SystemTaskCancellationTokenSource.Token);
        }

        /// <summary>
        /// 비동기 코드가 내부에서 취소 처리를 하는지 알 수 없는 경우, 해당 코드를 시스템 취소 토큰으로 제어하도록하여
        ///
        /// 토큰 취소 시에, 비동기 코드 내부가 취소될지 계속 실행될지는 알 수 없지만 최소한 해당 코드 이후가 취소로 인해 실행되지 않도록
        /// 보장해주는 메서드
        /// </summary>
        public static UniTask AttachSystemCancellation(this UniTask p_Task)
        {
            return p_Task.AttachExternalCancellation(_SystemTaskCancellationTokenSource.Token);
        }

        /// <summary>
        /// 비동기 코드가 내부에서 취소 처리를 하는지 알 수 없는 경우, 해당 코드를 시스템 취소 토큰으로 제어하도록하여
        ///
        /// 토큰 취소 시에, 비동기 코드 내부가 취소될지 계속 실행될지는 알 수 없지만 최소한 해당 코드 이후가 취소로 인해 실행되지 않도록
        /// 보장해주는 메서드
        /// </summary>
        public static UniTask<T> AttachSystemCancellation<T>(this UniTask<T> p_Task)
        {
            return p_Task.AttachExternalCancellation(_SystemTaskCancellationTokenSource.Token);
        }

        #endregion
    }
}