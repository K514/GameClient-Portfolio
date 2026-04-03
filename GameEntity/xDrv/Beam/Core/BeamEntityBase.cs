using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    /// <summary>
    /// 파티클 시스템을 포함하는, 연출용 오브젝트를 제어하는 컴포넌트 클래스
    /// </summary>
    public abstract partial class BeamEntityBase : GameEntityBase<BeamEntityBase, BeamPoolManager.CreateParams, BeamPoolManager.ActivateParams>
    {
        #region <Consts>

        private static readonly int __RayCastHitsCapacity = 16;
        private static readonly string __MainTextureName = "_MainTex";
        private static readonly string __NoiseTextureName = "_Noise";
        
        #endregion
        
        #region <Fields>

        private LineRenderer _Laser;
        private Material _Material;
        private Vector4 _LaserNoise;
        private RaycastHit[] _RayCastHits;
        private ParticleTool.ParticleSystemControl _HitEffectControl;
        private float _MainTextureLength;
        private float _NoiseTextureLength;
        private float _EmitDistance;
        private int _CurrentHitCount, _MaxHitCount;
        private float _MaxHitCountInv;
        private ProgressTimer _HitInterval;
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(BeamPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _Laser = GetComponent<LineRenderer>();
            _Material = _Laser.material;
            _LaserNoise = new Vector4(1,1,1,1);
            _RayCastHits = new RaycastHit[__RayCastHitsCapacity];
            
            var (_, hitEffectWrapper) = Affine.FindRecursive<Transform>("Hit");
            _HitEffectControl = new ParticleTool.ParticleSystemControl(hitEffectWrapper);

            _MainTextureLength = ModelDataRecord.MainTextureLength;
            _NoiseTextureLength = ModelDataRecord.NoiseTextureLength;
            
            _Material.SetTextureScale(__MainTextureName, new Vector2(_LaserNoise[0], _LaserNoise[1]));                    
            _Material.SetTextureScale(__NoiseTextureName, new Vector2(_LaserNoise[2], _LaserNoise[3]));

            OnCreateCollision();
        }

        protected override bool OnActivate(BeamPoolManager.CreateParams p_CreateParams, BeamPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                OnActivateBeamAttribute(p_ActivateParams);
                SetLifeSpan(p_ActivateParams.EmitDuration, p_ActivateParams.FadeDuration);

                _CurrentHitCount = 0;
                _EmitDistance = p_ActivateParams.EmitDistance;
                _MaxHitCount = p_ActivateParams.HitCount;
                _MaxHitCountInv = 1f / _MaxHitCount;
                _HitInterval = (_LiveSpanTimer.Duration + _DeadSpanTimer.Duration) * _MaxHitCountInv;
                _HitInterval.Terminate();
                _Laser.SetPosition(0, Affine.position);
                _Laser.SetPosition(1, Affine.position);

                AddStatus(StatusTool.BattleStatusGroupType.CompoundMul, BattleStatusTool.BattleStatusType.Attack_Melee, _MaxHitCountInv);
                AddState(GameEntityTool.EntityStateType.STABLE);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(BeamPoolManager.CreateParams p_CreateParams, bool p_IsPooled,
            bool p_IsDisposed)
        {
            OnRetrieveCollision();
            OnRetrieveBeamProjectileAttribute();
            
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed);
        }

        protected override void OnScaleChanged(float p_DeltaRatio)
        {
            base.OnScaleChanged(p_DeltaRatio);

            _Laser.widthMultiplier *= p_DeltaRatio;
        }

        protected override void OnUpdateAffineEventHandler(float p_DeltaTime)
        {
            p_DeltaTime *= this[StatusTool.BattleStatusGroupType.Total].MoveSpeedRate;
            
            base.OnUpdateAffineEventHandler(p_DeltaTime);
        }
        
        protected override void OnMasterChanged()
        {
            base.OnMasterChanged();
            
            SetFollowGroupMask(_Master);
        }
        
        protected override void OnLiveSpanStarted()
        {
            base.OnLiveSpanStarted();
            
            PlayParticleSystem();
            _HitEffectControl.Affine.position = Affine.position;
            _HitEffectControl.PlayParticle();
        }
        
        protected override void OnLiveSpanProgress(float p_DeltaTime)
        {
            p_DeltaTime *= this[StatusTool.BattleStatusGroupType.Total].MoveSpeedRate;
            
            base.OnLiveSpanProgress(p_DeltaTime);

            CastBeam(p_DeltaTime, 1);
        }

        protected override void OnDeadSpanProgress(float p_DeltaTime)
        {
            p_DeltaTime *= this[StatusTool.BattleStatusGroupType.Total].MoveSpeedRate;

            base.OnDeadSpanProgress(p_DeltaTime);

            SetScaleFactor(Scale * (1f - _DeadSpanTimer.ProgressRate));
            
            CastBeam(p_DeltaTime, 1);
        }
        
        protected override void OnDeadSpanOver()
        {
            var remaindHitCount = _MaxHitCount - _CurrentHitCount;
            if (remaindHitCount > 0)
            {
                _HitInterval.Terminate();
                CastBeam(0f, remaindHitCount);
            }
            
            base.OnDeadSpanOver();
        }

        #endregion
    }
}