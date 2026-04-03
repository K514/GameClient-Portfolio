using System;

namespace k514.Mono.Common
{
    public class ProjectileSpawnDataTable : PrefabSpawnDataTable<ProjectileSpawnDataTable, int, ProjectileSpawnDataTable.TableRecord, GameEntityModelDataTableQuery, IGameEntityModelDataTableRecordBridge, ProjectileComponentDataTableQuery, IProjectileComponentDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }
        
        #endregion
    }
}