#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    public class SystemAudioClipNameTable : AudioClipNameTable<SystemAudioClipNameTable, SystemAudioClipNameTable.TableRecord>
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
            _AudioClipTableLabel = AudioClipNameTableQuery.TableLabel.System;
            StartIndex = 0;
            EndIndex = 1_000_000;
        }

        #endregion
    }
}

#endif