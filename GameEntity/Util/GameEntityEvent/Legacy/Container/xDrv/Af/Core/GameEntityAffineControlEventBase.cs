namespace k514.Mono.Common
{
    public abstract class GameEntityAffineControlEventBase
    {
        public abstract bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler);
        public abstract bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime);
    }
}