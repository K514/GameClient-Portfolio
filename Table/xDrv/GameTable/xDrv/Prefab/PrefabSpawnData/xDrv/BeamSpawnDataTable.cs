using System;

namespace k514.Mono.Common
{
    public class BeamSpawnDataTable : PrefabSpawnDataTable<BeamSpawnDataTable, int, BeamSpawnDataTable.TableRecord, BeamModelDataTable, BeamModelDataTable.TableRecord, BeamComponentDataTableQuery, IBeamComponentDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}