using System;

namespace k514.Mono.Common
{
    public class VfxSpawnDataTable : PrefabSpawnDataTable<VfxSpawnDataTable, int, VfxSpawnDataTable.TableRecord, ParticleModelDataTableQuery, IParticleModelDataTableRecordBridge, VfxComponentDataTableQuery, IVfxComponentDataTableRecordBridge>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : PrefabSpawnDataTableRecord
        {
        }
        
        #endregion
    }
}