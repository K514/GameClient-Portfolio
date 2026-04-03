#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라를 흔드는 연출을 담당하는 부분 클래스
    /// 해당 이벤트는 벡터나 부동소수점의 러프 함수를 사용하지 않기에 다른 부분 클래스 처럼 외부 클래스를 통해
    /// 이벤트를 수행하지 않는다.
    /// </summary>
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 카메라 흔들림 연출을 수행하는 Affine 오브젝트
        /// </summary>
        public Transform ShakeWrapper { get; private set; }
        
        /// <summary>
        /// 카메라 흔드는 연출 프리셋
        /// </summary>
        private CameraShakePreset _ShakePreset;
        
        /// <summary>
        /// 카메라 흔들림 선딜 타이머
        /// </summary>
        private ProgressTimer _ShakePreDelayTimer;
        
        /// <summary>
        /// 카메라 흔들림 메인 타이머
        /// </summary>
        private ProgressTimer _ShakeMainTimer;
        
        #endregion

        #region <Callbacks>

        private void OnCreateShakeWrapper()
        {
            ShakeWrapper = _CameraAffineSet[CameraTool.CameraWrapperType.ViewControl_Shake];
        }

        private void OnResetShakeWrapperPartial()
        {
            _ShakePreDelayTimer = default;
            _ShakeMainTimer = default;
        }

        private void OnUpdateShake(float p_DeltaTime)
        {
            if (_ShakePreset.ValidFlag)
            {
                if (_ShakePreDelayTimer.IsProgressing())
                {
                    _ShakePreDelayTimer.Progress(p_DeltaTime);
                    return;
                }
            
                if (_ShakeMainTimer.IsProgressing())
                {
                    _ShakeMainTimer.Progress(p_DeltaTime);
  
                    var progressRate = _ShakeMainTimer.ProgressRate;
                    var pingpongHalfBound = _ShakePreset.PingPongBoundHalves;
                    var pingpongRate = Mathf.PingPong(progressRate + pingpongHalfBound, _ShakePreset.PingPongBound) - pingpongHalfBound;
                    var currentDistance = _ShakePreset.Distance * pingpongRate;
                    ShakeWrapper.localPosition = currentDistance * _ShakePreset.Direction;
                }
                else
                {
                    ShakeWrapper.localPosition = Vector3.zero;
                    _ShakePreDelayTimer = default;
                    _ShakeMainTimer = default;
                    _ShakePreset.ValidFlag = false;
                }
            }
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 속도가 일정한 카메라를 흔드는 연출을 수행하는 메서드
        /// </summary>
        public void SetShake(Vector3 p_Direction, float p_MaxDistance, float p_PreDelay, float p_Duration, int p_CycleCount)
        {
            _ShakePreDelayTimer = p_PreDelay;
            _ShakeMainTimer = p_Duration;
            
            var invCycleCount = 0.25f / p_CycleCount;
            _ShakePreset = 
                new CameraShakePreset
                (
                    CustomMath.GetRandomSign() * p_Direction,
                    p_MaxDistance,
                    invCycleCount,
                    0.5f * invCycleCount
                );
        }
        
        #endregion
    }
}

#endif