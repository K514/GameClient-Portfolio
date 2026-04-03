using System;

namespace k514
{
    /// <summary>
    /// 오브젝트 생성 파라미터 인터페이스
    /// </summary>
    public interface IObjectCreateParams : ICreateParams
    {
    }

    /// <summary>
    /// 오브젝트 생성 파라미터 프리셋
    /// </summary>
    public readonly struct ObjectCreateParams : IObjectCreateParams
    {
        #region <Fields>

        private readonly int _Hash;

        #endregion

        #region <Operator>

        /// <summary>
        /// 해쉬코드는 불변값이므로 미리 계산해서 캐싱한 값을 리턴한다.
        /// </summary>
        public override int GetHashCode()
        {
            return _Hash;
        }
            
        #endregion
    }
}