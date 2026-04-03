using UnityEngine;

#if !SERVER_DRIVE
namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 기능 정지 카운트
        /// </summary>
        private int _BlockCount;

        #endregion

        #region <Callbacks>

        private void OnRetrieveControl()
        {
            _BlockCount = 0;
            OnBlockStateChanged(false);
        }
        
        protected virtual void OnVisible()
        {
            _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.Hide);
            
            SetPlayAnimation();
            ResetInputState();
        }

        protected virtual void OnHide()
        {
            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.Hide);
        }
        
        protected virtual void OnBlockStateChanged(bool p_Flag)
        {
        }

        public virtual void OnControlInput(InputLayerEventParams p_Params)
        {
            var keyCode = p_Params.KeyCode;
            switch (keyCode)
            {
                case KeyCode.Escape:
                {
                    if (p_Params.IsTouched)
                    {
                        UIxControlRoot.GetInstanceUnsafe.PopFromControlStack(this);
                    }
                    break;
                }
            }
        }
        
        #endregion
        
        #region <Methods>

        public bool IsVisible()
        {
            return !_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.Hide);
        }
        
        public bool IsBlocked()
        {
            return _BlockCount > 0;
        }
        
        public virtual void SetHide(bool p_HideFlag)
        {
            if (IsVisible() == p_HideFlag)
            {
                if (p_HideFlag)
                {
                    gameObject.SetActiveSafe(false);
                    OnHide();
                }
                else
                {
                    gameObject.SetActiveSafe(true);
                    OnVisible();
                }
            }
        }

        public void ToggleHide()
        {
            SetHide(IsVisible());
        }
        
        public virtual void SetControlBlock(bool p_Flag)
        {
            if (p_Flag)
            {
                if (_BlockCount == 0)
                {
                    _BlockCount = 1;
                    OnBlockStateChanged(true);
                }
                else
                {
                    _BlockCount++;
                }
            }
            else
            {
                if (_BlockCount == 1)
                {
                    _BlockCount = 0;
                    OnBlockStateChanged(false);
                }
                else if(_BlockCount > 1)
                {
                    _BlockCount--;
                }
            }
        }

        public void PushToControlStack()
        {
            UIxControlRoot.GetInstanceUnsafe.PushToControlStack(this);
        }
        
        public void PopFromControlStack()
        {
            UIxControlRoot.GetInstanceUnsafe.PopFromControlStack(this);
        }
        
        #endregion
    }
}
#endif