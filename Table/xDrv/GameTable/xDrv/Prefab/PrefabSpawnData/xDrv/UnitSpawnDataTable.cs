using System;

namespace k514.Mono.Common
{
    public class UnitSpawnDataTable : PrefabSpawnDataTable<UnitSpawnDataTable, int, UnitSpawnDataTable.TableRecord, UnitModelDataTableQuery, IUnitModelDataTableRecordBridge, UnitComponentDataTableQuery, IUnitComponentDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}