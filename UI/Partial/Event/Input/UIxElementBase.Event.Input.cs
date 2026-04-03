#if !SERVER_DRIVE
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 해당 컴포넌트에서 처리할 수 있는 UI이벤트 타입
        /// </summary>
        protected UIxTool.UIEventType _UIEventFlagMask;

        /// <summary>
        /// 초기 오브젝트 위치
        /// </summary>
        protected Vector2 _DefaultBasePosition;
        
        /// <summary>
        /// 입력 시작 위치
        /// </summary>
        protected Vector2 _PointerDownPosition;

        /// <summary>
        /// 입력 이벤트 파라미터
        /// </summary>
        protected UIxTool.UIInputEventParams _InputEventParams;

        /// <summary>
        /// 터치 이벤트 파라미터
        /// </summary>
        protected UIxTool.UITouchEventParams _TouchEventParams;

        /// <summary>
        /// 현재 컴포넌트 입력 플래그
        /// </summary>
        protected bool _CurInputFlag;

        #endregion

        #region <Callbacks>

        protected virtual void OnCreateInputEvent()
        {
            OnCreateInputEventDrag();
        }

        private void OnActivateInputEvent()
        {
            _DefaultBasePosition = _PointerDownPosition = RectTransform.position.XYVector2();
            
            ResetInputState();
        }

        private void OnRetrieveInputEvent()
        {
            _PointerDownPosition = _DefaultBasePosition;
            RectTransform.position = _DefaultBasePosition.XYZVector3();

            RemoveInputEvent();
            RemoveTouchEvent();
            ResetContainer();
        }

        private void OnUpdateInputEvent(float p_DeltaTime)
        {
            if (_CurInputFlag)
            {
                OnUpdateInputGesture();
                CastPointeHoldingEvent();
            }
        }
        
        #endregion

        #region <Methods>

        public void ResetInputState()
        {
            _CurInputFlag = false;
      
            ResetInputPressState();
            ResetInputHoverState();
            ResetInputDragState();
            ResetInputReleaseState();
        }
        
        public void SetInputEvent(UIxTool.UIInputEventParams p_Params)
        {
            _InputEventParams = p_Params;
            _UIEventFlagMask.AddFlag(UIxTool.UIEventType.KeyCodeEvent);
            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DispatchInputEvent);
        }

        public void RemoveInputEvent()
        {
            _InputEventParams = default;
            _UIEventFlagMask.RemoveFlag(UIxTool.UIEventType.KeyCodeEvent);

            if (_UIEventFlagMask == UIxTool.UIEventType.None)
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DispatchInputEvent);
            }
        }
        
        public void SetTouchEvent(UIxTool.UITouchEventParams p_Params)
        {
            _TouchEventParams = p_Params;
            _UIEventFlagMask.AddFlag(UIxTool.UIEventType.TouchEvent);
            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.DispatchInputEvent);
        }

        public void RemoveTouchEvent()
        {
            _TouchEventParams = default;
            _UIEventFlagMask.RemoveFlag(UIxTool.UIEventType.TouchEvent);

            if (_UIEventFlagMask == UIxTool.UIEventType.None)
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.DispatchInputEvent);
            }
        }

        #endregion
    }
}
#endif