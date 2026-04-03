using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 해당 카메라의 View 벡터 기준으로 반앙각 아래 방향을 표시하는 아핀 객체
        /// </summary>
        private Transform _CameraFovHalfLowerLookVector;

        /// <summary>
        /// 카메라 랜더링 사각뿔대 근면 너비 절반 값
        /// </summary>
        private float NearPlaneBasisU;
        
        /// <summary>
        /// 카메라 랜더링 사각뿔대 근면 높이 절반 값
        /// </summary>
        private float NearPlaneBasisV;

        /// <summary>
        /// 카메라 랜더링 사각뿔대 근면에 외접하는 원의 반지름
        /// </summary>
        private float NearPlaneRadius;
        
        /// <summary>
        /// 카메라 랜더링 사각뿔대 근면에 외접하는 구의 반지름
        /// </summary>
        private float NearPlaneInnerCrossRadius;

        /// <summary>
        /// 박스 캐스트에 사용할 반경
        /// </summary>
        private Vector3 NearPlaneBoxHalfExtend;

        /// <summary>
        /// 카메라 뷰포트 평면 프리셋
        /// </summary>
        private CustomPlane _CameraViewPort;
        
        #endregion

        #region <Callbacks>

        private void OnCreateGeometry()
        {
            /* 메인 카메라에 하위 오브젝트를 추가해준다. */
            _CameraFovHalfLowerLookVector = new GameObject("FovHalfLowerForwardIndicator").transform;
            _CameraFovHalfLowerLookVector.SetParent(MainCameraTransform, false);

            var fovHalf = MainCamera.fieldOfView * 0.5f;
            _CameraFovHalfLowerLookVector.Rotate(Vector3.right, fovHalf, Space.Self);
            
            var cameraNearPlaneDistance = MainCamera.nearClipPlane;
            NearPlaneBasisV = cameraNearPlaneDistance * Mathf.Tan(fovHalf * Mathf.Deg2Rad);
            NearPlaneBasisU = NearPlaneBasisV * MainCamera.aspect;
            NearPlaneRadius = CustomMath.GetPitagorasValue(NearPlaneBasisU, NearPlaneBasisV);
            NearPlaneInnerCrossRadius = CustomMath.GetPitagorasValue(NearPlaneBasisU, NearPlaneBasisV, cameraNearPlaneDistance);
            NearPlaneBoxHalfExtend = new Vector3(NearPlaneBasisU, NearPlaneBasisV, 0f);
            
            _CameraViewPort = CustomPlane.GetLocationWithC01(new Vector3(0.5f, 0.5f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f));
        }

        #endregion
    }
}