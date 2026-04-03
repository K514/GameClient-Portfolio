namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        #region <Indexer>

        public RecordBridge this[Key p_Key] => GetRecord(p_Key);

        #endregion
        
        #region <Methods>

        public RecordBridge GetRecord(Key p_Key)
        {
            if (TryGetTable(p_Key, out var o_Table))
            {
                return o_Table.GetRecordBridge(p_Key);
            }
            else
            {
                return null;
            }
        }

        public RecordBridge GetRecordOrFallback(Key p_Key)
        {
            if (TryGetTable(p_Key, out var o_Table))
            {
                return o_Table.GetRecordBridgeOrFallback(p_Key);
            }
            else
            {
                return GetFallbackRecord();
            }
        }
        
        public RecordBridge GetFirstRecord()
        {
            if (TryGetFirstTable(out var o_Table))
            {
                return o_Table.GetFirstRecordBridge();
            }
            else
            {
                return null;
            }
        }
        
        public RecordBridge GetFallbackRecord()
        {
            if (TryGetFirstTable(out var o_Table))
            {
                return o_Table.GetFallbackRecordBridge();
            }
            else
            {
                return null;
            }
        }
        
        public RecordBridge CastRecord(ITableRecord p_Record, bool p_FallbackFlag)
        {
            if (p_FallbackFlag)
            {
                return p_Record as RecordBridge ?? GetFallbackRecord();
            }
            else
            {
                return p_Record as RecordBridge;
            }
        }
        
        #endregion
    }
}