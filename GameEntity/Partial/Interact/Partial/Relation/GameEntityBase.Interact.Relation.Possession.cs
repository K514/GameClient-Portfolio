using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private GameEntityRelation _PossessionEntityRelation;
        protected IGameEntityBridge _Master;
        protected IGameEntityBridge _Spawner;
        private GameEntityBaseEventReceiver _MasterEventReceiver;
        
        #endregion
        
        #region <Callbacks>
        
        private void OnCreatePossessionInteraction()
        {
            _PossessionEntityRelation = _GameEntityRelationTable[GameEntityRelationTool.GameEntityRelationType.Possession];
        }

        private void OnActivatePossessionInteraction(ActivateParams p_ActivateParams)
        {
            if (HasAttribute(GameEntityTool.GameEntityAttributeType.FollowFallenMaster))
            {
                _MasterEventReceiver 
                    = new GameEntityBaseEventReceiver
                    (
                        GameEntityTool.GameEntityBaseEventType.Dead | GameEntityTool.GameEntityBaseEventType.Retrieved, 
                        OnHandleMasterEvent
                    );
            }
            
            _Spawner = p_ActivateParams.Spawner;
            if (!ReferenceEquals(null, _Spawner))
            {
                _Spawner.GetMaster().AddSlave(this);
            }
        }
        
        private void OnRetrievePossessionInteraction()
        {
            _PossessionEntityRelation.ResetNode();

            if (!ReferenceEquals(null, _MasterEventReceiver))
            {
                _MasterEventReceiver.Dispose();
                _MasterEventReceiver = null;
            }
            
            _Spawner = default;
        }
        
        private void OnHandlePossessionEvent(GameEntityRelationTool.GameEntityRelationEventType p_EventType, GameEntityRelationTool.GameEntityRelationEventParams p_EventParams)
        {
            switch (p_EventType)
            {
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeAdded:
                {
                    var slave = p_EventParams.TargetEntity;
                    slave.OnPossessedBy(this);
                    break;
                }
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeRemoved:
                {
                    var slave = p_EventParams.TargetEntity;
                    slave.OnDiscardedBy(this);
                    break;
                }
            }
        }
        
        public void OnPossessedBy(IGameEntityBridge p_Master)
        {
            if (HasAnyMaster())
            {
                if (IsMaster(p_Master))
                {
                }
                else
                {
                    var prevMaster = _Master;
                    _Master = p_Master;
                    if (HasAttribute(GameEntityTool.GameEntityAttributeType.FollowFallenMaster))
                    {
                        _Master.AddReceiver(_MasterEventReceiver);
                    }
                    prevMaster.RemoveSlave(this);
                    
                    OnMasterChanged();
                }
            }
            else
            {
                _Master = p_Master;
                if (HasAttribute(GameEntityTool.GameEntityAttributeType.FollowFallenMaster))
                {
                    _Master.AddReceiver(_MasterEventReceiver);
                }
                
                OnMasterChanged();
            }
        }

        public void OnDiscardedBy(IGameEntityBridge p_Master)
        {
            if (IsMaster(p_Master))
            {
                if (HasAttribute(GameEntityTool.GameEntityAttributeType.FollowFallenMaster))
                {
                    _Master.RemoveReceiver(_MasterEventReceiver);
                }
                _Master = null;
            }
            else
            {
                if (HasAttribute(GameEntityTool.GameEntityAttributeType.FollowFallenMaster))
                {
                    p_Master.RemoveReceiver(_MasterEventReceiver);
                }
            }
        }

        private void OnHandleMasterEvent(GameEntityTool.GameEntityBaseEventType p_Type, GameEntityBaseEventParams p_Params)
        {
            switch (p_Type)
            {
                case GameEntityTool.GameEntityBaseEventType.Dead:
                case GameEntityTool.GameEntityBaseEventType.Retrieved:
                    OnMasterFallen();
                    break;
            }
        }

        protected virtual void OnMasterChanged()
        {
        }
        
        protected virtual void OnMasterFallen()
        {
            SetDead(false);
        }
        
        #endregion
        
        #region <Methods>

        public void AddSlave(IGameEntityBridge p_TargetEntity)
        {
            _PossessionEntityRelation.AddNode(p_TargetEntity);
        }
        
        public void RemoveSlave(IGameEntityBridge p_TargetEntity)
        {
            _PossessionEntityRelation.RemoveNode(p_TargetEntity);
        }
        
        public HashSet<IGameEntityBridge> GetSlaveGroup()
        {
            return _PossessionEntityRelation.GetSubNodes();
        }
        
        public IGameEntityBridge GetSpawner()
        {
            return _Spawner;
        }
        
        public bool TryGetSpawner(out IGameEntityBridge o_Spawner)
        {
            o_Spawner = _Spawner;
            return !ReferenceEquals(null, o_Spawner);
        }

        public bool HasAnyMaster()
        {
            return !ReferenceEquals(null, _Master);
        }
        
        public bool IsMaster(IGameEntityBridge p_Entity)
        {
            return ReferenceEquals(p_Entity, _Master);
        }
        
        public IGameEntityBridge GetMaster()
        {
            TryGetMaster(out IGameEntityBridge o_Master);
            return o_Master;
        }
        
        public bool TryGetMaster(out IGameEntityBridge o_Master)
        {
            if (HasAnyMaster())
            {
                _Master.TryGetMaster(out o_Master);
                return true;
            }
            else
            {
                o_Master = this;
                return false;
            }
        }

        #endregion
    }
}