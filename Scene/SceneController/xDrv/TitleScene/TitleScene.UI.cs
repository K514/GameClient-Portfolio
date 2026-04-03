#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public partial class TitleScene
    {
        #region <Callbacks>

        protected override void OnCreateUI()
        {
            base.OnCreateUI();
            
            OnCreateButton();
        }

        #endregion
    }
}
#endif
