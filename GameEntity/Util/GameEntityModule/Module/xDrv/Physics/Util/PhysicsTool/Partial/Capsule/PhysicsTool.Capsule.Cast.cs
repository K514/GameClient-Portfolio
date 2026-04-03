using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class PhysicsTool
    {
        /// <summary>
        /// 캡슐캐스팅을 수행하여 지정한 경로 내에 가장 가까운 충돌 오브젝트와의 거리를 리턴하는 메서드
        /// 충돌이 발생하지 않았다면 파라미터의 최대 거리를 리턴한다.
        /// 2번째 파라미터를 통해 반경의 배율을 지정할 수 있다.
        /// </summary>
        public static (bool, float) GetNearestDistance_CapsuleCast_MulRadius(Transform p_TransformExcept, Vector3 p_BasePosition, float p_Radius, float p_Height, float p_RadiusMulRate, Vector3 p_UV, float p_MaxDistance, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var basePosition = p_BasePosition + Vector3.up * (1f - p_RadiusMulRate);
            var radius = p_Radius * p_RadiusMulRate;
            var height = Mathf.Max(p_Height * p_RadiusMulRate, radius * 2f);
            
            return GetNearestDistance_CapsuleCast(p_TransformExcept, basePosition, radius, height, p_UV, p_MaxDistance, p_LayerMask, p_QueryTriggerInteraction);
        }
        
        /// <summary>
        /// 캡슐캐스팅을 수행하여 지정한 경로 내에 가장 가까운 충돌 오브젝트와의 거리를 리턴하는 메서드
        /// 충돌이 발생하지 않았다면 파라미터의 최대 거리를 리턴한다.
        /// 2번째 파라미터를 통해 반경의 추가치를 지정할 수 있다.
        /// </summary>
        public static (bool, float) GetNearestDistance_CapsuleCast_AddRadius(Transform p_TransformExcept, Vector3 p_BasePosition, float p_Radius, float p_Height, float p_RadiusAddRate, Vector3 p_UV, float p_MaxDistance, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var basePosition = p_BasePosition + Vector3.down * p_RadiusAddRate;
            var radius = p_Radius + p_RadiusAddRate;
            var height = Mathf.Max(p_Height + p_RadiusAddRate * 2f, radius * 2f);
            
            return GetNearestDistance_CapsuleCast(p_TransformExcept, basePosition, radius, height, p_UV, p_MaxDistance, p_LayerMask, p_QueryTriggerInteraction);
        }
        
        /// <summary>
        /// 캡슐캐스팅을 수행하여, 지정한 경로 내에 가장 가까운 충돌 오브젝트와의 거리를 리턴하는 메서드
        /// 충돌이 발생하지 않았다면 파라미터의 최대 거리를 리턴한다.
        /// </summary>
        public static (bool, float) GetNearestDistance_CapsuleCast(Transform p_TransformExcept, Vector3 p_BasePosition, float p_Radius, float p_Height, Vector3 p_UV, float p_MaxDistance, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var capsulePosLow = p_BasePosition;
            var capsulePosHigh = p_BasePosition + Vector3.up * p_Height;
            int hitCount = Physics.CapsuleCastNonAlloc(capsulePosLow, capsulePosHigh, p_Radius, p_UV, _NonAllocRayCast,　p_MaxDistance, p_LayerMask, p_QueryTriggerInteraction);
            if (hitCount > 0)
            {
                var result = false;
                var resultDistance = p_MaxDistance;
                for (int i = 0; i < hitCount; i++)
                {
                    var targetCastHit = _NonAllocRayCast[i];
                    if (!ReferenceEquals(targetCastHit.transform, p_TransformExcept))
                    {
                        result = true;
                        var targetDistance = targetCastHit.distance;
                        if (targetDistance < resultDistance)
                        {
                            resultDistance = targetDistance;
                        }
                    }
                }

#if UNITY_EDITOR
                if (result && Application.isPlaying && CustomDebug.DrawPivot)
                {
                    CustomDebug.DrawCapsule(p_BasePosition, capsulePosHigh, p_Radius, Color.green);
                }
#endif
                
                return (result, resultDistance);
            }
            else
            {
                return (false, p_MaxDistance);
            }
        }
        
        /// <summary>
        /// 캡슐캐스팅을 수행하여, 지정한 경로 내에 가장 가까운 충돌 오브젝트와의 거리를 리턴하는 메서드
        /// 충돌이 발생하지 않았다면 파라미터의 최대 거리를 리턴한다.
        /// </summary>
        public static (bool, Transform, float) GetNearestObjectWithDistance_CapsuleCast(Transform p_TransformExcept, Vector3 p_BasePosition, float p_Radius, float p_Height, Vector3 p_UV, float p_MaxDistance, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction)
        {
            var capsulePosLow = p_BasePosition;
            var capsulePosHigh = p_BasePosition + Vector3.up * p_Height;
            int hitCount = Physics.CapsuleCastNonAlloc(capsulePosLow, capsulePosHigh, p_Radius, p_UV, _NonAllocRayCast,　p_MaxDistance, p_LayerMask, p_QueryTriggerInteraction);
            if (hitCount > 0)
            {
                var result = false;
                var resultTransform = default(Transform);
                var resultDistance = p_MaxDistance;
                for (int i = 0; i < hitCount; i++)
                {
                    var targetCastHit = _NonAllocRayCast[i];
                    var targetAffine = targetCastHit.transform;
                    if (!ReferenceEquals(targetAffine, p_TransformExcept))
                    {
                        result = true;
                        resultTransform = targetAffine;
                        var targetDistance = targetCastHit.distance;
                        if (targetDistance < resultDistance)
                        {
                            resultDistance = targetDistance;
                        }
                    }
                }
                
                return (result, resultTransform, resultDistance);
            }
            else
            {
                return (false, null, p_MaxDistance);
            }
        }
        
        /// <summary>
        /// 캡슐캐스팅을 수행하여, 지정한 경로 내에 가장 가까운 충돌 오브젝트와의 거리를 리턴하는 메서드
        /// 충돌이 발생하지 않았다면 파라미터의 최대 거리를 리턴한다.
        /// </summary>
        public static int GetCapsuleCast(Vector3 p_BasePosition, float p_Radius, float p_Height, Vector3 p_UV, float p_MaxDistance, int p_LayerMask, QueryTriggerInteraction p_QueryTriggerInteraction, ref RaycastHit[] r_Result)
        {
            var capsulePosLow = p_BasePosition + p_Radius * Vector3.up;
            var capsulePosHigh = capsulePosLow + (p_Height - 2f * p_Radius) * Vector3.up;
            
            return Physics.CapsuleCastNonAlloc(capsulePosLow, capsulePosHigh, p_Radius, p_UV, r_Result,　p_MaxDistance, p_LayerMask, p_QueryTriggerInteraction);
        }
    }
}