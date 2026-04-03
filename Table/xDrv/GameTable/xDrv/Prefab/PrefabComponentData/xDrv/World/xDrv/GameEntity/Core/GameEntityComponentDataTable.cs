using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 엔터티 컴포넌트 테이블 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IGameEntityComponentDataTableBridge<out RecordBridge> : IWorldObjectComponentDataTableBridge<RecordBridge>, ITableBridgeLabel<GameEntityComponentDataTableQuery.TableLabel>
    {
    }

    /// <summary>
    /// 게임 엔터티 컴포넌트 테이블 레코드 클래스의 브릿지 인터페이스
    /// </summary>
    public interface IGameEntityComponentDataTableRecordBridge : IWorldObjectComponentDataTableRecordBridge
    {
        /// <summary>
        /// 해당 개체의 이름, 정보 등을 기술하는 언어 테이블 인덱스
        /// </summary>
        public int EntityLanguageId { get; }
        
        /// <summary>
        /// 기초 능력치 테이블 인덱스
        /// </summary>
        public int BaseStatusId { get; }
        
        /// <summary>
        /// 전투 능력치 테이블 인덱스
        /// </summary>
        public int BattleStatusId { get; }
        
        /// <summary>
        /// 능력치 강화 테이블 인덱스
        /// </summary>
        public int EnhanceId { get; }

        /// <summary>
        /// 초기 소속 그룹 인덱스
        /// </summary>
        public int GroupId { get; }
        
        /// <summary>
        /// 초기 레벨
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// 해당 프리팹에 등록된 모션 모듈 인덱스
        /// </summary>
        List<int> AnimationModuleList { get; }

        /// <summary>
        /// 해당 프리팹에 등록된 물리 연산 모듈 인덱스
        /// </summary>
        List<int> PhysicsModuleList { get; }

        /// <summary>
        /// 해당 프리팹에 등록된 사고 회로 모듈 인덱스
        /// </summary>
        List<int> MindModuleList { get; }

        /// <summary>
        /// 해당 유닛의 액션 타입에 적용할 유닛액션 모듈 인덱스
        /// </summary>
        List<int> ActionModuleList { get; }
        
        /// <summary>
        /// 해당 프리팹에 등록된 랜더러 모듈 인덱스
        /// </summary>
        List<int> RenderModuleList { get; }
        
        /// <summary>
        /// 해당 프리팹에 등록된 역할 모듈 인덱스
        /// </summary>
        List<int> RoleModuleList { get; }
                
        /// <summary>
        /// 해당 프리팹에 등록된 기하 모듈 인덱스
        /// </summary>
        List<int> GeometryModuleList { get; }
        
        /// <summary>
        /// 기초 능력치 프리셋
        /// </summary>
        [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)]
        public BaseStatusPreset BaseStatusPreset { get; }
        
        /// <summary>
        /// 전투 능력치 프리셋
        /// </summary>
        [TableTool.TableRecordAttribute(TableTool.TableRecordAttributeType.Runtime)] 
        public BattleStatusPreset BattleStatusPreset { get; }
        
        /// <summary>
        /// 해당 개체 언어 레코드를 리턴하는 메서드
        /// </summary>
        GameEntityLanguageDataTable.TableRecord GetEntityLanguageRecord();
        
        /// <summary>
        /// 해당 개체 강화 레코드를 리턴하는 메서드
        /// </summary>
        EnhanceTable.TableRecord GetEnhanceRecord();
        
        /// <summary>
        /// 해당 개체 그룹 프리셋을 리턴하는 메서드
        /// </summary>
        GameEntityGroupPreset GetGroupPreset();
        
        /// <summary>
        /// 해당 개체 액션 프리셋 리스트를 리턴하는 메서드
        /// </summary>
        List<ActionTool.ActionBindPreset> GetActionBindPresetList();
    }

    /// <summary>
    /// 게임 엔터티 컴포넌트 테이블 클래스의 기저 클래스
    /// </summary>
    public abstract class GameEntityComponentDataTable<Table, Record, RecordBridge> : WorldObjectComponentDataTable<Table, Record, RecordBridge>, IGameEntityComponentDataTableBridge<RecordBridge>
        where Table : GameEntityComponentDataTable<Table, Record, RecordBridge>, new()
        where Record : GameEntityComponentDataTable<Table, Record, RecordBridge>.GameEntityComponentDataTableRecord, RecordBridge, new()
        where RecordBridge : class, IGameEntityComponentDataTableRecordBridge
    {
        #region <Fields>

        protected GameEntityComponentDataTableQuery.TableLabel _GameEntityComponentLabel;
        GameEntityComponentDataTableQuery.TableLabel ITableBridgeLabel<GameEntityComponentDataTableQuery.TableLabel>.TableLabel => _GameEntityComponentLabel;

        #endregion
        
        #region <Record>

        /// <summary>
        /// 게임 엔터티 컴포넌트 테이블 클래스의 기저 클래스
        /// </summary>
        [Serializable]
        public abstract class GameEntityComponentDataTableRecord : WorldObjectComponentDataTableRecord, IGameEntityComponentDataTableRecordBridge
        {
            #region <Fields>

            public int EntityLanguageId { get; protected set; }
            public int BaseStatusId { get; protected set; }
            public int BattleStatusId { get; protected set; }
            public int EnhanceId { get; protected set; }
            public int GroupId { get; protected set; }
            public int Level { get; protected set; }
            public List<int> AnimationModuleList { get; protected set; }
            public List<int> PhysicsModuleList { get; protected set; }
            public List<int> MindModuleList { get; protected set; }
            public List<int> ActionModuleList { get; protected set; }
            public List<int> RenderModuleList { get; protected set; }
            public List<int> RoleModuleList { get; protected set; }
            public List<int> GeometryModuleList { get; protected set; }
            public BaseStatusPreset BaseStatusPreset { get; protected set; }
            public BattleStatusPreset BattleStatusPreset { get; protected set; }

            #endregion
            
            #region <Callbacks>
            
            public override async UniTask OnRecordAdded(Table p_Table, CancellationToken p_CancellationToken)
            {
                await base.OnRecordAdded(p_Table, p_CancellationToken);

                Level = Mathf.Max(1, Level);
                BaseStatusPreset = new BaseStatusPreset(BaseStatusId);
                BattleStatusPreset = new BattleStatusPreset(BattleStatusId);
            }
            
            #endregion
 
            #region <Operator>
#if UNITY_EDITOR
            public override string ToString()
            {
                return GetEntityLanguageRecord()?.Text ?? "NoName";
            }
#endif
            #endregion
            
            #region <Methods>

            public GameEntityLanguageDataTable.TableRecord GetEntityLanguageRecord()
            {
                return GameEntityLanguageDataTable.GetInstanceUnsafe.GetRecord(EntityLanguageId);
            }

            public EnhanceTable.TableRecord GetEnhanceRecord()
            {
                return EnhanceTable.GetInstanceUnsafe.GetRecord(EnhanceId);
            }

            public GameEntityGroupPreset GetGroupPreset()
            {
                return GameEntityGroupTable.GetInstanceUnsafe.GetRecord(GroupId).GameEntityGroupPreset;
            }

            public List<ActionTool.ActionBindPreset> GetActionBindPresetList()
            {
                if (ActionModuleList.CheckCollectionSafe())
                {
                    if (ActionModuleDataTableQuery.GetInstanceUnsafe.TryGetRecordBridge(ActionModuleList[0], out var o_ActionModule))
                    {
                        return o_ActionModule.ActionBindPresetList;
                    }
                }

                return default;
            }

            #endregion
        }
        
        #endregion
                    
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(BaseStatusTable));
            _Dependencies.Add(typeof(BattleStatusTable));
            _Dependencies.Add(typeof(GameEntityLanguageDataTable));
            _Dependencies.Add(typeof(GameEntityLanguageDataTable));
            _Dependencies.Add(typeof(ImageNameTable));
            _Dependencies.Add(typeof(GameEntityGroupTable));
            _Dependencies.Add(typeof(ActionModuleDataTableQuery));
        }
        
        protected override void OnCreateTableBridge()
        {
            _WorldObjectComponentLabel = WorldObjectComponentDataTableQuery.TableLabel.GameEntity;
        }
        
        #endregion
    }
}