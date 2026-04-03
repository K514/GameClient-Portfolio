using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IDebuffEventHandler : IEnchantEventHandler
    {
    }
    
    public abstract class DebuffEventHandlerBase<This> : EnchantEventHandlerBase<This>, IDebuffEventHandler
        where This : DebuffEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new IDebuffDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
            
            Record = base.Record as IDebuffDataTableRecordBridge;
        }

        #endregion
    }
}