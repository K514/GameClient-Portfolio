using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public partial class UnitBase
    {
        protected override void OnBindLabel()
        {
            base.OnBindLabel();
            
            GameEntityType = GameEntityTool.GameEntityType.Unit;
            
            OnBindLabelBubble();
        }
    }
}