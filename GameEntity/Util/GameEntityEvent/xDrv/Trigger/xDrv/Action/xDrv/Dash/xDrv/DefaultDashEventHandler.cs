namespace k514.Mono.Common
{
    public class DefaultDashEventHandler : DashEventHandlerBase<DefaultDashEventHandler>
    {
        #region <Callbacks>

        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AnimationModule.SwitchMotion(AnimationTool.MotionType.Dash, AnimationTool.MotionTransitionType.Bypass_StateMachine);
            
            var physicsModule = Entity.PhysicsModule;
            physicsModule.ClearForce();
            physicsModule.AddVelocity(PhysicsTool.ForceType.SyncWithController, Entity.GetLookUV() * Entity[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.JumpForce]);
        }

        public override void OnMindControl()
        {
            if (Entity.TryGetCurrentEnemy(out var o_Enemy))
            {
                Entity.SetLookBack(o_Enemy);
            }
        }
        
        protected override void OnClipCue(AnimationTool.ClipEventType p_Type)
        {
            base.OnClipCue(p_Type);
            
            switch (p_Type)
            {
                case AnimationTool.ClipEventType.EndCue:
                {
                    ActionModule.TryReleaseMainHandler(this);
                    break;
                }
            }
        }

        #endregion

        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}