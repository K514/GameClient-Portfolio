using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    public interface ITableRecord
    {
        UniTask OnRecordDecoded(CancellationToken p_Cancellation);
    }

    public interface ITableRecord<Key> : ITableRecord
    {
        Key KEY { get; }
    }

    public interface ITableRecord<Key, Record> : ITableRecord<Key>
        where Record : ITableRecord<Key, Record>
    {
        void OverlapRecord(Record p_Record);
    }
}