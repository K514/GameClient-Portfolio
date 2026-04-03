using System;
using System.Collections.Generic;
using k514.Mono.Common;
using k514.Mono.Feature;

namespace k514
{
    /// <summary>
    /// 프리팹 모델에 적용할 데이터를 가지는 프리셋
    /// </summary>
    public readonly struct PrefabModelDataParams : IEquatable<PrefabModelDataParams>
    {
        #region <Fields>

        /// <summary>
        /// 프리팹 모델 데이터
        /// </summary>
        public readonly IPrefabModelDataTableRecordBridge TableRecord;

        /// <summary>
        /// 프리팹 모델 스케일
        /// </summary>
        private readonly float ModelScale;
        
        /// <summary>
        /// 해시 값
        /// </summary>
        private readonly int _Hash;
        
        /// <summary>
        /// 유효성 플래그
        /// </summary>
        public readonly bool ValidFlag;
        
        #endregion

        #region <Constructor>
        
        public PrefabModelDataParams(IPrefabModelDataTableRecordBridge p_PrefabModelData)
        {
            if (ReferenceEquals(null, p_PrefabModelData))
            {
                TableRecord = default;
                ModelScale = 1f;
                _Hash = 0;
                ValidFlag = false;
            }
            else
            {
                TableRecord = p_PrefabModelData;
                ModelScale = TableRecord.ModelScale;
                _Hash = HashCode.Combine(TableRecord);
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
            return p_Right is PrefabModelDataParams c_Right && Equals(c_Right);
        }
        
        /// <summary>
        /// IEquatable<T>동등성 검증 메서드
        /// </summary>
        public bool Equals(PrefabModelDataParams p_RightValue)
        {
            return EqualityComparer<IPrefabModelDataTableRecordBridge>.Default.Equals(TableRecord, p_RightValue.TableRecord);
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
        public static bool operator ==(PrefabModelDataParams p_Left, PrefabModelDataParams p_Right)
        {
            return p_Left.Equals(p_Right);
        }

        /// <summary>
        /// 동등연산자 != 재정의
        /// </summary>
        public static bool operator !=(PrefabModelDataParams p_Left, PrefabModelDataParams p_Right)
        {
            return !p_Left.Equals(p_Right);
        }

        /// <summary>
        /// 변환연산자 () 재정의
        /// </summary>
        public static implicit operator bool(PrefabModelDataParams p_Conversion)
        {
            return p_Conversion.ValidFlag;
        }
        
#if UNITY_EDITOR
        public override string ToString()
        {
            if (ValidFlag)
            {
                return $"[Model : {TableRecord.KEY}]";
            }
            else
            {
                return "Null";
            }
        }
#endif

        #endregion

        #region <Methods>

        public float GetModelScale()
        {
            return ValidFlag ? ModelScale : 1f;
        }

        #endregion
    }
}