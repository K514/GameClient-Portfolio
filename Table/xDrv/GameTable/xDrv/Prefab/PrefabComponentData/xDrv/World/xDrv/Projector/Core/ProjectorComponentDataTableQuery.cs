namespace k514.Mono.Common
{
    /// <summary>
    /// 프로젝터 멀티테이블에 접근할 수 있는 쿼리 클래스
    /// </summary>
    public class ProjectorComponentDataTableQuery : MultiTableIndexBase<ProjectorComponentDataTableQuery, TableMetaData, ProjectorComponentDataTableQuery.TableLabel, IProjectorComponentDataTableBridge<IProjectorComponentDataTableRecordBridge>, IProjectorComponentDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            SingleSprite,   // 40_000_000 ~ 41_000_000
            MultiSprite,    // 41_000_000 ~ 42_000_000
        }

        #endregion
    }
}