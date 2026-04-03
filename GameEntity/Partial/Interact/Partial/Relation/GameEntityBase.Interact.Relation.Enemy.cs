using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private GameEntityRelation _EnemyEntityRelation;

        #endregion
        
        #region <Callbacks>
        
        private void OnCreateEnemyInteraction()
        {
            _EnemyEntityRelation = _GameEntityRelationTable[GameEntityRelationTool.GameEntityRelationType.Enemy];
        }
        
        private void OnActivateEnemyInteraction()
        {
            SetEnemySelectType(GameEntityTool.EnemySelectType.None);
        }
        
        private void OnRetrieveEnemyInteraction()
        {
            _EnemyEntityRelation.ResetNode();
        }
        
        private void OnHandleEnemyEvent(GameEntityRelationTool.GameEntityRelationEventType p_EventType, GameEntityRelationTool.GameEntityRelationEventParams p_EventParams)
        {
            switch (p_EventType)
            {
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeDead:
                    RemoveEnemy(p_EventParams.TargetEntity);
                    break;
            }
        }

        #endregion

        #region <Methods>

        public void AddEnemy(IGameEntityBridge p_TargetEntity)
        {
            _EnemyEntityRelation.AddNode(p_TargetEntity);
        }

        public void AddEnemy(List<IGameEntityBridge> p_TargetEntityList)
        {
            foreach (var entity in p_TargetEntityList)
            {
                AddEnemy(entity);
            }
        }

        public void SetEnemy(IGameEntityBridge p_TargetUnit)
        {
            _EnemyEntityRelation.SelectNode(p_TargetUnit);
        }
        
        public void RemoveEnemy(IGameEntityBridge p_TargetEntity)
        {
            _EnemyEntityRelation.RemoveNode(p_TargetEntity);
        }

        public void ClearEnemy()
        {
            _EnemyEntityRelation.ResetNode();
        }

        public HashSet<IGameEntityBridge> GetEnemyGroup()
        {
            return _EnemyEntityRelation.GetSubNodes();
        }

        public bool HasValidEnemy()
        {
            return GetCurrentEnemy().IsEngageable();
        }

        public IGameEntityBridge GetCurrentEnemy()
        {
            return _EnemyEntityRelation.CurrentSubNode;
        }

        public bool TryGetCurrentEnemy(out IGameEntityBridge o_Enemy)
        {
            o_Enemy = GetCurrentEnemy();
    
            return o_Enemy.IsEngageable();
        }
        
        public bool TryUpdateAndGetCurrentEnemy(out IGameEntityBridge o_Enemy)
        {
            UpdateEnemy();

            return TryGetCurrentEnemy(out o_Enemy);
        }

        #endregion
    }
}