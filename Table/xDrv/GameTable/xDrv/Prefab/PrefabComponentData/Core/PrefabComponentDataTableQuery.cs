namespace k514.Mono.Common
{
    /// <summary>
    /// 프리팹 컴포넌트 데이터 레코드의 루트
    /// </summary>
    public class PrefabComponentDataTableQuery : MultiTableIndexBase<PrefabComponentDataTableQuery, TableMetaData, PrefabComponentDataTableQuery.TableLabel, IPrefabComponentDataTableBridge<IPrefabComponentDataTableRecordBridge>, IPrefabComponentDataTableRecordBridge>
    {
        #region <Enums>

        /// <summary>
        /// 프리팹 컴포넌트 데이터를 구분하기 위한 열거형 상수
        /// </summary>
        public enum TableLabel
        {
            UI,     // 0 ~ 10_000_000
            World,  // 10_000_000 ~
        }

        #endregion
    }
}