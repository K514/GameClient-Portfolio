using System;

namespace k514.Mono.Common
{
    public class GridModelDataTable : WorldObjectModelDataTable<GridModelDataTable, GridModelDataTable.TableRecord, IWorldObjectModelDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : WorldObjectModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _WorldObjectModelLabel = WorldObjectModelDataTableQuery.TableLabel.Grid;
            StartIndex = 10_000_000;
            EndIndex = 20_000_000;
        }

        #endregion
    }
}
