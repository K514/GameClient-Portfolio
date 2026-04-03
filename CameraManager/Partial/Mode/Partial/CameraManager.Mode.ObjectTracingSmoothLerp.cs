using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Consts>

        /// <summary>
        /// ObjectTracingSmoothLerp 모드 추적 반경 기본값
        /// </summary>
        private const float Default_Smooth_Trace_Radius = 1.8f;

        #endregion

        #region <Fields>
        
        /// <summary>
        /// Smooth trace 중, 타겟 오브젝트와 최대로 거리가 벌어지는 반경
        /// </summary>
        private float _TracingSmoothRadius;

        /// <summary>
        /// Smooth Tracing Mode에서 사용할 Base Wrapper와 추적유닛 사이의 offset 벡터
        /// </summary>
        private Vector3 _SmoothTracingCameraOffset;

        #endregion
        
        #region <Callbacks>

        /// <summary>
        /// 지연 추적 모드
        /// </summary>
        public void OnUpdateSmoothTracing(float p_DeltaTime)
        {
            if (_CurrentFocusPreset.IsFocusableValid())
            {
                var targetPosition = _CurrentFocusPreset.FocusObject.GetBottomPosition();
                var deltaTime = p_DeltaTime;

                // 추적 대상의 이동이 감지되는 경우
                if (_CurrentFocusPreset.IsPositionChanged())
                {
                    var remaind = _SmoothTracingCameraOffset.magnitude - _TracingSmoothRadius;
                    if (remaind < 0f)
                    {
                        // 추적 대상이 이동하고 있을 때에는 지연 추적을 위해 _SmoothTracingCameraOffset 증가 값이 _SmoothLerpRadiusIncreaseSpeed/s 로 고정된다.
                        var offset = _CurrentFocusPreset.FocusObject.PositionTrace._CurrentDeltaDirectionUnitVector *
                                     _CurrentCameraConstantDataRecord.SmoothLerpRadiusIncreaseSpeed * deltaTime;
                        var tryOffset = _SmoothTracingCameraOffset - offset;
                        var tryOffsetScale = tryOffset.magnitude;

                        if (tryOffsetScale > _TracingSmoothRadius)
                        {
                            _SmoothTracingCameraOffset = _TracingSmoothRadius * tryOffset.normalized;
                        }
                        else
                        {
                            _SmoothTracingCameraOffset = tryOffset;
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    var decreaseRadiusDelta = _CurrentCameraConstantDataRecord.SmoothLerpRadiusDecreaseSpeed * deltaTime;
                    _SmoothTracingCameraOffset = _SmoothTracingCameraOffset.ZeroLerpSameRate(decreaseRadiusDelta);
                }

                SetRootPosition(targetPosition + _SmoothTracingCameraOffset);
                OnCheckTargetMove();
            }
        }
        
        #endregion
    }
}
#endif