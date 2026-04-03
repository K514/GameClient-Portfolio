using System;

namespace k514.Mono.Common
{
    public class AffineGeometryModuleDataTable : GeometryModuleDataTable<AffineGeometryModuleDataTable, AffineGeometryModuleDataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : GeometryModuleTableRecord
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GeometryModuleTableLabel = GeometryModuleDataTableQuery.TableLabel.Affine;
            StartIndex = 0;
            EndIndex = 1000;
        }

        #endregion
    }
}