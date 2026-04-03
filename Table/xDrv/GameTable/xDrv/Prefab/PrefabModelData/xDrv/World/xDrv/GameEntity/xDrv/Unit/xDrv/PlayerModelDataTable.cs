using System;

namespace k514.Mono.Common
{
    public class PlayerModelDataTable : UnitModelDataTable<PlayerModelDataTable, PlayerModelDataTable.TableRecord, PlayerModelDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : UnitModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitModelLabel = UnitModelDataTableQuery.TableLabel.Player;
            StartIndex = 1_200_000_000;
            EndIndex = 1_300_000_000;
        }

        #endregion
    }
}