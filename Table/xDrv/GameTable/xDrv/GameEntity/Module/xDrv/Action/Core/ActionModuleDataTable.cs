using System;
using System.Collections.Generic;

namespace k514.Mono.Common
{
    public interface IActionModuleDataTableBridge : IGameEntityModuleDataTableBridge<IActionModuleDataTableRecordBridge>, ITableBridgeLabel<ActionModuleDataTableQuery.TableLabel>
    {
    }    
    
    public interface IActionModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
        List<ActionTool.ActionBindPreset> ActionBindPresetList { get; }
    }

    public abstract class ActionModuleDataTable<Table, Record> : GameEntryModuleDataTable<Table, TableMetaData, Record, IActionModuleDataTableRecordBridge>, IActionModuleDataTableBridge
        where Table : ActionModuleDataTable<Table, Record>, new()
        where Record : ActionModuleDataTable<Table, Record>.ActionModuleTableRecord, new()
    {
        #region <Fields>

        protected ActionModuleDataTableQuery.TableLabel _ActionModuleTableLabel;
        ActionModuleDataTableQuery.TableLabel ITableBridgeLabel<ActionModuleDataTableQuery.TableLabel>.TableLabel => _ActionModuleTableLabel;
        
        #endregion

        #region <Record>

        [Serializable]
        public abstract class ActionModuleTableRecord : GameEntityModuleTableRecord, IActionModuleDataTableRecordBridge
        {
            #region <Fields>

            /// <summary>
            /// 해당 유닛에 등록시킬 커맨드 액션 리스트
            /// </summary>
            public List<ActionTool.ActionBindPreset> ActionBindPresetList { get; protected set;}

            #endregion
        }
        
        #endregion

        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Action;
        }

        #endregion
    }
}