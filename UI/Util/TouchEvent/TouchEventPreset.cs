#if !SERVER_DRIVE

using UnityEngine;
using UnityEngine.EventSystems;

namespace k514.Mono.Common
{
    public struct PointerEventDataPreset
    {
        #region <Fields>

        public readonly float TimeStamp;
        public readonly Vector2 ScreenPos;
        public readonly PointerEventData.InputButton InputButton;

        #endregion

        #region <Constructor>

        public PointerEventDataPreset(float p_TimeStamp, Vector2 p_ScreenPos)
        {
            TimeStamp = p_TimeStamp;
            ScreenPos = p_ScreenPos;
            InputButton = PointerEventData.InputButton.Left;
        }

        public PointerEventDataPreset(PointerEventData p_PointerEventData)
        {
            if (ReferenceEquals(null, p_PointerEventData))
            {
                TimeStamp = Time.time;
                ScreenPos = Vector2.zero;
                InputButton = PointerEventData.InputButton.Left;
            }
            else
            {
                TimeStamp = p_PointerEventData.clickTime;
                ScreenPos = p_PointerEventData.position;
                InputButton = p_PointerEventData.button;
            }
        }
        
        #endregion
        
        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"[TimeStamp : {TimeStamp}] [ScreenPos : {ScreenPos}] [InputButton : {InputButton}]";
        }
#endif

        #endregion
    }
    
    public struct TouchInputEventPreset
    {
        #region <Fields>

        public readonly InputEventTool.InputStateType InputStateType;
        public readonly TouchPressEventPreset TouchPressEventPreset;
        public readonly TouchHoldEventPreset TouchHoldEventPreset;
        public readonly TouchReleaseEventPreset TouchReleaseEventPreset;
        public readonly PointerEventDataPreset PointerEventDataPreset;
        public readonly UIxElementBase UI;
        
        #endregion

        #region <Constructor>

        public TouchInputEventPreset(UIxElementBase p_UI, TouchPressEventPreset p_TouchPressEventPreset)
        {
            InputStateType = InputEventTool.InputStateType.Press;
            TouchPressEventPreset = p_TouchPressEventPreset;
            TouchHoldEventPreset = default;
            TouchReleaseEventPreset = default;
            PointerEventDataPreset = TouchPressEventPreset.PointerEventData;
            UI = p_UI;
        }
        
        public TouchInputEventPreset(UIxElementBase p_UI, TouchPressEventPreset p_TouchPressEventPreset, TouchHoldEventPreset p_TouchHoldEventPreset)
        {
            InputStateType = InputEventTool.InputStateType.Holding;
            TouchPressEventPreset = p_TouchPressEventPreset;
            TouchHoldEventPreset = p_TouchHoldEventPreset;
            TouchReleaseEventPreset = default;
            PointerEventDataPreset = TouchHoldEventPreset.PointerEventData;
            UI = p_UI;
        }
        
        public TouchInputEventPreset(UIxElementBase p_UI, TouchPressEventPreset p_TouchPressEventPreset, TouchHoldEventPreset p_TouchHoldEventPreset, TouchReleaseEventPreset p_TouchReleaseEventPreset)
        {
            InputStateType = InputEventTool.InputStateType.Release;
            TouchPressEventPreset = p_TouchPressEventPreset;
            TouchHoldEventPreset = p_TouchHoldEventPreset;
            TouchReleaseEventPreset = p_TouchReleaseEventPreset;
            PointerEventDataPreset = TouchReleaseEventPreset.PointerEventData;
            UI = p_UI;
        }

        #endregion
        
        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"[InputStateType : {InputStateType}] [TouchPressEventPreset : {TouchPressEventPreset}] [TouchHoldEventPreset : {TouchHoldEventPreset}] [TouchReleaseEventPreset {TouchReleaseEventPreset}]";
        }
#endif

        #endregion
    }

    public struct TouchObjectResultPreset
    {
        #region <Fields>

        public readonly TouchEventTool.TouchObjectResult TouchObjectResultType;
        public readonly IGameEntityBridge Unit;
        public readonly Vector3 Position;
        public readonly GameObject HitObject;

        #endregion

        #region <Constructors>

        public TouchObjectResultPreset(Vector3 p_Position, GameObject p_HitObject)
        {
            TouchObjectResultType = TouchEventTool.TouchObjectResult.Position;
            Unit = null;
            Position = p_Position;
            HitObject = p_HitObject;
        }

        public TouchObjectResultPreset(IGameEntityBridge p_Unit, GameObject p_HitObject)
        {
            TouchObjectResultType = p_Unit.IsPlayer ? TouchEventTool.TouchObjectResult.Player : TouchEventTool.TouchObjectResult.Unit;
            Unit = p_Unit;
            Position = Unit.GetBottomPosition();
            HitObject = p_HitObject;
        }

        #endregion
    }
    
    public struct TouchPressEventPreset
    {
        #region <Fields>

        public readonly PointerEventDataPreset PointerEventData;
        public readonly int InputStack;

        #endregion

        #region <Constructor>

        public TouchPressEventPreset(PointerEventDataPreset p_PointerEventData, int p_InputStack)
        {
            PointerEventData = p_PointerEventData;
            InputStack = p_InputStack;
        }

        #endregion

        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"[PointerEventData : {PointerEventData}] [InputStack : {InputStack}]";
        }
#endif

        #endregion
    }

    public struct TouchHoldEventPreset
    {
        #region <Fields>

        public readonly PointerEventDataPreset PointerEventData;
        public readonly InputGesture InputGesture;
        public readonly bool IsUpdated;
        
        #endregion

        #region <Constructor>

        public TouchHoldEventPreset(PointerEventDataPreset p_PointerEventData, InputGesture p_InputGesture, bool p_IsUpdated)
        {
            PointerEventData = p_PointerEventData;
            InputGesture = p_InputGesture;
            IsUpdated = p_IsUpdated;
        }
        
        #endregion

        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"[PointerEventData : {PointerEventData}] [InputGesture : {InputGesture}]";
        }
#endif

        #endregion
    }

    public struct TouchReleaseEventPreset
    {
        #region <Fields>

        public readonly PointerEventDataPreset PointerEventData;

        #endregion

        #region <Constructor>

        public TouchReleaseEventPreset(PointerEventDataPreset p_PointerEventData)
        {
            PointerEventData = p_PointerEventData;
        }

        #endregion

        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"[PointerEventData : {PointerEventData}]";
        }
#endif

        #endregion
    }
}

#endif