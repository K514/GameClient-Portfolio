using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Fields>

        private Record _FallbackRecord;
        
        #endregion

        #region <Methods>

        private async UniTask AddFallbackRecords(CancellationToken p_CancellationToken)
        {
            if (ReferenceEquals(null, _FallbackRecord))
            {
                if (_Table.CheckCollectionSafe())
                {
                    _FallbackRecord = _FirstRecord;
                }
            }
            
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public bool TryGetFallbackKey(out Key o_Key)
        {
            if (ReferenceEquals(null, _FallbackRecord))
            {
                o_Key = default;

                return false;
            }
            else
            {
                o_Key = _FallbackRecord.KEY;
                
                return true;
            }
        }

        #endregion
    }
}