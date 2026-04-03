using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// Vfx 컴포넌트 테이블 클래스
    /// </summary>
    public class DefaultVfxComponentDataTable : VfxComponentDataTable<DefaultVfxComponentDataTable, DefaultVfxComponentDataTable.TableRecord, IVfxComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// Vfx 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : VfxComponentDataTableRecord
        {
            #region <Callbacks>

            protected override void TryInitiateFallbackComponent(DefaultVfxComponentDataTable p_Self)
            {
                MainComponentType = typeof(DefaultVfx);
            }

            #endregion
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _VfxComponentLabel = VfxComponentDataTableQuery.TableLabel.Default;
            StartIndex = 100_000_000;
            EndIndex = 101_000_000;
        }

        #endregion
    }
}