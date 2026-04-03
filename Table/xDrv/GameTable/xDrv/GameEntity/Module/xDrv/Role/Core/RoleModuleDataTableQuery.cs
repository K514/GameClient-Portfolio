namespace k514.Mono.Common
{
    public class RoleModuleDataTableQuery : MultiTableIndexBase<RoleModuleDataTableQuery, TableMetaData, RoleModuleDataTableQuery.TableLabel, IRoleModuleDataTableBridge, IRoleModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            None,
            
            /// <summary>
            /// 일반몹
            /// </summary>
            Default,
            
            /// <summary>
            /// 챔피언
            /// </summary>
            Champion,
 
            /// <summary>
            /// 종속유닛
            /// </summary>    
            Slave,
        }

        #endregion
    }
}