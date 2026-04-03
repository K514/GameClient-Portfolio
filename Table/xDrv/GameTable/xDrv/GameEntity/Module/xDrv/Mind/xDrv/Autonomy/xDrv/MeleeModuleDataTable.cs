using System;

namespace k514.Mono.Common
{
    public class MeleeModuleDataTable : AutonomyModuleDataTable<MeleeModuleDataTable, MeleeModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : AutonomyModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _AutonomyModuleTableLabel = AutonomyModuleDataTableQuery.TableLabel.Melee;
            StartIndex = 10000;
            EndIndex = 11000;
        }

        #endregion
    }
}