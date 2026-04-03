namespace k514.Mono.Common
{
    /// <summary>
    /// 프리팹 모델 데이터 레코드의 루트
    /// </summary>
    public class PrefabModelDataTableQuery : MultiTableIndexBase<PrefabModelDataTableQuery, TableMetaData, PrefabModelDataTableQuery.TableLabel, IPrefabModelDataTableBridge<IPrefabModelDataTableRecordBridge>, IPrefabModelDataTableRecordBridge>
    {
        #region <Enums>

        /// <summary>
        /// 프리팹 모델 데이터를 구분하기 위한 열거형 상수
        /// </summary>
        public enum TableLabel
        {
            UI,             // 0 ~ 10_000_000
            WorldObject,    // 10_000_000 ~
        }

        #endregion
    }
}