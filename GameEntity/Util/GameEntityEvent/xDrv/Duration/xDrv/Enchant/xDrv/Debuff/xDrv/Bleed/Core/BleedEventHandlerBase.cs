using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IBleedEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class BleedEventHandlerBase<This> : DebuffEventHandlerBase<This>, IBleedEventHandler
        where This : BleedEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new BleedDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as BleedDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}