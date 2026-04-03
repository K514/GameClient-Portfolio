using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class ProjectileEntityBase
    {
        protected override void OnBindLabel()
        {
            base.OnBindLabel();
            
            GameEntityType = GameEntityTool.GameEntityType.Projectile;
            
            OnBindLabelBubble();
        }
    }
}