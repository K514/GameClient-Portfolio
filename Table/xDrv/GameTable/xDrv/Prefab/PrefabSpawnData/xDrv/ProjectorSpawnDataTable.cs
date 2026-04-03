using System;

namespace k514.Mono.Common
{
    public class ProjectorSpawnDataTable : PrefabSpawnDataTable<ProjectorSpawnDataTable, int, ProjectorSpawnDataTable.TableRecord, ProjectorModelDataTable, ProjectorModelDataTable.TableRecord, ProjectorComponentDataTableQuery, IProjectorComponentDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }

        #endregion
    }
}