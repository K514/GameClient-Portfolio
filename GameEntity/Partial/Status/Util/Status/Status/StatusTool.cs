using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public static class StatusTool
    {
        #region <Enums>
             
        public enum PropertyValueType
        {
            None,
            
            /// <summary>
            /// 고정값
            /// </summary>
            FixedValue, 

            /// <summary>
            /// 단리 퍼센트
            /// </summary>
            SimpleRateValue, 
            
            /// <summary>
            /// 복리 퍼센트
            /// </summary>
            CompoundRateValue
        }
        
        /// <summary>
        /// 기저 능력치 그룹 타입
        /// </summary>
        public enum BaseStatusGroupType
        {
            /// <summary>
            /// 고유 능력치
            /// </summary>
            Main,
            
            /// <summary>
            /// 덧셈 연산계
            /// </summary>
            ExtraAdd,
            
            /// <summary>
            /// 단리 곱셈 연산계
            /// </summary>
            SimpleMul,

            /// <summary>
            /// 복리 곰셈 연산계
            /// </summary>
            CompoundMul,
            
            /// <summary>
            /// 전체 능력치
            /// </summary>
            Total,
        }
        
        /// <summary>
        /// 전투 능력치 그룹 타입
        /// </summary>
        public enum BattleStatusGroupType
        {
            /// <summary>
            /// 고유 능력치
            /// </summary>
            Main,
            
            /// <summary>
            /// 덧셈 연산계
            /// </summary>
            ExtraAdd,
            
            /// <summary>
            /// 단리 곱셈 연산계
            /// </summary>
            SimpleMul,

            /// <summary>
            /// 복리 곱셈 연산계
            /// </summary>
            CompoundMul,
            
            /// <summary>
            /// 전체 능력치
            /// </summary>
            Total,
            
            /// <summary>
            /// 전체 능력치의 역수
            /// </summary>
            TotalInverse,
            
            /// <summary>
            /// 전체 능력치의 제곱
            /// </summary>
            TotalSqr,
            
            /// <summary>
            /// 현재 적용중인 능력치
            /// </summary>
            Current,
        }

        /// <summary>
        /// 샷 능력치 그룹 타입
        /// </summary>
        public enum ShotStatusGroupType
        {
            /// <summary>
            /// 고유 능력치
            /// </summary>
            Main,
            
            /// <summary>
            /// 덧셈 연산계
            /// </summary>
            ExtraAdd,
            
            /// <summary>
            /// 단리 곱셈 연산계
            /// </summary>
            SimpleMul,

            /// <summary>
            /// 복리 곰셈 연산계
            /// </summary>
            CompoundMul,
            
            /// <summary>
            /// 전체 능력치
            /// </summary>
            Total,
        }
        
        public enum StatusChangeEventType
        {
            None,
            Combat,
            HealHP,
            HealMP,
            Shocking,
            Bleeding,
            Poisoning,
            Burning,
            Chilling,
        }
        
        [Flags]
        public enum StatusChangeAttribute
        {
            None = 0,
   
            HasTrigger = 1 << 0,
            HasTargetPosition = 1 << 1,
            ApplyCurrent = 1 << 2,
            Critical = 1 << 3,
        }

        #endregion

        #region <Structs>
        
        /// <summary>
        /// 능력치를 변화시켜야할 때, 추가 제어 변수를 기술하는 프리셋
        /// </summary>
        public struct StatusChangeParams
        {
            #region <Fields>

            /// <summary>
            /// 능력치 변화 이벤트 타입
            /// </summary>
            public readonly StatusChangeEventType EventType;
            
            /// <summary>
            /// 능력치 변화 속성 플래그마스크
            /// </summary>
            public StatusChangeAttribute AttributeMask;

            /// <summary>
            /// 해당 능력치 변화를 일으킨 개체
            /// </summary>
            public readonly IGameEntityBridge Trigger;

            /// <summary>
            /// 능력치 변화 이벤트 발동 기본 위치
            /// </summary>
            public readonly Vector3 TargetPosition;

            /// <summary>
            /// 능력치 변화 이벤트 발동 위치 반경
            /// </summary>
            public readonly float RandomizeRadius;

            #endregion
            
            #region <Constructor>
            
            public StatusChangeParams(StatusChangeAttribute p_AttributeMask = StatusChangeAttribute.None)
            {
                this = default;

                AttributeMask = p_AttributeMask;
            }
            
            public StatusChangeParams(StatusChangeEventType p_EventType, StatusChangeAttribute p_AttributeMask = StatusChangeAttribute.None)
            {
                this = default;

                EventType = p_EventType;
                AttributeMask = p_AttributeMask;
            }
            
            public StatusChangeParams(StatusChangeEventType p_EventType, IGameEntityBridge p_Trigger, StatusChangeAttribute p_AttributeMask = StatusChangeAttribute.None) 
                : this(p_EventType, p_AttributeMask)
            {
                Trigger = p_Trigger;
                
                AttributeMask.AddFlag(StatusChangeAttribute.HasTrigger);
            }
            
            public StatusChangeParams(StatusChangeEventType p_EventType, IGameEntityBridge p_Trigger, Vector3 p_TargetPosition, float p_RandomizeRadius = 1f, StatusChangeAttribute p_AttributeMask = StatusChangeAttribute.None) 
                : this(p_EventType, p_Trigger, p_AttributeMask)
            {
                TargetPosition = p_TargetPosition;
                RandomizeRadius = p_RandomizeRadius;
                
                AttributeMask.AddFlag(StatusChangeAttribute.HasTargetPosition);
            }
            
            #endregion

            #region <Methods>

            public bool HasAttribute(StatusChangeAttribute p_Attribute)
            {
                return AttributeMask.HasAnyFlagExceptNone(p_Attribute);
            }
            
            public void AddAttribute(StatusChangeAttribute p_Attribute)
            {
                AttributeMask.AddFlag(p_Attribute);
            }

            #endregion
        }

        /// <summary>
        /// 기저 능력치가 변화했을 때의 정보를 기술하는 프리셋
        /// </summary>
        public struct BaseStatusChangeResult
        {
            #region <Fields>

            /// <summary>
            /// 능력치 그룹 타입
            /// </summary>
            public BaseStatusGroupType GroupType;

            /// <summary>
            /// 능력치 타입
            /// </summary>
            public BaseStatusTool.BaseStatusType StatusType;

            /// <summary>
            /// 능력치 변화 파라미터
            /// </summary>
            public StatusChangeParams Params;

            /// <summary>
            /// 더해진 값
            /// </summary>
            public float AddValue;

            /// <summary>
            /// 실제 적용된 값
            /// </summary>
            public float DeltaValue;

            /// <summary>
            /// 실제 적용된 값이 유효한 값인지 검증하는 플래그
            /// </summary>
            public bool ValidFlag;

            #endregion

            #region <Constructors>

            public BaseStatusChangeResult(BaseStatusGroupType p_GroupType, BaseStatusTool.BaseStatusType p_StatusType, StatusChangeParams p_Params, float p_AddValue, float p_DeltaValue)
            {
                GroupType = p_GroupType;
                StatusType = p_StatusType;
                Params = p_Params;
                AddValue = p_AddValue;
                DeltaValue = p_DeltaValue;
                ValidFlag = !DeltaValue.IsReachedZero();
            }

            #endregion
        }
        
        /// <summary>
        /// 전투 능력치가 변화했을 때의 정보를 기술하는 프리셋
        /// </summary>
        public struct BattleStatusChangeResult
        {
            #region <Fields>

            /// <summary>
            /// 능력치 그룹 타입
            /// </summary>
            public BattleStatusGroupType GroupType;

            /// <summary>
            /// 능력치 타입
            /// </summary>
            public BattleStatusTool.BattleStatusType StatusType;

            /// <summary>
            /// 능력치 변화 파라미터
            /// </summary>
            public StatusChangeParams Params;

            /// <summary>
            /// 더해진 값
            /// </summary>
            public float AddValue;

            /// <summary>
            /// 실제 적용된 값
            /// </summary>
            public float DeltaValue;

            /// <summary>
            /// 실제 적용된 값이 유효한 값인지 검증하는 플래그
            /// </summary>
            public bool ValidFlag;

            #endregion

            #region <Constructors>

            public BattleStatusChangeResult(BattleStatusGroupType p_GroupType, BattleStatusTool.BattleStatusType p_StatusType, StatusChangeParams p_Params, float p_AddValue, float p_DeltaValue)
            {
                GroupType = p_GroupType;
                StatusType = p_StatusType;
                Params = p_Params;
                AddValue = p_AddValue;
                DeltaValue = p_DeltaValue;
                ValidFlag = !DeltaValue.IsReachedZero();
            }

            #endregion
        }
                
        /// <summary>
        /// 샷 능력치가 변화했을 때의 정보를 기술하는 프리셋
        /// </summary>
        public struct ShotStatusChangeResult
        {
            #region <Fields>

            /// <summary>
            /// 능력치 그룹 타입
            /// </summary>
            public ShotStatusGroupType GroupType;

            /// <summary>
            /// 능력치 타입
            /// </summary>
            public ShotStatusTool.ShotStatusType StatusType;

            /// <summary>
            /// 능력치 변화 파라미터
            /// </summary>
            public StatusChangeParams Params;

            /// <summary>
            /// 더해진 값
            /// </summary>
            public float AddValue;

            /// <summary>
            /// 실제 적용된 값
            /// </summary>
            public float DeltaValue;

            /// <summary>
            /// 실제 적용된 값이 유효한 값인지 검증하는 플래그
            /// </summary>
            public bool ValidFlag;

            #endregion

            #region <Constructors>

            public ShotStatusChangeResult(ShotStatusGroupType p_GroupType, ShotStatusTool.ShotStatusType p_StatusType, StatusChangeParams p_Params, float p_AddValue, float p_DeltaValue)
            {
                GroupType = p_GroupType;
                StatusType = p_StatusType;
                Params = p_Params;
                AddValue = p_AddValue;
                DeltaValue = p_DeltaValue;
                ValidFlag = !DeltaValue.IsReachedZero();
            }

            #endregion
        }
        
        #endregion
    }
}