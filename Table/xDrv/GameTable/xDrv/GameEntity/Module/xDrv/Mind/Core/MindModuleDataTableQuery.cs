namespace k514.Mono.Common
{
    public class MindModuleDataTableQuery : MultiTableIndexBase<MindModuleDataTableQuery, TableMetaData, MindModuleDataTableQuery.TableLabel, IMindModuleDataTableBridge<IMindModuleDataTableRecordBridge>, IMindModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            None,
            Bounded,
            Autonomy,
        }

        #endregion
    }
}