namespace k514.Mono.Common
{
    /// <summary>
    /// 파티클 멀티테이블에 접근할 수 있는 쿼리 클래스
    /// </summary>
    public class VfxComponentDataTableQuery : MultiTableIndexBase<VfxComponentDataTableQuery, TableMetaData, VfxComponentDataTableQuery.TableLabel, IVfxComponentDataTableBridge<IVfxComponentDataTableRecordBridge>, IVfxComponentDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Default,    // 100_000_000 ~ 101_000_000
        }

        #endregion
    }
}