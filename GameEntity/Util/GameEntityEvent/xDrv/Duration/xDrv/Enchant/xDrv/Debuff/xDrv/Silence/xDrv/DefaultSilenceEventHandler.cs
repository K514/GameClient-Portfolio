namespace k514.Mono.Common
{
    public class DefaultSilenceEventHandler : SilenceEventHandlerBase<DefaultSilenceEventHandler>
    {
        #region <Callbacks>

        protected override void OnEventStart()
        {
        }

        protected override void OnEventProgress(float p_DeltaTime, float p_Rate)
        {
        }

        protected override void OnEventTerminate()
        {
        }
        
        #endregion

        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}