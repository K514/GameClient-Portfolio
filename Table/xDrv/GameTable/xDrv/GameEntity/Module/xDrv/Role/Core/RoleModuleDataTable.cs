using System;

namespace k514.Mono.Common
{
    public interface IRoleModuleDataTableBridge : IGameEntityModuleDataTableBridge<IRoleModuleDataTableRecordBridge>, ITableBridgeLabel<RoleModuleDataTableQuery.TableLabel>
    {
    }   
    
    public interface IRoleModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
    }
    
    public abstract class RoleModuleDataTable<Table, Record> : GameEntryModuleDataTable<Table, TableMetaData, Record, IRoleModuleDataTableRecordBridge>, IRoleModuleDataTableBridge
        where Table : RoleModuleDataTable<Table, Record>, new()
        where Record : RoleModuleDataTable<Table, Record>.RoleModuleTableRecord, new()
    {
        #region <Fields>

        protected RoleModuleDataTableQuery.TableLabel _RoleModuleTableLabel;
        RoleModuleDataTableQuery.TableLabel ITableBridgeLabel<RoleModuleDataTableQuery.TableLabel>.TableLabel => _RoleModuleTableLabel;
        
        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class RoleModuleTableRecord : GameEntityModuleTableRecord, IRoleModuleDataTableRecordBridge
        {
            public float ExtraScale { get; protected set; }
            public float ExtraBaseStatusAdditiveRate { get; protected set; }
            public int ExtraBaseStatusIndex { get; protected set; }
            public int ExtraBattleStatusIndex { get; protected set; }
        }
        
        #endregion
        
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Role;
        }

        #endregion
    }
}