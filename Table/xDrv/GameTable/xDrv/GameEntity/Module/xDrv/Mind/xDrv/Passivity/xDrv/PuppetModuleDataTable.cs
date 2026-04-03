using System;

namespace k514.Mono.Common
{
    public class PuppetModuleDataTable : BoundedModuleDataTable<PuppetModuleDataTable, PuppetModuleDataTable.TableRecord>
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
            
            _BoundedModuleTableLabel = BoundedModuleDataTableQuery.TableLabel.Puppet;
            StartIndex = 1000;
            EndIndex = 2000;
        }
        
        #endregion
    }
}