using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 개체에 포함된 모듈들에게 특정 이벤트를 전파하는 메서드를 모아놓은 부분클래스
    /// </summary>
    public partial class GameEntityBase<Content, CreateParams, ActivateParams> : IGameEntityModuleEventReceiver
    {
        public void AwakeModule()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.AwakeModule();
            }
        }

        public void SleepModule()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.SleepModule();
            }
        }
        
        public void OnModule_PreUpdate(float p_DeltaTime)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_PreUpdate(p_DeltaTime);
            }
        }

        public void OnModule_Update(float p_DeltaTime)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_Update(p_DeltaTime);
            }
        }

        public void OnModule_Update_TimeBlock()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_Update_TimeBlock();
            }
        }

        public void OnModule_Update_Scale()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_Update_Scale();
            }
        }

        public void OnModule_PositionChanged(Vector3 p_Prev, Vector3 p_Current)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_PositionChanged(p_Prev, p_Current);
            }
        }
        
        public void OnModule_PositionMoved()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_PositionMoved();
            }
        }

        public void OnModule_PivotChanged(PositionTracer p_PositionStatePreset)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_PivotChanged(p_PositionStatePreset);
            }
        }

        public void OnModule_Strike(IGameEntityBridge p_Target, DamageCalculator.HitResult p_HitResult)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_Strike(p_Target, p_HitResult);
            }
        }

        public void OnModule_Hit(IGameEntityBridge p_Trigger, DamageCalculator.HitResult p_HitResult)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_Hit(p_Trigger, p_HitResult);
            }
        }

        public void OnModule_HitMotion_Start()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_HitMotion_Start();
            }
        }

        public void OnModule_HitMotion_Over()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_HitMotion_Over();
            }
        }

        public void OnModule_Dead(bool p_Instant)
        {
            GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.Dead, new GameEntityBaseEventParams(this));

            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_Dead(p_Instant);
            }
        }

        public void OnModule_BeginFloat(PhysicsTool.ForceType p_Mask, Vector3 p_CurrentForce)
        {
            AddState(GameEntityTool.EntityStateType.FLOAT);

            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_BeginFloat(p_Mask, p_CurrentForce);
            }
        }
        
        public void OnModule_ManualJump()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_ManualJump();
            }
        }
        
        public void OnModule_ReachedGround(PhysicsTool.StampPreset p_UnitStampPreset)
        {
            RemoveState(GameEntityTool.EntityStateType.FLOAT);
            
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_ReachedGround(p_UnitStampPreset);
            }
        }

        public virtual void OnModule_StampObject()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnModule_StampObject();
            }
        }

#if !SERVER_DRIVE
        private void OnModule_CameraFocused(CameraTool.CameraMode p_ModeType)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnCameraFocused(p_ModeType);
            }
        }

        private void OnModule_CameraModeChanged(CameraTool.CameraMode p_PrevCameraMode, CameraTool.CameraMode p_CurrentCameraMode)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnCameraModeChanged(p_PrevCameraMode, p_CurrentCameraMode);
            }
        }

        private void OnModule_CameraFocusTerminated()
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnCameraFocusTerminated();
            }
        }
#endif

        private void OnModule_NavigateStart(GeometryTool.NavigationResultPreset p_Preset)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnNavigateStart(p_Preset);
            }
        }
        
        private void OnModule_NavigateProgress(GeometryTool.NavigationResultPreset p_Preset)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnNavigateProgress(p_Preset);
            }
        }
        
        private void OnModule_NavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnNavigateEnd(p_Preset);
            }
        }
        
        public void OnModule_ActionActivateSuccess(IActionEventHandler p_Handler)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnActionActivateSuccess(p_Handler);
            }
        }
        
        public void OnModule_ActionActivateFail(IActionEventHandler p_Handler)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnActionActivateFail(p_Handler);
            }
        }

        public void OnModule_ActionTerminated(IActionEventHandler p_Handler)
        {
            var enumerator = GameEntityModuleTool._ModuleTypeEnumerator;
            foreach (var moduleType in enumerator)
            {
                _moduleClusterTable[moduleType].CurrentModule?.OnActionTerminated(p_Handler);
            }  
        }
    }
}