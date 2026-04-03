using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Callbacks>
        
        /// <summary>
        /// 테이블이 초기화된 경우 호출되는 콜백
        /// </summary>
        protected virtual void OnTableCleared()
        {
            _FirstRecord = null;
            _FallbackRecord = null;
        }
        
        /// <summary>
        /// 해당 클래스에 테이블이 로드되는 경우 호출되는 콜백
        /// </summary>
        protected virtual async UniTask OnTableLoadComplete(CancellationToken p_CancellationToken)
        {
            await InitTableDefaultData(p_CancellationToken);
        }
        
        /// <summary>
        /// 테이블 로드 실패 시에 호출되는 콜백
        /// </summary>
        private void OnTableLoadCanceled()
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
            {
                CustomDebug.LogError((this, $"{TableType} 테이블 로드가 취소되었습니다.\n{GetTableStateLog()}"));
            }
#endif
        }
        
        /// <summary>
        /// 테이블 로드 실패 시에 호출되는 콜백
        /// </summary>
        private async UniTask OnTableLoadFailed(CancellationToken p_CancellationToken)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
            {
                CustomDebug.LogError((this, $"{TableType} 테이블 로드에 실패했습니다.\n{GetTableStateLog()}", Color.yellow));
            }
#endif
            await OnTableLoadComplete(p_CancellationToken);
        }
        
        #endregion
    }
}












