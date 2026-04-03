using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IGameEntityBridge : IWorldObjectBridge, IGameEntityModuleContainer, ICameraFocusable,
        IGameEntityState, IGameEntityStatus, IGameEntityGroup, IGameEntityInspection, 
        IGameEntityIdentify, IGameEntityEvent, IGameEntityInteraction
    {
        /* Label */
        GameEntityTool.GameEntityType GameEntityType { get; }
        bool IsInteractableGameEntity { get; }
        bool IsUnitEntity { get; }
        
        /* State */
        void SetDisable(bool p_Flag);
        void SetDead(bool p_Instant);
        void SetLifeSpan(float p_LiveSpan, float p_DeadSpan);
        float GetLiveSpan();

        /* Status */
        float SightRange { get; }
        float SqrSightRange { get; }
        bool HasManaEnough(float p_Mana);
        void CostMana(float p_Mana);
        void GiveDamage(StatusTool.StatusChangeParams p_Params, float p_DamageRate = 1f);
        void GiveDamage(float p_Damage, StatusTool.StatusChangeParams p_Params);
        void Absorb();
        void HealHP(float p_Value);
        void HealRateHP(float p_Rate);
        void HealMP(float p_Value);
        void HealRateMP(float p_Rate);
        float GetCurrentStatusRate(BattleStatusTool.BattleStatusType p_Type);
        void SyncCurrentStatusToTotal();
#if APPLY_PRINT_LOG
        void PrintStatusInfo();
#endif
        /* Name*/
        void SetPostFix(string p_Content);
        string GetName();
        string GetModelName();
        Sprite GetPortrait();
        
        /* Volume */
        void OnTriggerEnterFrom(Collider other);
        void SetPhysicsCollideEnable(bool p_Flag);
        bool IsIntersectWith(CustomCircle p_Circle);
        bool IsIntersectWith(CustomPlane p_Plane);
#if UNITY_EDITOR
        void DrawCollider(Color p_Color, float p_Duration);
#endif
        /* Animation */
        int GetAnimatorIndex();
        
        /* Particle*/
        void AttachParticle(int p_Index, Vector3 p_Position);
        void RemoveAttachedParticle(int p_Index);
        void ResetAttachedParticle();
        
        /* Level */
        /// <summary>
        /// 강화 레코드를 바꾸는 메서드
        /// </summary>
        bool TryChangeEnhanceRecord(int p_Index);

        /// <summary>
        /// 강화 레코드를 바꾸는 메서드
        /// </summary>
        bool TryChangeEnhanceRecord(int p_Index, int p_StartLevel);

        /// <summary>
        /// 만렙에 도달했는지 검증하는 메서드
        /// </summary>
        /// <returns></returns>
        bool IsLevelCounterStop();

        /// <summary>
        /// 레벨을 지정한 값으로 변경하는 메서드
        /// </summary>
        void SetLevel(int p_Level);

        /// <summary>
        /// 레벨을 지정한 값만큼 더하는 메서드
        /// </summary>
        void AddLevel(int p_Value);

        /// <summary>
        /// 현재 레벨을 리턴하는 메서드
        /// </summary>
        int GetLevel();
        
        /* Inventory */
        bool HasItem(int p_ItemIndex);
        void SetItem(int p_ItemIndex, int p_Number = 1);
        void AddItem(int p_ItemIndex, int p_Number = 1);
        void RemoveSlot(int p_Index);
        void ClearInventory();
        Dictionary<int, GameEntityItemTool.GameItemSlot> GetInventory();
        int GetInventoryCount();
        bool TryGetItemSlot(int p_Index, out GameEntityItemTool.GameItemSlot o_Slot);
#if UNITY_EDITOR
        void PrintInventoryInfo();
#endif
    }
}