using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public interface IEnchantEventHandler : IDurationEventHandler<int, EnchantEventHandlerCreateParams, EnchantEventHandlerActivateParams>
    {
        IEnchantDataTableRecordBridge Record { get; }
    }
    
    public readonly struct EnchantEventHandlerCreateParams : IDurationEventHandlerCreateParams<int>
    {
        public int EventId { get; }
        public IEnchantDataTableRecordBridge Record { get; }
        public Type HandlerType => Record.EventHandlerType;

        public EnchantEventHandlerCreateParams(int p_EventId)
        {
            EventId = p_EventId;
            Record = EnchantDataTableQuery.GetInstanceUnsafe[EventId];
        }
    }

    public readonly struct EnchantEventHandlerActivateParams : IDurationEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public float Duration { get; }
        
        public EnchantEventHandlerActivateParams(IGameEntityBridge p_Entity, float p_Duration)
        {
            Entity = p_Entity;
            Duration = p_Duration;
        }
    }
    
    public abstract partial class EnchantEventHandlerBase<This> : DurationEventHandlerBase<This, int, EnchantEventHandlerCreateParams, EnchantEventHandlerActivateParams>, IEnchantEventHandler
        where This : EnchantEventHandlerBase<This>, new()
    {
        #region <Fields>

        public IEnchantDataTableRecordBridge Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<EnchantEventHandlerCreateParams> p_Wrapper, EnchantEventHandlerCreateParams p_CreateParams)
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