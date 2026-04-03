using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IStunEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class StunEventHandlerBase<This> : DebuffEventHandlerBase<This>, IStunEventHandler
        where This : StunEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new StunDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as StunDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}