namespace k514.Mono.Common
{
    public class UnitModelDataTableQuery : MultiTableIndexBase<UnitModelDataTableQuery, TableMetaData, UnitModelDataTableQuery.TableLabel, IUnitModelDataTableBridge<IUnitModelDataTableRecordBridge>, IUnitModelDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Default,    // 1_000_000_000 ~ 1_100_000_000
            Weapon,     // 1_100_000_000 ~ 1_200_000_000
            Player,     // 1_200_000_000 ~ 1_300_000_000
            Monster,    // 1_300_000_000 ~ 1_400_000_000
        }

        #endregion
    }
}