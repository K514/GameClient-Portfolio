namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Methods>

        public virtual bool TryGetRecord(Key p_Key, out Record o_Record)
        {
            if (_Table.TryGetValue(p_Key, out o_Record))
            {
                return true;
            }
            else
            {
                o_Record = GetFallbackRecord();
                return false;
            }
        }

        public bool TryGetFirstRecord(out Record o_Record)
        {
            if (ReferenceEquals(null, _FirstRecord))
            {
                o_Record = null;
                return false;
            }
            else
            {
                o_Record = _FirstRecord;
                return true;
            }
        }

        public bool TryGetFallbackRecord(out Record o_Record)
        {
            if (ReferenceEquals(null, _FallbackRecord))
            {
                o_Record = null;
                return false;
            }
            else
            {
                o_Record = _FallbackRecord;
                return true;
            }
        }

        #endregion
    }
}