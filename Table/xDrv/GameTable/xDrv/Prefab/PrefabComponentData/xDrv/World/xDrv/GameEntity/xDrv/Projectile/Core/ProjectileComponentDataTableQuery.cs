namespace k514.Mono.Common
{
    /// <summary>
    /// 투사체 멀티테이블에 접근할 수 있는 쿼리 클래스
    /// </summary>
    public class ProjectileComponentDataTableQuery : MultiTableIndexBase<ProjectileComponentDataTableQuery, TableMetaData, ProjectileComponentDataTableQuery.TableLabel, IProjectileComponentDataTableBridge<IProjectileComponentDataTableRecordBridge>, IProjectileComponentDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Default,    // 110_000_000 ~ 111_000_000
        }

        #endregion
    }
}