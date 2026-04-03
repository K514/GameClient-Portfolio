#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public static class TouchEventTool
    {
        #region <Enums>
      
        [Flags]
        public enum TouchEventType
        {
            None = 0,
            
            ControlView = 1 << 0,
            TouchWorldObject = 1 << 1,
        }

        public static TouchEventType[] _TouchEventTypeEnumerator;

        public enum TouchObjectResult
        {
            None,
            Position,
            Unit,
            Player
        }

        #endregion
        
        #region <Constructor>

        static TouchEventTool()
        {
            _TouchEventTypeEnumerator = EnumFlag.GetEnumEnumerator<TouchEventType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
        }

        #endregion
    }
}

#endif