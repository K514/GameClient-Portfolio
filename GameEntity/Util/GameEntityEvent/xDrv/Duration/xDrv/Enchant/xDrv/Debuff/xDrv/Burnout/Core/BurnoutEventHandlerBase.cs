using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IBurnoutEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class BurnoutEventHandlerBase<This> : DebuffEventHandlerBase<This>, IBurnoutEventHandler
        where This : BurnoutEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new BurnoutDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as BurnoutDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}