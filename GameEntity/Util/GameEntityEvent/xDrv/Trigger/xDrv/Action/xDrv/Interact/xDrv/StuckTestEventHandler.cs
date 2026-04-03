namespace k514.Mono.Common
{
    public class StuckTestEventHandler : InteractEventHandlerBase<StuckTestEventHandler>
    {
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            ActionEventType = ActionEventTool.ActionEventType.None;
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();

            Entity.AddState(GameEntityTool.EntityStateType.STUCK);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            Entity.RemoveState(GameEntityTool.EntityStateType.STUCK);
        }
        
        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryInterruptMainHandler(this);
        }

        protected override  void OnHolding(CommandEventParams p_CommandPreset)
        {
        }

        protected override  void OnRelease(CommandEventParams p_CommandPreset)
        {
            ActionModule.TryReleaseMainHandler(this);
        }

        #endregion

        #region <Methods>

        protected override bool IsManualReleasable()
        {
            return false;
        }

        public override void PreloadEvent()
        {
        }
        
        #endregion
    }
}