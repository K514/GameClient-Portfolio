namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        /// <summary>
        /// 메인 카메라 상호작용 프리셋
        /// </summary>
        private CameraCullingPreset _CameraInteractState;

        /// <summary>
        /// 해당 오브젝트가 현재 가시 상태인지 표시하는 플래그
        /// </summary>
        public bool IsObjectVisible => _CameraInteractState.UnitRenderStateMask.HasAnyFlagExceptNone(GameEntityTool.GameEntityRenderType.Model);
        
        /// <summary>
        /// 해당 오브젝트를 추적하는 UI가 현재 가시 상태인지 표시하는 플래그
        /// </summary>
        public bool IsObjectUIVisible => _CameraInteractState.UnitRenderStateMask.HasAnyFlagExceptNone(GameEntityTool.GameEntityRenderType.UI);

        private void OnCreateObserve()
        {
            RenderModule.ResetRendererLayer(RenderableTool.RenderGroupType.Whole);
        }

        private void OnActivateObserve()
        {
            _CameraInteractState = new CameraCullingPreset(GameEntityTool.GameEntityRenderType.Model);
        }
        
#if !SERVER_DRIVE
        private void OnUpdateCameraInteractState()
        {
            var prevState = _CameraInteractState;
            _CameraInteractState = CameraManager.GetInstanceUnsafe.GetCameraCullingState(this);
            
            var prevRenderState = prevState.UnitRenderStateMask;
            var currentRenderState = _CameraInteractState.UnitRenderStateMask;
            
            foreach (var renderState in GameEntityTool.RenderTypeEnumerator)
            {
                var stateValidType = prevRenderState.HasAnyFlagExceptNone(renderState).GetValidState(currentRenderState.HasAnyFlagExceptNone(renderState));
                switch (stateValidType)
                {
                    case ValidStateType.Added:
                        if (renderState == GameEntityTool.GameEntityRenderType.Model)
                        {
                            RenderModule.ResetRendererLayer(RenderableTool.RenderGroupType.Whole);
                        }
                        break;
                    case ValidStateType.Removed:
                        if (renderState == GameEntityTool.GameEntityRenderType.Model)
                        {
                            RenderModule.TurnRendererLayerTo(RenderableTool.RenderGroupType.Whole, GameConst.GameLayerType.CameraIgnore);
                        }
                        break;
                }
                
                GameEntityRenderEventSender.SendEvent(renderState, new GameEntityRenderEventParams(stateValidType, _CameraInteractState));
            }
        }
#endif
    }
}