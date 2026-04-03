using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        /// <summary>
        /// 메인 카메라 레퍼런스
        /// </summary>
        public Camera MainCamera { get; private set; }

        /// <summary>
        /// 메인 카메라 아핀 객체
        /// </summary>
        public Transform MainCameraTransform { get; private set; }

        /// <summary>
        /// 메인 카메라 오디오 리스너
        /// </summary>
        public AudioListener AudioListener { get; private set; }

        #endregion
        
        #region <Callbacks>
        
        private void OnCreateCamera()
        {
            // 메인 카메라를 카메라 타입 래퍼들의 최하위 자손으로 넣어준다.
            var terminalWrapperKV = _CameraAffineSet.Last();
            var terminateAffine = default(Transform);
            var wrapperType = terminalWrapperKV.Key;
            if (wrapperType.IsControllableWrapperType())
            {
                terminateAffine = _CameraAffineWrapperControllerSet[wrapperType].Rear;
            }
            else
            {
                terminateAffine = terminalWrapperKV.Value;
            }

            // 메인 카메라를 초기화 시켜준다.
            MainCamera = terminateAffine.GetBaseCamera(CameraClearFlags.SolidColor, Color.black);
            MainCamera.name = "MainManagerCamera";
            MainCameraTransform = MainCamera.transform;

            // 메인 카메라에 오디오 리스너를 추가해준다.
            AudioListener = MainCamera.gameObject.AddComponent<AudioListener>();
            
            // 메인 카메라에 트리거 컬라이더를 추가해준다.
            var sphereCollider = MainCamera.gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = MainCamera.nearClipPlane;
            
            // 메인 카메라의 레이어를 지정해준다.
            MainCamera.gameObject.TurnLayerTo(GameConst.GameLayerType.Camera, false);
            
            // 메인 카메라의 태그를 지정해준다.
            MainCamera.gameObject.TurnTagTo(GameConst.GameTagType.MainCamera);

            // 생성된 이후에는 카메라를 꺼준다.
            YieldCameraTo(null);
        }

        public void OnUpdateMainCameraConfig()
        {
            SetOrthographic(_CurrentCameraVariableDataRecord.OrthographicSize);
        }
        
        #endregion
        
        #region <Methods>

        public void SetAudioListenerEnable(bool p_Flag)
        {
            AudioListener.enabled = p_Flag;
        }

        public void OpenMainCamera()
        {
            MainCameraTransform.gameObject.SetActive(true);
        }
        
        public void CloseMainCamera()
        {
            MainCameraTransform.gameObject.SetActive(false);
        }
        
        public void YieldCameraTo(Camera p_Camera)
        {
            CloseMainCamera();
            
            if (!ReferenceEquals(null, p_Camera))
            {
                p_Camera.enabled = true;
            }
        }

        public void SetOrthographic(float p_Size)
        {
            if (p_Size > 0f)
            {
                MainCamera.orthographic = true;
                MainCamera.orthographicSize = p_Size;
            }
            else
            {
                MainCamera.orthographic = false;
                MainCamera.orthographicSize = 0f;
            }
        }

        #endregion
    }
}