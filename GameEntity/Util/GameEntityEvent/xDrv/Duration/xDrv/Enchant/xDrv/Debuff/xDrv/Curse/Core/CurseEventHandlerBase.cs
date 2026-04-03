using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface ICurseEventHandler : IDebuffEventHandler
    {
    }
    
    public abstract class CurseEventHandlerBase<This> : DebuffEventHandlerBase<This>, ICurseEventHandler
        where This : CurseEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new CurseDebuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as CurseDebuffDataTable.TableRecord;
        }
        
        #endregion
    }
}