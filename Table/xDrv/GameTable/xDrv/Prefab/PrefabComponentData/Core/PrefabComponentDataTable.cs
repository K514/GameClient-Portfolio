using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    /// <summary>
    /// 프리팹 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IPrefabComponentDataTableBridge<out RecordBridge> : ITableIndexBridge<TableMetaData, RecordBridge>, ITableBridgeLabel<PrefabComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// 프리팹 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IPrefabComponentDataTableRecordBridge : ITableRecord<int>
    {
        /// <summary>
        /// 해당 레코드를 참조하는 프리팹은 아래 컴포넌트를 가지게 된다.
        /// </summary>
        Type MainComponentType { get; }
        
        /// <summary>
        /// 생성될 오브젝트에 곱해줄 스케일 값
        /// </summary>
        float ComponentScaleFactor { get; }
 
        /// <summary>
        /// 해당 레코드의 설명을 기술하는 필드
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// 프리팹 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class PrefabComponentDataTable<Table, Record, RecordBridge> : GameTableIndexBridge<Table, TableMetaData, Record, RecordBridge>, IPrefabComponentDataTableBridge<RecordBridge>
        where Table : PrefabComponentDataTable<Table, Record, RecordBridge>, new() 
        where Record : PrefabComponentDataTable<Table, Record, RecordBridge>.PrefabComponentDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IPrefabComponentDataTableRecordBridge
    {
        #region <Fields>

        protected PrefabComponentDataTableQuery.TableLabel _PrefabComponentLabel;
        PrefabComponentDataTableQuery.TableLabel ITableBridgeLabel<PrefabComponentDataTableQuery.TableLabel>.TableLabel => _PrefabComponentLabel;

        #endregion
        
        #region <Record>
        
        /// <summary>
        /// 프리팹 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class PrefabComponentDataTableRecord : GameTableRecord, IPrefabComponentDataTableRecordBridge
        {
            #region <Field>

            public Type MainComponentType { get; protected set; }
            public float ComponentScaleFactor { get; protected set; }
            public string Description { get; protected set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);
                
                // 레코드 초기화 시, 해당 레코드의 초기 컴포넌트 타입이 없는 경우 테이블에서 미리 지정한 타입으로 초기화 시킨다.
                if (MainComponentType == null)
                {
                    TryInitiateFallbackComponent(p_Table);
                }
                
                if (ComponentScaleFactor < CustomMath.Epsilon)
                {
                    ComponentScaleFactor = 1f;
                }
            }
            
            /// <summary>
            /// 레코드의 초기 컴포넌트 값을 각 테이블마다 미리 지정해둔 값으로 세트하는 콜백
            /// </summary>
            protected abstract void TryInitiateFallbackComponent(Table p_Self);
            
            #endregion

            #region <Operator>
#if UNITY_EDITOR
            public override string ToString()
            {
                return MainComponentType?.Name ?? "NullType";
            }

#endif
            #endregion
        }
        
        #endregion
    }
}