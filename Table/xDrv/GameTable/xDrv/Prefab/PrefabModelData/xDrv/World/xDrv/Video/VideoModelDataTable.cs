#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    public class VideoModelDataTable : WorldObjectModelDataTable<VideoModelDataTable, VideoModelDataTable.TableRecord, IWorldObjectModelDataTableRecordBridge>
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

            _WorldObjectModelLabel = WorldObjectModelDataTableQuery.TableLabel.Video;
            StartIndex = 30_000_000;
            EndIndex = 40_000_000;
        }

        #endregion
    }
}

#endif