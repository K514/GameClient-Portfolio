using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 각 키코드의 입력 상태를 기술하는 클래스
    ///
    /// 실제로 입력된 값이 IKeyCode이고 시스템 상 매핑된 논리적인 입력값이 MIKeyCode이다.
    /// </summary>
    public class KeyInputState
    {
        #region <Fields>
        
        /// <summary>
        /// 실제로 입력되는 유니티 키코드 정수값
        /// </summary>
        public readonly int IKeyCode;
        
        /// <summary>
        /// 매핑된 키코드
        /// </summary>
        public int MIKeyCode;

        /// <summary>
        /// 매핑된 유니티 키코드
        /// </summary>
        public KeyCode MKeyCode;
        
        /// <summary>
        /// 키코드 타입
        /// </summary>
        public InputEventTool.InputKeyType KeyType;
        
        /// <summary>
        /// 트리거 키 타입
        /// </summary>
        public InputEventTool.TriggerKeyType TriggerKeyType;
        
        /// <summary>
        /// 입력 이벤트 프리셋
        /// </summary>
        public InputLayerEventParams InputEventParams;

        /// <summary>
        /// 입력 시점
        /// </summary>
        public float StartTimeStamp;        
        
        /// <summary>
        /// 시간 변화량
        /// </summary>
        public float DeltaTime;
        
        /// <summary>
        /// 입력이 유지된 시간
        /// </summary>
        public float ElapsedTime;

        #endregion

        #region <Constructor>

        public KeyInputState(int p_KeyCode)
        {
            IKeyCode = p_KeyCode;
        }

        #endregion

        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"IKeyCode {IKeyCode} / KeyCodeType {KeyType} / MIKey {MIKeyCode} / MKeyCode {MKeyCode} / InputCommandType {TriggerKeyType} / InputEventParams {InputEventParams} / Start {StartTimeStamp} / Dt {DeltaTime} / Elapsed {ElapsedTime}";
        }
#endif

        #endregion
        
        #region <Callbacks>

        /// <summary>
        /// 입력 레이어 이벤트로부터 키 입력 상태를 갱신하는 콜백
        /// </summary>
        public void OnUpdateKeyState(InputLayerEventParams p_Params)
        {
            InputEventParams = p_Params;

            switch (InputEventParams.InputStateType)
            {
                case InputEventTool.InputStateType.Release:
                {
                    break;
                }
                case InputEventTool.InputStateType.Press:
                {
                    StartTimeStamp = p_Params.StartTimeStamp;
                    ElapsedTime = 0f;
                    goto case InputEventTool.InputStateType.Holding;
                }
                case InputEventTool.InputStateType.Holding:
                {
                    switch (InputEventParams.InputDeviceType)
                    {
                        case InputEventTool.InputDeviceType.None:
                        case InputEventTool.InputDeviceType.UI:
                        {
                            break;
                        }
                        case InputEventTool.InputDeviceType.Keyboard:
                        {
                            switch (KeyType)
                            {
                                case InputEventTool.InputKeyType.None:
                                {
                                    InputEventParams.SetGesture(InputGesture.__DEFAULT_STABLE_GESTURE);
                                    break;
                                }
                                case InputEventTool.InputKeyType.ArrowKey:
                                {
                                    switch (TriggerKeyType)
                                    {
                                        case InputEventTool.TriggerKeyType.UpArrow:
                                            InputEventParams.SetGesture(InputGesture.__KEYBOARD_UPARROW_GESTURE);
                                            break;
                                        case InputEventTool.TriggerKeyType.LeftArrow:
                                            InputEventParams.SetGesture(InputGesture.__KEYBOARD_LEFTARROW_GESTURE);
                                            break;
                                        case InputEventTool.TriggerKeyType.DownArrow:
                                            InputEventParams.SetGesture(InputGesture.__KEYBOARD_DOWNARROW_GESTURE);
                                            break;
                                        case InputEventTool.TriggerKeyType.RightArrow:
                                            InputEventParams.SetGesture(InputGesture.__KEYBOARD_RIGHTARROW_GESTURE);
                                            break;
                                    }
                                    break;
                                }
                                case InputEventTool.InputKeyType.TriggerKey:
                                case InputEventTool.InputKeyType.FunctionKey:
                                {
                                    InputEventParams.SetGesture(InputGesture.__DEFAULT_STABLE_GESTURE);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 지정한 키맵을 기준으로 MKey, MIKey 값을 재배치하는 메서드
        /// </summary>
        public void UpdateMapping()
        {
            MIKeyCode = InputEventTool.MIKeyCodeTable[IKeyCode];
            MKeyCode = KeyCodeTable.InvMIKeyCodeTable[MIKeyCode];
            KeyType = MIKeyCode.GetInputKeyType();
            TriggerKeyType = MIKeyCode.ConvertToTriggerKey();
        }

        /// <summary>
        /// 키 입력이 홀딩상태인 경우 시간 값을 갱신하는 메서드
        /// </summary>
        public void UpdateElapsed(float p_DeltaTime)
        {
            DeltaTime = p_DeltaTime;
            ElapsedTime += DeltaTime;
        }

        #endregion
    }
}