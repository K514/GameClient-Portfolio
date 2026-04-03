using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class CircleWave : GearEntityBase
    {
        #region <Fields>

        private ProgressTimer _EventInterval;
        private int _CurrentCount, _WaveCount;
        
        #endregion

        #region <Callbacks>

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _EventInterval = 0.3f;
                _CurrentCount = 0;
                _WaveCount = 6;
            
                SetLifeSpan(999f, 1f);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnLiveSpanProgress(float p_DeltaTime)
        {
            base.OnLiveSpanProgress(p_DeltaTime);

            if (_EventInterval.IsOver())
            {
                _EventInterval.Reset();
                if (_CurrentCount < _WaveCount)
                {
                    _CurrentCount++;

                    var createParams = ProjectorPoolManager.GetInstanceUnsafe.GetCreateParams(3);
                    ProjectorPoolManager.GetInstanceUnsafe.SpawnProjector
                    (
                        createParams, this, new AffineCorrectionPreset(AffineTool.CorrectPositionType.CorrectSurface, 
                        new AffinePreset(GetCenterPosition(), 1f), GameConst.Terrain_LayerMask), Vector3.zero, 1f, 53021, ProjectorTool.ActivateParamsAttributeType.None, 6f, 6f, 0.2f, 0.3f, 0.1f
                    );
                }
                else
                {
                    SetDead(false);
                }
            }
            else
            {
                _EventInterval.Progress(p_DeltaTime);
            }
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
    }
}