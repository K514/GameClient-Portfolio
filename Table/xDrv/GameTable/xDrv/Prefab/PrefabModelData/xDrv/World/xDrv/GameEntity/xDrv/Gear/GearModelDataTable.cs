using System;

namespace k514.Mono.Common
{
    public class GearModelDataTable : GameEntityModelDataTable<GearModelDataTable, GearModelDataTable.TableRecord, IGameEntityModelDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : GameEntityModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _GameEntityModelLabel = GameEntityModelDataTableQuery.TableLabel.Gear;
            StartIndex = 500_000_000;
            EndIndex = 700_000_000;
        }

        #endregion
    }
}