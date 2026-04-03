using System;
using System.Collections.Generic;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class PhaseUnitComponentDataTable : UnitComponentDataTable<PhaseUnitComponentDataTable, PhaseUnitComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : UnitComponentDataTableRecord
        {
            public List<UnitPhase> PhaseList { get; private set; }
        }
        
        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitComponentLabel = UnitComponentDataTableQuery.TableLabel.Phase;
            StartIndex = 142_000_000;
            EndIndex = 143_000_000;
        }

        #endregion
    }
}