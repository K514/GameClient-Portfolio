using System;

namespace k514.Mono.Common
{
    public class VideoSpawnDataTable : PrefabSpawnDataTable<VideoSpawnDataTable, int, VideoSpawnDataTable.TableRecord, VideoModelDataTable, VideoModelDataTable.TableRecord, VideoComponentDataTable, VideoComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}