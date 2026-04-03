namespace k514.Mono.Common
{
    public class RenderModuleDataTableQuery : MultiTableIndexBase<RenderModuleDataTableQuery, TableMetaData, RenderModuleDataTableQuery.TableLabel, IRenderModuleDataTableBridge, IRenderModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            None,
            Default,
        }

        #endregion
    }
}