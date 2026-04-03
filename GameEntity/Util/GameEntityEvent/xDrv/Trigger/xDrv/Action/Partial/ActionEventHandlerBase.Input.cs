using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class ActionEventHandlerBase<This>
    {
        #region <Fields>

        private int _CurrentPriority;

        #endregion
        
        #region <Callbacks>

        protected abstract void OnPress(CommandEventParams p_CommandPreset);
        protected abstract void OnHolding(CommandEventParams p_CommandPreset);
        protected abstract void OnRelease(CommandEventParams p_CommandPreset);

        #endregion

        #region <Methods>

        public bool InputPress(CommandEventParams p_CommandPreset)
        {
            if (IsEnterable() && !IsHolding())
            {
                _CurrentPriority = p_CommandPreset.Priority;
                _ActionEventStateMask.AddFlag(ActionEventTool.ActionEventState.Holding);
                OnPress(p_CommandPreset);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InputHolding(CommandEventParams p_CommandPreset)
        {
            if (IsHolding() && _CurrentPriority <= p_CommandPreset.Priority)
            {
                _CurrentPriority = p_CommandPreset.Priority;
                OnHolding(p_CommandPreset);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InputRelease(CommandEventParams p_CommandPreset)
        {
            if (IsHolding() && _CurrentPriority <= p_CommandPreset.Priority)
            {
                _CurrentPriority = 0;
                _ActionEventStateMask.RemoveFlag(ActionEventTool.ActionEventState.Holding);
                OnRelease(p_CommandPreset);
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        protected virtual bool IsManualReleasable()
        {
            return true;
        }
        
        public bool ManualInputRelease()
        {
            if (IsManualReleasable())
            {
                InputRelease(CommandEventParams.GetActionCommandParams(TriggerKey, InputEventTool.InputStateType.Release));
                
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}