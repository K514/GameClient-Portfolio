using UnityEngine;

namespace k514.Mono.Common
{
    public class D2MoveEventHandler : MoveEventHandlerBase<D2MoveEventHandler>
    {
        #region <Callbacks>

        protected override void OnMove(Vector3 p_UV)
        {
            var x = p_UV.x;
            switch (this)
            {
                case var _ when x > 0f:
                    Entity.SetLookUV(Vector3.right);
                    break;
                case var _ when x < 0f:
                    Entity.SetLookUV(Vector3.left);
                    break;
            }

            var runSpeedRate = Entity.AnimationModule.GetMoveState() == AnimationTool.MoveMotionType.Run ? ActionTool.__RUN_SPEED_RATE : 1f; 
            var seedVector = runSpeedRate * Entity.GetScaledMovementSpeed() * p_UV;
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