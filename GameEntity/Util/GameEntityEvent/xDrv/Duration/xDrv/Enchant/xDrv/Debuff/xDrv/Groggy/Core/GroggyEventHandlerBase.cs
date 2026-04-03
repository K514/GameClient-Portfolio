using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IGroggyEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class GroggyEventHandlerBase<This> : DebuffEventHandlerBase<This>, IGroggyEventHandler
        where This : GroggyEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new GroggyDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as GroggyDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}