#if !SERVER_DRIVE

using System;

namespace k514.Mono.Common
{
    public class AudioModelDataTable : WorldObjectModelDataTable<AudioModelDataTable, AudioModelDataTable.TableRecord, IWorldObjectModelDataTableRecordBridge>
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
            
            _WorldObjectModelLabel = WorldObjectModelDataTableQuery.TableLabel.Audio;
            StartIndex = 20_000_000;
            EndIndex = 30_000_000;
        }

        #endregion
    }
}

#endif