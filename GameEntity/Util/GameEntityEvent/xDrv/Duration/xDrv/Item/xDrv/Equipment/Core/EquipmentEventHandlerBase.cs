using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IEquipmentEventHandler : IItemEventHandler
    {
    }
    
    public abstract class EquipmentEventHandlerBase<This> : ItemEventHandlerBase<This>, IEquipmentEventHandler
        where This : EquipmentEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new IEquipmentItemDataTableRecordBridge Record { get; private set; }

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ItemEventHandlerCreateParams> p_Wrapper, ItemEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as IEquipmentItemDataTableRecordBridge;
        }

        #endregion
    }
}