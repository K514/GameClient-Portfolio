#if !SERVER_DRIVE

using System;
using System.Reflection.Emit;

namespace k514.Mono.Common
{
    /// <summary>
    /// 비디오 컴포넌트 테이블 클래스
    /// </summary>
    public class VideoComponentDataTable : WorldObjectComponentDataTable<VideoComponentDataTable, VideoComponentDataTable.TableRecord, IWorldObjectComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// 비디오 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : WorldObjectComponentDataTableRecord
        {
            #region <Methods>

            protected override void TryInitiateFallbackComponent(VideoComponentDataTable p_Self)
            {
            }

            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _WorldObjectComponentLabel = WorldObjectComponentDataTableQuery.TableLabel.Video;
            StartIndex = 30_000_000;
            EndIndex = 40_000_000;
        }

        #endregion
    }
}

#endif