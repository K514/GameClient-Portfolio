using System;
using System.Collections.Generic;

namespace k514.Mono.Common
{
    public class DefaultUnitComponentDataTable : UnitComponentDataTable<DefaultUnitComponentDataTable, DefaultUnitComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : UnitComponentDataTableRecord
        {
        }
        
        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitComponentLabel = UnitComponentDataTableQuery.TableLabel.Default;
            StartIndex = 140_000_000;
            EndIndex = 141_000_000;
        }

        #endregion
    }
}