#if !SERVER_DRIVE

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라의 시야에 관련된 기능을 기술하는 부분 클래스
    /// </summary>
    public partial class CameraManager
    {
        /// <summary>
        /// 지정한 월드 좌표가 카메라 뷰포트 내에 있는지 검증하는 논리메서드
        /// </summary>
        public bool IsPositionAtCameraScreenSpace(Vector3 p_TargetPosition)
        {
            var viewPoint = MainCamera.WorldToViewportPoint(p_TargetPosition);
            var isEnteredViewPoint = viewPoint.x >= 0 && viewPoint.x <= 1 && viewPoint.y >= 0 && viewPoint.y <= 1 &&
                                     viewPoint.z > 0;
            return isEnteredViewPoint;
        }

        /// <summary>
        /// 지정한 오브젝트가 현재 장해물 등으로 가려져있는지 검증하는 메서드
        /// </summary>
        public bool IsBlockedToRender(ICameraFocusable p_Target)
        {
            var lowerPosition = p_Target.Affine.position;
            var upperPosition = p_Target.GetTopPosition();
            var radiusOffset = p_Target.GetRadius() * MainCameraTransform.right;
            var leftPosition = p_Target.GetCenterPosition() - radiusOffset;
            var rightPosition = p_Target.GetCenterPosition() + radiusOffset;
            var startPosition = MainCameraTransform.position;
           
            return PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(lowerPosition, startPosition, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(upperPosition, startPosition, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(leftPosition, startPosition, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(rightPosition, startPosition, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide);
        }
        
        /// <summary>
        /// 지정한 오브젝트가 현재 장해물 등으로 가려져있는지 검증하는 메서드
        /// 캐스팅할 거리를 아는 경우 이쪽을 사용한다.
        /// </summary>
        public bool IsBlockedToRender(ICameraFocusable p_Target, float p_Distance)
        {
            var lowerPosition = p_Target.Affine.position;
            var upperPosition = p_Target.GetTopPosition();
            var startPosition = MainCameraTransform.position;
 
            return PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos_n_Distance(lowerPosition, startPosition, p_Distance, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                   && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos_n_Distance(upperPosition, startPosition, p_Distance, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide);
        }
    }
}
#endif