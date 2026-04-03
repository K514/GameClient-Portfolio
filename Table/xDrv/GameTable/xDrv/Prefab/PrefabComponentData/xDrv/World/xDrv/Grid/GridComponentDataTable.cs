using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    /// <summary>
    /// Grid 컴포넌트 테이블 클래스
    /// </summary>
    public class GridComponentDataTable : WorldObjectComponentDataTable<GridComponentDataTable, GridComponentDataTable.TableRecord, IWorldObjectComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// Grid 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : WorldObjectComponentDataTableRecord
        {
            #region <Methods>

            protected override void TryInitiateFallbackComponent(GridComponentDataTable p_Self)
            {
                MainComponentType = typeof(DefaultGridObject);
            }

            #endregion
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _WorldObjectComponentLabel = WorldObjectComponentDataTableQuery.TableLabel.Grid;
            StartIndex = 10_000_000;
            EndIndex = 20_000_000;
        }
        
        #endregion
    }
}
