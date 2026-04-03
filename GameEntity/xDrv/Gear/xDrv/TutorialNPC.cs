using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class TutorialNPC : GearEntityBase
    {
        #region <Fields>

        private int _Phase;

        #endregion
        
        #region <Callbacks>

        protected override bool OnActivate(GearPoolManager.CreateParams p_CreateParams, GearPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                RemoveState(GameEntityTool.EntityStateType.STABLE);
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.HP_Base, 100000, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
                AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.HP_Fix_Recovery, 100000);
                SetLifeSpan(0f, 1f);
                SetGroupMask(2);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnUpdateEntity_TimeBlock()
        {
            base.OnUpdateEntity_TimeBlock();

            switch (_Phase)
            {
                case 0:
                {
                    SceneEnvironmentManager.GetInstanceUnsafe.TryGetGamePlaySceneEnvironment(out var o_Env);
                    var isOver = o_Env.IsLocationEliminate(SceneTool.SceneLocationType.ZakoSpawner);
                    if (isOver)
                    {
                        _Phase++;
                        Debug.LogError(_ParitcleSystemControl.ParticleMainDuration);
                        PlayParticleSystem();
                    }
                    break;
                }
                case 1:
                {
                    if (InteractManager.GetInstanceUnsafe.TryGetSqrDistance(this, PlayerManager.GetInstanceUnsafe.Player, out var o_SqrDistance) && o_SqrDistance < 9f)
                    {
                        _Phase++;
                        PlayDescription();
                    }
                    break;
                }
            }

        }
        
        #endregion

        #region <Methods>

        private async void PlayDescription()
        {
     
        }

#endregion
    }
}