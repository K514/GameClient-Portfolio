using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public static partial class InputEventTool
    {
        #region <Const>

        /// <summary>
        /// 커맨드 큐에 기록된 방향키 이력이 지워지는데 걸리는 제한시간
        /// </summary>
        public static readonly int CommandExpireTime;
        
        /// <summary>
        /// 커맨드 큐에 기록할 수 있는 최대 커맨드 숫자
        /// </summary>
        public static readonly int CommandMaxCapacity;

        #endregion
        
        #region <Constructor>
                
        static InputEventTool()
        {
            CommandExpireTime = SystemIntValueTable.GetValue(SystemIntValueTable.KeyType.CommandExpireMsec);
            CommandMaxCapacity = SystemIntValueTable.GetValue(SystemIntValueTable.KeyType.CommandMaxCapacity);

            InitKeyMap();
        }

        #endregion

        #region <Methods>
        
        /// <summary>
        /// KeyCode를 정수 키코드로 변환하는 정적 메서드
        /// </summary>
        public static int ConvertToMIKeyCode(this KeyCode p_Type)
        {
            return KeyCodeTable.GetInstanceUnsafe[p_Type].Value;
        }
        
        /// <summary>
        /// KeyCode를 KeyName으로 변환하는 정적 메서드
        /// </summary>
        public static string ConvertToKeyName(this KeyCode p_Type)
        {
            if (DefaultKeyCodeNameTable.TryGetValue(p_Type, out var o_Name))
            {
                return o_Name;
            }
            else
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// TriggerKeyType를 정수 키코드로 변환하는 정적 메서드
        /// </summary>
        public static int ConvertToMIKeyCode(this TriggerKeyType p_Type)
        {
            return p_Type == TriggerKeyType.None ? -1 : (int) Mathf.Log((int)p_Type, 2);
        }
        
        /// <summary>
        /// TriggerKeyType를 KeyCode로 변환하는 정적 메서드
        /// </summary>
        public static KeyCode ConvertToKeyCode(this TriggerKeyType p_Type)
        {
            if (InvTriggerKeyTable.TryGetValue(p_Type, out var o_Value))
            {
                return o_Value.ConvertToKeyCode();
            }
            else
            {
                return KeyCode.None;
            }
        }
        
        /// <summary>
        /// TriggerKeyType를 KeyName으로 변환하는 정적 메서드
        /// </summary>
        public static string ConvertToKeyName(this TriggerKeyType p_Type)
        {
            return p_Type.ConvertToKeyCode().ConvertToKeyName();
        }

        /// <summary>
        /// 정수 키코드를 유니티 키코드로 변환하는 정적 메서드
        /// </summary>
        public static KeyCode ConvertToKeyCode(this int p_Value)
        {
            return KeyCodeTable.InvMIKeyCodeTable.GetValueOrDefault(p_Value, KeyCode.None);
        }

        /// <summary>
        /// 키코드를 커맨드코드로 변환하는 정적 메서드
        /// </summary>
        public static TriggerKeyType ConvertToTriggerKey(this KeyCode p_Type)
        {
            return p_Type.ConvertToMIKeyCode().ConvertToTriggerKey();
        }

        /// <summary>
        /// 정수 키코드를 커맨드코드로 변환하는 정적 메서드
        /// </summary>
        public static TriggerKeyType ConvertToTriggerKey(this int p_Value)
        {
            var keyType = p_Value.GetInputKeyType();
            switch (keyType)
            {
                default:
                case InputKeyType.None:
                case InputKeyType.FunctionKey:
                    return TriggerKeyType.None;
                case InputKeyType.ArrowKey:
                case InputKeyType.TriggerKey:
                    return (TriggerKeyType) (1 << p_Value);
            }
        }
        
        /// <summary>
        /// 키코드의 입력타입을 리턴하는 정적 메서드
        /// </summary>
        public static InputKeyType GetInputKeyType(this int p_Value)
        {
            switch (p_Value)
            {
                case var _ when p_Value < 0 :
                    return InputKeyType.None;
                case var _ when p_Value < ARROW_MKEYCODE_UPPERBOUND :
                    return InputKeyType.ArrowKey;
                case var _ when p_Value < TRIGGER_MKEYCODE_UPPERBOUND :
                    return InputKeyType.TriggerKey;
                default:
                    return InputKeyType.FunctionKey;
            }
        }
        
        /// <summary>
        /// 커맨드코드의 입력타입을 리턴하는 정적 메서드
        /// </summary>
        public static InputKeyType GetInputKeyType(this TriggerKeyType p_Type)
        {
            if (InvTriggerKeyTable.TryGetValue(p_Type, out var o_MIKeyCode))
            {
                switch (o_MIKeyCode)
                {
                    case var _ when o_MIKeyCode < 0 :
                        return InputKeyType.None;
                    case var _ when o_MIKeyCode < ARROW_MKEYCODE_UPPERBOUND :
                        return InputKeyType.ArrowKey;
                    case var _ when o_MIKeyCode < TRIGGER_MKEYCODE_UPPERBOUND :
                        return InputKeyType.TriggerKey;
                    default:
                        return InputKeyType.FunctionKey;
                }
            }
            else
            {
                return InputKeyType.None;
            }
        }

        #endregion
    }
}