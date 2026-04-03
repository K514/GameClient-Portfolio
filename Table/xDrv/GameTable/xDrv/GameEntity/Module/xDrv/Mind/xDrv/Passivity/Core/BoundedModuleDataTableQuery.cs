namespace k514.Mono.Common
{
    public class BoundedModuleDataTableQuery : MultiTableIndexBase<BoundedModuleDataTableQuery, TableMetaData, BoundedModuleDataTableQuery.TableLabel, IBoundedModuleDataTableBridge, IBoundedModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Dummy,
            Puppet,
        }

        #endregion
    }
}