using UnityEngine;

namespace k514.Mono.Common
{
    public partial class PhysicsTool
    {
        #region <Methods>

        /// <summary>
        /// 캐릭터 컨트롤러 중심으로부터 레이캐스팅을 수행하여 충돌 정보를 리턴하는 메서드
        /// </summary>
        public static bool GetAnyObjectCenterBelow_CharacterController(CharacterController p_CharacterController, Transform p_Wrapper, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var uv = Vector3.down;
            var scaleFactor = p_Wrapper.lossyScale.y;
            var halfHeight = 0.5f * scaleFactor * p_CharacterController.height;
            var startPos = p_Wrapper.position + scaleFactor * p_CharacterController.center + halfHeight * uv;
            
            var hitCount = GetAnyObjectCount_RayCast_CorrectStartPos(startPos, uv, CustomMath.Epsilon + p_CharacterController.skinWidth, p_LayerMask, p_QueryTriggerInteraction);
            return IsAnyAffine_ExistAt_RayCastResult_Except(p_Wrapper, hitCount);
        }
        
        /// <summary>
        /// 캐릭터 컨트롤러 하단부 구로부터 스피어캐스팅을 수행하여 충돌 정보를 리턴하는 메서드
        /// </summary>
        public static bool GetAnyObjectLowerSphereBelow_CharacterController(CharacterController p_CharacterController, Transform p_Wrapper, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var uv = Vector3.down;
            var scaleFactor = p_Wrapper.lossyScale.y;
            var halfHeight = 0.5f * scaleFactor * p_CharacterController.height;
            var radius = scaleFactor * p_CharacterController.radius;
            var startPos = p_Wrapper.position + scaleFactor * p_CharacterController.center + (halfHeight - radius) * uv;
            var distance = p_CharacterController.skinWidth + 0.05f;
            var hitCount = GetAnyObjectCount_SphereCast(startPos, uv, radius, distance, p_LayerMask, p_QueryTriggerInteraction);
            return IsAnyAffine_ExistAt_RayCastResult_Except(p_Wrapper, hitCount);
        }
        
        #endregion
    }
}