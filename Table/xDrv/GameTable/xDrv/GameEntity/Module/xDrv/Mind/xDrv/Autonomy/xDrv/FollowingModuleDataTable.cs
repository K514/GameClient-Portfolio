using System;

namespace k514.Mono.Common
{
    public class FollowingModuleDataTable : AutonomyModuleDataTable<FollowingModuleDataTable, FollowingModuleDataTable.TableRecord>
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
            
            _AutonomyModuleTableLabel = AutonomyModuleDataTableQuery.TableLabel.Following;
            StartIndex = 12000;
            EndIndex = 13000;
        }

        #endregion
    }
}