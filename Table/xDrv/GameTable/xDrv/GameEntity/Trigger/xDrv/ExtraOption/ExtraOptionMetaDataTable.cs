using System;
using k514.Mono.Common;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class ExtraOptionMetaDataTable : GameTable<ExtraOptionMetaDataTable, TableMetaData, GameEntityExtraOptionTool.ExtraOptionType, ExtraOptionMetaDataTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            public int ExtraOptionLanguageIndex { get; private set; }
            
            #endregion

            #region <Methods>

            public bool TryGetLanguageRecord(out ExtraOptionLanguageDataTable.TableRecord o_Record)
            {
                return ExtraOptionLanguageDataTable.GetInstanceUnsafe.TryGetRecord(ExtraOptionLanguageIndex, out o_Record);
            }

            #endregion
        }
    }
}