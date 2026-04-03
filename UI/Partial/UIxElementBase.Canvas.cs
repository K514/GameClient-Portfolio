#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public abstract partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 해당 UI를 포함하는 캔버스 프리셋
        /// </summary>
        public UIxTool.UIxCanvasPreset CanvasPreset
        {
            get
            {
                if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.Contained))
                {
                    return _Container.CanvasPreset;
                }
                else
                {
                    return _CanvasPreset;
                }
            }
        }

        /// <summary>
        /// UI 루트로부터 생성 되었을 때 해당 UI가 배치된 캔버스 프리셋
        /// </summary>
        private UIxTool.UIxCanvasPreset _CanvasPreset;
        
        /// <summary>
        /// 해당 UI가 캔버스에 등록되어 있는 경우, 그 때의 UI 컨트롤 타입
        /// </summary>
        public UIxTool.UIxElementType UICanvasControlType { get; protected set; }

        #endregion

        #region <Callbacks>

        private void OnActivateCanvas(UIPoolManager.ActivateParams p_ActivateParams)
        {
            if (p_ActivateParams.ActivateParamType == UIPoolManager.ActivateParams.UIActivateParamType.CanvasControl)
            {
                UICanvasControlType = p_ActivateParams.ControlType;
            }
            
            _CanvasPreset = p_ActivateParams.CanvasPreset;
        }

        private void OnRetrieveCanvas()
        {
            if (UICanvasControlType != UIxTool.UIxElementType.None)
            {
                _CanvasPreset.OnElementReleased(this);
            }
            _CanvasPreset = null;
        }

        #endregion
    }
}

#endif