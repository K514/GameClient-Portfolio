using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    public class GameEntityGroupTable : GameTable<GameEntityGroupTable, TableMetaData, int, GameEntityGroupTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : GameTableRecord
        {
            public GameEntityGroupPreset GameEntityGroupPreset { get; private set; }
        }

        #endregion
    }
}