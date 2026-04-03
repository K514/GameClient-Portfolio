using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IConfuseEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class ConfuseEventHandlerBase<This> : DebuffEventHandlerBase<This>, IConfuseEventHandler
        where This : ConfuseEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new ConfuseDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as ConfuseDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}