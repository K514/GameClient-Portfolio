using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IAffineEventHandler : IDurationEventHandler<AffineEventTool.AffineEventType, AffineEventHandlerCreateParams, AffineEventHandlerActivateParams>
    {
        void OnTriggerEnterWithBoundary(Collider p_Other) ;
        void OnTriggerEnterWithTerrain(Collider p_Other);
        void OnTriggerEnterWithObstacle(Collider p_Other);
        void OnTriggerEnterWithEntityObject(Collider p_Other, IGameEntityBridge p_Collided);
    }
    
    public readonly struct AffineEventHandlerCreateParams : IDurationEventHandlerCreateParams<AffineEventTool.AffineEventType>
    {
        public AffineEventTool.AffineEventType EventId { get; }
        public Type HandlerType { get; }

        public AffineEventHandlerCreateParams(AffineEventTool.AffineEventType p_EventId, Type p_Type)
        {
            EventId = p_EventId;
            HandlerType = p_Type;
        }
    }

    public readonly struct AffineEventHandlerActivateParams : IDurationEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public float Duration { get; }
        public readonly Vector3 Vector;
        
        public AffineEventHandlerActivateParams(IGameEntityBridge p_Entity, float p_Duration, Vector3 p_Vector)
        {
            Entity = p_Entity;
            Duration = p_Duration;
            Vector = p_Vector;
        }
    }
    
    public abstract class AffineEventHandlerBase<This> : DurationEventHandlerBase<This, AffineEventTool.AffineEventType, AffineEventHandlerCreateParams, AffineEventHandlerActivateParams>, IAffineEventHandler
        where This : AffineEventHandlerBase<This>, new()
    {
        #region <Callbacks>

        public virtual void OnTriggerEnterWithBoundary(Collider p_Other)
        {
        }

        public virtual void OnTriggerEnterWithTerrain(Collider p_Other)
        {
        }

        public virtual void OnTriggerEnterWithObstacle(Collider p_Other)
        {
        }

        public virtual void OnTriggerEnterWithEntityObject(Collider p_Other, IGameEntityBridge p_Collided)
        {
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