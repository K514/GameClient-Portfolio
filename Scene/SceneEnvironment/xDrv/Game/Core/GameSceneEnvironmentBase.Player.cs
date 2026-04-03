using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameSceneEnvironmentBase
    {
        #region <Methods>

        private void SpawnPlayer()
        {
            var playerSpawnPosition = GetStartPosition();
            var tryPlayer = PlayerManager.GetInstanceUnsafe.Player;
            
            if (ReferenceEquals(null, tryPlayer))
            {
                var partyMemberList = GameManager.GetInstanceUnsafe.GetPartyMemberList();
                if (partyMemberList.TryGetElementSafe(0, out var o_MemberData))
                {
                    var playerCreateParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(o_MemberData.UnitSpawnDataTableIndex, ResourceLifeCycleType.ManualUnload);
                    var player = 
                        UnitPoolManager.GetInstanceUnsafe.Pop
                        (
                            playerCreateParams, 
                            new UnitPoolManager.ActivateParams
                            (
                        null, 
                                new AffineCorrectionPreset
                                (
                                    AffineTool.CorrectPositionType.ForceSurface, 
                                    new AffinePreset(playerSpawnPosition, Quaternion.identity, 1f),
                                    GameConst.Terrain_LayerMask
                                ), 
                                p_GameEntityActivateParamsAttributeMask: GameEntityTool.ActivateParamsAttributeType.GivePlayer,
                                p_Alias: "Player Bigxl"
                            )
                        );
                    
                    player.SetLevel(o_MemberData.Level);
                    player.SetExp(o_MemberData.Exp);
                    player.AddStatusRate(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, 1f);
                    player.AddStatusRate(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.MP_Base, 1f);
                    player.SetGroupMask(1);

                    var actionModule = player.ActionModule;
                    var extraPassiveActionIndexList = o_MemberData.PassiveSkillIndexTable;
                    if (extraPassiveActionIndexList.CheckCollectionSafe())
                    {
                        foreach (var actionIndexKV in extraPassiveActionIndexList)
                        {
                            actionModule.BindAction(new ActionTool.ActionBindPreset(actionIndexKV.Key, actionIndexKV.Value));
                        }
                    }
                    
                    /*
                        UIxControlRoot.GetInstanceUnsafe.UIxNameTheater.PopTheaterElement(player);
                        UIxControlRoot.GetInstanceUnsafe.UIxHpBarTheater.PopTheaterElement(player);  
                    */
                    
                    /* 파티 멤버 생성 */
                    SpawnPartyMember(1, InputEventTool.TriggerKeyType.A);
                    SpawnPartyMember(2, InputEventTool.TriggerKeyType.S);
                    SpawnPartyMember(3, InputEventTool.TriggerKeyType.D);
                }
            }
            else
            {
                /* 플레이어 시작 위치 초기화 */
                tryPlayer.TrySetTerrainSurfacePosition(playerSpawnPosition);
                
                /* 파티 멤버 초기화 */
                var partyMemberGroup = tryPlayer.GetPartyGroup();
                foreach (var partyMember in partyMemberGroup)
                {
                    partyMember.SwitchPersona(BoundedModuleDataTableQuery.TableLabel.Dummy);
                }
                
                /* 소유 개체 초기화 */
                var slaveGroup = tryPlayer.GetSlaveGroup();
                foreach (var slave in slaveGroup)
                {
                    var pivotPos = tryPlayer.GetBottomPosition();
                    var randPos = pivotPos.GetRandomPosition(XYZType.ZX, tryPlayer.GetRadius(2f), tryPlayer.GetRadius(4f));
                    var spawnPosition =
                        randPos.TryGetTerrainSurfacePosition(out var o_Position)
                            ? o_Position
                            : randPos;
                    slave.SetPosition(spawnPosition);
                    slave.SwitchPersona(AutonomyModuleDataTableQuery.TableLabel.Following);
                }
            }
        }

        private void SpawnPartyMember(int p_PartyIndex, InputEventTool.TriggerKeyType p_CommandType)
        {
            var player = PlayerManager.GetInstanceUnsafe.Player;
            var partyMemberList = GameManager.GetInstanceUnsafe.GetPartyMemberList();
            if (partyMemberList.TryGetElementSafe(p_PartyIndex, out var o_MemberData))
            {
                var memberCreateParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(o_MemberData.UnitSpawnDataTableIndex, ResourceLifeCycleType.ManualUnload);
                var memberSpawnPosition = player.GetBottomPosition();
                var member = UnitPoolManager.GetInstanceUnsafe
                    .Pop(memberCreateParams, 
                        new UnitPoolManager.ActivateParams(
                            null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(memberSpawnPosition, Quaternion.identity)), 
                            GameEntityTool.ActivateParamsAttributeType.GivePreserveCorpse, p_Alias: "Member"));
       
                member.TurnLayerTo(GameConst.GameLayerType.UnitC);
                member.SetLevel(o_MemberData.Level);
                member.SetExp(o_MemberData.Exp);
                member.SetGroupMask(2);

                /*
                var actionModule = player.ActionModule;
                var extraPassiveActionIndexList = o_MemberData.PassiveSkillIndexList;
                if (extraPassiveActionIndexList.CheckCollectionSafe())
                {
                    foreach (var actionIndex in extraPassiveActionIndexList)
                    {
                        actionModule.BindAction(actionIndex);
                    }
                }
                */
                
                player.AddPartyMember(member);
            }
        }

        #endregion
    }
}