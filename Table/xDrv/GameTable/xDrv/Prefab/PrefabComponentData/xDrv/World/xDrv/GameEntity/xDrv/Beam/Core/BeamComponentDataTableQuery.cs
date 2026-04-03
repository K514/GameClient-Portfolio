namespace k514.Mono.Common
{
    /// <summary>
    /// 투사체 멀티테이블에 접근할 수 있는 쿼리 클래스
    /// </summary>
    public class BeamComponentDataTableQuery : MultiTableIndexBase<BeamComponentDataTableQuery, TableMetaData, BeamComponentDataTableQuery.TableLabel, IBeamComponentDataTableBridge<IBeamComponentDataTableRecordBridge>, IBeamComponentDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Default, // 120_000_000 ~ 121_000_000
        }

        #endregion
    }
}