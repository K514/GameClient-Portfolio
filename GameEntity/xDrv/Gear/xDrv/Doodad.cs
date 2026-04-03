using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class Doodad : GearEntityBase
    {
        #region <Fields>

        private Animation _Animation;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(GearPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _Animation = GetComponentInChildren<Animation>();
        }

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                RemoveState(GameEntityTool.EntityStateType.STABLE);
                SetLifeSpan(0f, 1f);
                TurnLayerTo(GameConst.GameLayerType.UnitB);

                _Animation.Rewind(); 
                _Animation.Play(); 
                _Animation.Sample(); 
                _Animation.Stop();
                
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

            _Animation.Play(_Animation.clip.name);
        }

        #endregion
    }
}