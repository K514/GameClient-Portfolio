using k514.Mono.Feature;

namespace k514.Mono.Common
{ 
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        /// <summary>
        /// 해당 개체가 플레이어인지 검증하는 프로퍼티
        /// </summary>
        public bool IsPlayer => PlayerManager.GetInstanceUnsafe?.IsPlayer(this) ?? false;

        /// <summary>
        /// 해당 개체가 플레이어에 종속되어있는지 검증하는 프로퍼티
        /// </summary>
        public bool IsOwnedPlayer => TryGetMaster(out var o_Master) ? o_Master.IsPlayer : false;
        
        /// <summary>
        /// 해당 개체가 플레이어 파티에 속해있는지 검증하는 프로퍼티
        /// </summary>
        public bool IsJoinedPlayer => TryGetPartyLeader(out var o_Leader) ? o_Leader.IsPlayer : false;

        /// <summary>
        /// 해당 개체가 보스인지 검증하는 프로퍼티
        /// </summary>
        public bool IsBoss => BossManager.GetInstanceUnsafe?.IsBoss(this) ?? false;

        /// <summary>
        /// 해당 개체가 동작중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsFunctional => !HasState_Or(GameEntityTool.EntityStateType.DISABLE);

        /// <summary>
        /// 해당 개체가 정지중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDisable => HasState_Or(GameEntityTool.EntityStateType.DISABLE);
        
        /// <summary>
        /// 해당 개체가 불멸인지 검증하는 프로퍼티
        /// </summary>
        public bool IsImmortal => HasState_Or(GameEntityTool.EntityStateType.IMMORTAL);
        
        /// <summary>
        /// 해당 개체가 무적인지 검증하는 프로퍼티
        /// </summary>
        public bool IsInvincible => HasState_Or(GameEntityTool.EntityStateType.INVINCIBLE);

        /// <summary>
        /// 해당 개체가 살아있는지 검증하는 프로퍼티
        /// </summary>
        public bool IsAlive => IsContentActive && !HasState_Or(GameEntityTool.EntityStateType.DEAD);
        
        /// <summary>
        /// 해당 개체가 사망했는지 검증하는 프로퍼티
        /// </summary>
        public bool IsDead => HasState_Or(GameEntityTool.EntityStateType.DEAD);
        
        /// <summary>
        /// 해당 개체가 착지상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsGround => !HasState_Or(GameEntityTool.EntityStateType.FLOAT);
        
        /// <summary>
        /// 해당 개체가 체공상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsFloat => HasState_Or(GameEntityTool.EntityStateType.FLOAT);
                
        /// <summary>
        /// 해당 개체가 사출상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsLaunched => HasState_Or(GameEntityTool.EntityStateType.LAUNCH);
        
        /// <summary>
        /// 해당 개체가 피격상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsStuck => HasState_Or(GameEntityTool.EntityStateType.STUCK);
        
        /// <summary>
        /// 해당 개체가 피격상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsGroggy => HasState_Or(GameEntityTool.EntityStateType.GROGGY);
        
        /// <summary>
        /// 해당 개체가 이벤트를 진행 중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingEvent => HasState_Or(GameEntityTool.EntityStateType.DRIVE_EVENT);
        
        /// <summary>
        /// 해당 개체가 진행하는 이벤트가 없음을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeEvent => !HasState_Or(GameEntityTool.EntityStateType.DRIVE_EVENT);
        
        /// <summary>
        /// 해당 개체가 명령 수행중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingOrder => HasState_Or(GameEntityTool.EntityStateType.DRIVE_ORDER);
                
        /// <summary>
        /// 해당 개체가 수행중인 명령이 없음을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeOrder => !HasState_Or(GameEntityTool.EntityStateType.DRIVE_ORDER);
        
        /// <summary>
        /// 해당 개체가 이동 중인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingMove => HasState_Or(GameEntityTool.EntityStateType.DRIVE_MOVE);
        
        /// <summary>
        /// 해당 개체가 이동 중이 아님을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeMove => !HasState_Or(GameEntityTool.EntityStateType.DRIVE_MOVE);
        
        /// <summary>
        /// 해당 개체가 액션 진행상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsDrivingAction => HasState_Or(GameEntityTool.EntityStateType.DRIVE_ACTION);

        /// <summary>
        /// 해당 개체가 진행하는 액션이 없음을 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeAction => !HasState_Or(GameEntityTool.EntityStateType.DRIVE_ACTION);

        /// <summary>
        /// 해당 개체가 위치 이벤트 송신자인지 검증하는 프로퍼티
        /// </summary>
        public bool IsInteractionUpdater => HasAttribute(GameEntityTool.GameEntityAttributeType.InteractionUpdater);

        /// <summary>
        /// 해당 개체가 이동 상태인지 검증하는 프로퍼티
        /// </summary>
        public bool IsNavigating => GeometryModule.IsOnNavigate();
                
        /// <summary>
        /// 해당 개체가 이동 상태가 아닌지 검증하는 프로퍼티
        /// </summary>
        public bool IsFreeNavigate => !GeometryModule.IsOnNavigate();


#if !SERVER_DRIVE
        /// <summary>
        /// 해당 유닛이 카메라 포커스 대상인지 표시하는 플래그
        /// </summary>
        public bool IsFocused => CameraManager.GetInstanceUnsafe?.IsTracingTarget(this) ?? false;
#endif
    }
}