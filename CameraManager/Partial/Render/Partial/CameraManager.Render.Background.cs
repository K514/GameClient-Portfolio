using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Callbacks>

        private void OnUpdateCameraBackground()
        {
            var preset = SkyBoxNameTable.GetInstanceUnsafe.GetResource(_SkyBoxIndex, ResourceLifeCycleType.SceneUnload);
            if (preset)
            {
                MainCamera.clearFlags = CameraClearFlags.Skybox;
                RenderSettings.skybox = preset.Asset;
            }
            else
            {
                SetSolidColorBlack();
            }
        }

        #endregion

        #region <Methods>

        private void SetSolidColorBlack()
        {
            MainCamera.clearFlags = CameraClearFlags.SolidColor;
            MainCamera.backgroundColor = Color.black; 
        }

        #endregion
    }
}