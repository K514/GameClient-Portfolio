#if !SERVER_DRIVE

using System;

namespace k514
{
    public class SystemLanguageDataTable : LanguageDataTable<SystemLanguageDataTable, SystemLanguageDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : LanguageDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _LanguageTableLabel = LanguageDataTableQuery.TableLabel.SystemLanguage;
            StartIndex = 0;
            EndIndex = 10_000_000;
        }

        #endregion
    }
}

#endif