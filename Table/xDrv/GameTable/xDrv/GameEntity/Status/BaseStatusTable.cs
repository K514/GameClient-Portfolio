using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    public class BaseStatusTable : GameTable<BaseStatusTable, TableMetaData, int, BaseStatusTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            public BaseStatusPreset BaseStatusPreset { get; private set; }
            
            #endregion
        }
    }
}