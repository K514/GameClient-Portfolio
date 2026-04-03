using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public interface IPrefabModelDataTableBridge<out RecordBridge> : ITableIndexBridge<TableMetaData, RecordBridge>, ITableBridgeLabel<PrefabModelDataTableQuery.TableLabel>
    {
    }

    public interface IPrefabModelDataTableRecordBridge : ITableRecord<int>
    {
        string PrefabName { get; set; }
        float ModelScale { get; set; }
    }
    
    /// <summary>
    /// 프리팹 모델 데이터 테이블 클래스의 추상 클래스
    /// </summary>
    public abstract class PrefabModelDataTable<Table, Record, RecordBridge> : GameTableIndexBridge<Table, TableMetaData, Record, RecordBridge>, IPrefabModelDataTableBridge<RecordBridge>
        where Table : PrefabModelDataTable<Table, Record, RecordBridge>, new() 
        where Record : PrefabModelDataTable<Table, Record, RecordBridge>.PrefabModelDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IPrefabModelDataTableRecordBridge
    {
        #region <Fields>

        protected PrefabModelDataTableQuery.TableLabel _PrefabModelLabel;
        PrefabModelDataTableQuery.TableLabel ITableBridgeLabel<PrefabModelDataTableQuery.TableLabel>.TableLabel => _PrefabModelLabel;

        #endregion

        #region <Record>

        /// <summary>
        /// 프리팹 모델 데이터 테이블 레코드 클래스의 추상 클래스
        /// </summary>
        [Serializable]
        public abstract class PrefabModelDataTableRecord : GameTableRecord, IPrefabModelDataTableRecordBridge
        {
            #region <Fields>

            /// <summary>
            /// 해당 모델 데이터가 기술하는 프리팹 이름
            /// </summary>
            public string PrefabName { get; set; }

            /// <summary>
            /// 해당 프리팹 모델 스케일
            /// </summary>
            public float ModelScale { get; set; }
            
            #endregion

            #region <Callbacks>
            
            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);
                
                if (ModelScale < CustomMath.Epsilon)
                {
                    ModelScale = 1f;
                }
            }

            #endregion

            #region <Methods>
            
            public override async UniTask SetRecord(int p_Key, object[] p_RecordField, CancellationToken p_CancellationToken)
            {
                await base.SetRecord(p_Key, p_RecordField, p_CancellationToken);
                
                PrefabName = (string)p_RecordField.GetElementSafe(0);
                ModelScale = (float)p_RecordField.GetElementSafe(1);
            }
            
            #endregion
        }
        
        #endregion
    }
}