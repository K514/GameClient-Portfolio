namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Fields>

        private Record _FirstRecord;
        
        #endregion
        
        #region <Methods>

        public bool TryGetFirstKey(out Key o_Key)
        {
            if (ReferenceEquals(null, _FirstRecord))
            {
                o_Key = default;

                return false;
            }
            else
            {
                o_Key = _FirstRecord.KEY;
                
                return true;
            }
        }

        #endregion
    }
}