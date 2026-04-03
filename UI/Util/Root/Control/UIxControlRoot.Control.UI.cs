using System.Collections.Generic;
using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class UIxControlRoot
    {
        #region <Fields>

        private InputLayerEventReceiver _EventReceiver;
        private Stack<UIxElementBase> _ControlStack;
        
        #endregion

        #region <Callbacks>

        private void OnCreateControlUI()
        {
            _EventReceiver = new InputLayerEventReceiver(InputEventTool.InputLayerType.ControlUI, OnHandleInputLayerEvent);
            _ControlStack = new Stack<UIxElementBase>();
        }

        private void OnHandleInputLayerEvent(InputEventTool.InputLayerType p_Type, InputLayerEventParams p_Params)
        {
            if (_ControlStack.TryPeek(out var o_Peek))
            {
                o_Peek.OnControlInput(p_Params);
            }
            else
            {
                MainHUD.OnControlInput(p_Params);
            }
        }
        
        #endregion

        #region <Methods>

        private void InterceptInputSystem()
        {
            SystemBoot.StopWorldTimeScale(SystemBoot.WorldTimeScaleType.UIxControl);
            InputLayerManager.GetInstanceUnsafe.SetLayerBlock(InputEventTool.InputLayerType.ControlUnit, true);
            InputLayerManager.GetInstanceUnsafe.SetLayerBlock(InputEventTool.InputLayerType.ControlView, true);
        }
        
        private void ReleaseInputSystem()
        {
            SystemBoot.ResetWorldTimeScale(SystemBoot.WorldTimeScaleType.UIxControl);
            InputLayerManager.GetInstanceUnsafe.SetLayerBlock(InputEventTool.InputLayerType.ControlUnit, false);
            InputLayerManager.GetInstanceUnsafe.SetLayerBlock(InputEventTool.InputLayerType.ControlView, false);
        }
        
        public void PushToControlStack(UIxElementBase p_UI)
        {
            if (!ReferenceEquals(null, p_UI))
            {
                p_UI.SetHide(false);
                _ControlStack.Push(p_UI);

                InterceptInputSystem();
            }
        }
        
        public void PopFromControlStack(UIxElementBase p_UI)
        {
            if (_ControlStack.TryPeek(out var o_Peek) && ReferenceEquals(o_Peek, p_UI))
            {
                p_UI.SetHide(true);
                _ControlStack.Pop();
                if (_ControlStack.Count < 1)
                {
                    ReleaseInputSystem();
                }
            }
        }
        
        private void ClearControlStack()
        {
            while(_ControlStack.TryPeek(out var o_Peek))
            {
                o_Peek.SetHide(true);
                _ControlStack.Pop();
            }
            
            ReleaseInputSystem();
        }

        #endregion
    }
}

#endif