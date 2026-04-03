using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class InputEventTool
    {
        /// <summary>
        /// 시스템에서 사용할 방향 키코드 반복자
        ///
        /// 내부에는 유니티 키코드의 정수 값이 들어가 있다.
        /// * 방향키 순서는 위 = 0, 왼쪽 = 1, 아래 = 2, 오른쪽 = 3 (반시계방향)
        /// </summary>
        public static KeyCode[] ArrowKeyCodeIterator;
        
        /// <summary>
        /// 시스템에서 사용할 정수 방향 키코드 반복자
        ///
        /// 내부에는 유니티 키코드의 정수 값이 들어가 있다.
        /// * 방향키 순서는 위 = 0, 왼쪽 = 1, 아래 = 2, 오른쪽 = 3 (반시계방향)
        /// </summary>
        public static int[] ArrowIKeyCodeIterator;

        /// <summary>
        /// 시스템에서 사용할 트리거 키코드 반복자
        /// 
        /// 내부에는 유니티 키코드의 정수 값이 들어가 있다.
        /// </summary>
        public static KeyCode[] TriggerKeyCodeIterator;
        
        /// <summary>
        /// 시스템에서 사용할 정수 트리거 키코드 반복자
        /// 
        /// 내부에는 유니티 키코드의 정수 값이 들어가 있다.
        /// </summary>
        public static int[] TriggerIKeyCodeIterator;
        
        /// <summary>
        /// 시스템에서 사용할 기능 키코드 반복자
        /// 
        /// 내부에는 유니티 키코드의 정수 값이 들어가 있다.
        /// </summary>
        public static KeyCode[] FunctionKeyCodeIterator;
        
        /// <summary>
        /// 시스템에서 사용할 정수 기능 키코드 반복자
        /// 
        /// 내부에는 유니티 키코드의 정수 값이 들어가 있다.
        /// </summary>
        public static int[] FunctionIKeyCodeIterator;

        /// <summary>
        /// 유니티 KeyCode의 정수값 IKeyCode에 MIKeyCode가 매핑된 테이블
        /// 매핑 규칙은 KeyCodeTable을 기준으로 한다.
        ///
        /// [IKeyCode, MIKeyCode]
        ///
        /// 해당 테이블을 수정하는 것으로 입력 키값과 시스템 내부에서 인식하는 키를 재배치 할 수 있다.
        /// </summary>
        public static int[] MIKeyCodeTable;
        
        /// <summary>
        /// [TriggerKeyType, MIKeyCode] 매핑 테이블
        /// </summary>
        public static Dictionary<TriggerKeyType, int> InvTriggerKeyTable;
        
        /// <summary>
        /// 키맵 초기화 함수
        /// </summary>
        public static void InitKeyMap()
        {
            MIKeyCodeTable = new int[KeyCodeTable.IKeyCodeMax];
            Array.Fill(MIKeyCodeTable, -1);
            
            var keyCodeTable = KeyCodeTable.GetInstanceUnsafe.GetTable();
            foreach (var recordKV in keyCodeTable)
            {
                var keyCode = recordKV.Key;
                var _MIKeyCode = recordKV.Value.Value;

                MIKeyCodeTable[(int)keyCode] = _MIKeyCode;
            }

            InvTriggerKeyTable = new Dictionary<TriggerKeyType, int>();
            if (EnumFlag.TryGetEnumEnumerator<TriggerKeyType>(EnumFlag.GetEnumeratorType.ExceptMask, out var o_Enumerator))
            {
                foreach (var inputKeyCode in o_Enumerator)
                {
                    InvTriggerKeyTable.Add(inputKeyCode, inputKeyCode.ConvertToMIKeyCode());
                }
            }
            
            UpdateKeyMap();
        }

        private static void UpdateKeyMap()
        {
            var arrowIKeyCodeIterator = new List<int>();
            var triggerIKeyCodeIterator = new List<int>();
            var functionIKeyCodeIterator = new List<int>();

            for (int i = 0; i < KeyCodeTable.IKeyCodeMax; i++)
            {
                var _MIKeyCode = MIKeyCodeTable[i];
                switch (_MIKeyCode)
                {
                    case var _ when _MIKeyCode < 0 :
                        break;
                    case var _ when _MIKeyCode < ARROW_MKEYCODE_UPPERBOUND :
                        arrowIKeyCodeIterator.Add(i);
                        break;
                    case var _ when _MIKeyCode < TRIGGER_MKEYCODE_UPPERBOUND :
                        triggerIKeyCodeIterator.Add(i);
                        break;
                    default:
                        functionIKeyCodeIterator.Add(i);
                        break;
                }
            }

            ArrowIKeyCodeIterator = arrowIKeyCodeIterator.ToArray();
            ArrowKeyCodeIterator = ArrowIKeyCodeIterator.Select(_IKey => (KeyCode)_IKey).ToArray();
            TriggerIKeyCodeIterator = triggerIKeyCodeIterator.ToArray();
            TriggerKeyCodeIterator = TriggerIKeyCodeIterator.Select(_IKey => (KeyCode)_IKey).ToArray();
            FunctionIKeyCodeIterator = functionIKeyCodeIterator.ToArray();
            FunctionKeyCodeIterator = FunctionIKeyCodeIterator.Select(_IKey => (KeyCode)_IKey).ToArray();
        }
        
        /// <summary>
        /// IKeyCodeTable을 복사하여 리턴하는 메서드
        /// </summary>
        public static int[] CloneIKeyCodeTable()
        {
            return (int[]) MIKeyCodeTable.Clone();
        }
    }
}