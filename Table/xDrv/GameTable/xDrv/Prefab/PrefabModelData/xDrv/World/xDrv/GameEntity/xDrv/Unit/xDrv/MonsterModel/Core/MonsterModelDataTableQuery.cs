namespace k514.Mono.Common
{
    public class MonsterModelDataTableQuery : MultiTableIndexBase<MonsterModelDataTableQuery, TableMetaData, MonsterModelDataTableQuery.TableLabel, IMonsterModelDataTableBridge<IMonsterModelDataTableRecordBridge>, IMonsterModelDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Cave,       // 1_300_000_000 ~ 1_310_000_000
            Desert,     // 1_310_000_000 ~ 1_320_000_000
            Forest,     // 1_320_000_000 ~ 1_330_000_000
            Ice,        // 1_330_000_000 ~ 1_340_000_000
            
            Boss,       // 1_380_000_000 ~ 1_390_000_000
        }

        #endregion
    }
}