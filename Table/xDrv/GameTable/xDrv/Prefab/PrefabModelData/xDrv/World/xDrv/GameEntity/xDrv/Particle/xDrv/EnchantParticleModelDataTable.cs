using System;

namespace k514.Mono.Common
{
    public class EnchantParticleModelDataTable : ParticleModelDataTable<EnchantParticleModelDataTable, EnchantParticleModelDataTable.TableRecord, EnchantParticleModelDataTable.TableRecord>
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

            _ParticleModelLabel = ParticleModelDataTableQuery.TableLabel.Enchant;
            StartIndex = 110_000_000;
            EndIndex = 120_000_000;
        }

        #endregion
    }
}