using System;

namespace k514.Mono.Common
{
    /// <summary>
    /// 기본 레이저 컴포넌트 테이블 클래스
    /// </summary>
    public class DefaultBeamComponentDataTable : BeamComponentDataTable<DefaultBeamComponentDataTable, DefaultBeamComponentDataTable.TableRecord, IBeamComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// 기본 레이저 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : BeamComponentDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _BeamComponentLabel = BeamComponentDataTableQuery.TableLabel.Default;
            StartIndex = 120_000_000;
            EndIndex = 121_000_000;
        }

        #endregion
    }
}