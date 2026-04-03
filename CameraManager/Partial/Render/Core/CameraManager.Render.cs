#if !SERVER_DRIVE

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CameraManager
    {
        #region <Consts>
        
        /// <summary>
        /// 기본 유닛 랜더 모드
        /// </summary>
        private const CameraTool.CameraRenderProcessType DefaultCameraRenderProcessType = CameraTool.CameraRenderProcessType.Musashi;

        #endregion

        #region <Fields>
        
        /// <summary>
        /// 유닛 랜더 모드
        /// </summary>
        public CameraTool.CameraRenderProcessType _CameraRenderProcessType { get; private set; }

        #endregion

        #region <Callbacks>

        private void OnCreateRender()
        {
            SetCameraRenderProcessType(DefaultCameraRenderProcessType);

            OnCreateCulling();
#if APPLY_URP
            OnCreateURP();
#endif
#if APPLY_PPS
            OnCreatePPS();
#endif
        }

        private void OnUpdateCameraRenderConfig()
        {
            OnUpdateCameraCullingConfig();
        }

        #endregion

        #region <Methods>
                
        /// <summary>
        /// 랜더 모드를 지정하는 메서드
        /// </summary>
        public void SetCameraRenderProcessType(CameraTool.CameraRenderProcessType p_CameraRenderProcessType)
        {
            _CameraRenderProcessType = p_CameraRenderProcessType;
        }

        public CameraCullingPreset GetCameraCullingState(ICameraFocusable p_TryUnit)
        {
            switch (_CameraRenderProcessType)
            {
                case CameraTool.CameraRenderProcessType.Musashi:
                {
                    var (cullingType, sqrDistance) = GetCullingType(p_TryUnit);
                    switch (cullingType)
                    {
                        default:
                        case CameraTool.CameraCullingState.None:
                        case CameraTool.CameraCullingState.NearCulling:
                        case CameraTool.CameraCullingState.FarCulling:
                            return new CameraCullingPreset(GameEntityTool.GameEntityRenderType.None, cullingType, sqrDistance);
                        case CameraTool.CameraCullingState.Blocked:
                        case CameraTool.CameraCullingState.OutOfScreen:
                            return new CameraCullingPreset(GameEntityTool.GameEntityRenderType.Model | GameEntityTool.GameEntityRenderType.AttachObject, cullingType, sqrDistance);
                        case CameraTool.CameraCullingState.Visible:
                            return new CameraCullingPreset(GameEntityTool.GameEntityRenderType.Model | GameEntityTool.GameEntityRenderType.AttachObject | GameEntityTool.GameEntityRenderType.UI, cullingType, sqrDistance);
                    }
                }
                default:
                case CameraTool.CameraRenderProcessType.Kojiro:
                case CameraTool.CameraRenderProcessType.Nyaasu:
                case CameraTool.CameraRenderProcessType.Sonansu:
                    return default;
            }
        }
        
        #endregion
    }
}

#endif