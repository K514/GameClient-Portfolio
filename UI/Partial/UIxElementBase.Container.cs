#if !SERVER_DRIVE
namespace k514.Mono.Common
{
    public partial class UIxElementBase
    {
        #region <Fields>

        /// <summary>
        /// 클러스터 UI
        /// </summary>
        private UIxContainerBase _Container;

        #endregion

        #region <Methods>

        public void SetContainer(UIxContainerBase p_Container)
        {
            if (!ReferenceEquals(p_Container, _Container))
            {
                ResetContainer();

                if (!ReferenceEquals(null, p_Container))
                {
                    _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.Contained);
                    _Container = p_Container;
                    _Container.AddElement(this);
                }
            }
        }

        public UIxContainerBase GetContainer()
        {
            return _Container;
        }

        public void ResetContainer()
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.Contained))
            {
                _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.Contained);
                _Container.RemoveElement(this);
                _Container = null;
            }
        }

        #endregion
    }
}
#endif