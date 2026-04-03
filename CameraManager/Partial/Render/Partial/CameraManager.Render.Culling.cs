#if !SERVER_DRIVE

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 기본 컬링 마스크 값
        /// </summary>
        private int DefaultMask;
        
        /// <summary>
        /// 원거리 컬링 거리 배열
        /// </summary>
        private float[] FarCullingDistanceMap;

        /// <summary>
        /// 유닛 컬링 거리
        /// </summary>
        private ScaleFloatSqr _UnitCullingDistance;

        #endregion

        #region <Callbacks>

        private void OnCreateCulling()
        {
            // 카메라가 랜더링 하지 않을 레이어는 제거해준다.
            RemoveCullingMask(GameConst.GameLayerMaskType.CameraIgnore);
            DefaultMask = MainCamera.cullingMask;
            FarCullingDistanceMap = new float[32];
            _UnitCullingDistance = new ScaleFloatSqr(1f, 0f);
        }
        
        /// <summary>
        /// 현재 씬 설정 레코드로부터 추적 거리 프리셋을 가져오는 메서드
        /// </summary>
        private void OnUpdateCameraCullingConfig()
        {
            var farCullingDistances = _CurrentCameraVariableDataRecord.FarCullingDistances;
            if(farCullingDistances != null)
            {
                foreach (var farCullingKV in farCullingDistances)
                {
                    var tryLayerType = farCullingKV.Key;
                    var tryCullingDistance = farCullingKV.Value;
                    SetFarCullingDistance(tryLayerType, tryCullingDistance, false);

                    // UnitA레이어를 기준으로 삼는다
                    if (tryLayerType == GameConst.GameLayerType.UnitA)
                    {
                        SetFarCullingDistance(GameConst.GameLayerType.UnitB, tryCullingDistance, false);
                        SetFarCullingDistance(GameConst.GameLayerType.UnitC, tryCullingDistance, false);
                    }
                }
            }
            
            UpdateFarCullingDistances();
        }
        
        #endregion
        
        #region <Method/CullingMask>

        public void SetCameraBlind()
        {
            MainCamera.cullingMask = 0;
        }

        public void SetCullingMask(int p_CullingMask)
        {
            MainCamera.cullingMask = p_CullingMask;
        }

        public void ResetCullingMask()
        {
            SetCullingMask(DefaultMask);
        }

        public void ToggleCullingMask(GameConst.GameLayerMaskType p_LayerMaskType)
        {
            MainCamera.cullingMask ^= (int)p_LayerMaskType;
        }
        
        public void AddCullingMask(GameConst.GameLayerMaskType p_LayerMaskType)
        {
            MainCamera.cullingMask |= (int)p_LayerMaskType;
        }

        public void RemoveCullingMask(GameConst.GameLayerMaskType p_LayerMaskType)
        {
            MainCamera.cullingMask &= ~(int)p_LayerMaskType;
        }
   
        #endregion

        #region <Method/CullingDistance>

        /// <summary>
        /// 원거리 카메라 컬링 거리를 레이어 별로 설정하는 메서드
        /// </summary>
        public void SetFarCullingDistance(GameConst.GameLayerType p_LayerType, float p_Distance, bool p_UpdateFlag)
        {
            var layer = (int)p_LayerType;
            FarCullingDistanceMap[layer] = p_Distance;

            if (p_UpdateFlag)
            {
                UpdateFarCullingDistances();
            }
        }

        public void UpdateFarCullingDistances()
        {
            MainCamera.layerCullDistances = FarCullingDistanceMap;
            _UnitCullingDistance.SetScale(GetFarCullingDistance(GameConst.GameLayerType.UnitA));
            CameraEventSenderManager.GetInstanceUnsafe.SendEvent(CameraTool.CameraEventType.FarCullingDistanceChanged, default);
        }
        
        /// <summary>
        /// 지정한 레이어의 원거리 카메라 컬링 거리를 리턴하는 메서드
        /// </summary>
        public float GetFarCullingDistance(GameConst.GameLayerType p_LayerType)
        {
            var layer = (int)p_LayerType;
            return FarCullingDistanceMap[layer];
        }

        /// <summary>
        /// true 하는 경우, 거리 기반의 구면 레이어 컬링을 수행한다.
        /// false 하는 경우, far plane을 조정해서 Camera Frustum을 통해 컬링을 수행한다.
        ///
        /// 만약 카메라가 제자리 회전을 하는 경우, 각 오브젝트의 카메라 거리는 변하지 않으므로
        /// true 기준으로는 컬링되는 오브젝트 멤버가 변화가 없다.
        ///
        /// 반대로 false 기준으로는 회전에 따라 Frustum이 움직이므로, 회전만으로도
        /// 컬링되는 멤버가 변한다.
        ///
        /// 기본값은 false이다.
        /// </summary>
        public void SetLayerCullingSpherical(bool p_Flag)
        {
            MainCamera.layerCullSpherical = p_Flag;
        }

        private (CameraTool.CameraCullingState, float) GetCullingType(ICameraFocusable p_TryUnit)
        {
            var tryBottomPosition = p_TryUnit.GetBottomPosition();
            var tryCenterPosition = p_TryUnit.GetCenterPosition();
            var tryCenterViewPort = MainCamera.WorldToViewportPoint(tryCenterPosition);
            if (tryCenterViewPort.z > 0f)
            {
                var resultSqrDistance = (_ViewControlFocusWrapper.localPosition + tryBottomPosition).GetDirectionVectorTo(_ViewControlZoomWrapper.position).sqrMagnitude;
                if (resultSqrDistance > _UnitCullingDistance.CurrentValueSqr)
                {
                    return (CameraTool.CameraCullingState.FarCulling, resultSqrDistance);
                }
                else if(resultSqrDistance > _CurrentFocusPreset.NearCullingRadius.CurrentValueSqr)
                {
                    var targetRadius = p_TryUnit.GetRadius();
                    var targetHalfHeight = p_TryUnit.GetHeight(0.5f);
                    var viewPortUBias = MainCameraTransform.right;
                    var viewPortVBias = MainCameraTransform.up;
                    
                    var p0 = tryCenterPosition + targetRadius * viewPortUBias + targetHalfHeight * viewPortVBias;
                    var p2 = tryCenterPosition - targetRadius * viewPortUBias - targetHalfHeight * viewPortVBias;
                    var p3 = tryCenterPosition + targetRadius * viewPortUBias - targetHalfHeight * viewPortVBias;
                    var p1 = tryCenterPosition - targetRadius * viewPortUBias + targetHalfHeight * viewPortVBias;
                    
                    var p0ViewPort = MainCamera.WorldToViewportPoint(p0);
                    var p1ViewPort = MainCamera.WorldToViewportPoint(p1);
                    var p2ViewPort = MainCamera.WorldToViewportPoint(p2);
                    
                    var tryPlane = CustomPlane.GetLocationWith012(p0ViewPort, p1ViewPort, p2ViewPort);
                    var ppIntersectType = _CameraViewPort.GetPlanePlaneIntersectType(tryPlane, false);
                    var isEnteredViewPoint = ppIntersectType != CustomMath.PlanePlaneIntersectType.Out;
                    
                    if (isEnteredViewPoint)
                    {
                        var startPosition = MainCameraTransform.position;
                        var blocked = PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(startPosition, p0, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(startPosition, p1, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(startPosition, p2, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide)
                                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos(startPosition, p3, GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide);

                        var resultType = blocked ? CameraTool.CameraCullingState.Blocked : CameraTool.CameraCullingState.Visible;
                        return (resultType, resultSqrDistance);
                    }
                    else
                    {
                        return (CameraTool.CameraCullingState.OutOfScreen, resultSqrDistance);
                    }
                }
                else
                {
                    return (CameraTool.CameraCullingState.NearCulling, resultSqrDistance);
                }
            }
            else
            {
                return (CameraTool.CameraCullingState.OutOfScreen, 0f);
            }
        }
        
        #endregion
    }
}

#endif