using UnityEngine;

namespace k514.Mono.Common
{
    public class DefaultJumpEventHandler : JumpEventHandlerBase<DefaultJumpEventHandler>
    {
        #region <Callbacks>

        protected override void OnJump()
        {
            var physicsModule = Entity.PhysicsModule;
            physicsModule.ClearForceExcept(PhysicsTool.ForceType.SyncWithController);
            physicsModule.AddVelocity(PhysicsTool.ForceType.Jump, Entity[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.JumpForce] * Vector3.up);
        }

        #endregion

        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}