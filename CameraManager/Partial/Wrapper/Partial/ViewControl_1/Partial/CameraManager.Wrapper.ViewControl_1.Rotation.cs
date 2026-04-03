#if !SERVER_DRIVE

using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Method/Degree>

        /// <summary>
        /// 카메라의 Look벡터를 지정한 값으로 세트하는 메서드
        /// </summary>
        public void SetDegree(CameraTool.CameraWrapperType p_Type, Vector3 p_Degree)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].RotateController;
                targetHandler.SetValue(p_Degree);
            }
        }
        
        /// <summary>
        /// 카메라의 앙각을 지정한 값으로 세트하는 메서드
        /// </summary>
        public void SetTiltDegree(CameraTool.CameraWrapperType p_Type, float p_TiltDegree)
        {
            SetDegree(p_Type, p_TiltDegree * Vector3.right);
        }

        /// <summary>
        /// 카메라의 시야각을 지정한 값으로 세트하는 메서드
        /// </summary>
        public void SetSightDegree(CameraTool.CameraWrapperType p_Type, float p_SightDegree)
        {
            SetDegree(p_Type, p_SightDegree * Vector3.up);
        }
                
        /// <summary>
        /// 카메라의 Look벡터에 지정한 값만큼 세트하는 메서드
        /// </summary>
        public void AddDegree(CameraTool.CameraWrapperType p_Type, Vector3 p_Degree, float p_DeltaTime)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].RotateController;
                targetHandler.AddValue(p_Degree, p_DeltaTime);
            }
        }
        
        /// <summary>
        /// 카메라의 앙각을 지정한 값만큼 더하는 메서드
        /// </summary>
        public void AddTiltDegree(CameraTool.CameraWrapperType p_Type, float p_TiltDegree, float p_DeltaTime)
        {
            AddDegree(p_Type, p_TiltDegree * Vector3.right, p_DeltaTime);
        }

        /// <summary>
        /// 카메라의 시야각을 지정한 값만큼 더하는 메서드
        /// </summary>
        public void AddSightDegree(CameraTool.CameraWrapperType p_Type, float p_SightDegree, float p_DeltaTime)
        {
            AddDegree(p_Type, p_SightDegree * Vector3.up, p_DeltaTime);
        }

        /// <summary>
        /// 카메라의 회전도를 원본 값으로 초기화 시키는 메서드
        /// </summary>
        public void ResetDegree(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType())
            {
                _CameraAffineWrapperControllerSet[p_Type].RotateController.ResetValue();
            }
        }

        #endregion

        #region <Method/Degree/Lerp>

        /// <summary>
        /// 카메라의 Look벡터를 지정한 값으로 러프하는 메서드
        /// </summary>
        public void SetDegreeLerp(CameraTool.CameraWrapperType p_Type, Vector3 p_Degree, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].RotateController;
                targetHandler.SetValueLerp(p_Degree, p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
        
        /// <summary>
        /// 카메라의 앙각을 지정한 값으로 러프하는 메서드
        /// </summary>
        public void SetTiltDegreeLerp(CameraTool.CameraWrapperType p_Type, float p_TiltDegree, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            SetDegreeLerp(p_Type, p_TiltDegree * Vector3.right, p_PreDelayMsec, p_LerpDurationMsec);
        }

        /// <summary>
        /// 카메라의 시야각을 지정한 값으로 러프하는 메서드
        /// </summary>
        public void SetSightDegreeLerp(CameraTool.CameraWrapperType p_Type, float p_SightDegree, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            SetDegreeLerp(p_Type, p_SightDegree * Vector3.up, p_PreDelayMsec, p_LerpDurationMsec);
        }

        /// <summary>
        /// 카메라의 회전도를 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddDegreeLerp(CameraTool.CameraWrapperType p_Type, Vector3 p_Degree, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                var targetHandler = _CameraAffineWrapperControllerSet[p_Type].RotateController;
                targetHandler.AddValueLerp(p_Degree, p_PreDelayMsec, p_LerpDurationMsec);
            }
        }
        
        /// <summary>
        /// 카메라의 앙각을 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddTiltDegreeLerp(CameraTool.CameraWrapperType p_Type, float p_TiltDegree, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            AddDegreeLerp(p_Type, p_TiltDegree * Vector3.right, p_PreDelayMsec, p_LerpDurationMsec);
        }

        /// <summary>
        /// 카메라의 시야각을 지정한 값 만큼 더하는 메서드
        /// </summary>
        public void AddSightDegreeLerp(CameraTool.CameraWrapperType p_Type, float p_SightDegree, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            AddDegreeLerp(p_Type, p_SightDegree * Vector3.up, p_PreDelayMsec, p_LerpDurationMsec);
        }

        /// <summary>
        /// 카메라의 회전을 원본 값으로 초기화 시키는 메서드
        /// </summary>
        public void ResetDegreeLerp(CameraTool.CameraWrapperType p_Type, uint p_PreDelayMsec, uint p_LerpDurationMsec)
        {
            if (p_Type.IsControllableWrapperType())
            {
                _CameraAffineWrapperControllerSet[p_Type].RotateController.ResetValueLerp(p_PreDelayMsec, p_LerpDurationMsec);
            }
        }

        /// <summary>
        /// 카메라의 회전 변환 러프 이벤트를 캔슬시키는 메서드
        /// </summary>
        public void CancelDegreeLerp(CameraTool.CameraWrapperType p_Type)
        {
            if (p_Type.IsControllableWrapperType() && !IsCameraDegreeLerpValid(p_Type))
            {
                _CameraAffineWrapperControllerSet[p_Type].RotateController.Terminate();
            }
        }
        
        #endregion
    }
}

#endif