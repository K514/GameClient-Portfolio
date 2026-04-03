using System.Collections.Generic;

namespace k514.Mono.Common
{
    public interface IGameEntityInteraction
    {
        void OnInteractSceneStart();
        void OnInteractSceneTransition();
        void OnStrikeEntity(IGameEntityBridge p_Entity, StatusTool.StatusChangeParams p_Params);
        void OnKilledEntity(IGameEntityBridge p_Entity, StatusTool.StatusChangeParams p_Params);
        void ReserveUpdateCameraInteract();
        
        /* Enemy */
        void AddEnemy(IGameEntityBridge p_TargetEntity);
        void AddEnemy(List<IGameEntityBridge> p_TargetEntityList);
        HashSet<IGameEntityBridge> GetEnemyGroup();
        bool HasValidEnemy();
        bool TryGetCurrentEnemy(out IGameEntityBridge o_Enemy);
        bool TryUpdateAndGetCurrentEnemy(out IGameEntityBridge o_Enemy);
        void SetEnemy(IGameEntityBridge p_TargetUnit);
        void SetEnemySelectType(GameEntityTool.EnemySelectType p_Type);
        void UpdateEnemy();
        void ClearEnemy();
        
        /* Possession */
        void OnPossessedBy(IGameEntityBridge p_Master);
        void OnDiscardedBy(IGameEntityBridge p_Master);
        void AddSlave(IGameEntityBridge p_TargetEntity);
        void RemoveSlave(IGameEntityBridge p_TargetEntity);
        HashSet<IGameEntityBridge> GetSlaveGroup();
        IGameEntityBridge GetSpawner();
        bool TryGetSpawner(out IGameEntityBridge o_Spawner);
        bool HasAnyMaster();
        bool IsMaster(IGameEntityBridge p_Entity);
        IGameEntityBridge GetMaster();
        bool TryGetMaster(out IGameEntityBridge o_Master);
        
        /* Party */
        public void OnJoinPartyOf(IGameEntityBridge p_Leader);
        public void OnLeavePartyFrom(IGameEntityBridge p_Leader);
        void AddPartyMember(IGameEntityBridge p_TargetEntity);
        void RemovePartyMember(IGameEntityBridge p_TargetEntity);
        void RemovePartyMember(int p_Index);
        bool TryGetPartyMember(int p_Index, out IGameEntityBridge o_Member);
        HashSet<IGameEntityBridge> GetPartyGroup();
        IGameEntityBridge GetLeader();
        bool TryGetPartyLeader(out IGameEntityBridge o_Leader);
        bool HasAnyParty();
        bool IsPartyLeader(IGameEntityBridge p_Entity);
        
        /* Focus */
        void OnFocusRelationChanged(ValidStateType p_Type, IGameEntityBridge p_Target, float p_SqrDistance);
        void AddFocus(IGameEntityBridge p_TargetEntity);
        void AddFocus(List<IGameEntityBridge> p_TargetEntityList);
        void RemoveFocus(IGameEntityBridge p_TargetEntity);
        IGameEntityBridge GetCurrentFocus();
        (bool, IGameEntityBridge) TryGetCurrentFocus();
        HashSet<IGameEntityBridge> GetFocusGroup();
    }
}