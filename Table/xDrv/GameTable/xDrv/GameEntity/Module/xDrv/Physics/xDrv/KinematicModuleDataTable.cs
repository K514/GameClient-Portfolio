using System;

namespace k514.Mono.Common
{
    public class KinematicModuleDataTable : PhysicsModuleDataTable<KinematicModuleDataTable, KinematicModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PhysicsModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _PhysicsModuleTableLabel = PhysicsModuleDataTableQuery.TableLabel.Kinematic;
            StartIndex = 1000;
            EndIndex = 2000;
        }

        #endregion
    }
}