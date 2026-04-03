using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    /// <summary>
    /// Vfx 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IVfxComponentDataTableBridge<out RecordBridge> : IGameEntityComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<VfxComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// Vfx 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IVfxComponentDataTableRecordBridge : IGameEntityComponentDataTableRecordBridge
    {
        /// <summary>
        /// 해당 플래그가 활성화된 경우, Vfx 오브젝트는 배치 변환에 의해 회전값이 변하지 않는다.
        /// </summary>
        bool FixedRotationFlag { get; }
        
        /// <summary>
        /// Vfx 시스템 시뮬레이트 배속율
        /// </summary>
        float SimulateSpeedFactor{ get; }
    }
    
    /// <summary>
    /// Vfx 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class VfxComponentDataTable<Table, Record, RecordBridge> : GameEntityComponentDataTable<Table, Record, RecordBridge>, IVfxComponentDataTableBridge<RecordBridge>
        where Table : VfxComponentDataTable<Table, Record, RecordBridge>, new() 
        where Record : VfxComponentDataTable<Table, Record, RecordBridge>.VfxComponentDataTableRecord, RecordBridge,  new()
        where RecordBridge : class, IVfxComponentDataTableRecordBridge
    {
        #region <Fields>

        protected VfxComponentDataTableQuery.TableLabel _VfxComponentLabel;
        VfxComponentDataTableQuery.TableLabel ITableBridgeLabel<VfxComponentDataTableQuery.TableLabel>.TableLabel => _VfxComponentLabel;

        #endregion
        
        #region <Record>

        /// <summary>
        /// Vfx 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class VfxComponentDataTableRecord : GameEntityComponentDataTableRecord, IVfxComponentDataTableRecordBridge
        {
            #region <Fields>

            public bool FixedRotationFlag { get; protected set; }
            public float SimulateSpeedFactor{ get; protected set; }

            #endregion

            #region <Callbacks>

            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);
                
                // 시뮬레이션 속도가 지정되지 않았다면 1배속으로 세트한다.
                if (SimulateSpeedFactor < CustomMath.Epsilon)
                {
                    SimulateSpeedFactor = 1f;
                }
            }

            #endregion
        }
        
        #endregion
                                                        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GameEntityComponentLabel = GameEntityComponentDataTableQuery.TableLabel.Vfx;
        }
        
        #endregion
    }
}