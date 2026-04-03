using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityEnchantStorage
    {
        #region <Callbacks>

        private void OnCreateDebuff()
        {
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Curse, new CurseDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Shock, new ShockDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Stun, new StunDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Bleed, new BleedDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Poison, new PoisonDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Burn, new BurnDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Chill, new ChillDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Freeze, new FreezeDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Confuse, new ConfuseDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Blind, new BlindDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Silence, new SilenceDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Bind, new BindDebuff());
            _EntityDebuffTable.Add(DebuffDataTableQuery.TableLabel.Groggy, new GroggyDebuff());
        }

        #endregion

        #region <Classess>
        
        /// <summary>
        /// 저주 디버프
        /// </summary>
        public class CurseDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as CurseDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Curse]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.CURSED);
                        handler.AddAdditiveStatus(-record.AdditiveBattleStatus);
                        handler.AddSimpleMultiplyStatus(-record.SimpleMultiplyBattleStatus);
                        handler.AddCompoundMultiplyStatus(-record.CompoundMultiplyBattleStatus);

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        public class ShockDebuff : GameEntityEnchantEventBase
        {
            private const int _StackUpperBound = 1;
            private const float _Interval = 1f;
            
            public override bool IsCastable(IGameEntityEnchantEventContainer p_Container)
            {
                var stackCount = p_Container.Caster.GetStateStackCount(GameEntityTool.EntityStateType.SHOCK);
                return stackCount < _StackUpperBound;
            }

            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as ShockDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Shock]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.SHOCK);
                        // handler.AddCompoundMultiplyStatus(new BattleStatusPreset(BattleStatusTool.BattleStatusType.AttackSpeedRate, -record.Value2));
                        handler.AddTickEvent
                        (
                            _Interval,
                            (handler, param) =>
                            {
                                if (handler.Container.TryGetRecord(out ShockDebuffDataTable.TableRecord o_Record))
                                {
                                    var caster = handler.Caster;
                                    var trigger = handler.CommonParams.Trigger;
                                    // var damage = Mathf.Max(1, o_Record.Value * trigger[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));
                                    // caster.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Shocking, trigger));
                                }
                            },
                            true
                        );
                        
                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// 기절 디버프
        /// </summary>
        public class StunDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as StunDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Stun]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.STUN);

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// 출혈 디버프
        /// </summary>
        public class BleedDebuff : GameEntityEnchantEventBase
        {
            private const int _StackUpperBound = 4;
            private const float _Interval = 1f;
            
            public override bool IsCastable(IGameEntityEnchantEventContainer p_Container)
            {
                var stackCount = p_Container.Caster.GetStateStackCount(GameEntityTool.EntityStateType.BLEED);
                return stackCount < _StackUpperBound;
            }

            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as BleedDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Bleed]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.BLEED);
                        // handler.AddCompoundMultiplyStatus(new BattleStatusPreset(BattleStatusTool.BattleStatusType.AntiDamageRate, -record.Value2));
                        handler.AddTickEvent
                        (
                            _Interval,
                            (handler, param) =>
                            {
                                if (handler.Container.TryGetRecord(out BleedDebuffDataTable.TableRecord o_Record))
                                {
                                    var caster = handler.Caster;
                                    var trigger = handler.CommonParams.Trigger;
                                    // var damage = Mathf.Max(1, o_Record.Value * trigger[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));
                                    // caster.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Bleeding, trigger));
                                }
                            },
                            true
                        );

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
                
        /// <summary>
        /// 중독 디버프
        /// </summary>
        public class PoisonDebuff : GameEntityEnchantEventBase
        {
            private const int _StackUpperBound = 2;
            private const float _Interval = 0.5f;
            
            public override bool IsCastable(IGameEntityEnchantEventContainer p_Container)
            {
                var stackCount = p_Container.Caster.GetStateStackCount(GameEntityTool.EntityStateType.POISON);
                return stackCount < _StackUpperBound;
            }

            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as PoisonDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Poison]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.POISON);
                        handler.AddTickEvent
                        (
                            _Interval,
                            (handler, param) =>
                            {
                                if (handler.Container.TryGetRecord(out PoisonDebuffDataTable.TableRecord o_Record))
                                {
                                    var caster = handler.Caster;
                                    var trigger = handler.CommonParams.Trigger;
                                    // var damage = Mathf.Max(1, o_Record.Value * trigger[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));
                                    // caster.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Poisoning, trigger));
                                }
                            },
                            true
                        );

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
          
        /// <summary>
        /// 화상 디버프
        /// </summary>
        public class BurnDebuff : GameEntityEnchantEventBase
        {
            private const int _StackUpperBound = 2;
            private const float _Interval = 1f;
            
            public override bool IsCastable(IGameEntityEnchantEventContainer p_Container)
            {
                var stackCount = p_Container.Caster.GetStateStackCount(GameEntityTool.EntityStateType.BURN);
                return stackCount < _StackUpperBound;
            }

            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as BurnDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Burn]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.BURN);
                        // handler.AddCompoundMultiplyStatus(new BattleStatusPreset(BattleStatusTool.BattleStatusType.DamageRate, -record.Value2));
                        handler.AddTickEvent
                        (
                            _Interval,
                            (handler, param) =>
                            {
                                if (handler.Container.TryGetRecord(out BurnDebuffDataTable.TableRecord o_Record))
                                {
                                    var caster = handler.Caster;
                                    var trigger = handler.CommonParams.Trigger;
                           //         var damage = Mathf.Max(1, o_Record.Value * trigger[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));
                                    
                             //       caster.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Burning, trigger));
                                }
                            },
                            true
                        );

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
                        
        /// <summary>
        /// 오한 디버프
        /// </summary>
        public class ChillDebuff : GameEntityEnchantEventBase
        {
            private const int _StackUpperBound = 2;
            private const float _Interval = 1f;
            
            public override bool IsCastable(IGameEntityEnchantEventContainer p_Container)
            {
                var stackCount = p_Container.Caster.GetStateStackCount(GameEntityTool.EntityStateType.CHILL);
                return stackCount < _StackUpperBound;
            }

            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as ChillDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Chill]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.CHILL);
                //        handler.AddCompoundMultiplyStatus(new BattleStatusPreset(BattleStatusTool.BattleStatusType.MoveSpeedRate, -record.Value2));
                        handler.AddTickEvent
                        (
                            _Interval,
                            (handler, param) =>
                            {
                                if (handler.Container.TryGetRecord(out ChillDebuffDataTable.TableRecord o_Record))
                                {
                                    var caster = handler.Caster;
                                    var trigger = handler.CommonParams.Trigger;
                                  //  var damage = Mathf.Max(1, o_Record.Value * trigger[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee));

                                   // caster.GiveDamage(damage, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.Chilling, trigger));
                                }
                            },
                            true
                        );

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
                
        /// <summary>
        /// 빙결 디버프
        /// </summary>
        public class FreezeDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as FreezeDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Freeze]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.FREEZE);

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// 혼란 디버프
        /// </summary>
        public class ConfuseDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as ConfuseDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Confuse]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.CONFUSE);

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// 실명 디버프
        /// </summary>
        public class BlindDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as BlindDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Blind]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.BLIND);
                      //  handler.AddCompoundMultiplyStatus(new BattleStatusPreset(BattleStatusTool.BattleStatusType.SightRange, -record.Value));

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 침묵 디버프
        /// </summary>
        public class SilenceDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as SilenceDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Silence]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.SILENCE);

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// 구속 디버프
        /// </summary>
        public class BindDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as BindDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Bind]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.BIND);

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        /// <summary>
        /// 그로기 디버프
        /// </summary>
        public class GroggyDebuff : GameEntityEnchantEventBase
        {
            public override bool CastEnchant(IGameEntityEnchantEventContainer p_Container)
            {
                var record = p_Container.Record as GroggyDebuffDataTable.TableRecord;
                if (ReferenceEquals(null, record))
                {
                    return false;
                }
                else
                {
                    var resistRate = 
                        p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Groggy]
                        + p_Container.Caster[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ResistRate_Debuff];
                    
                    if (resistRate < 1f)
                    {
                        var duration = (1f - resistRate) * record.Duration;
                        var handler = p_Container.AddDurationHandler(duration);
                        handler.AddState(GameEntityTool.EntityStateType.GROGGY);
                      //  handler.AddAdditiveStatus(new BattleStatusPreset(BattleStatusTool.BattleStatusType.AntiDamageRate, -record.Value));

                        return true;
                    }
                    // 면역
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        #endregion
    }
}