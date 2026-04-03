using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체 모듈의 이벤트를 기술하는 인터페이스
    /// </summary>
#if SERVER_DRIVE
    public interface IGameEntityModuleEvent : INavigateEventReceiver
#else
    public interface IGameEntityModuleEventReceiver : ICameraFocusEventReceiver, INavigateEventReceiver, IGameEntityActionEventReceiver
#endif
    {
        /// <summary>
        /// 갱신 콜백
        /// </summary>
        void OnModule_PreUpdate(float p_DeltaTime);
        
        /// <summary>
        /// 갱신 콜백
        /// PreUpdate보다 늦게 호출된다.
        /// </summary>
        void OnModule_Update(float p_DeltaTime);
        
        /// <summary>
        /// 갱신 콜백
        /// 다른 갱신 콜백보다 주기가 길다.
        /// </summary>
        void OnModule_Update_TimeBlock();
        
        /// <summary>
        /// 해당 유닛의 스케일이 변경된 경우 호출되는 콜백
        /// </summary>
        void OnModule_Update_Scale();

        /// <summary>
        /// 해당 유닛의 위치가 특정 좌표로 바뀐 경우 호출되는 콜백
        /// </summary>
        public void OnModule_PositionChanged(Vector3 p_Prev, Vector3 p_Current);

        /// <summary>
        /// 해당 유닛이 위치가 이전 위치에서 변경된 경우 호출되는 콜백
        /// </summary>
        void OnModule_PositionMoved();
        
        /// <summary>
        /// 해당 유닛의 Pivot이 특정 좌표로 바뀐 경우 호출되는 콜백
        /// </summary>
        void OnModule_PivotChanged(PositionTracer p_PositionStatePreset);
        
        /// <summary>
        /// 해당 유닛이 다른 유닛을 공격한 경우 호출되는 콜백
        /// </summary>
        void OnModule_Strike(IGameEntityBridge p_Target, DamageCalculator.HitResult p_HitResult);
        
        /// <summary>
        /// 해당 유닛이 피격당한 경우 호출되는 콜백
        /// </summary>
        void OnModule_Hit(IGameEntityBridge p_Trigger, DamageCalculator.HitResult p_HitResult);

        /// <summary>
        /// 해당 유닛의 피격 모션이 시작된 경우 호출되는 콜백
        /// </summary>
        void OnModule_HitMotion_Start();

        /// <summary>
        /// 해당 유닛의 피격 모션이 종료된 경우 호출되는 콜백
        /// </summary>
        void OnModule_HitMotion_Over();
   
        /// <summary>
        /// 사망한 경우 호출되는 콜백
        /// </summary>
        void OnModule_Dead(bool p_Instant);

        /// <summary>
        /// 해당 유닛이 체공상태로 변했을 때 호출되는 콜백
        /// </summary>
        void OnModule_BeginFloat(PhysicsTool.ForceType p_Mask, Vector3 p_CurrentForce);

        /// <summary>
        /// 점프 시전시 호출되는 콜백
        /// </summary>
        void OnModule_ManualJump();
        
        /// <summary>
        /// 해당 유닛이 착지상태로 변했을 때 호출되는 콜백
        /// </summary>
        void OnModule_ReachedGround(PhysicsTool.StampPreset p_UnitStampPreset);
        
        /// <summary>
        /// 해당 유닛이 특정 오브젝트를 밟은 경우 호출되는 콜백
        /// </summary>
        void OnModule_StampObject();
    }
}