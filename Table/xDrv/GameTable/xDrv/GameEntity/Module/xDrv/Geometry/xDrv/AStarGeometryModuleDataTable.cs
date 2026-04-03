using System;

namespace k514.Mono.Common
{
    public class AStarGeometryModuleDataTable : GeometryModuleDataTable<AStarGeometryModuleDataTable, AStarGeometryModuleDataTable.TableRecord>
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
            
            _GeometryModuleTableLabel = GeometryModuleDataTableQuery.TableLabel.AStar;
            StartIndex = 200;
            EndIndex = 300;
        }

        #endregion
    }
}