using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public abstract partial class UIxElementBase
    {
        #region <Fields>

        private Vector3 _PivotPosition; 

        #endregion

        #region <Callbacks>

        protected override void OnCreateAffine()
        {
        }
        
        protected override void OnActivateAffine(UIPoolManager.ActivateParams p_ActivateParams)
        {
            if (p_ActivateParams.ActivateParamType == UIPoolManager.ActivateParams.UIActivateParamType.PivotPosition)
            {
                SetPivotPosition(p_ActivateParams.PivotPosition);
            }
        }
        
        protected override void OnRetrieveAffine()
        {
            _UIDynamicStateFlagMask.RemoveFlag(UIxTool.UIxDynamicStateType.PivotPosition);
            _PivotPosition = default;
        }

        private void OnUpdatePivotPosition()
        {
            if (_UIDynamicStateFlagMask.HasAnyFlagExceptNone(UIxTool.UIxDynamicStateType.PivotPosition))
            {
                SetPosition(_PivotPosition);
            }
        }

        #endregion

        #region <Methods>

        public void SetPivotPosition(Vector3 p_Position)
        {
            _UIDynamicStateFlagMask.AddFlag(UIxTool.UIxDynamicStateType.PivotPosition);
            _PivotPosition = p_Position;
        }

        public void SetPosition(Vector3 p_Position)
        {
            switch (CanvasPreset.RootPreset.RenderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                {
                    RectTransform.SetScreenPos(p_Position);

                    #region <Legacy>

                    /*// 구간[Para : 0f, Perp : 1f]
                    var cameraRotateDotValue = CameraManager.GetInstanceUnsafe._TraceUp_CameraLook_DotValue_Abs;
                    // 구간[Near : 1f, Far : 0f]
                    var cameraZoomRate = 2f - CameraManager.GetInstanceUnsafe._CurrentZoomDistanceRate;
                    var screenOffsetRate = 1f + cameraRotateDotValue * cameraZoomRate;
            
                    RectTransform.SetAddScreenPos(50f * screenOffsetRate * Vector3.up);*/

                    #endregion
                    
                    break;
                }
                case RenderMode.ScreenSpaceCamera:
                case RenderMode.WorldSpace:
                    Affine.position = p_Position;
                    break;
            }
        }
        
        #endregion
    }
}

#endif