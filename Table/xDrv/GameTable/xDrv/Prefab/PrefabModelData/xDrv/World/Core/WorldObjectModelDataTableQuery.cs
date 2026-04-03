namespace k514.Mono.Common
{
    /// <summary>
    /// 월드 오브젝트 모델 데이터 레코드의 루트
    /// </summary>
    public class WorldObjectModelDataTableQuery : MultiTableIndexBase<WorldObjectModelDataTableQuery, TableMetaData, WorldObjectModelDataTableQuery.TableLabel, IWorldObjectModelDataTableBridge<IWorldObjectModelDataTableRecordBridge>, IWorldObjectModelDataTableRecordBridge>
    {
        #region <Enums>

        /// <summary>
        /// 월드 오브젝트 모델 데이터를 구분하기 위한 열거형 상수
        /// </summary>
        public enum TableLabel
        {
            Grid,           // 10_000_000 ~ 20_000_000
            Audio,          // 20_000_000 ~ 30_000_000
            Video,          // 30_000_000 ~ 40_000_000
            Projector,      // 40_000_000 ~ 50_000_000
            GameEntity,     // 100_000_000 ~
        }

        #endregion
    }
}