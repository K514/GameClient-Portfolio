using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IBerserkEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class BerserkEventHandlerBase<This> : DebuffEventHandlerBase<This>, IBerserkEventHandler
        where This : BerserkEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new BerserkDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as BerserkDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}