using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class ActionTool
    {
        /// <summary>
        /// 모듈에 등록할 액션을 기술하는 프리셋
        /// </summary>
        [Serializable]
        public struct ActionBindPreset
        {
            #region <Fields>

            /// <summary>
            /// 레코드 인덱스
            /// </summary>
            public readonly int Index;

            /// <summary>
            /// 입력 키
            /// </summary>
            public readonly InputEventTool.TriggerKeyType QuickCommand;

            /// <summary>
            /// 추가할 액션 레벨
            /// </summary>
            public readonly int StartLevel;
            
            /// <summary>
            /// 유효성 플래그
            /// </summary>
            public readonly bool ValidFlag;
            
            #endregion

            #region <Constructors>

            public ActionBindPreset(int p_Index, int p_Level = 1)
            {
                Index = p_Index;
                QuickCommand = InputEventTool.TriggerKeyType.None;
                StartLevel = p_Level;
                ValidFlag = true;
            }
            
            public ActionBindPreset(int p_Index, InputEventTool.TriggerKeyType p_QuickCommand, int p_Level = 1)
            {
                Index = p_Index;
                QuickCommand = p_QuickCommand;
                StartLevel = p_Level;
                ValidFlag = true;
            }

            #endregion
        }
        
        /// <summary>
        /// 액션이 발동했을 때 그 정보를 기술하는 프리셋
        /// </summary>
        public struct ActionActivateInfo
        {
            #region <Fields>

            /// <summary>
            /// 예약된 액션
            /// </summary>
            public readonly IActionEventHandler ActionEventHandler;

            /// <summary>
            /// 입력된 퀵 커맨드
            /// </summary>
            public readonly InputEventTool.TriggerKeyType QuickCommand;

            /// <summary>
            /// 입력된 연속 커맨드
            /// </summary>
            public readonly SequenceCommand SequenceCommand;

            /// <summary>
            /// 입력된 타임 스탬프
            /// </summary>
            public readonly float TimeStamp;

            #endregion

            #region <Constructor>

            public ActionActivateInfo(InputEventTool.TriggerKeyType p_QuickCommand, IActionEventHandler p_EventHandler)
            {
                QuickCommand = p_QuickCommand;
                ActionEventHandler = p_EventHandler;
                SequenceCommand = default;
                TimeStamp = Time.time;
            }
            
            #endregion
        }
        
        /// <summary>
        /// 시퀀스 커맨드 프리셋
        /// </summary>
        [Serializable]
        public struct SequenceCommand : IEquatable<SequenceCommand>
        {
            #region <Fields>

            /// <summary>
            /// 이전에 입력했던 방향키 4자리 코드, [1111, 4444] 구간의 값을 갖는다.
            /// </summary>
            public readonly int ArrowSequence;

            /// <summary>
            /// 해당 커맨드를 발생시킨 트리거 커맨드 타입
            /// </summary>
            public readonly InputEventTool.TriggerKeyType TriggerKey;
                    
            /// <summary>
            /// 유효성 플래그
            /// </summary>
            public readonly bool ValidFlag;

            #endregion
            
            #region <Constructors>
            
            public SequenceCommand(int p_ArrowSequence, InputEventTool.TriggerKeyType p_TriggerKey)
            {
                ArrowSequence = p_ArrowSequence;
                TriggerKey = p_TriggerKey;
                ValidFlag = true;
            }

            #endregion

            #region <Operator>
 
            /// <summary>
            /// IEquatable 동등성 검증 메서드
            /// </summary>
            public override bool Equals(object p_Right)
            {
                return p_Right is SequenceCommand c_Right && Equals(c_Right);
            }
            
            /// <summary>
            /// IEquatable<T>동등성 검증 메서드
            /// </summary>
            public bool Equals(SequenceCommand p_RightValue)
            {
                return TriggerKey == p_RightValue.TriggerKey
                    && ArrowSequence == p_RightValue.ArrowSequence;
            }

            /// <summary>
            /// 해쉬코드는 불변값이므로 미리 계산해서 캐싱한 값을 리턴한다.
            /// </summary>
            public override int GetHashCode()
            {
                return HashCode.Combine(ArrowSequence, TriggerKey);
            }

            /// <summary>
            /// 동등연산자 == 재정의
            /// </summary>
            public static bool operator ==(SequenceCommand p_Left, SequenceCommand p_Right)
            {
                return p_Left.Equals(p_Right);
            }

            /// <summary>
            /// 동등연산자 != 재정의
            /// </summary>
            public static bool operator !=(SequenceCommand p_Left, SequenceCommand p_Right)
            {
                return !p_Left.Equals(p_Right);
            }
 
#if UNITY_EDITOR
            public override string ToString()
            {
                return $"[Seq : (ArrowSeq : {ArrowSequence}) + (Trigger : {TriggerKey})]";
            }
#endif
 
            #endregion

            #region <Methods>

            public SequenceCommand GetInverseSequence()
            {
                var seq = ArrowSequence;
                var invArrowSequence = 0;
                var pow = 1;
                while (seq > 0)
                {
                    var arrow = seq % 10;
                    switch (arrow)
                    {
                        case 2:
                            invArrowSequence += 4 * pow;
                            break;
                        case 4:
                            invArrowSequence += 2 * pow;
                            break;
                        default:
                            invArrowSequence += arrow * pow;
                            break;
                    }
                    seq /= 10;
                    pow *= 10;
                }

                return new SequenceCommand(invArrowSequence, TriggerKey);
            }
            
            public SequenceCommand GetLowerSequence()
            {
                var seq = ArrowSequence;
                var comparer = 1;
                while (seq > 10 * comparer)
                {
                    comparer *= 10;
                }

                var lowerArrowSequence = seq % comparer;
                return new SequenceCommand(lowerArrowSequence, TriggerKey);
            }

            #endregion
        }
    }
}