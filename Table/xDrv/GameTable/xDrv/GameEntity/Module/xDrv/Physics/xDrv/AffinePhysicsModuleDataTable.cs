using System;

namespace k514.Mono.Common
{
    public class AffinePhysicsModuleDataTable : PhysicsModuleDataTable<AffinePhysicsModuleDataTable, AffinePhysicsModuleDataTable.TableRecord>
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
            
            _PhysicsModuleTableLabel = PhysicsModuleDataTableQuery.TableLabel.Affine;
            StartIndex = 0;
            EndIndex = 1000;
        }

        #endregion
    }
}