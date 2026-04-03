#if APPLY_URP

using UnityEngine.Rendering.Universal;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Fields>

        private UniversalAdditionalCameraData _URP_CameraData;

        #endregion

        #region <Callbacks>

        private void OnCreateURP()
        {
            _URP_CameraData = MainCameraTransform.GetComponent<UniversalAdditionalCameraData>();
            _URP_CameraData.volumeLayerMask = (int) (GameConst.GameLayerMaskType.Default | GameConst.GameLayerMaskType.PostProcessVolume);
        }

        #endregion
    }
}

#endif