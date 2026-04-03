using System;

namespace k514.Mono.Common
{
    public class CharacterControllerModuleDataTable : PhysicsModuleDataTable<CharacterControllerModuleDataTable, CharacterControllerModuleDataTable.TableRecord>
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
            
            _PhysicsModuleTableLabel = PhysicsModuleDataTableQuery.TableLabel.CharacterController;
            StartIndex = 3000;
            EndIndex = 4000;
        }

        #endregion
    }
}