using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface ISuperArmorEventHandler : IBuffEventHandler
    {
    }
    
    public abstract class SuperArmorEventHandlerBase<This> : BuffEventHandlerBase<This>, ISuperArmorEventHandler
        where This : SuperArmorEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new SuperArmorBuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as SuperArmorBuffDataTable.TableRecord;
        }
        
        #endregion
    }
}