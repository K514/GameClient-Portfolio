using System;

namespace k514.Mono.Common
{
    public interface IPhysicsModuleDataTableBridge : IGameEntityModuleDataTableBridge<IPhysicsModuleDataTableRecordBridge>, ITableBridgeLabel<PhysicsModuleDataTableQuery.TableLabel>
    {
    }    
    
    public interface IPhysicsModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
    }
    
    public abstract class PhysicsModuleDataTable<Table, Record> : GameEntryModuleDataTable<Table, TableMetaData, Record, IPhysicsModuleDataTableRecordBridge>, IPhysicsModuleDataTableBridge
        where Table : PhysicsModuleDataTable<Table, Record>, new()
        where Record : PhysicsModuleDataTable<Table, Record>.PhysicsModuleTableRecord, new()
    {
        #region <Fields>

        protected PhysicsModuleDataTableQuery.TableLabel _PhysicsModuleTableLabel;
        PhysicsModuleDataTableQuery.TableLabel ITableBridgeLabel<PhysicsModuleDataTableQuery.TableLabel>.TableLabel => _PhysicsModuleTableLabel;
        
        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class PhysicsModuleTableRecord : GameEntityModuleTableRecord, IPhysicsModuleDataTableRecordBridge
        {
        }

        #endregion
                        
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Physics;
        }

        #endregion
    }
}