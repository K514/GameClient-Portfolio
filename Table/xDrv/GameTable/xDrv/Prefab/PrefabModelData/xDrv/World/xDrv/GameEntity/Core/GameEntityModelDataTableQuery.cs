using System;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체 모델 데이터 레코드의 루트
    /// </summary>
    public class GameEntityModelDataTableQuery : MultiTableIndexBase<GameEntityModelDataTableQuery, TableMetaData, GameEntityModelDataTableQuery.TableLabel, IGameEntityModelDataTableBridge<IGameEntityModelDataTableRecordBridge>, IGameEntityModelDataTableRecordBridge>
    {
        #region <Enums>

        public enum TableLabel
        {
            Particle,       // 100_000_000 ~ 300_000_000
            Beam,           // 300_000_000 ~ 500_000_000
            Gear,           // 500_000_000 ~ 700_000_000
            Unit,           // 1_000_000_000 ~ 2_000_000_000
        }

        [Flags]
        public enum GameEntityModelAttribute
        {
            None = 0,
            BottomPivot = 1 << 0,
        }
        
        #endregion
    }
}