using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    public class BattleStatusTable : GameTable<BattleStatusTable, TableMetaData, int, BattleStatusTable.TableRecord>
    {
        [Serializable]
        public class TableRecord : GameTableRecord
        {
            #region <Fields>

            public BattleStatusPreset BattleStatusPreset { get; private set; }

            #endregion
        }
    }
}