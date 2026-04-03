using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public class DefaultUnitModelDataTable : UnitModelDataTable<DefaultUnitModelDataTable, DefaultUnitModelDataTable.TableRecord, DefaultUnitModelDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : UnitModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitModelLabel = UnitModelDataTableQuery.TableLabel.Default;
            StartIndex = 1_000_000_000;
            EndIndex = 1_100_000_000;
        }

        #endregion
    }
}