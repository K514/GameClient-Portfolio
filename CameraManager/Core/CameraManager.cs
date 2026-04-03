#if !SERVER_DRIVE

using Object = UnityEngine.Object;
using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라 수명 제어 및 카메라 연출 이벤트를 제어하는 싱글톤 클래스
    /// </summary>
    public partial class CameraManager : SceneChangeEventReceiveAsyncSingleton<CameraManager>
    {
        #region <Fields>

        private CameraConstantDataTable.TableRecord _CurrentCameraConstantDataRecord;
        private CameraVariableDataTable.TableRecord _CurrentCameraVariableDataRecord;
        private int _SkyBoxIndex;

        #endregion
        
        #region <Callbacks>
        
        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();
            
            Priority = 100;
            _Dependencies.Add(typeof(CameraConstantDataTable));
            _Dependencies.Add(typeof(CameraVariableDataTable));
            _Dependencies.Add(typeof(SceneEnvironmentManager));
        }

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            OnCreateState();
            OnCreateWrapper();
            OnCreateCamera();
            OnCreateMode();
            OnCreateGeometry();
            OnCreateRender();
            OnCreateEvent();

            // 카메라 오브젝트를 불변으로 만들어준다.
            RootWrapper.DontDestroyOnLoadSafe();
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public void OnLateUpdate(float p_DeltaTime)
        {    
            switch (_CurrentCameraMode)
            {
                default:
                case CameraTool.CameraMode.None:
                    break;
                case CameraTool.CameraMode.ObjectTracing:
                    OnUpdateTracing(p_DeltaTime);
                    break;
                case CameraTool.CameraMode.ObjectTracingSmoothLerp:
                    OnUpdateSmoothTracing(p_DeltaTime);
                    break;
                case CameraTool.CameraMode.FirstPersonTracing:
                    OnUpdateFirstPersonTracing(p_DeltaTime);
                    break;
            }

            switch (_RotationDirectionType)
            {
                default:
                case ArrowType.None:
                    break;
                case ArrowType.Up:
                case ArrowType.Left:
                case ArrowType.Down:
                case ArrowType.Right:
                case ArrowType.UpLeft:
                case ArrowType.LeftDown:
                case ArrowType.DownRight:
                case ArrowType.RightUp:
                    OnUpdateViewControl(p_DeltaTime);
                    break;
            }
            
            OnUpdateShake(p_DeltaTime);
            OnUpdateViewControl(p_DeltaTime);
        }
        
        protected override void OnDisposeSingleton()
        {
            OnDisposeEvent();
#if APPLY_PPS
            OnDisposePPS();
#endif
            if (RootWrapper != null)
            {
                Object.Destroy(RootWrapper.gameObject);
            }
            
            base.OnDisposeSingleton();
        }
  
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            SetCameraValid(true);

            OpenMainCamera();
            SetAudioListenerEnable(true);
            ResetCullingMask();
            ResetViewControl();

            // 카메라 모드가 없는 상태에서 씬이 시작되는 경우
            if (_CurrentCameraMode == CameraTool.CameraMode.None)
            {
                if (SceneEnvironmentManager.GetInstanceUnsafe.TryGetSceneEnvironment(out var o_SceneEnv))
                {
                    var pos = o_SceneEnv.GetSceneStartPreset().StartPosition.GetValue();
                    SetRootPosition(pos);
                }
            }
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            SetCameraValid(false);
            
            OnResetMode();
            OnResetState();
            OnResetShakeWrapperPartial();
            CancelAllLerp();
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            // 월드 카메라 초기화
            SetCameraBlind();
            SetSolidColorBlack();
            SetRootPositionZero();
            SetAudioListenerEnable(false);
            
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        public void SetCameraDefaultConfig()
        {
            SetCameraConfig(CameraConstantDataTable.GetInstanceUnsafe[0], CameraVariableDataTable.GetInstanceUnsafe[0], 0);
        }
        
        public void SetCameraConfig(CameraConstantDataTable.TableRecord p_CameraConstantDataRecord, CameraVariableDataTable.TableRecord p_CameraVariableDataRecord, int p_SkyBoxIndex)
        {
            _CurrentCameraConstantDataRecord = p_CameraConstantDataRecord;
            _CurrentCameraVariableDataRecord = p_CameraVariableDataRecord;

            UpdateCameraConfig();
        }
        
        private void UpdateCameraConfig()
        {
            OnUpdateTraceTargetConfig();
            OnUpdateCameraModeConfig();
            OnUpdateMainCameraConfig();
            OnUpdateCameraBackground();
            OnUpdateCameraRenderConfig();
            OnUpdateViewControlConfig();
        }

        #endregion
    }
}
#endif