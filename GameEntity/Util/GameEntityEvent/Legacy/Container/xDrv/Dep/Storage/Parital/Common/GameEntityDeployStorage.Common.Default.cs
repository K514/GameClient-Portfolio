using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityDeployStorage
    {
        #region <Callbacks>

        private void OnCreateDefault()
        {
            _EntityEventTable.Add(1, new NoneAction());
            _EntityEventTable.Add(2, new DefaultJumpAction());
            _EntityEventTable.Add(3, new NoneAction());
            _EntityEventTable.Add(4, new NoneAction());
            _EntityEventTable.Add(5, new CallPartyMemberAction(0));
            _EntityEventTable.Add(6, new CallPartyMemberAction(1));
            _EntityEventTable.Add(7, new CallPartyMemberAction(2));
            _EntityEventTable.Add(8, new CallPartyMemberAction(3));
            _EntityEventTable.Add(100, new HitAction());
            _EntityEventTable.Add(101, new GroggyAction());
            _EntityEventTable.Add(102, new PlayerGroggyAction());
        }

        #endregion

        #region <Classess>

        public class NoneAction : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken){}
        }
        
        public class DefaultJumpAction : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                caster.PhysicsModule.ClearForceExcept(PhysicsTool.ForceType.SyncWithController);
                caster.GeometryModule.StopNavigate();
                caster.PhysicsModule.OverlapVelocity(PhysicsTool.ForceType.Jump, Vector3.up * caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.JumpForce]);
            }
        }

        public class CallPartyMemberAction : GameEntityDeployEventBase
        {
            #region <Fields>

            private VfxPoolManager.CreateParams _EffectCreateParams0;
            private VfxPoolManager.CreateParams _EffectCreateParams1;
            private VfxPoolManager.CreateParams _EffectCreateParams2;
            private VfxPoolManager.CreateParams _EffectCreateParams3;
            private int _MemberIndex;

            #endregion

            #region <Constructor>

            public CallPartyMemberAction(int p_MemberIndex)
            {
                _EffectCreateParams0 = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__PartySkillVfxIndex0);
                _EffectCreateParams1 = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__PartySkillVfxIndex1);
                _EffectCreateParams2 = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__PartySkillVfxIndex2);
                _EffectCreateParams3 = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(VfxTool.__PartySkillVfxIndex3);
 
                _MemberIndex = p_MemberIndex;
            }

            #endregion

            #region <Methods>
            
            public override void PreloadDeployEvent()
            {
                base.PreloadDeployEvent();
                
            }
            
            public override bool IsRunnable(GameEntityDeployEventContainer p_Handler)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                if (caster.TryGetPartyMember(_MemberIndex, out var o_Member))
                {
                    return !o_Member.IsDead && o_Member.IsDisable;
                }
                else
                {
                    return false;
                }
            }

            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                if (caster.TryGetPartyMember(_MemberIndex, out var o_PartyEntity) 
                    && !o_PartyEntity.IsDead 
                    && o_PartyEntity.IsDisable 
                    && o_PartyEntity.TrySetTerrainSurfacePosition(caster.GetTopRelativePosition(0.75f)))
                {
                    var cnt = _MemberIndex % 4;
                    var spawnPos = o_PartyEntity.GetBottomPosition() + 0.001f * Vector3.up;
                    var affineCorrect = new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPos, 3f));
                    
                    switch (cnt)
                    {
                        case 0:
                        {
                            VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams0, new VfxPoolManager.ActivateParams(null, affineCorrect));
                            break;
                        }
                        case 1:
                        {
                            VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams1, new VfxPoolManager.ActivateParams(null, affineCorrect));
                            break;
                        }
                        case 2:
                        {
                            VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams2, new VfxPoolManager.ActivateParams(null, affineCorrect));
                            break;
                        }
                        case 3:
                        {
                            VfxPoolManager.GetInstanceUnsafe.Pop(_EffectCreateParams3, new VfxPoolManager.ActivateParams(null, affineCorrect));
                            break;
                        }
                    }

                    o_PartyEntity.SetDisable(false);
                    o_PartyEntity.AddState(GameEntityTool.EntityStateType.STABLE);
                    o_PartyEntity.SetLookUV(caster.GetLookUV());
                    
                    // 파티원에게 명령
                    var mindModule = o_PartyEntity.MindModule;
                    var isOrderValid = false;
                    if (mindModule.ReserveMove(MindTool.MindOrderReserveType.Overlap, new MindTool.MoveOrderParams(caster.GetRelativePosition(2f), GeometryTool.NavigationAttributeFlag.ForceSurface, AnimationTool.MoveMotionType.Run)))
                    {
                        o_PartyEntity.AddStatusRate(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.MP_Base, 1f);
                        o_PartyEntity.ActionModule.ResetCooldown();
                        /*if (mindModule.ReserveEinAction(MindTool.MindOrderReserveType.Add, out var o_ReservedAction))
                        {
                            if (mindModule.ReserveDisable(MindTool.MindOrderReserveType.Add, 0.2f))
                            {
                                if (param.TryGetAction(out var o_Action))
                                {
                                    o_Action.SetCooldown(o_ReservedAction.Record.CoolDown);
                                }
                                
                                isOrderValid = true;
                            }
                        }*/
                    }

                    // 명령에 실패한 경우
                    if (!isOrderValid)
                    {
                        o_PartyEntity.SetDisable(true); 
                    }
                }
            }
            
            #endregion
        }

        public class HitAction : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                /*var queryParams 
                    = new EntityQueryTool.FilterQueryParams
                    (
                        default, GameEntityTool.GameEntityGroupRelateType.Enemy, 
                        EntityQueryTool.FilterQueryFlagType.ExceptMe | EntityQueryTool.FilterQueryFlagType.FreeAll, 
                        GameEntityTool.EntityStateType.EngageCandidateFilterMask, p_FilterResultType: EntityQueryTool.FilterResultType.RemainNearestOne
                    );
            
                if (caster.FilterFocusEntity(queryParams))*/
                {
                    var filterGroup = param.Caster.FilterResultGroup;
                    foreach (var filtered in filterGroup)
                    {
                        filtered.GiveDamage(5, new StatusTool.StatusChangeParams());
                    }
                }
            }
        }
        
        public class GroggyAction : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                // caster.Enchant(17000001, new GameEntityEventCommonParams(caster));
            }
        }
        
        public class PlayerGroggyAction : GameEntityDeployEventBase
        {
            public override async UniTask RunEvent(GameEntityDeployEventContainer p_Handler, CancellationToken p_CancellationToken)
            {
                var param = p_Handler.DeployParams;
                var caster = param.Caster;
                // PlayerManager.GetInstanceUnsafe.Player.Enchant(17000001, new GameEntityEventCommonParams(caster));
            }
        }

        #endregion
    }
}