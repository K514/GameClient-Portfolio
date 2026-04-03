using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class ActionEventHandlerBase<This>
    {
        #region <Fields>

        protected ActionEventTool.ActionEventState _ActionEventStateMask;

        #endregion

        #region <Callbacks>

        public virtual void OnInterruptSuccess()
        {
            Debug.LogError($"[{GetType().Name}] OnInterruptSuccess");
            
            _ActionEventStateMask.AddFlag(ActionEventTool.ActionEventState.Selected);
            _CurrentCueIndex = 0;
            
            PayCost();
            RunCooldown();
            Entity.OnActionActivateSuccess(this);
        }

        public void OnInterruptFail()
        {
            Debug.LogError($"[{GetType().Name}] OnInterruptFail");
   
            Entity.OnActionActivateSuccess(this);
        }

        public virtual void OnInterrupted()
        {
            Debug.LogError($"[{GetType().Name}] OnInterrupted");
            
            _ActionEventStateMask.RemoveFlag(ActionEventTool.ActionEventState.Selected);
            Entity.OnActionTerminated(this);
        }
        
        public virtual void OnMindControl()
        {
            if (Entity.TryGetCurrentEnemy(out var o_Enemy))
            {
                Entity.SetLook(o_Enemy);
            }
        }
        
        public virtual void OnReachedGround()
        {
        }

        #endregion

        #region <Methods>

        public bool IsHolding()
        {
            return _ActionEventStateMask.HasAnyFlagExceptNone(ActionEventTool.ActionEventState.Holding);
        }
        
        public bool IsSelected()
        {
            return _ActionEventStateMask.HasAnyFlagExceptNone(ActionEventTool.ActionEventState.Selected);
        }

        #endregion
    }
}