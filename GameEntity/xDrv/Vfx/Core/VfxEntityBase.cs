using k514.Mono.Common;

namespace k514.Mono.Feature
{
    /// <summary>
    /// 파티클 시스템을 포함하는, 연출용 오브젝트를 제어하는 컴포넌트 클래스
    /// </summary>
    public abstract partial class VfxEntityBase : GameEntityBase<VfxEntityBase, VfxPoolManager.CreateParams, VfxPoolManager.ActivateParams>
    {
        #region <Callbacks>

        protected override bool OnActivate(VfxPoolManager.CreateParams p_CreateParams, VfxPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                TurnLayerTo(GameConst.GameLayerType.Vfx);
                AddState(GameEntityTool.EntityStateType.STABLE);

                if (p_ActivateParams.ActivateParamsAttributeMask.HasAnyFlagExceptNone(VfxTool.ActivateParamsAttributeType.DeferredPlayParticle))
                {
                }
                else
                {
                    PlayParticleSystem();
                }
                
                return true;
            }
            else
            {
                return false;
            }
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

        #region <Methods>

        protected override void PlayParticleSystem()
        {
            base.PlayParticleSystem();
            
            if (_ParitcleSystemControl.ValidFlag)
            {
                SetLifeSpan(_ParitcleSystemControl.ParticleMainDuration, _ParitcleSystemControl.ParticleEmitDuration);
            }
            else
            {
                SetDead(true);
            }
        }

        #endregion
    }
}