using System;

namespace k514.Mono.Common
{
    public class GearSpawnDataTable : PrefabSpawnDataTable<GearSpawnDataTable, int, GearSpawnDataTable.TableRecord, GearModelDataTable, GearModelDataTable.TableRecord, GearComponentDataTable, GearComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }
        
        #endregion
    }
}