using System;

namespace k514.Mono.Common
{
    public class DefaultActionModuleDataTable : ActionModuleDataTable<DefaultActionModuleDataTable, DefaultActionModuleDataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : ActionModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _ActionModuleTableLabel = ActionModuleDataTableQuery.TableLabel.Default;
            StartIndex = 0;
            EndIndex = 1000;
        }

        #endregion
    }
}