using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 필터 결과 리스트
        /// </summary>
        public ref List<IGameEntityBridge> FilterResultGroup => ref _FilterResultGroup;

        #endregion
        
        #region <Methods>
        
        public virtual EntityQueryTool.FilterSpaceConfig GetSightRangeFilterConfig()
        {
            return new EntityQueryTool.FilterSpaceConfig
            (
                EntityQueryTool.FilterSpaceType.Circle, 
                SightRange, SightRange, 
                p_FilterParamsFlag: EntityQueryTool.FilterSpaceAttributeFlag.None
            );
        }

        public bool FilterFocusEntityWithSightRange
        (
            EntityQueryTool.FilterStateQueryFlagType p_FilterQueryFlagMask = EntityQueryTool.FilterStateQueryFlagType.FreeAll | EntityQueryTool.FilterStateQueryFlagType.ExceptMe,
            GameEntityTool.EntityStateType p_FilterStateMask = GameEntityTool.EntityStateType.DEAD, 
            GameEntityTool.GameEntityGroupRelateType p_FilterGroupRelateMask = GameEntityTool.GameEntityGroupRelateType.Enemy,
            EntityQueryTool.FilterResultType p_FilterResultType = EntityQueryTool.FilterResultType.None
        )
        {
            var filterState = new EntityQueryTool.FilterState(p_FilterQueryFlagMask, p_FilterStateMask, p_FilterGroupRelateMask);
            var filterSpaceConfig = GetSightRangeFilterConfig();
            var filterSpace = filterSpaceConfig.GetFilterSpace(this);
            
            return InteractManager.GetInstanceUnsafe.FilterFocusEntity(this, ref filterState, ref filterSpace, p_FilterResultType);
        }
        
        #endregion
    }
}