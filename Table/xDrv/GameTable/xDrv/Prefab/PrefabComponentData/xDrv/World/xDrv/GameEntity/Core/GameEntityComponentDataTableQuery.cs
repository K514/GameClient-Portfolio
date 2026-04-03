namespace k514.Mono.Common
{
    /// <summary>
    /// 프리팹 컴포넌트 데이터 레코드의 루트
    /// </summary>
    public class GameEntityComponentDataTableQuery : MultiTableIndexBase<GameEntityComponentDataTableQuery, TableMetaData, GameEntityComponentDataTableQuery.TableLabel, IGameEntityComponentDataTableBridge<IGameEntityComponentDataTableRecordBridge>, IGameEntityComponentDataTableRecordBridge>
    {
        #region <Enums>

        /// <summary>
        /// 프리팹 컴포넌트 데이터를 구분하기 위한 열거형 상수
        /// </summary>
        public enum TableLabel
        {
            Vfx,            // 100_000_000 ~ 110_000_000
            Projectile,     // 110_000_000 ~ 120_000_000
            Beam,           // 120_000_000 ~ 130_000_000
            Gear,           // 130_000_000 ~ 140_000_000
            Unit,           // 140_000_000 ~ 150_000_000
        }

        #endregion
    }
}