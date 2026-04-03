#if !SERVER_DRIVE && APPLY_PPS

using UnityEngine;

#if APPLY_URP
    using UnityEngine.Rendering.Universal;
#else
    using UnityEngine.Rendering.PostProcessing;
#endif

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Consts>

#if APPLY_URP
#else
        private const string __Default_PPS_Resources_Name = "PostProcessResources.asset";
#endif
   
        #endregion
        
        #region <Fields>

#if APPLY_URP
#else
        private PostProcessLayer _PPS_Layer;
        private bool _Is_PPS_Layer_Valid => !ReferenceEquals(null, _PPS_Layer);
        
        /// <summary>
        /// PostProcessResources 에셋은 PostProcessLayer 컴포넌트가 동작하려면 로드되어야 하는 에셋인데
        /// 스크립트에서 AddComponent로 PostProcessLayer 컴포넌트를 동적생성하면 로드되지 않고
        /// 인스펙터로 동적생성하거나 아니면 정적생성되어 있어야 게임 실행시에 로드되는 모양이라
        /// 
        /// 스크립트에서 PostProcessLayer를 추가해주기 전에 먼저 로드해준다.
        ///
        /// 해당 에셋은 PPS 패키지 폴더 내부에 존재하는데 Dependency 리소스 폴더로 복붙해서
        /// 로드하는 방식으로 문제를 처리했다.
        /// </summary>
        private AssetLoadResult<Object> _AssetLoadResult;
#endif

        #endregion

        #region <Callbacks>

        private void OnCreatePPS()
        {
#if APPLY_URP
            _URP_CameraData.renderPostProcessing = true;
#else
            // 비동기로 로드하면 예외가 발생하다 없어진다.
            _AssetLoadResult = AssetLoaderManager.GetInstanceUnsafe.LoadAsset<Object>((ResourceLifeCycleType.WholeGame, __Default_PPS_Resources_Name));

            var ppsResource = _AssetLoadResult.Asset as PostProcessResources;
            if (!ReferenceEquals(null, ppsResource))
            {
                _PPS_Layer = MainCamera.gameObject.AddComponent<PostProcessLayer>();
                _PPS_Layer.Init(ppsResource);
            }
            
    #if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintCameraLog.HasOpen(ReferenceEquals(null, ppsResource)))
            {
                CustomDebug.LogError((this, $"PostProcessLayer 를 초기화 시키기 위한 리소스가 존재하지 않습니다."));
            }
    #endif
            
            AddPPSLayer(GameConst.GameLayerMaskType.PostProcessVolume);
            SetPPSTrigger(RootWrapper);

#endif
        }

        private void OnDisposePPS()
        {
#if APPLY_URP
#else
            AssetLoaderManager.GetInstanceUnsafe?.UnloadAsset(ref _AssetLoadResult);
#endif
        }

        #endregion

        #region <Methods>

#if APPLY_URP
#else
        /// <summary>
        /// 볼륨 블랜딩을 수행할 아핀 객체를 지정한다.
        /// </summary>
        private void SetPPSTrigger(Transform p_Transform)
        {
            if (_Is_PPS_Layer_Valid)
            {
                _PPS_Layer.volumeTrigger = p_Transform;
            }
        }

        /// <summary>
        /// PPS 볼륨 레이어에 플래그를 추가하는 메서드
        /// </summary>
        private void AddPPSLayer(GameConst.GameLayerMaskType p_LayerType)
        {
            if (_Is_PPS_Layer_Valid)
            {
                _PPS_Layer.volumeLayer |= (int)p_LayerType;
            }
        }
#endif

        #endregion
    }
}

#endif
