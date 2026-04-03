using System;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public class UIxModelDataTable : PrefabModelDataTable<UIxModelDataTable, UIxModelDataTable.TableRecord, IPrefabModelDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _PrefabModelLabel = PrefabModelDataTableQuery.TableLabel.UI;
            StartIndex = 0;
            EndIndex = 10_000_000;
        }
        
        #endregion
    }
}

#endif