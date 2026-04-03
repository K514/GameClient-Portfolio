namespace k514.Mono.Common
{
    public class ActionDataTableQuery : MultiTableIndexBase<ActionDataTableQuery, TableMetaData, ActionDataTableQuery.TableLabel, IActionDataTableBridge<IActionDataTableRecordBridge>, IActionDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Move,
            Jump,
            Dash,
            Guard,
            Interact,
            Skill
        }

        #endregion
    }
}