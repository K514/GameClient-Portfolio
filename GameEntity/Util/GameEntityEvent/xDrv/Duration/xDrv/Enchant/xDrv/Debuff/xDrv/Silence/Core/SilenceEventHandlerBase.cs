using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface ISilenceEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class SilenceEventHandlerBase<This> : DebuffEventHandlerBase<This>, ISilenceEventHandler
        where This : SilenceEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new SilenceDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as SilenceDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}