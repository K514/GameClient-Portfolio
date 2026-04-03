using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 레이저 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IBeamComponentDataTableBridge<out RecordBridge> : IGameEntityComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<BeamComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// 레이저 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IBeamComponentDataTableRecordBridge : IGameEntityComponentDataTableRecordBridge
    {
    }
    
    /// <summary>
    /// 레이저 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class BeamComponentDataTable<Table, Record, RecordBridge> : GameEntityComponentDataTable<Table, Record, RecordBridge>, IBeamComponentDataTableBridge<RecordBridge>
        where Table : BeamComponentDataTable<Table, Record, RecordBridge>, new() 
        where Record : BeamComponentDataTable<Table, Record, RecordBridge>.BeamComponentDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IBeamComponentDataTableRecordBridge
    {
        #region <Fields>

        protected BeamComponentDataTableQuery.TableLabel _BeamComponentLabel;
        BeamComponentDataTableQuery.TableLabel ITableBridgeLabel<BeamComponentDataTableQuery.TableLabel>.TableLabel => _BeamComponentLabel;

        #endregion

        #region <Record>

        /// <summary>
        /// 레이저 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class BeamComponentDataTableRecord : GameEntityComponentDataTableRecord, IBeamComponentDataTableRecordBridge
        {
            #region <Callbacks>
            
            protected override void TryInitiateFallbackComponent(Table p_Self)
            {
                var tryLabelType = p_Self._BeamComponentLabel;
                
                switch (tryLabelType)
                {
                    case BeamComponentDataTableQuery.TableLabel.Default:
                        MainComponentType = typeof(DefaultBeam);
                        break;
                }
            }
            
            #endregion
        }
        
        #endregion
                                        
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GameEntityComponentLabel = GameEntityComponentDataTableQuery.TableLabel.Beam;
        }
        
        #endregion
    }
}