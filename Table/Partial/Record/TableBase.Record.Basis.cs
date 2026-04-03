using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        protected virtual async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }
    }
}