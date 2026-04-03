using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IInfestEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class InfestEventHandlerBase<This> : DebuffEventHandlerBase<This>, IInfestEventHandler
        where This : InfestEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new InfestDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as InfestDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}