using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IBlindEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class BlindEventHandlerBase<This> : DebuffEventHandlerBase<This>, IBlindEventHandler
        where This : BlindEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new BlindDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as BlindDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}