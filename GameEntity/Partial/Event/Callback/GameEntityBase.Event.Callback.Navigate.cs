namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        public void OnNavigateStart(GeometryTool.NavigationResultPreset p_Preset)
        {
            OnModule_NavigateStart(p_Preset);
        }

        public void OnNavigateProgress(GeometryTool.NavigationResultPreset p_Preset)
        {
            OnModule_NavigateProgress(p_Preset);
        }

        public void OnNavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
            OnModule_NavigateEnd(p_Preset);
        }
    }
}