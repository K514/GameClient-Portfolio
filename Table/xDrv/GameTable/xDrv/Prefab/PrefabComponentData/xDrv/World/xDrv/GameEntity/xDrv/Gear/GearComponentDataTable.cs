using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// Gear 컴포넌트 테이블 클래스
    /// </summary>
    public class GearComponentDataTable : GameEntityComponentDataTable<GearComponentDataTable, GearComponentDataTable.TableRecord, IGameEntityComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// Grid 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : GameEntityComponentDataTableRecord
        {
            #region <Methods>

            protected override void TryInitiateFallbackComponent(GearComponentDataTable p_Self)
            {
                MainComponentType = typeof(DefaultGear);
            }

            #endregion
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _GameEntityComponentLabel = GameEntityComponentDataTableQuery.TableLabel.Gear;
            StartIndex = 130_000_000;
            EndIndex = 131_000_000;
        }
        
        #endregion
    }
}