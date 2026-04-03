using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IBlessEventHandler : IBuffEventHandler
    {
    }
    
    public abstract class BlessEventHandlerBase<This> : BuffEventHandlerBase<This>, IBlessEventHandler
        where This : BlessEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new BlessBuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as BlessBuffDataTable.TableRecord;
        }
        
        #endregion
    }
}