namespace k514.Mono.Common
{
    public class ActiveSkillDataTableQuery : MultiTableIndexBase<ActiveSkillDataTableQuery, TableMetaData, ActiveSkillDataTableQuery.TableLabel, IActiveSkillDataTableBridge<IActiveSkillDataTableRecordBridge>, IActiveSkillDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Group_0,
        }

        #endregion
    }
}