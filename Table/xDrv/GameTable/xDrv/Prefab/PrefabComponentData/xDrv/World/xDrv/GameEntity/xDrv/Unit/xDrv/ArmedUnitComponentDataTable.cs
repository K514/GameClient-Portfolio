using System;

namespace k514.Mono.Common
{
    public class ArmedUnitComponentDataTable : UnitComponentDataTable<ArmedUnitComponentDataTable, ArmedUnitComponentDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : UnitComponentDataTableRecord
        {
            /// <summary>
            /// 왼쪽 무기 인덱스
            /// </summary>
            public int LeftWeaponIndex { get; protected set; }
        
            /// <summary>
            /// 오른쪽 무기 인덱스
            /// </summary>
            public int RightWeaponIndex { get; protected set; }
        }
        
        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitComponentLabel = UnitComponentDataTableQuery.TableLabel.Armed;
            StartIndex = 141_000_000;
            EndIndex = 142_000_000;
        }

        #endregion
    }
}