namespace k514.Mono.Common
{
    public class SkillDataTableQuery : MultiTableIndexBase<SkillDataTableQuery, TableMetaData, SkillDataTableQuery.TableLabel, ISkillDataTableBridge<ISkillDataTableRecordBridge>, ISkillDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Active,
            Passive,
        }

        #endregion
    }
}