using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Callbacks>
        
        public void OnTriggerEnterFrom(Collider other)
        {
            switch (_CurrentLifeSpanPhase)
            {
                case GameEntityTool.EntityLifeSpanPhase.DeadSpan:
                case GameEntityTool.EntityLifeSpanPhase.LifeSpanTerminate:
                    break;
                case GameEntityTool.EntityLifeSpanPhase.None:
                case GameEntityTool.EntityLifeSpanPhase.LiveSpan:
                {
                    // 장해물/지형에 부딪힌 경우
                    if (other.IsLayerType(GameConst.GameLayerMaskType.BlockSet))
                    {
                        OnTriggerEnterWithBlockObject(other);
                    }
                    else
                    {
                        if (InteractManager.GetInstanceUnsafe.TryGetEntity(other, out var o_Entity))
                        {
                            if (!ReferenceEquals(this, o_Entity))
                            {
                                OnTriggerEnterWithEntityObject(other, o_Entity);
                            }
                        }
                    }
                    
                    break;
                }
            }
        }

        private void OnTriggerEnterWithBlockObject(Collider p_Other)
        {
            var layer = p_Other.gameObject.layer;
            switch (p_Other)
            {
                case var _ when layer == GameConst.DepthBoundary_Layer:
                case var _ when layer == GameConst.OuterBoundary_Layer:
                {
                    OnTriggerEnterWithBoundary(p_Other);
                    break;
                }
                case var _ when layer == GameConst.Terrain_Layer:
                {
                    OnTriggerEnterWithTerrain(p_Other);
                    break;
                }
                case var _ when layer == GameConst.Obstacle_Layer:
                {
                    OnTriggerEnterWithObstacle(p_Other);
                    break;
                }
            }
        }

        protected virtual void OnTriggerEnterWithBoundary(Collider p_Other)
        {
            Debug.LogError($"부딪힘! {GetName()} -> 벽");

            foreach (var eventHandler in _AffineEventHandlerList)
            {
                eventHandler.OnTriggerEnterWithBoundary(p_Other);
            }
        }
        
        protected virtual void OnTriggerEnterWithTerrain(Collider p_Other)
        {
            Debug.LogError($"부딪힘! {GetName()} -> 바닥");
            
            foreach (var eventHandler in _AffineEventHandlerList)
            {
                eventHandler.OnTriggerEnterWithTerrain(p_Other);
            }
        }

        protected virtual void OnTriggerEnterWithObstacle(Collider p_Other)
        {
            Debug.LogError($"부딪힘! {GetName()} -> 장해물");
                
            foreach (var eventHandler in _AffineEventHandlerList)
            {
                eventHandler.OnTriggerEnterWithObstacle(p_Other);
            };
        }

        protected virtual void OnTriggerEnterWithEntityObject(Collider p_Other, IGameEntityBridge p_Trigger)
        {
            Debug.LogError($"부딪힘! {GetName()} -> {p_Trigger.GetName()}");
            
            foreach (var eventHandler in _AffineEventHandlerList)
            {
                eventHandler.OnTriggerEnterWithEntityObject(p_Other, p_Trigger);
            };
        }
        
        #endregion
    }
}