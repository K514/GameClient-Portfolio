namespace k514.Mono.Common
{
    public class AutonomyModuleDataTableQuery : MultiTableIndexBase<AutonomyModuleDataTableQuery, TableMetaData, AutonomyModuleDataTableQuery.TableLabel, IAutonomyModuleDataTableBridge, IAutonomyModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Melee,
            Coward,
            Following,
        }

        #endregion
    }
}