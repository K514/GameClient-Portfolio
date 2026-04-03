using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    /// <summary>
    /// 해당 게임 개체의 상태, 모드 등을 기술하는 부분 클래스
    /// </summary>
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 상태 마스크
        /// </summary>
        private GameEntityTool.EntityStateType _EntityStateMask;
        
        /// <summary>
        /// 상태 마스크 접근 프로퍼티
        /// </summary>
        public GameEntityTool.EntityStateType EntityStateMask => _EntityStateMask;

        /// <summary>
        /// 각 상태 별 스택 테이블
        /// </summary>
        private Dictionary<GameEntityTool.EntityStateType, int> _StateStackTable;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateState()
        {
            _StateStackTable = new Dictionary<GameEntityTool.EntityStateType, int>();
            
            var enumerator = GameEntityTool.StateEnumerator;
            foreach (var stateType in enumerator)
            {
                if (stateType.IsStackable())
                {
                    _StateStackTable.Add(stateType, 0);
                }
            }

            OnCreateElement();
            OnCreateMaterial();
            OnCreateDead();
        }

        private void OnActivateState()
        {
            ClearState();
            
            OnActivateElement();
            OnActivateMaterial();
            OnActivateDead();
        }

        private void OnRetrieveState()
        {
            OnRetrieveDead();
            OnRetrieveMaterial();
            OnRetrieveElement();
            
            ClearState();
        }

        private void OnStateAdded(GameEntityTool.EntityStateType p_Type, bool p_IsReentered)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
            {
                CustomDebug.Log((this, $"state transition to [{p_Type}]"));
            }
#endif

            switch (p_Type)
            {
                case GameEntityTool.EntityStateType.SUPERARMOR:
                {
                    if (!p_IsReentered)
                    {
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.IMMORTAL:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__ImmortalVfxIndex, GetCenterPosition());
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.CLOAK:
                {
                    if (!p_IsReentered)
                    {
                        SetAlphaLerp(0.33f, 0.8f);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BLESSED:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__BlessingVfxIndex, GetBottomPosition());
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.CURSED:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__CurseVfxIndex, GetBottomPosition());
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.STUCK:
                {
                    ActionModule.ReleaseAllInput();
                    AnimationModule.SwitchHitMotion();
                    GeometryModule.StopNavigate();
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    break;
                }
                case GameEntityTool.EntityStateType.STUN:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__StunVfxIndex, GetTopUpPosition(0.5f));
                        ActionModule.ReleaseAllInput();
                        AnimationModule.SwitchHitMotion();
                        GeometryModule.StopNavigate();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.FREEZE:
                {
                    if (!p_IsReentered)
                    {
                        ActionModule.ReleaseAllInput();
                        AnimationModule.SwitchHitMotion();
                        GeometryModule.StopNavigate();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.CONFUSE:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__ConfuseVfxIndex, GetTopUpPosition(0.5f));
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BLIND:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__BlindVfxIndex, GetTopUpPosition(0.5f));
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.SILENCE:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__SilenceVfxIndex, GetTopUpPosition(0.5f));
                        ActionModule.ReleaseAllInput();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BIND:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__BindVfxIndex, GetBottomUpPosition(0.01f));
                        PhysicsModule.ClearForceExceptGravity();
                        GeometryModule.StopNavigate();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.GROGGY:
                {
                    if (!p_IsReentered)
                    {
                        AttachParticle(VfxTool.__GroggyVfxIndex, GetBottomPosition());
                        CameraManager.GetInstanceUnsafe.SetShake(Vector3.right, 12f, 0, 150, 6);

                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BLOCK_MOVE:
                {
                    if (!p_IsReentered)
                    {
                        PhysicsModule.ClearForceExceptGravity();
                        GeometryModule.StopNavigate();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, true));
                    }
                    break;
                }
            }
        }

        private void OnStateRemoved(GameEntityTool.EntityStateType p_Type, bool p_IsStackRemaind)
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
            {
                CustomDebug.Log((this, $"state [{p_Type}] over"));
            }
#endif
            switch (p_Type)
            {
                case GameEntityTool.EntityStateType.SUPERARMOR:
                {
                    if (!p_IsStackRemaind)
                    {
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.IMMORTAL:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__ImmortalVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.CLOAK:
                {
                    if (!p_IsStackRemaind)
                    {
                        SetAlphaLerp(1f, 0.8f);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BLESSED:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__BlessingVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.CURSED:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__CurseVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.STUCK:
                {
                    if (!p_IsStackRemaind)
                    {
                        AnimationModule.TryHitMotionResume();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.STUN:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__StunVfxIndex);
                        AnimationModule.TryHitMotionResume();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.FREEZE:
                {
                    if (!p_IsStackRemaind)
                    {
                        AnimationModule.TryHitMotionResume();
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.CONFUSE:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__ConfuseVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BLIND:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__BlindVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.SILENCE:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__SilenceVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.BIND:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__BindVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
                case GameEntityTool.EntityStateType.GROGGY:
                {
                    if (!p_IsStackRemaind)
                    {
                        RemoveAttachedParticle(VfxTool.__GroggyVfxIndex);
                        GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.State_Change, new GameEntityBaseEventParams(this, p_Type, false));
                    }
                    break;
                }
            }
        }
        
        #endregion
    
        #region <Methods>

        public int GetStateStackCount(GameEntityTool.EntityStateType p_Type)
        {
            if (HasState_Or(p_Type))
            {
                if (p_Type.IsStackable())
                {
                    return _StateStackTable[p_Type];
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }
        
        /// <summary>
        /// 파라미터 마스크의 플래그를 전부 보유했는지 검증
        /// </summary>
        public bool HasState_And(GameEntityTool.EntityStateType p_CompareMask)
        {
            return _EntityStateMask.HasAllFlag(p_CompareMask);
        }
        
        /// <summary>
        /// 파라미터 마스크의 플래그 중에 하나라도 보유했는지 검증
        /// </summary>      
        public bool HasState_Or(GameEntityTool.EntityStateType p_CompareMask)
        {
            return _EntityStateMask.HasAnyFlagExceptNone(p_CompareMask);
        }
        
        /// <summary>
        /// 파라미터 마스크에 포함된 플래그만 가지고 있는지 검증
        /// </summary>
        public bool HasState_Only(GameEntityTool.EntityStateType p_AvailableStateMask)
        {
            return _EntityStateMask.HasFlagOnly(p_AvailableStateMask);
        }
        
        public void AddState(GameEntityTool.EntityStateType p_Type)
        {
            if (GameEntityTool.EntityStateType.StackableStateGroupMask.HasAnyFlagExceptNone(p_Type))
            {
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
                {
                    CustomDebug.Log((this, $"state stacked [{p_Type}] ({_StateStackTable[p_Type]} => {_StateStackTable[p_Type] + 1})"));
                }
#endif
                _StateStackTable[p_Type]++;
            }

            if (HasState_Or(p_Type))
            {
                OnStateAdded(p_Type, true);
            }
            else
            {
                _EntityStateMask.AddFlag(p_Type);
                OnStateAdded(p_Type, false);
            }
        }
        
        public void RemoveState(GameEntityTool.EntityStateType p_Type)
        {
            if (HasState_Or(p_Type))
            {
                if (GameEntityTool.EntityStateType.StackableStateGroupMask.HasAnyFlagExceptNone(p_Type))
                {
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintGameObjectLog.HasOpen())
                    {
                        CustomDebug.Log((this, $"state stacked [{p_Type}] ({_StateStackTable[p_Type]} => {Mathf.Max(0, _StateStackTable[p_Type] - 1)})"));
                    }
#endif
                    _StateStackTable[p_Type] = Mathf.Max(0, _StateStackTable[p_Type] - 1);
                    if (_StateStackTable[p_Type] == 0)
                    {
                        _EntityStateMask.RemoveFlag(p_Type);
                        OnStateRemoved(p_Type, false);
                    }
                    else
                    {
                        OnStateRemoved(p_Type, true);
                    }
                }
                else
                {
                    _EntityStateMask.RemoveFlag(p_Type);
                    OnStateRemoved(p_Type, false);
                }
            }
        }
                
        public void TurnState(GameEntityTool.EntityStateType p_Mask)
        {
            foreach (var stateType in GameEntityTool.StateEnumerator)
            {
                if (p_Mask.HasAnyFlagExceptNone(stateType))
                {
                    if (!HasState_Or(stateType))
                    {
                        AddState(stateType);
                    }
                }
                else
                {
                    if (HasState_Or(stateType))
                    {
                        if (GameEntityTool.EntityStateType.StackableStateGroupMask.HasAnyFlagExceptNone(stateType))
                        {
                            _StateStackTable[stateType] = 0;
                            _EntityStateMask.RemoveFlag(stateType);
                            OnStateRemoved(stateType, false);
                        }
                        else
                        {
                            _EntityStateMask.RemoveFlag(stateType);
                            OnStateRemoved(stateType, false); 
                        }
                    }
                }
            }
        }
        
        public void AddStateMask(GameEntityTool.EntityStateType p_Mask)
        {
            foreach (var stateType in GameEntityTool.StateEnumerator)
            {
                if (p_Mask.HasAnyFlagExceptNone(stateType))
                {
                    AddState(stateType);
                }
            }
        }
        
        public void RemoveStateMask(GameEntityTool.EntityStateType p_Mask)
        {
            foreach (var stateType in GameEntityTool.StateEnumerator)
            {
                if (p_Mask.HasAnyFlagExceptNone(stateType))
                {
                    RemoveState(stateType);
                }
            }
        }
        
        public void ClearState()
        {
            TurnState(GetDefaultState());
        }

        public GameEntityTool.EntityStateType GetDefaultState()
        {
            return GameEntityTool.EntityStateType.None;
        }

#if APPLY_PRINT_LOG
        public void PrintState()
        {
            var result = string.Empty;
            foreach (var stateType in GameEntityTool.StateEnumerator)
            {
                if (EntityStateMask.HasAnyFlagExceptNone(stateType))
                {
                    result = string.IsNullOrEmpty(result) ? $"[{stateType}]" : $"{result}, [{stateType}]";
                }
            }
            CustomDebug.LogError(result);
        }
#endif
        
        #endregion
    }
}