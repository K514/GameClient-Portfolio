using System;

namespace k514.Mono.Common
{
    public class AudioSpawnDataTable : PrefabSpawnDataTable<AudioSpawnDataTable, int, AudioSpawnDataTable.TableRecord, AudioModelDataTable, AudioModelDataTable.TableRecord, AudioComponentDataTable, AudioComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}