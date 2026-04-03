namespace k514
{
    public partial class MultiTableBase<This, Key, Meta, Label, TableBridge, RecordBridge>
    {
        #region <Methods>

        public bool TryGetRecord(Key p_Key, out object o_Record)
        {
            if (TryGetRecordBridge(p_Key, out var o_RecordBridge))
            {
                o_Record = o_RecordBridge;
                return true;
            }
            else
            {
                o_Record = null;
                return false;
            }
        }
        
        public bool TryGetFirstRecord(out object o_Record)
        {
            if (TryGetFirstRecordBridge(out var o_RecordBridge))
            {
                o_Record = o_RecordBridge;
                return true;
            }
            else
            {
                o_Record =  null;
                return false;
            }
        }

        public bool TryGetFallbackRecord(out object o_Record)
        {
            if (TryGetFallbackRecordBridge(out var o_RecordBridge))
            {
                o_Record = o_RecordBridge;
                return true;
            }
            else
            {
                o_Record =  null;
                return false;
            }
        }

        #endregion
    }
}