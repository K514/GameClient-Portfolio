#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        #region <Enums>
        
        /// <summary>
        /// UI 이벤트 타입
        /// </summary>
        [Flags]
        public enum UIEventType
        {
            None = 0,
            
            KeyCodeEvent = 1 << 0,
            TouchEvent = 1 << 1,
            
            WholeEvent = KeyCodeEvent | TouchEvent,
        }
        
        public static readonly UIEventType[] _UIEventTypeEnumerator;

        #endregion
        
        #region <Struct>

        public struct UIInputEventParams
        {
            #region <Fields>

            public readonly InputEventTool.InputLayerType InputLayerMask;
            public readonly KeyCode KeyCode;

            #endregion

            #region <Constructor>

            public UIInputEventParams(InputEventTool.InputLayerType p_InputLayerMask, KeyCode p_KeyCode = KeyCode.None)
            {
                InputLayerMask = p_InputLayerMask;
                KeyCode = p_KeyCode;
            }

            #endregion
        }
        
        public struct UITouchEventParams
        {
            #region <Fields>

            public readonly TouchEventTool.TouchEventType TouchEventType;

            #endregion
            
            #region <Constructor>

            public UITouchEventParams(TouchEventTool.TouchEventType p_TouchEventMask)
            {
                TouchEventType = p_TouchEventMask;
            }

            #endregion
        }

        #endregion
    }
}
#endif