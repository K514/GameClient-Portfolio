/*#if !SERVER_DRIVE
using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class RenderableBase
    {
        #region <Fields>

        private Dictionary<GameEntityTool.AttachingVfxType, VFXUnit> _AttachedVfxCollection;

        #endregion

        #region <Callbacks>

        private void OnInitializeAttachVfx()
        {
            _AttachedVfxCollection = new Dictionary<GameEntityTool.AttachingVfxType, VFXUnit>();
            var enumerator = GameEntityTool.AttachingVfxTypeEnumerator;
            foreach (var vfxType in enumerator)
            {
                _AttachedVfxCollection.Add(vfxType, null);
            }
        }

        #endregion

        #region <Methods>

        public (bool, VFXUnit) AttachVfx(GameEntityTool.AttachingVfxType p_Type, int p_SpawnIndex, TransformTool.AffineCachePreset p_Affine, uint p_PreDelay = 0)
        {
            var tryVfx = _AttachedVfxCollection[p_Type];
            if (ReferenceEquals(null, tryVfx))
            {
                var (isValid, spawned) = UnitRenderingManager.GetInstanceUnsafe.CastUnitAttachedVfx(p_SpawnIndex, _GameEntity, p_Affine, p_PreDelay);
                _AttachedVfxCollection[p_Type] = spawned;

                return (isValid, spawned);
            }
            else
            {
                return (false, tryVfx);
            }
        }
        
        public void DetachVfx(GameEntityTool.AttachingVfxType p_Type)
        {
            var tryVfx = _AttachedVfxCollection[p_Type];
            if (ReferenceEquals(null, tryVfx))
            {
            }
            else
            {
                tryVfx.Pooling();
                _AttachedVfxCollection[p_Type] = null;
            }
        }

        #endregion
    }
}
#endif*/