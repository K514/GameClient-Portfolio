using System;
using System.Collections.Generic;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IUnitComponentDataTableBridge<out RecordBridge> : IGameEntityComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<UnitComponentDataTableQuery.TableLabel>
    {
    }
    
    /// <summary>
    /// 유닛 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IUnitComponentDataTableRecordBridge : IGameEntityComponentDataTableRecordBridge
    {
        UnitTool.ClassType ClassType { get; }
        List<int> ProjectileSpawnIndexList { get; }
        List<int> LatentAbilityIndexList { get; }
        UnitClassDataTable.TableRecord GetClassTableRecord();
    }
    
    /// <summary>
    /// 유닛 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class UnitComponentDataTable<Table, Record> : GameEntityComponentDataTable<Table, Record, IUnitComponentDataTableRecordBridge>, IUnitComponentDataTableBridge<IUnitComponentDataTableRecordBridge>
        where Table : UnitComponentDataTable<Table, Record>, new() 
        where Record : UnitComponentDataTable<Table, Record>.UnitComponentDataTableRecord, new()
    {
        #region <Fields>

        protected UnitComponentDataTableQuery.TableLabel _UnitComponentLabel;
        UnitComponentDataTableQuery.TableLabel ITableBridgeLabel<UnitComponentDataTableQuery.TableLabel>.TableLabel => _UnitComponentLabel;

        #endregion
        
        #region <Record>

        /// <summary>
        /// 유닛 컴포넌트 테이블 레코드 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class UnitComponentDataTableRecord : GameEntityComponentDataTableRecord, IUnitComponentDataTableRecordBridge
        {
            #region <Fields>

            public UnitTool.ClassType ClassType { get; protected set; }
            public List<int> ProjectileSpawnIndexList { get; protected set; }
            public List<int> LatentAbilityIndexList { get; protected set; }

            #endregion
            
            #region <Callbacks>

            protected override void TryInitiateFallbackComponent(Table p_Self)
            {
                var labelType = p_Self._UnitComponentLabel;
                switch (labelType)
                {
                    case UnitComponentDataTableQuery.TableLabel.Default:
                        MainComponentType = typeof(DefaultUnit);
                        break;
                    case UnitComponentDataTableQuery.TableLabel.Armed:
                        MainComponentType = typeof(ArmedUnit);
                        break;
                    case UnitComponentDataTableQuery.TableLabel.Phase:
                        MainComponentType = typeof(PhaseUnit);
                        break;
                }
            }

            #endregion

            #region <Methods>

            public UnitClassDataTable.TableRecord GetClassTableRecord()
            {
                return UnitClassDataTable.GetInstanceUnsafe.GetRecord(ClassType);
            }

            #endregion
        }

        #endregion
                                                                
        #region <Callbacks>
        
        protected override void OnCreateTableBridge()
        {
            base.OnCreateTableBridge();
            
            _GameEntityComponentLabel = GameEntityComponentDataTableQuery.TableLabel.Unit;
        }
        
        #endregion
    }
}