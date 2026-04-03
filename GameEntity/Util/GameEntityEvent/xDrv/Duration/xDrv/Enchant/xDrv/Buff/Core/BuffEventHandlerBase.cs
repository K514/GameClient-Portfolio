using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IBuffEventHandler : IEnchantEventHandler
    {
    }
    
    public abstract class BuffEventHandlerBase<This> : EnchantEventHandlerBase<This>, IBuffEventHandler
        where This : BuffEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new IBuffDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
            
            Record = base.Record as IBuffDataTableRecordBridge;
        }

        #endregion
    }
}