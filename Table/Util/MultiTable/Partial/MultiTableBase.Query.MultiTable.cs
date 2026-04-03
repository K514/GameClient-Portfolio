namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        public bool TryGetTable(Key p_Key, out TableBridge o_Table)
        {
            foreach (var listKV in _LabelTableListTable)
            {
                var list = listKV.Value;
                foreach (var table in list)
                {
                    if (table.IsBridged(p_Key))
                    {
                        o_Table = table;
                        return true;
                    }
                }
            }
            
            o_Table = null;
            return false;
        }
        
        public bool TryGetFirstTable(out TableBridge o_Table)
        {
            foreach (var listKV in _LabelTableListTable)
            {
                var list = listKV.Value;
                foreach (var table in list)
                {
                    o_Table = table;
                    return true;
                }
            }

            o_Table = null;
            return false;
        }
                
        public bool TryGetRecordBridge(Key p_Key, out RecordBridge o_Record)
        {
            if (TryGetTable(p_Key, out var o_Table))
            {
                return o_Table.TryGetRecordBridge(p_Key, out o_Record);
            }
            else
            {
                o_Record = null;
                return false;
            }
        }
        
        public bool TryGetFirstRecordBridge(out RecordBridge o_Record)
        {
            if (TryGetFirstTable(out var o_Table))
            {
                return o_Table.TryGetFirstRecordBridge(out o_Record);
            }
            else
            {
                o_Record = null;
                return false;
            }
        }
        
        public bool TryGetFallbackRecordBridge(out RecordBridge o_Record)
        {
            if (TryGetFirstTable(out var o_Table))
            {
                return o_Table.TryGetFallbackRecordBridge(out o_Record);
            }
            else
            {
                o_Record = null;
                return false;
            }
        }
    }
}