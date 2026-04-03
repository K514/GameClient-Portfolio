using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage 
    {
        #region <Callbacks>

        private void OnCreateSummon()
        {
            _EntityEventTable.Add(36000, new Summon_00(4013, 2));
            _EntityEventTable.Add(36001, new Summon_00(4013, 5));
        }

        #endregion
        
        #region <Classess>
        
        /// <summary>
        /// 고드름 2기 소환
        /// </summary>
        public class Summon_00 : GameEntityDeployEventBase
        {
            private UnitPoolManager.CreateParams _UnitCreateParams;
            private int _ShotCount;
 
            public Summon_00(int p_EntityIndex, int p_ShotCount)
            {
                _UnitCreateParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(p_EntityIndex);
                _ShotCount = p_ShotCount;
            }
               
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                
                for (var i = 0; i < _ShotCount; i++)
                {
                    await UniTask.Delay(100, cancellationToken: p_CancellationToken);
                    var spawnPosition = caster.GetBottomPosition().GetRandomPosition(XYZType.ZX, 2f, 5f);
                    var entity = 
                        UnitPoolManager.GetInstanceUnsafe
                            .Pop
                            (
                                _UnitCreateParams, 
                                new UnitPoolManager.ActivateParams
                                (
                            null, 
                                    new AffineCorrectionPreset
                                    (
                                        AffineTool.CorrectPositionType.ForceSurface, 
                                        new AffinePreset(spawnPosition, Quaternion.identity), 
                                        GameConst.Terrain_LayerMask
                                    ),
                                    p_GameEntityActivateParamsAttributeMask: GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, caster
                                )
                            );
                                 
                    // entity.SwitchPersona(MindModuleDataTableQuery.TableLabel.Following); 
                    entity.SetLifeSpan(20f, 1f);
                    entity.SetGroupMask(caster.GetAllyMask(), caster.GetEnemyMask());

                    var vfxCreateParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(81);
                    var activateParams =
                        new VfxPoolManager.ActivateParams
                        ( 
                            null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(entity.GetBottomPosition(), 2f * entity.Scale))
                        );
                    VfxPoolManager.GetInstanceUnsafe.Pop(vfxCreateParams, activateParams);
                }
            }
        }
        
        #endregion
    }
}