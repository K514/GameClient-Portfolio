using System;

namespace k514.Mono.Common
{
    public class ProjectileParticleModelDataTable : ParticleModelDataTable<ProjectileParticleModelDataTable, ProjectileParticleModelDataTable.TableRecord, ProjectileParticleModelDataTable.TableRecord>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ParticleModelDataTableRecord
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _ParticleModelLabel = ParticleModelDataTableQuery.TableLabel.Projectile;
            StartIndex = 140_000_000;
            EndIndex = 150_000_000;
        }

        #endregion
    }
}