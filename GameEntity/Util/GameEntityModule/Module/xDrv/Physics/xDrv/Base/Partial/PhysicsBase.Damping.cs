using UnityEngine;

namespace k514.Mono.Common
{
    public partial class PhysicsBase
    {
        /// <summary>
        /// 가속도 테이블에 감쇄를 적용하여, 일정 크기 미만이된 가속도를 제거하는 메서드
        /// 중력 속도에는 감쇄가 적용되지 않는다.
        /// </summary>
        protected void ApplyDampingForce()
        {
            foreach (var forceType in PhysicsTool._ForceTypeEnumeratorExceptGravity)
            {
                _PhysicsSystemTable[forceType].DampingForce();
            }
        }
    }
}