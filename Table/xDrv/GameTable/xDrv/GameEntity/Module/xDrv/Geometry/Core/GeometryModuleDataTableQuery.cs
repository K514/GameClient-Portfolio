namespace k514.Mono.Common
{
    public class GeometryModuleDataTableQuery : MultiTableIndexBase<GeometryModuleDataTableQuery, TableMetaData, GeometryModuleDataTableQuery.TableLabel, IGeometryModuleDataTableBridge, IGeometryModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            None,
            Affine,
            NavMesh,
            AStar,
        }

        #endregion
    }
}