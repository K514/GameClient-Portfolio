namespace k514.Mono.Common
{
    public class GameEntityModuleDataTableQuery : MultiTableIndexBase<GameEntityModuleDataTableQuery, TableMetaData, GameEntityModuleDataTableQuery.TableLabel, IGameEntityModuleDataTableBridge<IGameEntityModuleDataTableRecordBridge>, IGameEntityModuleDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Action,
            Mind,
            Animation,
            Physics,
            Geometry,
            Render,
            Role
        }

        #endregion
    }
}