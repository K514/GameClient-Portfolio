using System;

namespace k514.Mono.Common
{
    /// <summary>
    /// 기본 투사체 컴포넌트 테이블 클래스
    /// </summary>
    public class DefaultProjectileComponentDataTable : ProjectileComponentDataTable<DefaultProjectileComponentDataTable, DefaultProjectileComponentDataTable.TableRecord, IProjectileComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// 기본 투사체 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : ProjectileComponentDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _ProjectileComponentLabel = ProjectileComponentDataTableQuery.TableLabel.Default;
            StartIndex = 110_000_000;
            EndIndex = 111_000_000;
        }

        #endregion
    }
}