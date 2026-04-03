using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IHealEventHandler : IBuffEventHandler
    {
    }
    
    public abstract class HealEventHandlerBase<This> : BuffEventHandlerBase<This>, IHealEventHandler
        where This : HealEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new HealBuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as HealBuffDataTable.TableRecord;
        }
        
        #endregion
    }
}