using System;

namespace k514.Mono.Common
{
    public class NavMeshGeometryModuleDataTable : GeometryModuleDataTable<NavMeshGeometryModuleDataTable, NavMeshGeometryModuleDataTable.TableRecord>
    {
        #region <Record>
        
        [Serializable]
        public class TableRecord : GeometryModuleTableRecord
        {
            public int NavMeshAgentPriority { get; private set; }
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GeometryModuleTableLabel = GeometryModuleDataTableQuery.TableLabel.NavMesh;
            StartIndex = 1000;
            EndIndex = 2000;
        }

        #endregion
    }
}