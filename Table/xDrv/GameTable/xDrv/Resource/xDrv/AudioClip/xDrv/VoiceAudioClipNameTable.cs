#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    public class VoiceAudioClipNameTable : AudioClipNameTable<VoiceAudioClipNameTable, VoiceAudioClipNameTable.TableRecord>
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
            _AudioClipTableLabel = AudioClipNameTableQuery.TableLabel.Voice;
            StartIndex = 3_000_000;
            EndIndex = 4_000_000;
        }

        #endregion
    }
}

#endif