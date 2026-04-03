using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    public class EliteModuleDataTable : RoleModuleDataTable<EliteModuleDataTable, EliteModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : RoleModuleTableRecord
        {
            public GameEntityTool.ElementType ElementType { get; private set; }
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {            
            base.OnCreateTableBridge();
            
            _RoleModuleTableLabel = RoleModuleDataTableQuery.TableLabel.Champion;
            StartIndex = 2000;
            EndIndex = 3000;
        }

        #endregion
    }
}