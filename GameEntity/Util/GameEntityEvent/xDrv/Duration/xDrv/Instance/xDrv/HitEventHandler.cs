namespace k514.Mono.Common
{
    public class HitEventHandler : InstanceEventHandlerBase<HitEventHandler>
    {
        #region <Callbacks>

        protected override void OnEventStart()
        {
            Entity.AddState(GameEntityTool.EntityStateType.STUCK);
        }

        protected override void OnEventProgress(float p_DeltaTime, float p_Rate)
        {
        }

        protected override void OnEventTerminate()
        {
            Entity.RemoveState(GameEntityTool.EntityStateType.STUCK);
        }

        #endregion
        
        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}