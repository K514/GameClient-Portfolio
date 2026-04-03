using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IScarEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class ScarEventHandlerBase<This> : DebuffEventHandlerBase<This>, IScarEventHandler
        where This : ScarEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new ScarDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as ScarDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}