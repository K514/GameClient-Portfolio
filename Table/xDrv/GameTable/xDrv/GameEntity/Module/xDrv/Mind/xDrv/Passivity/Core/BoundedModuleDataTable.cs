using System;

namespace k514.Mono.Common
{
    public interface IBoundedModuleDataTableBridge : IMindModuleDataTableBridge<IBoundedModuleDataTableRecordBridge>, ITableBridgeLabel<BoundedModuleDataTableQuery.TableLabel>
    {
    }    
    
    public interface IBoundedModuleDataTableRecordBridge : IMindModuleDataTableRecordBridge
    {
    }
    
    public abstract class BoundedModuleDataTable<Table, Record> : MindModuleDataTable<Table, Record, IBoundedModuleDataTableRecordBridge>, IBoundedModuleDataTableBridge
        where Table : BoundedModuleDataTable<Table, Record>, new() 
        where Record : BoundedModuleDataTable<Table, Record>.BoundedModuleTableRecord, IBoundedModuleDataTableRecordBridge, new()
    {
        #region <Fields>

        protected BoundedModuleDataTableQuery.TableLabel _BoundedModuleTableLabel;
        BoundedModuleDataTableQuery.TableLabel ITableBridgeLabel<BoundedModuleDataTableQuery.TableLabel>.TableLabel => _BoundedModuleTableLabel;
        
        #endregion
        
        #region <Record>
        
        [Serializable]
        public abstract class BoundedModuleTableRecord : MindModuleTableRecord, IBoundedModuleDataTableRecordBridge
        {
        }

        #endregion
                
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _MindModuleTableLabel = MindModuleDataTableQuery.TableLabel.Bounded;
        }

        #endregion
    }
}