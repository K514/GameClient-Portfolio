using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityModuleBase
    {
        #region <Callbacks>

        public virtual void OnModule_PreUpdate(float p_DeltaTime)
        {
        }

        public virtual void OnModule_Update(float p_DeltaTime)
        {
        }

        public virtual void OnModule_Update_TimeBlock()
        {
        }

        public virtual void OnModule_Update_Scale()
        {
        }

        public virtual void OnModule_PositionChanged(Vector3 p_Prev, Vector3 p_Current)
        {
        }

        public virtual void OnModule_PositionMoved()
        {
        }

        public virtual void OnModule_PivotChanged(PositionTracer p_PositionStatePreset)
        {
        }

        public virtual void OnModule_Strike(IGameEntityBridge p_Target, DamageCalculator.HitResult p_HitResult)
        {
        }

        public virtual void OnModule_Hit(IGameEntityBridge p_Trigger, DamageCalculator.HitResult p_HitResult)
        {
        }

        public virtual void OnModule_HitMotion_Start()
        {
        }

        public virtual void OnModule_HitMotion_Over()
        {
        }
        
        public virtual void OnModule_Dead(bool p_Instant)
        {
        }

        public virtual void OnModule_BeginFloat(PhysicsTool.ForceType p_Mask, Vector3 p_CurrentForce)
        {
        }

        public virtual void OnModule_ManualJump()
        {
        }
        
        public virtual void OnModule_ReachedGround(PhysicsTool.StampPreset p_UnitStampPreset)
        {
        }

        public virtual void OnModule_StampObject()
        {
        }

        public virtual void OnCameraFocused(CameraTool.CameraMode p_ModeType)
        {
        }

        public virtual void OnCameraModeChanged(CameraTool.CameraMode p_PrevCameraMode, CameraTool.CameraMode p_CurrentCameraMode)
        {
        }

        public virtual void OnCameraFocusTerminated()
        {
        }
        
        public virtual void OnNavigateStart(GeometryTool.NavigationResultPreset p_Preset)
        {
        }

        public virtual void OnNavigateProgress(GeometryTool.NavigationResultPreset p_Preset)
        {
        }
        
        public virtual void OnNavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
        }
        
        public virtual void OnActionActivateSuccess(IActionEventHandler p_Handler)
        {
        }
        
        public virtual void OnActionActivateFail(IActionEventHandler p_Handler)
        {
        }

        public virtual void OnActionTerminated(IActionEventHandler p_Handler)
        {
        }
        
        #endregion
    }
}