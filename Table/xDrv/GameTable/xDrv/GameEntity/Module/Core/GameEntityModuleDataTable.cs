using System;

namespace k514.Mono.Common
{
    public interface IGameEntityModuleDataTableBridge<out RecordBridge> : ITableIndexBridge<TableMetaData, RecordBridge>, ITableBridgeLabel<GameEntityModuleDataTableQuery.TableLabel>
        where RecordBridge : IGameEntityModuleDataTableRecordBridge
    {
    }    
    
    public interface IGameEntityModuleDataTableRecordBridge : ITableRecord<int>
    {
    }

    public abstract class GameEntryModuleDataTable<Table, Meta, Record, RecordBridge> : GameTableIndexBridge<Table, TableMetaData, Record, RecordBridge>, IGameEntityModuleDataTableBridge<RecordBridge>
        where Table : GameEntryModuleDataTable<Table, Meta, Record, RecordBridge>, new() 
        where Meta : TableMetaData, new()
        where Record : GameEntryModuleDataTable<Table, Meta, Record, RecordBridge>.GameEntityModuleTableRecord, RecordBridge, new()
        where RecordBridge : class, IGameEntityModuleDataTableRecordBridge
    {
        #region <Fields>

        protected GameEntityModuleDataTableQuery.TableLabel _ModuleLabel;
        GameEntityModuleDataTableQuery.TableLabel ITableBridgeLabel<GameEntityModuleDataTableQuery.TableLabel>.TableLabel => _ModuleLabel;

        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class GameEntityModuleTableRecord : GameTableRecord, IGameEntityModuleDataTableRecordBridge
        {
        }

        #endregion
    }
}