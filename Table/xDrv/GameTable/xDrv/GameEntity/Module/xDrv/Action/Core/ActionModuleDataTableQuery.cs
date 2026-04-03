namespace k514.Mono.Common
{
    public class ActionModuleDataTableQuery : MultiTableIndexBase<ActionModuleDataTableQuery, TableMetaData, ActionModuleDataTableQuery.TableLabel, IActionModuleDataTableBridge, IActionModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            None,
            Default,
        }

        #endregion
    }
}