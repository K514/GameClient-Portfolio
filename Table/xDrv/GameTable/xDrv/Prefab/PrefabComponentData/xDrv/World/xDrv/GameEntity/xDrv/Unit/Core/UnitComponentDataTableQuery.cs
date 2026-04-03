namespace k514.Mono.Common
{
    /// <summary>
    /// 프로젝터 멀티테이블에 접근할 수 있는 쿼리 클래스
    /// </summary>
    public class UnitComponentDataTableQuery : MultiTableIndexBase<UnitComponentDataTableQuery, TableMetaData, UnitComponentDataTableQuery.TableLabel, IUnitComponentDataTableBridge<IUnitComponentDataTableRecordBridge>, IUnitComponentDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Default,    // 140_000_000 ~ 141_000_000
            Armed,      // 141_000_000 ~ 142_000_000
            Phase       // 142_000_000 ~ 143_000_000
        }

        #endregion
    }
}