using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class PhysicsTool
    {
        /// <summary>
        /// 가상 캡슐을 오버랩하여 해당 박스 내에 지정한 Transform이 있는지 검증하는 논리메서드
        /// </summary>
        public static bool FindObjectDistance_CapsuleOverlap(Vector3 p_BasePosition, float p_Radius, float p_Height, Transform p_TargetTransform, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var capsulePosLow = p_BasePosition;
            var capsulePosHigh = p_BasePosition + Vector3.up * p_Height;
#if UNITY_EDITOR
            if (Application.isPlaying && CustomDebug.DrawPivot)
            {
                CustomDebug.DrawCapsule(capsulePosLow, capsulePosHigh, p_Radius, Color.red, 16, 3f);
            }
#endif
            int hitCount = Physics.OverlapCapsuleNonAlloc(capsulePosLow, capsulePosHigh, p_Radius, _NonAllocCollider, p_LayerMask, p_QueryTriggerInteraction);
            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    var targetOverlap = _NonAllocCollider[i];
                    if (targetOverlap.transform == p_TargetTransform)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 가상 캡슐을 오버랩하여 해당 박스 내에 특정 레이어를 가진 물리 오브젝트가 존재하는지 검증하는메서드
        /// </summary>
        public static int GetCount_CapsuleOverlap(Vector3 p_BasePosition, float p_Radius, float p_Height, float p_RadiusOffset, float p_HeightOffset, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            // 캡슐의 상하 반구를 제외한 높이
            var baiRadius = 2f * p_Radius;
            var heightExceptSphere = Mathf.Max(p_Height - baiRadius, baiRadius) + p_HeightOffset;
            var basePosition = p_BasePosition + (p_Radius - p_HeightOffset) * Vector3.up;
       
            return GetCount_CapsuleOverlap(basePosition, p_Radius + p_RadiusOffset, heightExceptSphere, p_LayerMask, p_QueryTriggerInteraction);
        }
        
        /// <summary>
        /// 가상 캡슐을 오버랩하여 해당 박스 내에 특정 레이어를 가진 물리 오브젝트가 존재하는지 검증하는메서드
        /// </summary>
        public static int GetCount_CapsuleOverlap(Vector3 p_BasePosition, float p_Radius, float p_Height, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var capsulePosLow = p_BasePosition;
            var capsulePosHigh = capsulePosLow + p_Height * Vector3.up;

            return Physics.OverlapCapsuleNonAlloc(capsulePosLow, capsulePosHigh, p_Radius, _NonAllocCollider, p_LayerMask, p_QueryTriggerInteraction);
        }
    }
}