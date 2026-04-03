using UnityEngine;

namespace k514.Mono.Common
{
    public static partial class PhysicsTool
    {
        private static readonly Vector3[] _CheckEntityLowerCollisionOffsetGroups = new[]
        {
            Vector3.left,
            Vector3.forward,
            Vector3.right,
            Vector3.back,
        };
        
        /// <summary>
        /// 지정한 개체 밑에 밟을 수 있는 다른 오브젝트가 있는지 체크하는 메서드
        /// </summary>
        public static bool CheckEntityLowerCollision(IGameEntityBridge p_Entity)
        {
            var heightSkinOffset = p_Entity.GetHeightSkinOffset();
            var startPos = p_Entity.GetBottomUpPosition(heightSkinOffset);
            var distance = heightSkinOffset + 0.05f;

            if (CheckEntityLowerCollision(p_Entity, startPos, distance)) return true;
            
            var radius = p_Entity.GetRadius();
            foreach (var offset in _CheckEntityLowerCollisionOffsetGroups)
            {
                if (CheckEntityLowerCollision(p_Entity, startPos + radius * offset, distance)) return true;
            }

            return false;
        }

        public static bool CheckEntityLowerCollision(IGameEntityBridge p_Entity, Vector3 p_StartPos, float p_Distance)
        {
            var hitCount = GetAnyObjectCount_RayCast(p_StartPos, Vector3.down, p_Distance, GameConst.VisibleBlock_Unit_LayerMask, QueryTriggerInteraction.Ignore);
            return p_Entity.IsRayCastHitting(_NonAllocRayCast, hitCount);
        }
    }
}