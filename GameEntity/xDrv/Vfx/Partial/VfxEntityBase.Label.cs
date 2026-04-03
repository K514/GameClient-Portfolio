using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class VfxEntityBase
    {
        protected override void OnBindLabel()
        {
            base.OnBindLabel();
            
            GameEntityType = GameEntityTool.GameEntityType.Vfx;
            
            OnBindLabelBubble();
        }
    }
}