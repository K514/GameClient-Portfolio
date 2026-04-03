using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IClockEventHandler : IBuffEventHandler
    {
    }
    
    public abstract class ClockEventHandlerBase<This> : BuffEventHandlerBase<This>, IClockEventHandler
        where This : ClockEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new ClockBuffDataTable.TableRecord Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as ClockBuffDataTable.TableRecord;
        }
        
        #endregion
    }
}