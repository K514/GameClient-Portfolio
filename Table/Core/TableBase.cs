using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using k514.Mono.Common;
using xk514;

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;
#endif

namespace k514
{
    /// <summary>
    /// 게임 데이터를 기술하는 추상 클래스
    /// 게임 데이터 값들을 테이블로 관리할 때 사용한다.
    /// 각 게임 데이터는 하나의 싱글톤으로 구성된다
    ///
    /// 기존에는 싱글톤을 상속받았으나, 테이블의 크기가 큰 경우에
    /// 테이블 로드 타임이 오래걸리게 되어 비동기 싱글톤을 상속받도록 변경되었다.
    /// 
    /// </summary>
    /// <typeparam name="Key">테이블 키 타입</typeparam>
    /// <typeparam name="Record">테이블 레코드 타입</typeparam>
    public abstract partial class TableBase<Table, Meta, Key, Record> : AsyncSingleton<Table>, ITable<Meta, Key, Record>
        where Table : TableBase<Table, Meta, Key, Record>, new()
        where Meta : TableMetaData, new()
        where Record : TableBase<Table, Meta, Key, Record>.TableRecordBase, new() 
    {
        #region <Fields>

        /// <summary>
        /// [키값, 레코드] 컬렉션
        /// </summary>
        protected Dictionary<Key, Record> _Table;

        /// <summary>
        /// 테이블 타입
        /// </summary>
        private TableTool.TableType _TableType;

        /// <summary>
        /// 테이블 타입 프로퍼티
        /// </summary>
        public TableTool.TableType TableType { get; protected set; }

        /// <summary>
        /// 테이블 직렬화 타입
        /// </summary>
        public TableTool.TableSerializeType TableSerializeType { get; protected set; }

        /// <summary>
        /// 테이블 접근자
        /// </summary>
        public Dictionary<Key, Record> GetTable()
        {
            return _Table;
        }

        #endregion

        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            _Dependencies.Add(typeof(TableManager));
            _Dependencies.Add(typeof(TableLoader));
#if UNITY_EDITOR
            _Dependencies.Add(typeof(TableModifier));
#endif
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            OnCreateTableData();
            
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 게임 데이터 싱글톤 초기화 : 딱히 수행할 것 없음.
        /// 테이블 최초 생성 시, 테이블 리로딩 시 테이블이 완성된 상태에서
        /// 수행할 테이블 외 초기화 작업을 기술한다.
        /// </summary>
        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await LoadTable(p_CancellationToken);
            
#if APPLY_PRINT_LOG
            var recordCount = GetTable().Count;
            if (recordCount < 1)
            {
                if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
                {
                    CustomDebug.LogError((this, $"텅빈 테이블입니다. {GetTableStateLog()}", Color.yellow));
                }
            }
            else
            {
                if (CustomDebug.CustomDebugLogFlag.PrintTableLog.HasOpen())
                {
                    CustomDebug.Log((this, $"[Record Count : {recordCount}] {GetTableStateLog()}"));
                }
            }
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// 테이블 파일이 생성된 경우 수행할 작업을 기술하는 콜백
        /// </summary>
        private async UniTask OnWriteTableTextFile(string p_WriteFullPath, CancellationToken p_CancellationToken)
        {
            p_CancellationToken.ThrowIfCancellationRequested();

            if (TableType != TableTool.TableType.SystemTable)
            {
                var tableName = p_WriteFullPath.GetFileNameFromPath(true);
                if (ResourceListTable.GetInstanceUnsafe.TryGetRecord(tableName, out var o_Record))
                {
                    if (!string.Equals(o_Record.ResourceFullPath, p_WriteFullPath))
                    {
#if APPLY_PRINT_LOG
                        CustomDebug.LogError($"테이블 레코드 경로 불일치! [{o_Record.ResourceFullPath}] != [{p_WriteFullPath}] "); 
#endif
                        await ResourceListTable.GetInstanceUnsafe.AddRecord(tableName, true, p_CancellationToken, string.Empty, p_WriteFullPath);
                        SystemBoot.UpdateResourceListTableFlag = true;
                    }
                }
                else
                {
    #if APPLY_PRINT_LOG
                    CustomDebug.LogError($"{tableName} 이 생성되어, 리소스 리스트에 추가되었습니다."); 
    #endif
                    await ResourceListTable.GetInstanceUnsafe.AddRecord(tableName, false, p_CancellationToken, string.Empty, p_WriteFullPath);
                    SystemBoot.UpdateResourceListTableFlag = true;
                }
            }
            
            AssetDatabase.Refresh();
        }
#endif

        /// <summary>
        /// 메타 데이터가 초기화되는 경우 호출되는 콜백
        /// </summary>
        protected void OnMetaDataBlowUp()
        {
        }
        
        /// <summary>
        /// 테이블 데이터가 초기화되는 경우 호출되는 콜백
        /// </summary>
        protected void OnTableBlowUp()
        {
            if (!ReferenceEquals(null, _Table))
            {
                ClearTable(false);
            }
        }

        /// <summary>
        /// 게임 데이터 싱글톤 파기 : 정적 변수 초기화
        ///
        /// xml 테이블 자체는 로드와 동시에 제거되고, 메모리에 GameData 인스턴스 형태로 올라온
        /// 해당 인스턴스를 제거하는 쪽이 Dispose 계열 메서드이다.
        /// 
        /// </summary>
        protected override void OnDisposeSingleton()
        {
            OnDisposeLifeCycle();
            OnMetaDataBlowUp();
            OnTableBlowUp();
            
            _TableStateFlag = TableTool.TableStateFlag.None;
            
            base.OnDisposeSingleton();
        }

        #endregion
    }
}