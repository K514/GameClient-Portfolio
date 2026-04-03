using System;

namespace k514.Mono.Common
{
    public interface IMindModuleDataTableBridge<out Bridge> : IGameEntityModuleDataTableBridge<Bridge>, ITableBridgeLabel<MindModuleDataTableQuery.TableLabel>
        where Bridge : IMindModuleDataTableRecordBridge
    {
    }    
    
    public interface IMindModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
    }

    public abstract class MindModuleDataTable<Table, Record, Bridge> : GameEntryModuleDataTable<Table, TableMetaData, Record, Bridge>, IMindModuleDataTableBridge<Bridge>
        where Table : MindModuleDataTable<Table, Record, Bridge>, new()
        where Record : MindModuleDataTable<Table, Record, Bridge>.MindModuleTableRecord, Bridge, new()
        where Bridge : class, IMindModuleDataTableRecordBridge
    {
        #region <Fields>

        protected MindModuleDataTableQuery.TableLabel _MindModuleTableLabel;
        MindModuleDataTableQuery.TableLabel ITableBridgeLabel<MindModuleDataTableQuery.TableLabel>.TableLabel => _MindModuleTableLabel;
        
        #endregion
        
        #region <Record>
        
        [Serializable]
        public abstract class MindModuleTableRecord : GameEntryModuleDataTable<Table, TableMetaData, Record, Bridge>.GameEntityModuleTableRecord, IMindModuleDataTableRecordBridge
        {
        }
        
        #endregion
        
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Mind;
        }

        #endregion
    }
}