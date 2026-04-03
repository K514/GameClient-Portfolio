using System.Collections.Generic;

namespace k514.Mono.Common
{
    public interface IGameEntityInspection : IGameEntityFilter, IGameEntityQuery
    {
    }
 
    public interface IGameEntityFilter
    {
        ref List<IGameEntityBridge> FilterResultGroup { get; }
        EntityQueryTool.FilterSpaceConfig GetSightRangeFilterConfig();
        bool FilterFocusEntityWithSightRange
        (
            EntityQueryTool.FilterStateQueryFlagType p_FilterQueryFlagMask = EntityQueryTool.FilterStateQueryFlagType.FreeAll | EntityQueryTool.FilterStateQueryFlagType.ExceptMe,
            GameEntityTool.EntityStateType p_FilterStateMask = GameEntityTool.EntityStateType.DEAD, 
            GameEntityTool.GameEntityGroupRelateType p_FilterGroupRelateMask = GameEntityTool.GameEntityGroupRelateType.Enemy,
            EntityQueryTool.FilterResultType p_FilterResultType = EntityQueryTool.FilterResultType.None
        );
    }
    
    public interface IGameEntityQuery
    {
        /// <summary>
        /// 해당 개체가 플레이어인지 검증하는 프로퍼티
        /// </summary>
        bool IsPlayer { get; }

        /// <summary>
        /// 해당 개체가 플레이어에 종속되어있는지 검증하는 프로퍼티
        /// </summary>
        bool IsOwnedPlayer { get; }
                
        /// <summary>
        /// 해당 개체가 플레이어 파티에 속해있는지 검증하는 프로퍼티
        /// </summary>
        bool IsJoinedPlayer { get; }
        
        /// <summary>
        /// 해당 개체가 보스인지 검증하는 프로퍼티
        /// </summary>
        bool IsBoss { get; }
        
        /// <summary>
        /// 해당 개체가 동작중인지 검증하는 프로퍼티
        /// </summary>
        bool IsFunctional { get; }

        /// <summary>
        /// 해당 개체가 정지중인지 검증하는 프로퍼티
        /// </summary>
        bool IsDisable { get; }

        /// <summary>
        /// 해당 개체가 불멸인지 검증하는 프로퍼티
        /// </summary>
        bool IsImmortal { get; }
        
        /// <summary>
        /// 해당 개체가 무적인지 검증하는 프로퍼티
        /// </summary>
        bool IsInvincible { get; }

        /// <summary>
        /// 해당 개체가 살아있는지 검증하는 프로퍼티
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// 해당 개체가 사망했는지 검증하는 프로퍼티
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// 해당 개체가 착지상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsGround { get; }

        /// <summary>
        /// 해당 개체가 체공상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsFloat { get; }
        
        /// <summary>
        /// 해당 개체가 사출상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsLaunched { get; }

        /// <summary>
        /// 해당 개체가 피격상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsStuck { get; }
        
        /// <summary>
        /// 해당 개체가 그로기상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsGroggy { get; }
        
        /// <summary>
        /// 해당 개체가 이벤트를 진행 중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingEvent { get; }
        
        /// <summary>
        /// 해당 개체가 진행하는 이벤트가 없음을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeEvent { get; }
        
        /// <summary>
        /// 해당 개체가 이동 중인지 검증하는 프로퍼티
        /// </summary>
        bool IsDrivingMove { get; }
        
        /// <summary>
        /// 해당 개체가 이동 중이 아님을 검증하는 프로퍼티
        /// </summary>
        bool IsFreeMove { get; }
        
        /// <summary>
        /// 해당 개체가 명령 수행중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingOrder { get; }
                
        /// <summary>
        /// 해당 개체가 수행중인 명령이 없음을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeOrder { get; }
  
        /// <summary>
        /// 해당 개체가 액션 진행상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingAction { get; }

        /// <summary>
        /// 해당 개체가 진행하는 액션이 없음을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeAction { get; }
        
        /// <summary>
        /// 해당 개체가 상호작용 업데이터인지 검증하는 프로퍼티
        /// </summary>
        bool IsInteractionUpdater { get; }

        /// <summary>
        /// 해당 개체가 이동 상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsNavigating { get; }
        
        /// <summary>
        /// 해당 개체가 대기 상태인지 검증하는 프로퍼티
        /// </summary>
        bool IsFreeNavigate { get; }
#if !SERVER_DRIVE
        /// <summary>
        /// 해당 유닛이 카메라 포커스 대상인지 표시하는 플래그
        /// </summary>
        bool IsFocused { get; }
#endif
        /// <summary>
        /// 해당 오브젝트가 현재 가시 상태인지 표시하는 플래그
        /// </summary>
        bool IsObjectVisible { get; }
        
        /// <summary>
        /// 해당 오브젝트를 추적하는 UI가 현재 가시 상태인지 표시하는 플래그
        /// </summary>
        bool IsObjectUIVisible { get; }
    }
}