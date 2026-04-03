using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IPossessEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class PossessEventHandlerBase<This> : DebuffEventHandlerBase<This>, IPossessEventHandler
        where This : PossessEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new PossessDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as PossessDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}