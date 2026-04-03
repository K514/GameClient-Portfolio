using System;

namespace k514.Mono.Common
{
    public interface IGeometryModuleDataTableBridge : IGameEntityModuleDataTableBridge<IGeometryModuleDataTableRecordBridge>, ITableBridgeLabel<GeometryModuleDataTableQuery.TableLabel>
    {
    }    
    
    public interface IGeometryModuleDataTableRecordBridge : IGameEntityModuleDataTableRecordBridge
    {
    }

    public abstract class GeometryModuleDataTable<Table, Record> : GameEntryModuleDataTable<Table, TableMetaData, Record, IGeometryModuleDataTableRecordBridge>, IGeometryModuleDataTableBridge
        where Table : GeometryModuleDataTable<Table, Record>, new()
        where Record : GeometryModuleDataTable<Table, Record>.GeometryModuleTableRecord, new()
    {
        #region <Fields>

        protected GeometryModuleDataTableQuery.TableLabel _GeometryModuleTableLabel;
        GeometryModuleDataTableQuery.TableLabel ITableBridgeLabel<GeometryModuleDataTableQuery.TableLabel>.TableLabel => _GeometryModuleTableLabel;
        
        #endregion
        
        #region <Record>

        [Serializable]
        public abstract class GeometryModuleTableRecord : GameEntityModuleTableRecord, IGeometryModuleDataTableRecordBridge
        {
        }
        
        #endregion
                
        #region <Methods>

        protected override void OnCreateTableBridge()
        {
            _ModuleLabel = GameEntityModuleDataTableQuery.TableLabel.Geometry;
        }

        #endregion
    }
}