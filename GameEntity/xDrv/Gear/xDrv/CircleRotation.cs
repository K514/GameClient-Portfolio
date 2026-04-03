using System.Collections.Generic;
using System.Numerics;
using k514.Mono.Common;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace k514.Mono.Feature
{
    public class CircleRotation : GearEntityBase
    {
        #region <Fields>

        private Transform _RotationPivot;
        private List<ProjectileEntityBase> _ProjectileGroup;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(GearPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _RotationPivot = Affine.GetChild(0);
            _ProjectileGroup = new List<ProjectileEntityBase>();
        }

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                var caster = GetMaster();
                var spawnScale = caster[StatusTool.ShotStatusGroupType.Total][ShotStatusTool.ShotStatusType.ShotScale];
                var shotDirection = GetLookUV();
                var cnt = 8;
                var damageRate = 2f / cnt;
                var moveSpeedRate = 2f;

                var theta = 360f / cnt;
                for (var i = 0; i < cnt; i++)
                {
                    var spawnPos = GetCenterPosition();
                    var affine = new AffinePreset(spawnPos,  spawnScale);
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, affine, GameConst.Terrain_LayerMask);
                    var spawned = 
                        ProjectilePoolManager.GetInstanceUnsafe
                            .SpawnForwardProjectile(default, caster, affineCorrect, shotDirection, moveSpeedRate, damageRate, 20f);

                    spawned.Affine.SetParent(_RotationPivot, true);
                    _ProjectileGroup.Add(spawned);
                
                    shotDirection = shotDirection.RotationVectorByPivot(Vector3.up, theta);
                }
            
                SetLifeSpan(10f, 1f);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(GearPoolManager.CreateParams p_CreateParams, bool p_IsPooled,
            bool p_IsDisposed)
        {
            ReleaseSubProjectile();
   
            base.OnRetrieve(p_CreateParams, p_IsPooled, p_IsDisposed); 
        }

        protected override void OnLiveSpanProgress(float p_DeltaTime)
        {
            base.OnLiveSpanProgress(p_DeltaTime);

            _RotationPivot.Rotate(Vector3.up, 90f * p_DeltaTime);
        }

        protected override void OnDeadSpanStarted()
        {
            base.OnDeadSpanStarted();

            ReleaseSubProjectile();
        }
        
        protected override void OnTriggerEnterWithTerrain(Collider p_Other)
        {
            base.OnTriggerEnterWithTerrain(p_Other);
            
            SetDead(false);
        }

        protected override void OnTriggerEnterWithBoundary(Collider p_Other)
        {
            base.OnTriggerEnterWithBoundary(p_Other);
       
            SetDead(false);
        }

        protected override void OnTriggerEnterWithObstacle(Collider p_Other)
        {
            base.OnTriggerEnterWithObstacle(p_Other);
    
            SetDead(false);
        }

        #endregion

        #region <Methods>

        private void ReleaseSubProjectile()
        {
            if (_ProjectileGroup.CheckCollectionSafe())
            {
                foreach (var projectile in _ProjectileGroup)
                {
                    if (projectile.IsEntityValid())
                    {
                        projectile.SetDead(false);
                    }
                }
            }
        }

        #endregion
    }
}