namespace k514.Mono.Common
{
    public static partial class GameEntityTool
    {
        public static bool IsEntityAvailable(this IGameEntityBridge p_This, EntityStateType p_PassMask)
        {
            return p_This.IsContentValid() && p_This.HasState_Or(p_PassMask);
        }
            
        public static bool IsEntityFree(this IGameEntityBridge p_This, EntityStateType p_FilterMask)
        {
            return p_This.IsContentValid() && !p_This.HasState_Or(p_FilterMask);
        }
        
        public static bool IsEntityValid(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityFree(EntityStateType.ValidationFilterMask);
        }
        
        public static bool IsEntityInvalid(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityAvailable(EntityStateType.ValidationFilterMask);
        }
        
        public static bool IsEngageable(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityFree(EntityStateType.EngageCandidateFilterMask);
        }
        
        public static bool IsAvailableProjectileCollide(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityFree(EntityStateType.ProjectileCollisionFilterMask);
        }
        
        public static bool IsAvailableBeamCollide(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityFree(EntityStateType.BeamCollisionFilterMask);
        }
        
        public static bool IsAvailablePlayHitMotion(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityFree(EntityStateType.HitMotionPlayFilterMask);
        }

        public static bool IsAvailableStopHitMotion(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityAvailable(EntityStateType.HitMotionStopFilterMask);
        }
        
        public static bool IsAvailableResumeHitMotion(this IGameEntityBridge p_This)
        {
            return p_This.IsEntityFree(EntityStateType.HitMotionStopFilterMask);
        }

        public static bool IsInteractableGameEntity(this IGameEntityBridge p_This)
        {
            if (p_This.IsEntityValid())
            {
                return p_This.IsInteractableGameEntity;
            }
            else
            {
                return false;
            }
        }
    }
}