using System;

namespace k514.Mono.Common
{
    public class DefaultRoleModuleDataTable : RoleModuleDataTable<DefaultRoleModuleDataTable, DefaultRoleModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : RoleModuleTableRecord
        {
        }

        #endregion
        
        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _RoleModuleTableLabel = RoleModuleDataTableQuery.TableLabel.Default;
            StartIndex = 0;
            EndIndex = 1000;
        }

        #endregion
    }
}