using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        public async UniTask<bool> LoadTable(CancellationToken p_CancellationToken)
        {
            if (!ReferenceEquals(null, _LabelTableListTable))
            {
                foreach (var labelTableListKV in _LabelTableListTable)
                {
                    var labelTableList = labelTableListKV.Value;
                    foreach (var labelTable in labelTableList)
                    {
                        await labelTable.LoadTable(p_CancellationToken);
                    }
                }
                
                return true;
            }
            
            return false;
        }
    }
}