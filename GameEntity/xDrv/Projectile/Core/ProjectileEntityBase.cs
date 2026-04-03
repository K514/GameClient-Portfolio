using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public abstract partial class ProjectileEntityBase : GameEntityBase<ProjectileEntityBase, ProjectilePoolManager.CreateParams, ProjectilePoolManager.ActivateParams>
    {
        #region <Callbacks>

        protected override void OnCreate(ProjectilePoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            OnCreateCollision();
        }

        protected override bool OnActivate(ProjectilePoolManager.CreateParams p_CreateParams, ProjectilePoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                OnActivateProjectileAttribute(p_ActivateParams);
            
                AddState(GameEntityTool.EntityStateType.STABLE);
                PlayParticleSystem();
                SetLifeSpan(p_ActivateParams.Duration, _ParitcleSystemControl.ParticleEmitDuration);
                
                return true;
            }
            else
            {
                return false;
            }


        }

        protected override void OnRetrieve(ProjectilePoolManager.CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed)
        {
            OnRetrieveCollision();
            OnRetrieveProjectileAttribute();
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }

        protected override void OnMasterChanged()
        {
            base.OnMasterChanged();
            
            SetFollowGroupMask(_Master);
        }
        
        protected override void OnMasterFallen()
        {
            SetDead(true);
        }
        
        protected override void OnDeadSpanStarted()
        {
            base.OnDeadSpanStarted();

            if (!HasAttribute(GameEntityTool.GameEntityAttributeType.PreserveCorpse))
            {
                SetRenderEnable(false);
            }
        }
        
        #endregion
    }
}