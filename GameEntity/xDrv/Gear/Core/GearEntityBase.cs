using k514.Mono.Common;

namespace k514.Mono.Feature
{
    /// <summary>
    /// WorldObjectBase 기본 구현체
    /// </summary>
    public abstract partial class GearEntityBase : GameEntityBase<GearEntityBase, GearPoolManager.CreateParams, GearPoolManager.ActivateParams>
    {
        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                AddState(GameEntityTool.EntityStateType.STABLE);
                
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}