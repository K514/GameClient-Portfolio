using System;

namespace k514.Mono.Common
{
    public class GridSpawnDataTable : PrefabSpawnDataTable<GridSpawnDataTable, int, GridSpawnDataTable.TableRecord, GridModelDataTable, GridModelDataTable.TableRecord, GridComponentDataTable, GridComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}