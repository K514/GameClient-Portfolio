using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IPoisonEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class PoisonEventHandlerBase<This> : DebuffEventHandlerBase<This>, IPoisonEventHandler
        where This : PoisonEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new PoisonDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as PoisonDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}