using System;
using System.Collections.Generic;
using k514.Mono.Common;
using k514.Mono.Feature;

namespace k514
{
    /// <summary>
    /// 프리팹 컴포넌트에 적용할 데이터를 가지는 프리셋
    /// </summary>
    public readonly struct PrefabComponentDataParams : IEquatable<PrefabComponentDataParams>
    {
        #region <Fields>

        /// <summary>
        /// 프리팹 컴포넌트 데이터 레코드 외의 컴포넌트 데이터 타입
        /// </summary>
        public readonly PrefabPoolTool.PrefabComponentDataParamType PrefabComponentDataParamType;

        /// <summary>
        /// 프리팹 컴포넌트 데이터 테이블 레코드
        /// </summary>
        public readonly IPrefabComponentDataTableRecordBridge TableRecord;

        /// <summary>
        /// 단일 추가 컴포넌트
        /// </summary>
        public readonly Type MainComponentType;

        /// <summary>
        /// 컴포넌트 스케일
        /// </summary>
        private readonly float ComponentScale;
        
        /// <summary>
        /// 해시 값
        /// </summary>
        private readonly int _Hash;
        
        /// <summary>
        /// 유효성 플래그
        /// </summary>
        public readonly bool ValidFlag;
        
        #endregion
        
        #region <Constructors>

        public PrefabComponentDataParams(IPrefabComponentDataTableRecordBridge p_ComponentDataRecord)
        {
            if (ReferenceEquals(null, p_ComponentDataRecord))
            {
                PrefabComponentDataParamType = PrefabPoolTool.PrefabComponentDataParamType.None;
                TableRecord = default;
                MainComponentType = default;
                ComponentScale = 1f;
                _Hash = 0;
                ValidFlag = false;
            }
            else
            {
                PrefabComponentDataParamType = PrefabPoolTool.PrefabComponentDataParamType.TableComponent;
                TableRecord = p_ComponentDataRecord;
                var tryComponentType = TableRecord.MainComponentType;
                MainComponentType = tryComponentType.IsMonoBehaviour() ? tryComponentType : null;
                ComponentScale = TableRecord.ComponentScaleFactor;
                _Hash = HashCode.Combine(PrefabComponentDataParamType, TableRecord.GetHashCode());
                ValidFlag = true;
            }
        }

        public PrefabComponentDataParams(Type p_Component)
        {
            if (ReferenceEquals(null, p_Component) || !p_Component.IsMonoBehaviour())
            {
                PrefabComponentDataParamType = PrefabPoolTool.PrefabComponentDataParamType.None;
                TableRecord = default;
                MainComponentType = default;
                ComponentScale = 1f;
                _Hash = 0;
                ValidFlag = false;
            }
            else
            {
                PrefabComponentDataParamType = PrefabPoolTool.PrefabComponentDataParamType.MonoComponent;
                TableRecord = default;
                MainComponentType = p_Component;
                ComponentScale = 1f;
                _Hash = HashCode.Combine(PrefabComponentDataParamType, MainComponentType);
                ValidFlag = true;
            }
        }

        #endregion

        #region <Operator>

        /// <summary>
        /// IEquatable 동등성 검증 메서드
        /// </summary>
        public override bool Equals(object p_Right)
        {
            return p_Right is PrefabComponentDataParams c_Right && Equals(c_Right);
        }
        
        /// <summary>
        /// IEquatable<T>동등성 검증 메서드
        /// </summary>
        public bool Equals(PrefabComponentDataParams p_RightValue)
        {
            switch (PrefabComponentDataParamType)
            {
                default:
                case PrefabPoolTool.PrefabComponentDataParamType.None:
                    return EqualityComparer<PrefabPoolTool.PrefabComponentDataParamType>.Default.Equals(PrefabComponentDataParamType, p_RightValue.PrefabComponentDataParamType);
                case PrefabPoolTool.PrefabComponentDataParamType.TableComponent:
                    return EqualityComparer<PrefabPoolTool.PrefabComponentDataParamType>.Default.Equals(PrefabComponentDataParamType, p_RightValue.PrefabComponentDataParamType)
                        && EqualityComparer<IPrefabComponentDataTableRecordBridge>.Default.Equals(TableRecord, p_RightValue.TableRecord);
                case PrefabPoolTool.PrefabComponentDataParamType.MonoComponent:
                    return EqualityComparer<PrefabPoolTool.PrefabComponentDataParamType>.Default.Equals(PrefabComponentDataParamType, p_RightValue.PrefabComponentDataParamType)
                           && EqualityComparer<Type>.Default.Equals(MainComponentType, p_RightValue.MainComponentType);
            }
        }
            
        /// <summary>
        /// 해쉬코드는 불변값이므로 미리 계산해서 캐싱한 값을 리턴한다.
        /// </summary>
        public override int GetHashCode()
        {
            return _Hash;
        }
                
        /// <summary>
        /// 동등연산자 == 재정의
        /// </summary>        
        public static bool operator ==(PrefabComponentDataParams p_Left, PrefabComponentDataParams p_Right)
        {
            return p_Left.Equals(p_Right);
        }

        /// <summary>
        /// 동등연산자 != 재정의
        /// </summary>
        public static bool operator !=(PrefabComponentDataParams p_Left, PrefabComponentDataParams p_Right)
        {
            return !p_Left.Equals(p_Right);
        }
        
        /// <summary>
        /// 변환연산자 () 재정의
        /// </summary>
        public static implicit operator bool(PrefabComponentDataParams p_Conversion)
        {
            return p_Conversion.ValidFlag;
        }
        
        /// <summary>
        /// 변환연산자 () 재정의
        /// </summary>
        public static implicit operator PrefabComponentDataParams(Type p_Conversion)
        {
            return new PrefabComponentDataParams(p_Conversion);
        }
        
#if UNITY_EDITOR
        public override string ToString()
        {
            if (ValidFlag)
            {
                switch (PrefabComponentDataParamType)
                {
                    default:
                    case PrefabPoolTool.PrefabComponentDataParamType.None:
                    {
                        return "None";
                    }
                    case PrefabPoolTool.PrefabComponentDataParamType.TableComponent:
                    {
                        return $"[Extra : {TableRecord.KEY}]\n[{TableRecord}]";
                    }
                    case PrefabPoolTool.PrefabComponentDataParamType.MonoComponent:
                    {
                        return $"[Type : {MainComponentType}]";
                    }
                }
            }
            else
            {
                return "Null";
            }
        }
#endif
        #endregion

        #region <Methods>

        public float GetComponentScale()
        {
            return ValidFlag ? ComponentScale : 1f;
        }

        #endregion
    }
}