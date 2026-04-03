#if !SERVER_DRIVE

using System;
using k514.Mono.Common;

namespace k514.Mono.Common
{
    /// <summary>
    /// UI 컴포넌트 테이블 클래스
    /// </summary>
    public class UIxComponentDataTable : PrefabComponentDataTable<UIxComponentDataTable, UIxComponentDataTable.TableRecord, IPrefabComponentDataTableRecordBridge>
    {
        #region <Record>

        /// <summary>
        /// UI 컴포넌트 테이블 레코드 클래스
        /// </summary>
        [Serializable]
        public class TableRecord : PrefabComponentDataTableRecord
        {
            #region <Methods>

            protected override void TryInitiateFallbackComponent(UIxComponentDataTable p_Self)
            {
                MainComponentType = typeof(UIxDefaultPanel);
            }

            #endregion
        }

        #endregion
        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            _PrefabComponentLabel = PrefabComponentDataTableQuery.TableLabel.UI;
            StartIndex = 0;
            EndIndex = 10_000_000;
        }
        
        #endregion
    }
}

#endif