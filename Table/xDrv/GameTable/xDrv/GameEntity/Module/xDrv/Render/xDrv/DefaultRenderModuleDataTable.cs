using System;

namespace k514.Mono.Common
{
    public class DefaultRenderModuleDataTable : RenderModuleDataTable<DefaultRenderModuleDataTable, DefaultRenderModuleDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : RenderModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _RenderModuleTableLabel = RenderModuleDataTableQuery.TableLabel.Default;
            StartIndex = 0;
            EndIndex = 1000;
        }

        #endregion
    }
}