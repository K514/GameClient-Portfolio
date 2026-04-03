#if !SERVER_DRIVE

using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라의 줌, 회전, 초점 변환을 기술하는 클래스
    /// </summary>
    public class CameraViewControl
    {
        #region <Fields>

        /// <summary>
        /// 카메라 뷰 컨트롤 래퍼 타입
        /// </summary>
        private CameraTool.CameraWrapperType _WrapperType;

        /// <summary>
        /// [뷰 컨트롤 타입, 아핀 제어 프리셋]
        /// </summary>
        private Dictionary<CameraTool.ViewControlType, AffineLerpPreset> _ViewControlAffinePresetTable;

        /// <summary>
        /// 카메라 초점 Offset 이동 컨트롤러
        /// </summary>
        public PositionLerpIterator FocusController { get; private set; }

        /// <summary>
        /// 카메라 회전 컨트롤러
        /// </summary>
        public DirectionLerpIterator RotateController { get; private set; }

        /// <summary>
        /// 카메라 줌 컨트롤러
        /// </summary>
        public FloatValueLerpIterator ZoomController { get; private set; }

        /// <summary>
        /// Head Transform, 최상단 ~ 부모 Transform
        /// </summary>
        public Transform Head { get; private set; }

        /// <summary>
        /// 초점 Transform
        /// </summary>
        public Transform FocusWrapper { get; private set; }
        
        /// <summary>
        /// 회전 Transform
        /// </summary>
        public Transform RotationWrapper { get; private set; }
        
        /// <summary>
        /// 줌 Transform
        /// </summary>
        public Transform ZoomWrapper { get; private set; }
        
        /// <summary>
        /// Rear Transform, 최하단 ~ 자식 Transform
        /// </summary>
        public Transform Rear { get; private set; }

        /// <summary>
        /// 앙각 회전 상한/하한값
        /// </summary>
        private float _RotateZBoundValue;

        #endregion
        
        #region <Indexer>

        public AffineLerpPreset this[CameraTool.ViewControlType p_Type] => _ViewControlAffinePresetTable[p_Type];
        
        #endregion
        
        #region <Constructor>

        public CameraViewControl(CameraTool.CameraWrapperType p_WrapperType)
        {
            _WrapperType = p_WrapperType;
            _ViewControlAffinePresetTable = new Dictionary<CameraTool.ViewControlType, AffineLerpPreset>();
            
            var enumerator = CameraTool.ViewControlEnumerator;
            Rear = default;
                
            foreach (var transformType in enumerator)
            {
                var spawnedTransform = new GameObject($"{_WrapperType}_{transformType}_Wrapper").transform;
                switch (transformType)
                {
                    case CameraTool.ViewControlType.Focus:
                    {
                        _ViewControlAffinePresetTable
                            .Add
                            (
                                transformType,
                                new AffineLerpPreset
                                (
                                    FocusWrapper = spawnedTransform, 
                                    FocusController = (PositionLerpIterator) IteratorTool.GetIterator(IteratorTool.LerpIteratorType.PositionLerp, OnFocusControllerUpdate)
                                )
                            );
                        break;
                    }
                    case CameraTool.ViewControlType.Rotate:
                    {
                        _ViewControlAffinePresetTable
                            .Add
                                (
                                    transformType, 
                                    new AffineLerpPreset
                                    (
                                        RotationWrapper = spawnedTransform, 
                                        RotateController = (DirectionLerpIterator) IteratorTool.GetIterator(IteratorTool.LerpIteratorType.DirectionLerp, OnRotationControllerUpdate)
                                    )
                                );
                        break;
                    }
                    case CameraTool.ViewControlType.Zoom:
                    {
                        _ViewControlAffinePresetTable
                            .Add
                                (
                                    transformType, 
                                    new AffineLerpPreset
                                    (
                                        ZoomWrapper = spawnedTransform,
                                        ZoomController = (FloatValueLerpIterator) IteratorTool.GetIterator(IteratorTool.LerpIteratorType.FloatValueLerp, OnZoomControllerUpdate)
                                    )
                                );
                        break;
                    }
                }

                // Head를 부모로 하여 뷰 컨트롤 순서대로 아핀 객체들을 재귀적으로 달아준다.
                // 가장 하위의 아핀 객체는 Rear가 된다.
                if (ReferenceEquals(null, Rear))
                {
                    Head = spawnedTransform;
                }
                else
                {
                    spawnedTransform.SetParent(Rear, false);
                }

                Rear = spawnedTransform;
            }
        }

        #endregion

        #region <Callbacks>

        public void OnUpdateViewControl(float p_DeltaTime)
        {
            foreach (var affineLerpPresetKV in _ViewControlAffinePresetTable)
            {
                affineLerpPresetKV.Value.OnUpdate(p_DeltaTime);
            }
        }
        
        /// <summary>
        /// 레코드에 기록된 카메라 관련 씬 설정을 적용하는 메서드
        /// </summary>
        public void OnUpdateViewControlConfig(CameraVariableDataTable.TableRecord p_CameraData)
        {
            // 최초 생성 기본 래퍼를 기준으로 Transform Tracker 초기화
            ResetAffineToDefault();
            
            FocusController.SetDefaultValue(p_CameraData.CameraTraceOffset);
            RotateController.SetDefaultValue(p_CameraData.ApplyCaemraWrapperDegree(RotationWrapper));
            ZoomController.SetDefaultValue(p_CameraData.CameraDistance);
            
            // 선정된 초기 변환값을 각 래퍼에 적용해준다.
            OnUpdateViewControl();
        }
        
        private void OnUpdateViewControl()
        {
            OnFocusControllerUpdate();
            OnRotationControllerUpdate();
            OnZoomControllerUpdate();
        }

        public void OnFocusControllerUpdate()
        {
            FocusWrapper.localPosition = FocusController._CurrentValue;
        }

        public void OnRotationControllerUpdate()
        {
            RotationWrapper.forward = RotateController._CurrentValue;
        }

        public void OnZoomControllerUpdate()
        {
            ZoomWrapper.localPosition = Vector3.Scale(Vector3.forward, ZoomWrapper.InverseTransformVector(RotationWrapper.forward) * -ZoomController._CurrentValue);
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 현재 아핀 변환을 수행중인 핸들러가 있는지 체크하는 논리 메서드
        /// </summary>
        public bool IsProcessingViewControl()
        {
            return FocusController.ValidFlag 
                   || RotateController.ValidFlag
                   || ZoomController.ValidFlag;
        }

        /// <summary>
        /// ViewControl 이벤트를 취소시키고 아핀 객체의 정보를 초기 값으로 되돌리는 메서드
        /// </summary>
        private void ResetAffineToDefault()
        {
            foreach (var wrapperKV in _ViewControlAffinePresetTable)
            {
                var wrapper = wrapperKV.Value;
                wrapper.ResetToDefault();
            }
        }

        /// <summary>
        /// 모든 핸들러를 초기화 시키는 메서드
        /// </summary>
        public void ResetAllAffineTransform()
        {
            foreach (var wrapperKV in _ViewControlAffinePresetTable)
            {
                var wrapper = wrapperKV.Value;
                wrapper.LerpIterator.RevertValue();
                wrapper.Reset();
            }
        }

        /// <summary>
        /// 모든 카메라 이벤트를 종료시킨다.
        /// </summary>
        public void CancelAllCameraEvent()
        {
            foreach (var wrapperKV in _ViewControlAffinePresetTable)
            {
                var wrapper = wrapperKV.Value;
                wrapper.LerpIterator.Terminate();
            }
        }

        #endregion
    }
}

#endif