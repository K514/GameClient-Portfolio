using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514
{
    public partial class TableBase<Table, Meta, Key, Record>
    {
        #region <Callbacks>

        private void OnCreateTableData()
        {
            OnCreateTableFlag();
            OnCreateTableName();
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 해당 테이블에 들어갈 기본 값을 테이블 컬렉션에 레코드 인스턴스로 추가하는 메서드
        /// </summary>
        protected async UniTask InitTableDefaultData(CancellationToken p_CancellationToken)
        {
            AddDefaultMetaData();
            await AddDefaultRecords(p_CancellationToken);
            await AddFallbackRecords(p_CancellationToken);
        }
        
        public int GetRecordCount()
        {
            return GetTable().Count;
        }
        
#if APPLY_PRINT_LOG
        private string GetTableStateLog()
        {
        #if UNITY_EDITOR
            return $"[Table Type : {TableType} / Defined : {TableSerializeType} / Applied : {(SystemFlagTable.IsUsingSerializedTable() ? TableSerializeType : TableTool.TableSerializeType.NoneSerialize)}]";
        #else
              return $"[Table Type : {TableType} / Defined : {TableSerializeType}]";
        #endif
        }
        
        /// <summary>
        /// 해당 테이블에 등록된 레코드를 유니티 디버그 메서드를 통해 출력하는 메서드
        /// </summary>
        public void PrintTable()
        {
            if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
            {
                var tryTable = GetTable();
                CustomDebug.LogError((this, $"*****************************************************"));
                CustomDebug.LogError((this, $"Table Name : [{GetTableFileName(TableTool.TableNameQueryType.WithMainTableName)}]"));
                CustomDebug.LogError((this, $"Table Record Count : [{GetTable().Count}]"));
                foreach (var record in tryTable)
                {
                    CustomDebug.LogError((this, record.Value.GetRecordDescription()));
                }
                CustomDebug.LogError((this, $"*****************************************************"));
            }
        }
#endif
        #endregion
    }
}