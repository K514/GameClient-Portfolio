using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 해당 개체에 포함된 파티클 시스템을 제어하는 인스턴스
        /// </summary>
        protected ParticleTool.ParticleSystemControl _ParitcleSystemControl;

        /// <summary>
        /// 해당 개체에 부착된 이펙트 테이블
        /// </summary>
        private Dictionary<int, VfxEntityBase> _AttachedVfxTable;
        
        #endregion

        #region <Callbacks>

        private void OnCreateParticleSystem()
        {
            _ParitcleSystemControl = new ParticleTool.ParticleSystemControl(Affine);
            _AttachedVfxTable = new Dictionary<int, VfxEntityBase>();
        }

        protected virtual void OnActivateParticleSystem()
        {
        }
        
        private void OnRetrieveParticleSystem()
        {
            _ParitcleSystemControl.ResetParticle();
            ResetAttachedParticle();
        }

        private void OnUpdateParticleSystemScale(float p_DeltaRatio)
        {
            _ParitcleSystemControl.SetParticleScale(p_DeltaRatio);
        }

        
        #endregion

        #region <Methods>

        public void AttachParticle(int p_Index, Vector3 p_Position)
        {
            RemoveAttachedParticle(p_Index);
            
            var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(p_Index);
            var activateParams =
                new VfxPoolManager.ActivateParams
                (
                    Affine,
                    new AffineCorrectionPreset
                    (
                        AffineTool.CorrectPositionType.None, 
                        new AffinePreset(p_Position, 1f)
                    ),
                    GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, this
                );
            
            _AttachedVfxTable.Add(p_Index, VfxPoolManager.GetInstanceUnsafe.Pop(createParams, activateParams));
        }

        public void RemoveAttachedParticle(int p_Index)
        {
            if (_AttachedVfxTable.TryGetValue(p_Index, out var o_Vfx))
            {
                if (o_Vfx.IsEntityValid())
                {
                    o_Vfx.SetDead(false);
                }
                
                _AttachedVfxTable.Remove(p_Index);
            }
        }
        
        public void ResetAttachedParticle()
        {
            foreach (var vfxKV in _AttachedVfxTable)
            {
                var vfx = vfxKV.Value;
                if (vfx.IsEntityValid())
                {
                    vfx.SetDead(false);
                }
            }
            _AttachedVfxTable.Clear();
        }

        protected virtual void PlayParticleSystem()
        {
            _ParitcleSystemControl.PlayParticle();
        }

        protected void StopParticleSystem()
        {
            _ParitcleSystemControl.StopParticle();
        }

        #endregion
    }
}