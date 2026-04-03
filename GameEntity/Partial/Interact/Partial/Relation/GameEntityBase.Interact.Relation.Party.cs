using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private GameEntityRelation _PartyEntityRelation;
        private IGameEntityBridge _PartyLeader;
        private List<IGameEntityBridge> _PartyMemberList;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreatePartyInteraction()
        {
            _PartyEntityRelation = _GameEntityRelationTable[GameEntityRelationTool.GameEntityRelationType.Party];
            _PartyMemberList = new List<IGameEntityBridge>();
        }

        private void OnActivatePartyInteraction()
        {
        }
        
        private void OnRetrievePartyInteraction()
        {
            _PartyEntityRelation.ResetNode();
        }
        
        private void OnHandlePartyEvent(GameEntityRelationTool.GameEntityRelationEventType p_EventType, GameEntityRelationTool.GameEntityRelationEventParams p_EventParams)
        {
            switch (p_EventType)
            {
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeAdded:
                {
                    var comrade = p_EventParams.TargetEntity;
                    _PartyMemberList.Add(comrade); 
                    comrade.OnJoinPartyOf(this);
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.PartyMember_Change, new GameEntityBaseEventParams(comrade, true));
                    break;
                }
                case GameEntityRelationTool.GameEntityRelationEventType.SubNodeRemoved:
                {
                    var comrade = p_EventParams.TargetEntity;
                    comrade.OnLeavePartyFrom(this);
                    _PartyMemberList.Remove(comrade);
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.PartyMember_Change, new GameEntityBaseEventParams(comrade, false));
                    break;
                }
            }
        }

        public void OnJoinPartyOf(IGameEntityBridge p_Leader)
        {
#if UNITY_EDITOR
            Debug.LogError("파티원 입갤");
#endif
            if (ReferenceEquals(null, _PartyLeader))
            {
                _PartyLeader = p_Leader;
            }
            else
            {
                if (ReferenceEquals(p_Leader, _PartyLeader))
                {
                    
                }
                else
                {
                    var prevLeader = _PartyLeader;
                    _PartyLeader = p_Leader;
                    prevLeader.RemovePartyMember(this);
                }
            }
            
            SwitchPersona(BoundedModuleDataTableQuery.TableLabel.Dummy);
            SetDisable(true);
        }

        public void OnLeavePartyFrom(IGameEntityBridge p_Leader)
        {
            if (ReferenceEquals(p_Leader, _PartyLeader))
            {
#if UNITY_EDITOR
                Debug.LogError("파티원 퇴갤");
#endif
                _PartyLeader = null;
                ReservePooling();
            }
        }

        #endregion

        #region <Methods>

        public void AddPartyMember(IGameEntityBridge p_TargetEntity)
        {
            _PartyEntityRelation.AddNode(p_TargetEntity);
        }
        
        public void RemovePartyMember(IGameEntityBridge p_TargetEntity)
        {
            _PartyEntityRelation.RemoveNode(p_TargetEntity);
        }
        
        public void RemovePartyMember(int p_Index)
        {
            if (TryGetPartyMember(p_Index, out var o_Member))
            {
                RemovePartyMember(o_Member);
            }
        }

        public bool TryGetPartyMember(int p_Index, out IGameEntityBridge o_Member)
        {
            return _PartyMemberList.TryGetElementSafe(p_Index, out o_Member);
        }
        
        public HashSet<IGameEntityBridge> GetPartyGroup()
        {
            return _PartyEntityRelation.GetSubNodes();
        }

        public IGameEntityBridge GetLeader()
        {
            return _PartyLeader;
        }
        
        public bool TryGetPartyLeader(out IGameEntityBridge o_Leader)
        {
            o_Leader = _PartyLeader;
            return !ReferenceEquals(null, o_Leader);
        }
        
        public bool HasAnyParty()
        {
            return !ReferenceEquals(null, _PartyLeader);
        }
        
        public bool IsPartyLeader(IGameEntityBridge p_Entity)
        {
            return ReferenceEquals(p_Entity, _PartyLeader);
        }
        
        #endregion
    }
}