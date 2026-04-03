#if !SERVER_DRIVE

using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라 래퍼들 중에서 두번째 상위 계급 Transform인 Root Wrapper Transform을 담당하는 부분 클래스
    /// 카메라를 흔드는 연출을 담당하는 부분 클래스
    /// 해당 이벤트는 벡터나 부동소수점의 러프 함수를 사용하지 않기에 다른 부분 클래스 처럼 외부 클래스를 통해
    /// 이벤트를 수행하지 않는다.
    /// </summary>
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 뷰 컨트롤러
        /// </summary>
        private CameraViewControl _ViewControlAffineTransformObject => _CameraAffineWrapperControllerSet[CameraTool.CameraWrapperType.ViewControl_0];

        /// <summary>
        /// 초점 아핀 오브젝트
        /// </summary>
        private Transform _ViewControlFocusWrapper => _ViewControlAffineTransformObject.FocusWrapper;
        
        /// <summary>
        /// 회전 아핀 오브젝트
        /// </summary>
        private Transform _ViewControlRotationWrapper => _ViewControlAffineTransformObject.RotationWrapper;
        
        /// <summary>
        /// 줌 아핀 오브젝트
        /// </summary>
        private Transform _ViewControlZoomWrapper => _ViewControlAffineTransformObject.ZoomWrapper;
        
        /// <summary>
        /// 회전 아핀 오브젝트의 뷰 벡터, 즉 카메라 뷰 벡터
        /// </summary>
        public Vector3 _CameraViewVector => _ViewControlRotationWrapper.forward;
        
        /// <summary>
        /// 회전 아핀 오브젝트의 정면 벡터, 즉 카메라 뷰 벡터의 XZ성분
        /// </summary>
        public Vector3 _CameraForwardVector => _CameraViewVector.XZUVector();

        /// <summary>
        /// 카메라 천정 벡터
        /// </summary>
        public Vector3 _CameraCeilingVector => _ViewControlRotationWrapper.up;
        
        /// <summary>
        /// 카메라 오른쪽 벡터
        /// </summary>
        public Vector3 _CameraSideVector => _ViewControlRotationWrapper.right;
        
        /// <summary>
        /// 초점 위치 보간 오브젝트
        /// </summary>
        private PositionLerpIterator _FocusLerpIterator => _ViewControlAffineTransformObject.FocusController;

        /// <summary>
        /// 회전 방향 보간 오브젝트
        /// </summary>
        private DirectionLerpIterator _RotationLerpIterator => _ViewControlAffineTransformObject.RotateController;
        
        /// <summary>
        /// 줌 거리 보간 오브젝트
        /// </summary>
        private FloatValueLerpIterator _ZoomLerpIterator => _ViewControlAffineTransformObject.ZoomController;

        /// <summary>
        /// 현재 뷰컨트롤 회전 방향
        /// </summary>
        public ArrowType _RotationDirectionType;

        /// <summary>
        /// 현재 로직 상 줌 거리, ZoomLerpIterator CurrentValue 값이 실제 줌 거리이고
        /// 이쪽은 뷰컨트롤에 의해 화면을 확대/축소 해야하는 경우 수정된 논리상 거리값을 나타낸다
        /// </summary>
        private float _CurrentLogicZoomDistance;

        /// <summary>
        /// 현재 카메라 회전속도 배율
        /// </summary>
        private float _CurrentRotationSpeedRate;
                
        /// <summary>
        /// [줌 최소거리, 줌 최대거리] 구간에서 현재 줌 거리 배율, 가장 가까울 때 1 가장 멀리 있을 때 2를 가진다.
        /// </summary>
        public float _CurrentZoomDistanceRate;

        /// <summary>
        /// 현재 추적 대상의 up벡터와 카메라 Look벡터 내적값
        /// </summary>
        public float _TraceUp_CameraLook_DotValue;
        
        /// <summary>
        /// 현재 추적 대상의 up벡터와 카메라 Look벡터 내적 절대값
        /// </summary>
        public float _TraceUp_CameraLook_DotValue_Abs;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateViewControlWrapper()
        {
            _RotationLerpIterator.AddValueChangedCallback(OnUpdateRotationLerpIterator);
            _ZoomLerpIterator.AddValueChangedCallback(OnUpdateZoomLerpIterator);
        }
                
        private void OnUpdateViewControlConfig()
        {
            _ViewControlAffineTransformObject.OnUpdateViewControlConfig(_CurrentCameraVariableDataRecord);
            _CurrentLogicZoomDistance = GetDefaultZoomDistance();
            _CurrentRotationSpeedRate = _CurrentCameraConstantDataRecord.RotationSpeedMinRate;
        
            CheckViewControlZoomAgainstTerrain();
        }
        
        private void OnUpdateViewControl(float p_DeltaTime)
        {
            var direction = Vector2.Scale(_CurrentCameraConstantDataRecord.CameraRotationSpeedRateMask, CustomMath.ArrowViewPortPerpendicularVectorCollection_RightHandPivotSystem[_RotationDirectionType]);
            var localPivot = _ViewControlRotationWrapper.TransformVector(direction);
            _CurrentRotationSpeedRate = Mathf.Min(_CurrentCameraConstantDataRecord.RotationSpeedMaxRate, _CurrentRotationSpeedRate + p_DeltaTime * _CurrentCameraConstantDataRecord.RotationSpeedRate);
            _RotationLerpIterator.AddValue(localPivot, _CurrentCameraConstantDataRecord.RotationSpeed * _CurrentRotationSpeedRate * p_DeltaTime);

#if !KEEP_VIEW_DRAG_DIRECTION
            _RotationDirectionType = ArrowType.None;
#endif
        }
        
        /// <summary>
        /// 카메라 회전 시에 호출되는 콜백
        /// 
        /// 카메라가 지형 너머로 넘어가지 않도록 거리를 보정한다.
        /// </summary>
        private void OnUpdateRotationLerpIterator()
        {
            CheckViewControlZoomAgainstTerrain();

            if (_CurrentFocusPreset.IsFocusableValid())
            {
                _TraceUp_CameraLook_DotValue = Vector3.Dot(_CameraViewVector, -_CurrentFocusPreset.FocusObject.GetCeilingUV());
                _TraceUp_CameraLook_DotValue_Abs = Mathf.Abs(_TraceUp_CameraLook_DotValue);
            }
            
            CameraEventSenderManager.GetInstanceUnsafe.SendEvent(CameraTool.CameraEventType.Rotate, new CameraEventParams());
        }
        
        /// <summary>
        /// 카메라 줌 거리 변화시에 호출되는 콜백
        ///
        /// 
        /// </summary>
        private void OnUpdateZoomLerpIterator()
        {
            _CurrentZoomDistanceRate = 1f + (_ZoomLerpIterator._CurrentValue - GetCameraZoomLowerBound()) * _CurrentFocusPreset.InverseZoomRate;
          
            CameraEventSenderManager.GetInstanceUnsafe.SendEvent(CameraTool.CameraEventType.Zoom, new CameraEventParams());
        }

        #endregion

        #region <Methods>

        public float GetDefaultZoomDistance()
        {
            return _ZoomLerpIterator._DefaultValue;
        }

        public float GetCameraZoomLowerBound()
        {
            return _CurrentFocusPreset.NearBlockRadius;
        }

        public float GetCameraZoomUpperBound()
        {
            return _CurrentFocusPreset.FarBlockRadius;
        }

        /// <summary>
        /// 뷰 컨트롤 줌 거리를 지정한 값으로 하는 메서드
        /// </summary>
        public void SetViewControlZoom(float p_ZoomDistance)
        {
            _ZoomLerpIterator.Terminate();
            _CurrentLogicZoomDistance = Mathf.Clamp(p_ZoomDistance, GetCameraZoomLowerBound(), GetCameraZoomUpperBound());
            CheckViewControlZoomAgainstTerrain();
        }
        
        /// <summary>
        /// 뷰 컨트롤 줌을 지정한 값만큼 확대/축소 시키는 메서드
        /// </summary>
        public void AddViewControlZoom(float p_ZoomSpeed, float p_DeltaTime, bool p_SetLogicDistanceFlag)
        {
            var targetDistance = p_SetLogicDistanceFlag ? _CurrentLogicZoomDistance : _ZoomLerpIterator._CurrentValue;
            SetViewControlZoom(targetDistance - p_ZoomSpeed * p_DeltaTime);
        }

        public void ResetViewControl()
        {
            ResetViewControlFocus();
            ResetViewControlRotate();
            ResetViewControlZoom();
        }

        public void ResetViewControlFocus()
        {
            _FocusLerpIterator.ResetValueLerp(_CurrentCameraConstantDataRecord.ResetLerpPreMsec, _CurrentCameraConstantDataRecord.ResetLerpMsec);
        }

        /// <summary>
        /// 뷰 컨트롤 줌을 초기화 시키는 메서드
        /// </summary>
        public void ResetViewControlZoom()
        {
            _CurrentLogicZoomDistance = Mathf.Clamp(GetDefaultZoomDistance(), GetCameraZoomLowerBound(), GetCameraZoomUpperBound());
            _ZoomLerpIterator.ResetValueLerp(_CurrentCameraConstantDataRecord.ResetLerpPreMsec, _CurrentCameraConstantDataRecord.ResetLerpMsec);
        }

        /// <summary>
        /// 뷰 컨트롤 회전을 초기화 시키는 메서드
        /// </summary>
        public void ResetViewControlRotate()
        {
            _CurrentRotationSpeedRate = _CurrentCameraConstantDataRecord.RotationSpeedMinRate;
            _RotationLerpIterator.ResetValueLerp(_CurrentCameraConstantDataRecord.ResetLerpPreMsec, _CurrentCameraConstantDataRecord.ResetLerpMsec);
        }

        /// <summary>
        /// 초점으로부터 카메라로 충돌체크를 하여, 카메라가 Terrain과 충돌하지 않도록 거리를 앞당기는 메서드
        /// 
        ///     1. 줌을 수동으로 변경된 경우
        ///     2. 카메라가 회전하는 경우
        ///     3. 추적 타겟이 움직이거나
        ///
        /// 같은 타이밍에 호출되며, 만약 카메라가 당겨지는 과정에서 추적대상과의 최소 거리가 보다 가까운 곳에서
        /// 충돌이 발생했다면 최소 거리를 우선적으로 지켜준다.
        /// </summary>
        public void CheckViewControlZoomAgainstTerrain()
        {
#if CAMERA_AUTO_ZOOM
            // 지형 충돌을 수행한다.
            var (hasCollision, collisionDistance) = GetViewControlZoomCollisionDistance();

            // 충돌이 발생한 경우, 줌 거리를 충돌 거리 값으로 세트한다.
            // 이 때, 현재 줌 거리(_CurrentZoomDistanceAtLogic)는 갱신되지 않고 캐싱되어
            // 충돌을 벗어났을 때 원래 거리로 되돌리는데 사용된다.
            //
            // 또한 충돌 검증 메서드에서는 기본적으로 현재 줌 거리(_CurrentZoomDistanceAtLogic)를 최대값으로 진행되고
            // _CurrentZoomDistanceAtLogic는 SetZoom 계열 메서드에 의해 _CurrentTraceTargetPreset.FarBlockRadius 보다 항상 낮게 클램핑된다.
            //
            // 즉, _CurrentZoomDistanceAtLogic <= _CurrentTraceTargetPreset.FarBlockRadius 이므로 충돌 거리는 항상 카메라
            // 최대 거리를 상한 값으로 가지고, 하한 값(_CurrentTraceTargetPreset.NearBlockRadius)의 경우에는 충돌검증에서 따로 처리하지 않으므로
            // 아래 조건문에서 처리를 해준다.
            if (hasCollision)
            {
                _ZoomLerpIterator.TerminateEventHandler();
                _ZoomLerpIterator.SetValue(collisionDistance);
            }
            // 충돌이 검증되지 않은 경우, 현재 줌 거리를 원본 값으로 되돌려준다.
            else
#endif
            {
                if (_ZoomLerpIterator.ValidFlag)
                {
                    // 다른 줌 이벤트가 있다면, 그 이벤트를 우선한다.
                }
                else
                {
                    var eventSpawned = _ZoomLerpIterator.SetValueLerp(_CurrentLogicZoomDistance, 0, _CurrentCameraConstantDataRecord.ResetLerpMsec);
                    if (eventSpawned)
                    {
                        CameraEventSenderManager.GetInstanceUnsafe.SendEvent(CameraTool.CameraEventType.Zoom, new CameraEventParams());
                    }
                }
            }

            if (_CameraState.HasAnyFlagExceptNone(CameraTool.CameraStateFlag.SyncLogicZoomDistanceToCurrentZoomDistance))
            {
                _CurrentLogicZoomDistance = _ZoomLerpIterator._CurrentValue;
            }
        }

        /// <summary>
        /// [초점 래퍼로부터 줌 래퍼 쪽으로] 지정한 거리만큼
        /// 박스 캐스팅을 수행하여 그 사이에 장해물이 존재하는지 검증하고
        /// 있다면 초점 래퍼로부터 가장 가까운 지형지물의 거리를 리턴하는 메서드
        /// </summary>
        private (bool, float) GetViewControlZoomCollisionDistance()
        {
            var rayDistance = _CurrentLogicZoomDistance;
           
            // 포커스 대상이 있는 경우, 포커스 대상과 카메라 사이에 장해물이 있는지 검증한다.
            if (_CurrentFocusPreset.IsFocusableValid())
            {
                // 로직상 카메라 줌 거리와 현재 적용중인 줌 거리 간의 차이를 고려해서, 장해물 검증을 할 좌표를 구한다.
                var backDistanceOffset = (_ZoomLerpIterator._CurrentValue - _CurrentLogicZoomDistance) * _CameraViewVector;
                var cameraPosition = MainCameraTransform.position + backDistanceOffset;
                
                // 충돌검증은 유닛 가상범위의 특정 4지점으로의 레이캐스팅으로 수행한다.
                var tryFocusable = _CurrentFocusPreset.FocusObject;
                var blockedRender = 
                    PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos_n_Distance
                    (
                        tryFocusable.GetTopPosition(), cameraPosition, rayDistance, 
                        GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide
                    )
                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos_n_Distance
                    (
                        tryFocusable.GetBottomPosition(), cameraPosition, rayDistance,
                        GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide
                    )
                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos_n_Distance
                    (
                        tryFocusable.GetCenterPosition() + tryFocusable.GetRadius() * _CameraSideVector, cameraPosition, rayDistance,
                        GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide
                    )
                    && PhysicsTool.CheckAnyObject_RayCast_CorrectStartPos_WithTargetPos_n_Distance
                    (
                        tryFocusable.GetCenterPosition() - tryFocusable.GetRadius() * _CameraSideVector, cameraPosition, rayDistance,
                        GameConst.VisibleBlock_LayerMask, QueryTriggerInteraction.Collide
                    );
                
                // 포커스 대상과 카메라 사이에 장해물이 검증된 경우, 카메라의 NearPlane만큼의 가상 직육면체 박스를 캐스팅하여
                // 카메라의 View가 위치할 거리를 구해준다.
                //
                // 굳이 레이캐스팅 후, 박스 캐스팅을 하는 이유는 박스캐스팅으로는 현재 유닛이 다른 장해물에 가려져있는지 파악하기가 어렵고
                // 레이캐스팅으로는 카메라의 NearPlane이 '잘리지 않고' 배치될 정확한 위치를 파악하기 어렵기 때문이다.
                if (blockedRender)
                {
                    return PhysicsTool.GetNearestObjectDistance_BoxCast
                    (
                        _ViewControlAffineTransformObject.Head.position,
                        -_RotationLerpIterator._CurrentValue, 
                        NearPlaneBoxHalfExtend,
                        _ViewControlRotationWrapper.rotation,
                        rayDistance, 
                        GameConst.VisibleBlock_LayerMask
                    ); 
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return PhysicsTool.GetNearestObjectDistance_BoxCast
                (
                    _ViewControlAffineTransformObject.Head.position,
                    -_RotationLerpIterator._CurrentValue, 
                    NearPlaneBoxHalfExtend,
                    _ViewControlRotationWrapper.rotation,
                    rayDistance, 
                    GameConst.VisibleBlock_LayerMask
                ); 
            }
        }

        #endregion
    }
}

#endif