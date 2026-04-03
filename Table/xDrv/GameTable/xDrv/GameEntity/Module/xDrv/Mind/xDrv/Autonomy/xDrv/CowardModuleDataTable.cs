using System;

namespace k514.Mono.Common
{
    public class CowardModuleDataTable : AutonomyModuleDataTable<CowardModuleDataTable, CowardModuleDataTable.TableRecord>
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
            
            _AutonomyModuleTableLabel = AutonomyModuleDataTableQuery.TableLabel.Coward;
            StartIndex = 11000;
            EndIndex = 12000;
        }

        #endregion
    }
}