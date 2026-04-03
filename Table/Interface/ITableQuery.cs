using System.Collections.Generic;

namespace k514
{
    public interface ITableKeyQuery<Key>
    {
        bool HasKey(Key p_Key);
        bool IsAddibleKey(Key p_Key);
        List<Key> GetCurrentKeyEnumerator();
        bool TryGetFirstKey(out Key o_Key);
        bool TryGetFallbackKey(out Key o_Key);
        bool TryGetRecordObject<Record>(Key p_Key, out Record o_Record) where Record : ITableRecord<Key>;
    }
    
    public interface ITableMetaQuery<Meta> where Meta : TableMetaData
    {
        Meta GetMetaData();
    }

    public interface ITableUnsafeRecordQuery<Key, out Record>
    {
        Record this[Key p_Key] { get; }
        Record GetRecord(Key p_Key);
        Record GetRecordOrFallback(Key p_Key);
        Record GetFirstRecord();
        Record GetFallbackRecord();
        Record CastRecord(ITableRecord p_Record, bool p_FallbackFlag);
    }
    
    public interface ITableSafeRecordQuery<Key, Record>
    {
        bool TryGetRecord(Key p_Key, out Record o_Record);
        bool TryGetFirstRecord(out Record o_Record);
        bool TryGetFallbackRecord(out Record o_Record);
    }
}