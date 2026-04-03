using System;

namespace k514.Mono.Common
{
    public interface IMonsterModelDataTableBridge<out RecordBridge> : IUnitModelDataTableBridge<RecordBridge>, ITableBridgeLabel<MonsterModelDataTableQuery.TableLabel>
    {
    }

    public interface IMonsterModelDataTableRecordBridge : IUnitModelDataTableRecordBridge
    {
    }
    
    public abstract class MonsterModelDataTable<Table, Record, RecordBridge> : UnitModelDataTable<Table, Record, RecordBridge>, IMonsterModelDataTableBridge<RecordBridge>
        where Table : MonsterModelDataTable<Table, Record, RecordBridge>, new() 
        where Record : MonsterModelDataTable<Table, Record, RecordBridge>.MonsterModelDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IMonsterModelDataTableRecordBridge
    {
        #region <Fields>

        protected MonsterModelDataTableQuery.TableLabel _MonsterModelLabel;
        MonsterModelDataTableQuery.TableLabel ITableBridgeLabel<MonsterModelDataTableQuery.TableLabel>.TableLabel => _MonsterModelLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public class MonsterModelDataTableRecord : UnitModelDataTableRecord, IMonsterModelDataTableRecordBridge
        {
        }

        #endregion

        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _UnitModelLabel = UnitModelDataTableQuery.TableLabel.Monster;
        }

        #endregion
    }
}