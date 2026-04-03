namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Fields>

        private TableTool.TableStateFlag _TableStateFlag;
        
        #endregion

        #region <Callbacks>

        private void OnCreateTableFlag()
        {     
            var keyType = typeof(Key);
            if (!keyType.IsValueType)
            {
                _TableStateFlag.AddFlag(TableTool.TableStateFlag.ReferenceTypeKey);
            } 
        }

        #endregion
        
        #region <Methods>

        protected bool HasTableState(TableTool.TableStateFlag p_Flag)
        {
            return _TableStateFlag.HasAnyFlagExceptNone(p_Flag);
        }

        #endregion
    }
}