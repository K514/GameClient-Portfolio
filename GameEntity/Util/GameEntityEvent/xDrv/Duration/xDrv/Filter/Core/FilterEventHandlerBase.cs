using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IFilterEventHandler : IDurationEventHandler<FilterEventTool.FilterEventType, FilterEventHandlerCreateParams, FilterEventHandlerActivateParams>
    {
    }
    
    public readonly struct FilterEventHandlerCreateParams : IDurationEventHandlerCreateParams<FilterEventTool.FilterEventType>
    {
        public FilterEventTool.FilterEventType EventId { get; }
        public Type HandlerType { get; }

        public FilterEventHandlerCreateParams(FilterEventTool.FilterEventType p_EventId, Type p_Type)
        {
            EventId = p_EventId;
            HandlerType = p_Type;
        }
    }

    public readonly struct FilterEventHandlerActivateParams : IDurationEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public float Duration { get; }
        public readonly EntityQueryTool.FilterSpaceConfig FilterSpaceParams;
        
        public FilterEventHandlerActivateParams(IGameEntityBridge p_Entity, float p_Duration, EntityQueryTool.FilterSpaceConfig p_FilterSpaceParams)
        {
            Entity = p_Entity;
            Duration = p_Duration;
            FilterSpaceParams = p_FilterSpaceParams;
        }
    }
    
    public abstract class FilterEventHandlerBase<This> : DurationEventHandlerBase<This, FilterEventTool.FilterEventType, FilterEventHandlerCreateParams, FilterEventHandlerActivateParams>, IFilterEventHandler
        where This : FilterEventHandlerBase<This>, new()
    {
        #region <Methods>
        
        public override bool IsEnterable()
        {
            return true;
        }

        #endregion
    }
}