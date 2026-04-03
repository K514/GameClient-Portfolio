using UnityEngine;

namespace k514.Mono.Common
{
    public class D3MoveEventHandler : MoveEventHandlerBase<D3MoveEventHandler>
    {
        #region <Callbacks>

        protected override void OnMove(Vector3 p_UV)
        {
            if (Entity.IsFocused)
            {
                var lookVector = CameraManager.GetInstanceUnsafe.GetCameraUV(p_UV);
                Entity.SetLookUV(lookVector);
            }
            else
            {
                var lookVector = p_UV;
                Entity.SetLookUV(lookVector);
            }

            var runSpeedRate = Entity.AnimationModule.GetMoveState() == AnimationTool.MoveMotionType.Run ? ActionTool.__RUN_SPEED_RATE : 1f; 
            var seedVector = runSpeedRate * Entity.GetScaledMovementSpeed() * Entity.Affine.forward;
            Entity.PhysicsModule.OverlapVelocity(PhysicsTool.ForceType.SyncWithController, seedVector);
        }

        #endregion

        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}