using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IFreezeEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class FreezeEventHandlerBase<This> : DebuffEventHandlerBase<This>, IFreezeEventHandler
        where This : FreezeEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new FreezeDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as FreezeDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}