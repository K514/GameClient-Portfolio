using System;

namespace k514.Mono.Common
{
    public class FieldParticleModelDataTable : ParticleModelDataTable<FieldParticleModelDataTable, FieldParticleModelDataTable.TableRecord, FieldParticleModelDataTable.TableRecord>
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

            _ParticleModelLabel = ParticleModelDataTableQuery.TableLabel.Field;
            StartIndex = 120_000_000;
            EndIndex = 130_000_000;
        }

        #endregion
    }
}