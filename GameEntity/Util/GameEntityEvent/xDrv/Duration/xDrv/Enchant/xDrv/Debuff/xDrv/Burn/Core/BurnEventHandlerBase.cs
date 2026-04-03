using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IBurnEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class BurnEventHandlerBase<This> : DebuffEventHandlerBase<This>, IBurnEventHandler
        where This : BurnEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new BurnDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as BurnDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}