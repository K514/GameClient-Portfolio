using System;

namespace k514.Mono.Common
{
    public interface IRenderModuleDataTableBridge : IGameEntityModuleDataTableBridge<IRenderModuleDataTableRecordBridge>, ITableBridgeLabel<RenderModuleDataTableQuery.TableLabel>
    {
    } 
    
    public interface IRenderModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
        RenderableTool.ShaderControlType ShaderTypeMask { get; }
    }
    
    public abstract class RenderModuleDataTable<Table, Record> : GameEntryModuleDataTable<Table, TableMetaData, Record, IRenderModuleDataTableRecordBridge>, IRenderModuleDataTableBridge
        where Table : RenderModuleDataTable<Table, Record>, new()
        where Record : RenderModuleDataTable<Table, Record>.RenderModuleTableRecord, new()
    {
        #region <Fields>

        protected RenderModuleDataTableQuery.TableLabel _RenderModuleTableLabel;
        RenderModuleDataTableQuery.TableLabel ITableBridgeLabel<RenderModuleDataTableQuery.TableLabel>.TableLabel => _RenderModuleTableLabel;
        
        #endregion

        #region <Record>

        [Serializable]
        public abstract class RenderModuleTableRecord : GameEntityModuleTableRecord, IRenderModuleDataTableRecordBridge
        {
            /// <summary>
            /// 해당 셰이더 모듈이 전이할 수 없는 셰이더 타입
            /// </summary>
            public RenderableTool.ShaderControlType ShaderTypeMask { get; protected set; }
        }

        #endregion
                                
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Render;
        }

        #endregion
    }
}