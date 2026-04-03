using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    /// <summary>
    /// 입력이 지속되고 있는 동안의 정보를 기술하는 구조체
    /// </summary>
    public struct InputGesture
    {
        #region <Consts>

        public static readonly InputGesture __DEFAULT_STABLE_GESTURE = new InputGesture(InputEventTool.InputGestureType.Stable, default, default);
        public static readonly InputGesture __KEYBOARD_UPARROW_GESTURE = new InputGesture(InputEventTool.InputGestureType.Stable, ArrowType.Up, Vector3.forward);
        public static readonly InputGesture __KEYBOARD_LEFTARROW_GESTURE = new InputGesture(InputEventTool.InputGestureType.Stable, ArrowType.Left, Vector3.left);
        public static readonly InputGesture __KEYBOARD_DOWNARROW_GESTURE = new InputGesture(InputEventTool.InputGestureType.Stable, ArrowType.Down, Vector3.back);
        public static readonly InputGesture __KEYBOARD_RIGHTARROW_GESTURE = new InputGesture(InputEventTool.InputGestureType.Stable, ArrowType.Right, Vector3.right);
        
        #endregion
        
        #region <Fields>

        public readonly InputEventTool.InputGestureType GestureType;
        public readonly ArrowType ArrowType;
        public readonly Vector3 UV;
        public readonly bool ValidFlag;
        public bool IsDragHandled => GestureType != InputEventTool.InputGestureType.None;
        
        #endregion

        #region <Constructor>

        public InputGesture(Vector3 p_UV) : this(InputEventTool.InputGestureType.None, ArrowType.None, p_UV)
        {
        }
        
        public InputGesture(InputEventTool.InputGestureType p_GestureType, ArrowType p_ArrowType, Vector3 p_UV)
        {
            GestureType = p_GestureType;
            ArrowType = p_ArrowType;
            UV = p_UV;
            ValidFlag = true;
        }
        
        #endregion

        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"[GestureType : {GestureType}] [ArrowType : {ArrowType}] [UV : {UV}] [IsDragHandled : {IsDragHandled}]";
        }
#endif

        #endregion
    }
}