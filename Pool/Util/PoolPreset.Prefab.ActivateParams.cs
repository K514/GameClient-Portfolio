using UnityEngine;

namespace k514
{
    /// <summary>
    /// 프리팹 초기화 파라미터 인터페이스
    /// </summary>
    public interface IPrefabActivateParams : IActivateParams
    {
        /// <summary>
        /// 오브젝트의 부모 아핀 객체
        /// </summary>
        public Transform Wrapper { get; }
    }
    
    /// <summary>
    /// 프리팹 초기화 파라미터 프리셋
    /// </summary>
    public struct PrefabActivateParams : IPrefabActivateParams
    {
        #region <Fields>

        public Transform Wrapper { get; private set; }

        #endregion

        #region <Constructor>

        public PrefabActivateParams(Transform p_Wrapper)
        {
            Wrapper = p_Wrapper;
        }

        #endregion
    }
}