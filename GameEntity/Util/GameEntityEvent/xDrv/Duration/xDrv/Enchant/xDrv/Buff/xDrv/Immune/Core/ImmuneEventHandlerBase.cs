using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IImmuneEventHandler : IBuffEventHandler
    {
    }
    
    public abstract class ImmuneEventHandlerBase<This> : BuffEventHandlerBase<This>, IImmuneEventHandler
        where This : ImmuneEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new ImmuneBuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as ImmuneBuffDataTable.TableRecord;
        }
        
        #endregion
    }
}