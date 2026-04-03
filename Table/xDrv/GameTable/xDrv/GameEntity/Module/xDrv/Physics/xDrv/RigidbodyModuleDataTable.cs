using System;

namespace k514.Mono.Common
{
    public class RigidbodyModuleDataTable : PhysicsModuleDataTable<RigidbodyModuleDataTable, RigidbodyModuleDataTable.TableRecord>
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
            
            _PhysicsModuleTableLabel = PhysicsModuleDataTableQuery.TableLabel.Rigidbody;
            StartIndex = 2000;
            EndIndex = 3000;
        }

        #endregion
    }
}