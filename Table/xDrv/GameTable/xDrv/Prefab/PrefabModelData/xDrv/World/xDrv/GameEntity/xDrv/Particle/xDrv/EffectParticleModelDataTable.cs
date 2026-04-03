using System;

namespace k514.Mono.Common
{
    public class EffectParticleModelDataTable : ParticleModelDataTable<EffectParticleModelDataTable, EffectParticleModelDataTable.TableRecord, EffectParticleModelDataTable.TableRecord>
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

            _ParticleModelLabel = ParticleModelDataTableQuery.TableLabel.Effect;
            StartIndex = 130_000_000;
            EndIndex = 140_000_000;
        }

        #endregion
    }
}