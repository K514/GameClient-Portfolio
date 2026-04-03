using System;

namespace k514.Mono.Common
{
    public interface IAutonomyModuleDataTableBridge : IMindModuleDataTableBridge<IAutonomyModuleDataTableRecordBridge>, ITableBridgeLabel<AutonomyModuleDataTableQuery.TableLabel>
    {
    }    
    
    public interface IAutonomyModuleDataTableRecordBridge : IMindModuleDataTableRecordBridge
    {
    }
    
    public abstract class AutonomyModuleDataTable<Table, Record> : MindModuleDataTable<Table, Record, IAutonomyModuleDataTableRecordBridge>, IAutonomyModuleDataTableBridge
        where Table : AutonomyModuleDataTable<Table, Record>, new() 
        where Record : AutonomyModuleDataTable<Table, Record>.AutonomyModuleTableRecord, IAutonomyModuleDataTableRecordBridge, new()
    {
        #region <Fields>

        protected AutonomyModuleDataTableQuery.TableLabel _AutonomyModuleTableLabel;
        AutonomyModuleDataTableQuery.TableLabel ITableBridgeLabel<AutonomyModuleDataTableQuery.TableLabel>.TableLabel => _AutonomyModuleTableLabel;
        
        #endregion
        
        #region <Record>
        
        [Serializable]
        public abstract class AutonomyModuleTableRecord : MindModuleTableRecord, IAutonomyModuleDataTableRecordBridge
        {
        }

        #endregion
                
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();

            _MindModuleTableLabel = MindModuleDataTableQuery.TableLabel.Autonomy;
        }

        #endregion
    }
}