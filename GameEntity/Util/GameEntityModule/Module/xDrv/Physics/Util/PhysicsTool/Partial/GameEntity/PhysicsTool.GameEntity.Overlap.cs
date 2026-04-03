using UnityEngine;

namespace k514.Mono.Common
{
    public static partial class PhysicsTool
    {
        /// <summary>
        /// 가상 캡슐을 오버랩하여 해당 박스 내에 특정 레이어를 가진 물리 오브젝트가 존재하는지 검증하는메서드
        /// </summary>
        public static int GetCount_CapsuleOverlap(IGameEntityBridge p_GameEntity, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            return GetCount_CapsuleOverlap(p_GameEntity.Affine.position, p_GameEntity.Radius.CurrentValue, p_GameEntity.Height.CurrentValue,
                0f, p_GameEntity.Height.CurrentOffset, p_LayerMask, p_QueryTriggerInteraction);
        }

    }
}