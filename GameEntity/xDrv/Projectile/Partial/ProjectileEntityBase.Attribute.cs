using System.Collections.Generic;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public partial class ProjectileEntityBase
    {
        #region <Fields>

        private ProjectileTool.ProjectileAttributeType _ProjectileAttribuetFlagMask;
        private bool _HasPierce => _ProjectileAttribuetFlagMask.HasAnyFlagExceptNone(ProjectileTool.ProjectileAttributeType.Pierce);
        private float _KnockBackForce;
        private int _PierceCount;
        
        #endregion

        #region <Callbacks>

        private void OnActivateProjectileAttribute(ProjectilePoolManager.ActivateParams p_ActivateParams)
        {
            var activateParamAttributeFlagMask = p_ActivateParams.ActivateParamsAttributeMask;
            if (activateParamAttributeFlagMask.HasAnyFlagExceptNone(ProjectileTool.ActivateParamsAttributeType.GivePierce))
            {
                SetPierce(p_ActivateParams.PierceCount);
            }
            if (activateParamAttributeFlagMask.HasAnyFlagExceptNone(ProjectileTool.ActivateParamsAttributeType.GiveKnockBack))
            {
                SetKnockBack(p_ActivateParams.KnockBack);
            }
            if (activateParamAttributeFlagMask.HasAnyFlagExceptNone(ProjectileTool.ActivateParamsAttributeType.GiveNonCollision))
            {
                SetCollision(false);
            }
        }

        private void OnRetrieveProjectileAttribute()
        {
            SetPierce(0);
            SetKnockBack(0f);
            SetCollision(true);
            
            _ProjectileAttribuetFlagMask = ProjectileTool.ProjectileAttributeType.None;
        }
        
        #endregion

        #region <Methods>

        public void SetPierce(int p_Count)
        {
            if (p_Count > 0)
            {
                _ProjectileAttribuetFlagMask.AddFlag(ProjectileTool.ProjectileAttributeType.Pierce);
                _PierceCount = p_Count;
            }
            else
            {
                _ProjectileAttribuetFlagMask.RemoveFlag(ProjectileTool.ProjectileAttributeType.Pierce);
                _PierceCount = 0;
            }
        }
        
        public void SetKnockBack(float p_Force)
        {
            if (p_Force.IsReachedZero())
            {
                _ProjectileAttribuetFlagMask.RemoveFlag(ProjectileTool.ProjectileAttributeType.KnockBack);
                _KnockBackForce = 0f;
            }
            else
            {
                _ProjectileAttribuetFlagMask.AddFlag(ProjectileTool.ProjectileAttributeType.KnockBack);
                _KnockBackForce = p_Force;
            }
        }

        public void SetCollision(bool p_Flag)
        {
            if (p_Flag)
            {
                _ProjectileAttribuetFlagMask.RemoveFlag(ProjectileTool.ProjectileAttributeType.NonCollision);
                SetPhysicsCollideEnable(true);
            }
            else
            {
                _ProjectileAttribuetFlagMask.AddFlag(ProjectileTool.ProjectileAttributeType.NonCollision);
                SetPhysicsCollideEnable(false);
            }
        }

        #endregion
    }
}