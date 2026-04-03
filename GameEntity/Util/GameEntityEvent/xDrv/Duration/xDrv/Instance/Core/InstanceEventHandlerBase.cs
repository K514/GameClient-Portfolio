using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IInstanceEventHandler : IDurationEventHandler<InstanceEventTool.InstanceEventType, InstanceEventHandlerCreateParams, InstanceEventHandlerActivateParams>
    {
    }
    
    public readonly struct InstanceEventHandlerCreateParams : IDurationEventHandlerCreateParams<InstanceEventTool.InstanceEventType>
    {
        public InstanceEventTool.InstanceEventType EventId { get; }
        public Type HandlerType { get; }

        public InstanceEventHandlerCreateParams(InstanceEventTool.InstanceEventType p_EventId, Type p_Type)
        {
            EventId = p_EventId;
            HandlerType = p_Type;
        }
    }

    public readonly struct InstanceEventHandlerActivateParams : IDurationEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public float Duration { get; }
        
        public InstanceEventHandlerActivateParams(IGameEntityBridge p_Entity, float p_Duration)
        {
            Entity = p_Entity;
            Duration = p_Duration;
        }
    }
    
    public abstract class InstanceEventHandlerBase<This> : DurationEventHandlerBase<This, InstanceEventTool.InstanceEventType, InstanceEventHandlerCreateParams, InstanceEventHandlerActivateParams>, IInstanceEventHandler
        where This : InstanceEventHandlerBase<This>, new()
    {
        #region <Methods>
        
        public override bool IsEnterable()
        {
            return true;
        }

        #endregion
    }
}