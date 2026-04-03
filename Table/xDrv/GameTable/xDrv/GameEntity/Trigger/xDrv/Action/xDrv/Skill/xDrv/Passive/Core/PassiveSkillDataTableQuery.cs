namespace k514.Mono.Common
{
    public class PassiveSkillDataTableQuery : MultiTableIndexBase<PassiveSkillDataTableQuery, TableMetaData, PassiveSkillDataTableQuery.TableLabel, IPassiveSkillDataTableBridge<IPassiveSkillDataTableRecordBridge>, IPassiveSkillDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Group_0,
        }

        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            _Dependencies.Add(typeof(ExtraOptionDataTable));
        } 

        #endregion
    }
}