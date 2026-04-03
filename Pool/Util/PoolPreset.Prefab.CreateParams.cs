using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// 프리팹 생성 파라미터 인터페이스
    /// </summary>
    public interface IPrefabCreateParams<Me> : ICreateParams where Me : IPrefabCreateParams<Me>
    {
        /// <summary>
        /// 로드된 프리팹 데이터
        /// </summary>
        public AssetLoadKey AssetLoadKey { get; }
        
        /// <summary>
        /// 프리팹 모델 데이터
        /// </summary>
        public PrefabModelDataParams ModelData { get; }

        /// <summary>
        /// 프리팹 컴포넌트 데이터
        /// </summary>
        public PrefabComponentDataParams ComponentData { get; }

        /// <summary>
        /// 컴포넌트 레코드로부터 프리팹에 부착해야할 컴포넌트 타입을 리턴하는 메서드
        /// </summary>
        Type GetComponent();

        /// <summary>
        /// 기본 스케일 배율을 리턴하는 메서드
        /// </summary>
        float GetDefaultScale();
    }
    
    /// <summary>
    /// 프리팹 생성 파라미터 기저클래스
    /// </summary>
    public abstract class PrefabCreateParamsBase<Me> : IEquatable<PrefabCreateParamsBase<Me>>, IPrefabCreateParams<Me>
     where Me : PrefabCreateParamsBase<Me>, new()
    {
        #region <Consts>

        public static int GenerateHashCode(AssetLoadKey p_AssetLoadKey, PrefabModelDataParams p_ModelData, PrefabComponentDataParams p_ComponentData)
        {
            return HashCode.Combine(p_AssetLoadKey, p_ModelData, p_ComponentData);
        }

        public static Me GetCreateParams(AssetLoadKey p_AssetLoadKey, PrefabModelDataParams p_ModelData, PrefabComponentDataParams p_ComponentData)
        {
            var spawned = new Me();
            spawned.ConstructorContinuation(p_AssetLoadKey, p_ModelData, p_ComponentData);
            
            return spawned;
        }

        #endregion
        
        #region <Fields>

        public AssetLoadKey AssetLoadKey { get; private set; }
        public PrefabModelDataParams ModelData { get; private set; }
        public PrefabComponentDataParams ComponentData { get; private set; }
        private int _Hash;

        #endregion

        #region <Constructor>

        protected PrefabCreateParamsBase()
        {
        }
        
        protected PrefabCreateParamsBase(AssetLoadKey p_AssetLoadKey, PrefabModelDataParams p_ModelData, PrefabComponentDataParams p_ComponentData)
        { 
            ConstructorContinuation(p_AssetLoadKey, p_ModelData, p_ComponentData);
        }

        private void ConstructorContinuation(AssetLoadKey p_AssetLoadKey, PrefabModelDataParams p_ModelData, PrefabComponentDataParams p_ComponentData)
        {
            AssetLoadKey = p_AssetLoadKey;
            ModelData = p_ModelData;
            ComponentData = p_ComponentData;
            _Hash = GenerateHashCode(AssetLoadKey, ModelData, ComponentData);
        }

        #endregion

        #region <Operator>

        /// <summary>
        /// IEquatable 동등성 검증 메서드
        /// </summary>
        public override bool Equals(object p_Right)
        {
            return p_Right is PrefabCreateParamsBase<Me> c_Right && Equals(c_Right);
        }
        
        /// <summary>
        /// IEquatable<T>동등성 검증 메서드
        /// </summary>
        public bool Equals(PrefabCreateParamsBase<Me> p_RightValue)
        {
            if (ReferenceEquals(null, p_RightValue)) return false;
            if (ReferenceEquals(this, p_RightValue)) return true;

            return EqualityComparer<AssetLoadKey>.Default.Equals(AssetLoadKey, p_RightValue.AssetLoadKey)
                   && EqualityComparer<PrefabModelDataParams>.Default.Equals(ModelData, p_RightValue.ModelData)
                   && EqualityComparer<PrefabComponentDataParams>.Default.Equals(ComponentData, p_RightValue.ComponentData);
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
        public static bool operator ==(PrefabCreateParamsBase<Me> p_Left, PrefabCreateParamsBase<Me> p_Right)
        {
            return p_Left.Equals(p_Right);
        }

        /// <summary>
        /// 동등연산자 != 재정의
        /// </summary>
        public static bool operator !=(PrefabCreateParamsBase<Me> p_Left, PrefabCreateParamsBase<Me> p_Right)
        {
            return !p_Left.Equals(p_Right);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"Asset : [{AssetLoadKey}]\nModel : [{ModelData}]\nExtra : [{ComponentData}]";
        }
#endif
            
        #endregion
        
        #region <Methods>

        public Type GetComponent()
        {
            return ComponentData.MainComponentType;
        }

        public float GetDefaultScale()
        {
            return ModelData.GetModelScale() * ComponentData.GetComponentScale();
        }

        #endregion
    }
}