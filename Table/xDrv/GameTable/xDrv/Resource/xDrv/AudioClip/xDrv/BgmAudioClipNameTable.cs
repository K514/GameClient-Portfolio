#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    public class BgmAudioClipNameTable : AudioClipNameTable<BgmAudioClipNameTable, BgmAudioClipNameTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : AudioClipNameTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _AudioClipTableLabel = AudioClipNameTableQuery.TableLabel.BGM;
            StartIndex = 1_000_000;
            EndIndex = 2_000_000;
        }

        #endregion
    }
}

#endif