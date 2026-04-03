#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    /// <summary>
    /// 비디오 컴포넌트 테이블 클래스
    /// </summary>
    public class AudioComponentDataTable : WorldObjectComponentDataTable<AudioComponentDataTable, AudioComponentDataTable.TableRecord, IWorldObjectComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// 비디오 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : WorldObjectComponentDataTableRecord
        {
            #region <Methods>

            protected override void TryInitiateFallbackComponent(AudioComponentDataTable p_Self)
            {
            }

            #endregion
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _WorldObjectComponentLabel = WorldObjectComponentDataTableQuery.TableLabel.Audio;
            StartIndex = 20_000_000;
            EndIndex = 30_000_000;
        }

        #endregion
    }
}

#endif