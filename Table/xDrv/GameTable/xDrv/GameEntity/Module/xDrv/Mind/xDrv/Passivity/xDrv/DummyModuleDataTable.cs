using System;

namespace k514.Mono.Common
{
    public class DummyModuleDataTable : BoundedModuleDataTable<DummyModuleDataTable, DummyModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : BoundedModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _BoundedModuleTableLabel = BoundedModuleDataTableQuery.TableLabel.Dummy;
            StartIndex = 0;
            EndIndex = 1000;
        }
        
        #endregion
    }
}