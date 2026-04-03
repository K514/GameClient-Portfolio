#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    public class EffectAudioClipNameTable : AudioClipNameTable<EffectAudioClipNameTable, EffectAudioClipNameTable.TableRecord>
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
            _AudioClipTableLabel = AudioClipNameTableQuery.TableLabel.Effect;
            StartIndex = 2_000_000;
            EndIndex = 3_000_000;
        }

        #endregion
    }
}

#endif