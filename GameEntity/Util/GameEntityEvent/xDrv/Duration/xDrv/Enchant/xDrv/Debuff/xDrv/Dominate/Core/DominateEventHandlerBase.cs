using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IDominateEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class DominateEventHandlerBase<This> : DebuffEventHandlerBase<This>, IDominateEventHandler
        where This : DominateEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new DominateDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as DominateDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}