using System;

namespace k514.Mono.Common
{
    public class SlaveModuleDataTable : RoleModuleDataTable<SlaveModuleDataTable, SlaveModuleDataTable.TableRecord>
    {
        #region <Enums>

        public enum MasterContractType
        {
            /// <summary>
            /// 주인이 없음, 별도 함수로 주인을 지정해야함
            /// </summary>
            None,
            
            /// <summary>
            /// 처음 조우한 유닛이 주인이 됨
            /// </summary>
            FirstEncounter,
            
            /// <summary>
            /// 시스템 플레이어가 주인이 됨
            /// </summary>
            Player,
        }

        #endregion
        
        #region <Record>

        [Serializable]
        public class TableRecord : RoleModuleTableRecord
        {
            public MasterContractType MasterContractType { get; private set; }
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _RoleModuleTableLabel = RoleModuleDataTableQuery.TableLabel.Slave;
            StartIndex = 1000;
            EndIndex = 2000;
        }

        #endregion
    }
}