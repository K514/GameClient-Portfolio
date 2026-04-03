using System;
using k514.Mono.Feature;
using UnityEngine.UIElements;

namespace k514.Mono.Common
{
    public interface IItemEventHandler : IDurationEventHandler<int, ItemEventHandlerCreateParams, ItemEventHandlerActivateParams>
    {
        IItemDataTableRecordBridge Record { get; }
    }
    
    public readonly struct ItemEventHandlerCreateParams : IDurationEventHandlerCreateParams<int>
    {
        public int EventId { get; }
        public IItemDataTableRecordBridge Record { get; }
        public Type HandlerType => Record.EventHandlerType;

        public ItemEventHandlerCreateParams(int p_EventId)
        {
            EventId = p_EventId;
            Record = ItemDataTableQuery.GetInstanceUnsafe[EventId];
        }
    }

    public readonly struct ItemEventHandlerActivateParams : IDurationEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public float Duration { get; }
        
        public ItemEventHandlerActivateParams(IGameEntityBridge p_Entity, float p_Duration)
        {
            Entity = p_Entity;
            Duration = p_Duration;
        }
    }
    
    public abstract class ItemEventHandlerBase<This> : DurationEventHandlerBase<This, int, ItemEventHandlerCreateParams, ItemEventHandlerActivateParams>, IItemEventHandler
        where This : ItemEventHandlerBase<This>, new()
    {
        #region <Fields>

        public IItemDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<ItemEventHandlerCreateParams> p_Wrapper, ItemEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
      
            Record = p_CreateParams.Record;
        }

        #endregion

        #region <Methods>

        public override bool IsEnterable()
        {
            return true;
        }
   
        #endregion
    }
}