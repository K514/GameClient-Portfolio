using System;

namespace k514.Mono.Common
{
    public class WeaponParticleModelDataTable : ParticleModelDataTable<WeaponParticleModelDataTable, WeaponParticleModelDataTable.TableRecord, WeaponParticleModelDataTable.TableRecord>
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

            _ParticleModelLabel = ParticleModelDataTableQuery.TableLabel.Weapon;
            StartIndex = 100_000_000;
            EndIndex = 110_000_000;
        }

        #endregion
    }
}