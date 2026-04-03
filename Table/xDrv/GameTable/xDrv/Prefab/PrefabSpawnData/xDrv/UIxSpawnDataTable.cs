#if !SERVER_DRIVE

using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    public class UIxSpawnDataTable : PrefabSpawnDataTable<UIxSpawnDataTable, UIxTool.UIxElementType, UIxSpawnDataTable.TableRecord, UIxModelDataTable, UIxModelDataTable.TableRecord, UIxComponentDataTable, UIxComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}

#endif