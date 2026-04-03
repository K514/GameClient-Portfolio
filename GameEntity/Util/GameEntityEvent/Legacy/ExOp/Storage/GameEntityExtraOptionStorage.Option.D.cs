using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityExtraOptionStorage
    {
        #region <Callbacks>

        private void OnCreateOptionD()
        {
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.ContractNPC, new ContractNPC());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.FirstRerollService, new FirstRerollService());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.SummonSkeleton, new SummonSkeleton());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.PurifyZone, new PurifyZone());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.CallMeteor, new CallMeteor());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.BlastZone, new BlastZone());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.MonsterReborn, new MonsterReborn());
            
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.Titanize, new Titanize());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.ExtraShot, new ExtraShot());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.BurstShot, new BurstShot());
            _ExtraOptionTable.Add(GameEntityExtraOptionTool.ExtraOptionType.AddVfx, new AddVfx());
        }

        #endregion

        #region <Classess>
        
        public class ContractNPC : GameEntityExtraOptionBase
        {
            private readonly int[] _NPC_Table = { 11, 21, 31, 41 };
            
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var caster = p_Handler.Caster;
                var spawnKey = _NPC_Table.GetRandomElement();
                var createParams = UnitPoolManager.GetInstanceUnsafe.GetCreateParams(spawnKey, ResourceLifeCycleType.ManualUnload);
                var pivotPosition = caster.GetBottomPosition();
                var randPosition = pivotPosition.GetRandomPosition(XYZType.ZX, caster.GetRadius(2f), caster.GetRadius(4f));
                var spawnPosition =
                    randPosition.TryGetTerrainSurfacePosition(out var o_Position)
                        ? o_Position
                        : pivotPosition;
                
                var spawned = UnitPoolManager.GetInstanceUnsafe
                    .Pop(createParams, 
                        new UnitPoolManager.ActivateParams(
                            null, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, new AffinePreset(spawnPosition, Quaternion.identity)), 
                            GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster, p_Alias: "NPC"));
       
                spawned.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.HP_Fix_Recovery, 30f);
                spawned.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MP_Fix_Recovery, 30f);
                spawned.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.AntiDamageRate_Melee, 0.3f);
                spawned.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.AttackSpeedRate, 0.3f);
                spawned.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedRate, 0.3f);
                spawned.TurnLayerTo(GameConst.GameLayerType.UnitC);
                // spawned.SwitchPersona(MindModuleDataTableQuery.TableLabel.Following);
                spawned.SetGroupMask(caster.GroupPreset);
                caster.AddSlave(spawned);
   
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class FirstRerollService : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                caster.AddAttribute(GameEntityTool.GameEntityAttributeType.FirstRerollService);
                
                return true;
            }
            
            public override bool DeactivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                caster.RemoveAttribute(GameEntityTool.GameEntityAttributeType.FirstRerollService);
                
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class SummonSkeleton : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                Debug.LogError("SummonSkeleton!");
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class PurifyZone : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                Debug.LogError("PurifyZone!");
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class CallMeteor : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                Debug.LogError("CallMeteor!");
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class BlastZone : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                Debug.LogError("BlastZone!");
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class MonsterReborn : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                Debug.LogError("MonsterReborn!");
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class Titanize : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var scaleFactor = record.Value + param.Count * record.Value2;
                p_Handler.Caster.MulScaleFactor(scaleFactor);
                p_Handler.SetHandlerRemain(true);
                
                return true;
            }

            public override bool DeactivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var record = p_Handler.Record;
                var param = p_Handler.ExtraOptionParams;
                var scaleFactor = record.Value + param.Count * record.Value2;
                p_Handler.Caster.MulScaleFactor(1f / scaleFactor);
                
                return true;
            }

            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var scaleFactor = p_Record.Value + p_Params.Count * p_Record.Value2;
                    return string.Format(o_Record.Description1, 100 * scaleFactor);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class ExtraShot : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                p_Handler.AddEvent
                (
                    GameEntityTool.GameEntityBaseEventType.Activate_DefaultCommand,
                    (handler, param, extraOption) =>
                    {
                        var record = handler.Record;
                        var possibility = record.Value;
                        var passiveLevel = extraOption.Count;
                        
                        if (possibility > Random.value)
                        {
                            var caster = handler.Caster;
                            var deployId = record.Index;
                            // caster.TryRunDeployEvent(deployId, new GameEntityEventCommonParams(passiveLevel));
                            handler.SetCooldown(record.Value2);
                        }
                    }
                );
                
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    var deployEventIndex = p_Record.Index;
                    if (GameEntityDeployStorage.GetInstanceUnsafe.TryGetDeployEvent(deployEventIndex, out var o_DeployEvent))
                    {
                        var possibility = 100f * p_Record.Value;
                        var cooldown = p_Record.Value2;
                        return string.Format(o_Record.Description1, possibility, cooldown);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class BurstShot : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                return true;
            }
            
            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return string.Format(o_Record.Description1, p_Record.Value, p_Record.Index);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public class AddVfx : GameEntityExtraOptionBase
        {
            public override bool ActivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                caster.AttachParticle(record.Index, caster.GetBottomPosition());
                p_Handler.SetHandlerRemain(true);
                
                return true;
            }

            public override bool DeactivateOption(GameEntityExtraOptionHandler p_Handler)
            {
                var caster = p_Handler.Caster;
                var record = p_Handler.Record;
                caster.RemoveAttachedParticle(record.Index);
                
                return true;
            }

            public override string GetOptionDescription(ExtraOptionDataTable.TableRecord p_Record, GameEntityEventCommonParams p_Preset, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params)
            {
                if (p_Record.TryGetLanguageRecord(out var o_Record))
                {
                    return o_Record.Description1;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        #endregion
    }
}