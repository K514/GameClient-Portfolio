using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IChillEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class ChillEventHandlerBase<This> : DebuffEventHandlerBase<This>, IChillEventHandler
        where This : ChillEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new ChillDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as ChillDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}