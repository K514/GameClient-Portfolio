using System.Collections.Generic;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Indexer>

        /// <summary>
        /// 인덱스 쿼리
        /// </summary>
        public Record this[Key p_Key] => GetRecord(p_Key);

        #endregion
        
        #region <Methods>

        public Record GetRecord(Key p_Key)
        {
            return _Table.GetValueOrDefault(p_Key);
        }

        public Record GetRecordOrFallback(Key p_Key)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return o_Record;
            }
            else
            {
                return GetFallbackRecord();
            }
        }

        public Record GetFirstRecord()
        {
            return _FirstRecord;
        }

        public Record GetFallbackRecord()
        {
            return _FallbackRecord;
        }
        
        #endregion
    }
}