using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IMaterialEventHandler : IItemEventHandler
    {
    }
    
    public abstract class MaterialEventHandlerBase<This> : ItemEventHandlerBase<This>, IMaterialEventHandler
        where This : MaterialEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new IMaterialItemDataTableRecordBridge Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ItemEventHandlerCreateParams> p_Wrapper, ItemEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as IMaterialItemDataTableRecordBridge;
        }

        #endregion
    }
}