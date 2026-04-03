using System;

namespace k514.Mono.Common
{
    public interface IParticleModelDataTableBridge<out RecordBridge> : IGameEntityModelDataTableBridge<RecordBridge>, ITableBridgeLabel<ParticleModelDataTableQuery.TableLabel>
    {
    }

    public interface IParticleModelDataTableRecordBridge : IGameEntityModelDataTableRecordBridge
    {
    }
    
    public abstract class ParticleModelDataTable<Table, Record, RecordBridge> : GameEntityModelDataTable<Table, Record, RecordBridge>, IParticleModelDataTableBridge<RecordBridge>
        where Table : ParticleModelDataTable<Table, Record, RecordBridge>, new() 
        where Record : ParticleModelDataTable<Table, Record, RecordBridge>.ParticleModelDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IParticleModelDataTableRecordBridge
    {
        #region <Fields>

        protected ParticleModelDataTableQuery.TableLabel _ParticleModelLabel;
        ParticleModelDataTableQuery.TableLabel ITableBridgeLabel<ParticleModelDataTableQuery.TableLabel>.TableLabel => _ParticleModelLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public class ParticleModelDataTableRecord : GameEntityModelDataTableRecord, IParticleModelDataTableRecordBridge
        {
        }

        #endregion

        #region <Callbacks>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _GameEntityModelLabel = GameEntityModelDataTableQuery.TableLabel.Particle;
        }

        #endregion
    }
}