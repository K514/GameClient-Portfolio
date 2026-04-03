using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class BeamEntityBase
    {
        #region <Fields>

        private BeamTool.BeamAttributeType _BeamAttribuetFlagMask;
        private bool _HasPierce => _BeamAttribuetFlagMask.HasAnyFlagExceptNone(BeamTool.BeamAttributeType.Pierce);
        private int _PierceCount;
        
        #endregion

        #region <Callbacks>

        private void OnActivateBeamAttribute(BeamPoolManager.ActivateParams p_ActivateParams)
        {
            SetPierce(p_ActivateParams.PierceCount);
        }

        private void OnRetrieveBeamProjectileAttribute()
        {
            SetPierce(0);
            
            _BeamAttribuetFlagMask = BeamTool.BeamAttributeType.None;
        }
        
        #endregion

        #region <Methods>

        public void SetPierce(int p_Count)
        {
            if (p_Count > 0)
            {
                _BeamAttribuetFlagMask.AddFlag(BeamTool.BeamAttributeType.Pierce);
                _PierceCount = p_Count;
            }
            else
            {
                _BeamAttribuetFlagMask.RemoveFlag(BeamTool.BeamAttributeType.Pierce);
                _PierceCount = 0;
            }
        }

        #endregion
    }
}