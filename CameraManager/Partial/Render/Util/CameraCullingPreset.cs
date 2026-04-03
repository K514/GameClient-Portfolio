#if !SERVER_DRIVE

namespace k514.Mono.Common
{
    public struct CameraCullingPreset
    {
        #region <Fields>

        public GameEntityTool.GameEntityRenderType UnitRenderStateMask;
        public CameraTool.CameraCullingState CameraCullingState;
        public float SqrDistance;
        
        #endregion

        #region <Constructors>

        public CameraCullingPreset(GameEntityTool.GameEntityRenderType p_UnitRenderStateMask, CameraTool.CameraCullingState cameraCullingState = default, float p_SqrDistance = default)
        {
            UnitRenderStateMask = p_UnitRenderStateMask;
            CameraCullingState = cameraCullingState;
            SqrDistance = p_SqrDistance;
        }

        #endregion

        #region <Operator>

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"RenderState : {UnitRenderStateMask} / CullingType : {CameraCullingState} / SqrDist : {SqrDistance}";
        }
#endif

        #endregion
    }
}

#endif