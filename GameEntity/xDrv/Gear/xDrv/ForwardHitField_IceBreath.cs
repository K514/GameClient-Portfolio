using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public class ForwardHitField_IceBreath : ForwardHitField
    {
        protected override void OnHitEntity(IGameEntityBridge p_Entity)
        {
            base.OnHitEntity(p_Entity);
            
            // p_Entity.Enchant(16000000, new GameEntityEventCommonParams(_Master));
        }
    }
}