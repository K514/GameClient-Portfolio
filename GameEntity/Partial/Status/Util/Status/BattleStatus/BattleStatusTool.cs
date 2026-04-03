using System;
using System.Collections.Generic;
using System.Linq;
using k514.Mono.Feature;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public static class BattleStatusTool
    {
        #region <Consts>

        public const float DefaultCriticalDamageRate = 1.5f;
        public const float AntiDamageRateUpperBound = 0.9f;
        public const float PropertyLowerBound = CustomMath.Epsilon;
        public const float PropertyLowerBoundInv = 1f / PropertyLowerBound;
        public static readonly BattleStatusPreset BasisBattleStatusPreset;
        public static readonly BattleStatusPreset BasisInvBattleStatusPreset;
        public static readonly BattleStatusPreset BasisSqrBattleStatusPreset;
        public static readonly BattleStatusPreset EinBattleStatusPreset;
        public static readonly BattleStatusPreset LevelUpBonusBattleStatusPreset;

        #endregion
        
        #region <Enums>

        /// <summary>
        /// 프로퍼티 Read 전용 타입
        /// </summary>
        public enum BattleStatusType
        {
            None,
            
            Attack_Melee,
            Attack_Spell,
            
            CriticalRate_Melee,
            CriticalRate_Spell,
            
            DamageRate,
            DamageRate_Melee,
            DamageRate_Spell,
            DamageRate_Critical,
            DamageRate_Pierce,
            DamageRate_Boss,
            DamageRate_Curse,
            DamageRate_Shock,
            DamageRate_Stun,
            DamageRate_Bleed,
            DamageRate_Poison,
            DamageRate_Burn,
            DamageRate_Chill,
            DamageRate_Freeze,
            DamageRate_Confuse,
            DamageRate_Blind,
            DamageRate_Silence,
            DamageRate_Bind,
            DamageRate_Groggy,
            DamageRate_Debuff,
            
            ElementEnhance_Fire,
            ElementEnhance_Water,
            ElementEnhance_Ground,
            ElementEnhance_Wind,
            ElementEnhance_Light,
            ElementEnhance_Darkness,
            ElementEnhance_All,

            Defense_Melee,
            Defense_Spell,
            
            AntiCriticalRate_Melee,
            AntiCriticalRate_Spell,
            
            AntiDamageRate,
            AntiDamageRate_Melee,
            AntiDamageRate_Spell,
            AntiDamageRate_Critical,

            ResistRate_Curse,
            ResistRate_Shock,
            ResistRate_Stun,
            ResistRate_Bleed,
            ResistRate_Poison,
            ResistRate_Burn,
            ResistRate_Chill,
            ResistRate_Freeze,
            ResistRate_Confuse,
            ResistRate_Blind,
            ResistRate_Silence,
            ResistRate_Bind,
            ResistRate_Groggy,
            ResistRate_Debuff,
            
            ElementRegist_Fire,
            ElementRegist_Water,
            ElementRegist_Ground,
            ElementRegist_Wind,
            ElementRegist_Light,
            ElementRegist_Darkness,
            ElementRegist_All,

            HP_Base, 
            MP_Base, 
            HP_Fix_Recovery,
            MP_Fix_Recovery,
            HP_Rate_Recovery,
            MP_Rate_Recovery,
            
            AttackSpeedBasis,
            SpellCastSpeedBasis,
            MoveSpeedBasis,

            AttackSpeedRate,
            SpellCastSpeedRate,
            MoveSpeedRate,

            JumpForce,
            JumpCount,
            SightRange,
            Absorb,
               
            HitRate,
            DodgeRate,
            CostRate,
            CooldownRate,
            CooldownRecoverySpeedRate,
            HitMotionRecoverySpeedRate,
            
            ExpChanceExtraRate,
            GoldChanceExtraRate,
            ItemChanceExtraRate,
        }
        
        public static BattleStatusType[] BattleStatusTypeEnumerator;

        [Flags]
        public enum BattleStatusType1
        {
            None = 0,
            
            Attack_Melee = 1 << 0,
            Attack_Spell = 1 << 1,
            
            CriticalRate_Melee = 1 << 2,
            CriticalRate_Spell = 1 << 3,
            
            DamageRate = 1 << 4,
            DamageRate_Melee = 1 << 5,
            DamageRate_Spell = 1 << 6,
            DamageRate_Critical = 1 << 7,
            
            DamageRate_Pierce = 1 << 8,
            DamageRate_Boss = 1 << 9,
            DamageRate_Curse = 1 << 10,
            DamageRate_Shock = 1 << 11,
            DamageRate_Stun = 1 << 12,
            DamageRate_Bleed = 1 << 13,
            DamageRate_Poison = 1 << 14,
            DamageRate_Burn = 1 << 15,
            DamageRate_Chill = 1 << 16,
            DamageRate_Freeze = 1 << 17,
            DamageRate_Confuse = 1 << 18,
            DamageRate_Blind = 1 << 19,
            DamageRate_Silence = 1 << 20,
            DamageRate_Bind = 1 << 21,
            DamageRate_Groggy = 1 << 22,
            DamageRate_Debuff = 1 << 23,
            
            ElementEnhance_Fire = 1 << 25,
            ElementEnhance_Water = 1 << 26,
            ElementEnhance_Ground = 1 << 27,
            ElementEnhance_Wind = 1 << 28,
            ElementEnhance_Light = 1 << 29,
            ElementEnhance_Darkness = 1 << 30,
            ElementEnhance_All = 1 << 31,
        }

        public static BattleStatusType1[] BattleStatusType1Enumerator;

        [Flags]
        public enum BattleStatusType2
        {
            None = 0,
            
            Defense_Melee = 1 << 0,
            Defense_Spell = 1 << 1,
            
            AntiCriticalRate_Melee = 1 << 2,
            AntiCriticalRate_Spell = 1 << 3,
            
            AntiDamageRate = 1 << 4,
            AntiDamageRate_Melee = 1 << 5,
            AntiDamageRate_Spell = 1 << 6,
            AntiDamageRate_Critical = 1 << 7,
            
            ResistRate_Curse = 1 << 8,
            ResistRate_Shock = 1 << 9,
            ResistRate_Stun = 1 << 10,
            ResistRate_Bleed = 1 << 11,
            ResistRate_Poison = 1 << 12,
            ResistRate_Burn = 1 << 13,
            ResistRate_Chill = 1 << 14,
            ResistRate_Freeze = 1 << 15,
            ResistRate_Confuse = 1 << 16,
            ResistRate_Blind = 1 << 17,
            ResistRate_Silence = 1 << 18,
            ResistRate_Bind = 1 << 19,
            ResistRate_Groggy = 1 << 20,
            ResistRate_Debuff = 1 << 21,
            
            ElementRegist_Fire = 1 << 25,
            ElementRegist_Water = 1 << 26,
            ElementRegist_Ground = 1 << 27,
            ElementRegist_Wind = 1 << 28,
            ElementRegist_Light = 1 << 29,
            ElementRegist_Darkness = 1 << 30,
            ElementRegist_All = 1 << 31,
        }
        
        public static BattleStatusType2[] BattleStatusType2Enumerator;

        [Flags]
        public enum BattleStatusType3
        {
            None = 0,
            
            HP_Base = 1 << 0, 
            MP_Base = 1 << 1, 
            HP_Fix_Recovery = 1 << 2, 
            MP_Fix_Recovery = 1 << 3, 
            HP_Rate_Recovery = 1 << 4, 
            MP_Rate_Recovery = 1 << 5,           
            
            AttackSpeedBasis = 1 << 6,
            SpellCastSpeedBasis = 1 << 7,
            MoveSpeedBasis = 1 << 8,
            
            AttackSpeedRate = 1 << 9,
            SpellCastSpeedRate = 1 << 10,
            MoveSpeedRate = 1 << 11,
            
            JumpForce = 1 << 12,
            JumpCount = 1 << 13,
            SightRange = 1 << 14,
            Absorb = 1 << 15,
            
            HitRate = 1 << 16,
            DodgeRate = 1 << 17,
            CostRate = 1 << 18,
            CooldownRate = 1 << 19,
            CooldownRecoverySpeedRate = 1 << 20,
            HitMotionRecoverySpeedRate = 1 << 21,
            
            ExpChanceExtraRate = 1 << 22,
            GoldChanceExtraRate = 1 << 23,
            ItemChanceExtraRate = 1 << 24,
        }
        
        public static BattleStatusType3[] BattleStatusType3Enumerator;

        #endregion

        #region <Constructor>

        static BattleStatusTool()
        {
            BattleStatusTypeEnumerator = EnumFlag.GetEnumEnumerator<BattleStatusType>(EnumFlag.GetEnumeratorType.ExceptNone);
            BattleStatusType1Enumerator = EnumFlag.GetEnumEnumerator<BattleStatusType1>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            BattleStatusType2Enumerator = EnumFlag.GetEnumEnumerator<BattleStatusType2>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            BattleStatusType3Enumerator = EnumFlag.GetEnumEnumerator<BattleStatusType3>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
  
            var defaultMetaValueSet = new Dictionary<BattleStatusType, float>();
            foreach (var battleStatusType in BattleStatusTypeEnumerator)
            {
                switch (battleStatusType)
                {
                    default:
                    case BattleStatusType.None:
                        break;
                    case BattleStatusType.Attack_Melee:
                    case BattleStatusType.Attack_Spell:
                    case BattleStatusType.CriticalRate_Melee:
                    case BattleStatusType.CriticalRate_Spell:
                    case BattleStatusType.ElementEnhance_Fire:
                    case BattleStatusType.ElementEnhance_Water:
                    case BattleStatusType.ElementEnhance_Ground:
                    case BattleStatusType.ElementEnhance_Wind:
                    case BattleStatusType.ElementEnhance_Light:
                    case BattleStatusType.ElementEnhance_Darkness:
                    case BattleStatusType.ElementEnhance_All:
                    case BattleStatusType.Defense_Melee:
                    case BattleStatusType.Defense_Spell:
                    case BattleStatusType.AntiCriticalRate_Melee:
                    case BattleStatusType.AntiCriticalRate_Spell:
                    case BattleStatusType.AntiDamageRate:
                    case BattleStatusType.AntiDamageRate_Melee:
                    case BattleStatusType.AntiDamageRate_Spell:
                    case BattleStatusType.AntiDamageRate_Critical:
                    case BattleStatusType.ResistRate_Curse:
                    case BattleStatusType.ResistRate_Shock:
                    case BattleStatusType.ResistRate_Stun:
                    case BattleStatusType.ResistRate_Bleed:
                    case BattleStatusType.ResistRate_Poison:
                    case BattleStatusType.ResistRate_Burn:
                    case BattleStatusType.ResistRate_Chill:
                    case BattleStatusType.ResistRate_Freeze:
                    case BattleStatusType.ResistRate_Confuse:
                    case BattleStatusType.ResistRate_Blind:
                    case BattleStatusType.ResistRate_Silence:
                    case BattleStatusType.ResistRate_Bind:
                    case BattleStatusType.ResistRate_Groggy:
                    case BattleStatusType.ResistRate_Debuff:
                    case BattleStatusType.ElementRegist_Fire:
                    case BattleStatusType.ElementRegist_Water:
                    case BattleStatusType.ElementRegist_Ground:
                    case BattleStatusType.ElementRegist_Wind:
                    case BattleStatusType.ElementRegist_Light:
                    case BattleStatusType.ElementRegist_Darkness:
                    case BattleStatusType.ElementRegist_All:
                    case BattleStatusType.HP_Base:
                    case BattleStatusType.MP_Base:
                    case BattleStatusType.HP_Fix_Recovery:
                    case BattleStatusType.MP_Fix_Recovery:
                    case BattleStatusType.HP_Rate_Recovery:   
                    case BattleStatusType.MP_Rate_Recovery:   
                    case BattleStatusType.AttackSpeedBasis:
                    case BattleStatusType.SpellCastSpeedBasis:
                    case BattleStatusType.MoveSpeedBasis:
                    case BattleStatusType.JumpForce:
                    case BattleStatusType.JumpCount:
                    case BattleStatusType.SightRange:
                    case BattleStatusType.Absorb:
                        defaultMetaValueSet.Add(battleStatusType, 0f);
                        break;
                    case BattleStatusType.DamageRate:
                    case BattleStatusType.DamageRate_Melee:
                    case BattleStatusType.DamageRate_Spell:
                    case BattleStatusType.DamageRate_Pierce:
                    case BattleStatusType.DamageRate_Boss:
                    case BattleStatusType.DamageRate_Curse:
                    case BattleStatusType.DamageRate_Shock:
                    case BattleStatusType.DamageRate_Stun:
                    case BattleStatusType.DamageRate_Bleed:
                    case BattleStatusType.DamageRate_Poison:
                    case BattleStatusType.DamageRate_Burn:
                    case BattleStatusType.DamageRate_Chill:
                    case BattleStatusType.DamageRate_Freeze:
                    case BattleStatusType.DamageRate_Confuse:
                    case BattleStatusType.DamageRate_Blind:
                    case BattleStatusType.DamageRate_Silence:
                    case BattleStatusType.DamageRate_Bind:
                    case BattleStatusType.DamageRate_Groggy:
                    case BattleStatusType.DamageRate_Debuff:
                    case BattleStatusType.AttackSpeedRate:
                    case BattleStatusType.SpellCastSpeedRate:
                    case BattleStatusType.MoveSpeedRate:
                    case BattleStatusType.HitRate:
                    case BattleStatusType.DodgeRate:
                    case BattleStatusType.CostRate:
                    case BattleStatusType.CooldownRate:
                    case BattleStatusType.CooldownRecoverySpeedRate:
                    case BattleStatusType.HitMotionRecoverySpeedRate:
                    case BattleStatusType.ExpChanceExtraRate:
                    case BattleStatusType.GoldChanceExtraRate:
                    case BattleStatusType.ItemChanceExtraRate:
                        defaultMetaValueSet.Add(battleStatusType, 1f);
                        break;
                    case BattleStatusType.DamageRate_Critical:
                        defaultMetaValueSet.Add(battleStatusType, DefaultCriticalDamageRate);
                        break;
                }
            }

            BasisBattleStatusPreset = new BattleStatusPreset(defaultMetaValueSet);
            BasisInvBattleStatusPreset = 1f / BasisBattleStatusPreset;
            BasisSqrBattleStatusPreset = BasisBattleStatusPreset * BasisBattleStatusPreset;
            EinBattleStatusPreset = 1f;
            LevelUpBonusBattleStatusPreset = new BattleStatusPreset();
            LevelUpBonusBattleStatusPreset.SetProperty(BattleStatusTool.BattleStatusType.HP_Base, 1f);
            LevelUpBonusBattleStatusPreset.SetProperty(BattleStatusTool.BattleStatusType.MP_Base, 1f);
            LevelUpBonusBattleStatusPreset.SetProperty(BattleStatusTool.BattleStatusType.Attack_Melee, 1f);
        }

        #endregion

        #region <Methods>
        
        public static StatusTool.PropertyValueType GetPropertyValueType(this BattleStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType.None:
                    return StatusTool.PropertyValueType.None;
                case BattleStatusType.Attack_Melee:
                case BattleStatusType.Attack_Spell:
                case BattleStatusType.ElementEnhance_Fire:
                case BattleStatusType.ElementEnhance_Water:
                case BattleStatusType.ElementEnhance_Ground:
                case BattleStatusType.ElementEnhance_Wind:
                case BattleStatusType.ElementEnhance_Light:
                case BattleStatusType.ElementEnhance_Darkness:
                case BattleStatusType.ElementEnhance_All:
                case BattleStatusType.Defense_Melee:
                case BattleStatusType.Defense_Spell:
                case BattleStatusType.ElementRegist_Fire:
                case BattleStatusType.ElementRegist_Water:
                case BattleStatusType.ElementRegist_Ground:
                case BattleStatusType.ElementRegist_Wind:
                case BattleStatusType.ElementRegist_Light:
                case BattleStatusType.ElementRegist_Darkness:
                case BattleStatusType.ElementRegist_All:
                case BattleStatusType.HP_Base:
                case BattleStatusType.MP_Base:
                case BattleStatusType.HP_Fix_Recovery:
                case BattleStatusType.MP_Fix_Recovery:
                case BattleStatusType.AttackSpeedBasis:
                case BattleStatusType.SpellCastSpeedBasis:
                case BattleStatusType.MoveSpeedBasis:
                case BattleStatusType.JumpForce:
                case BattleStatusType.JumpCount:
                case BattleStatusType.SightRange:
                case BattleStatusType.Absorb:
                    return StatusTool.PropertyValueType.FixedValue;
                case BattleStatusType.CriticalRate_Melee:
                case BattleStatusType.CriticalRate_Spell:
                case BattleStatusType.DamageRate:
                case BattleStatusType.DamageRate_Melee:
                case BattleStatusType.DamageRate_Spell:
                case BattleStatusType.DamageRate_Critical:
                case BattleStatusType.DamageRate_Pierce:
                case BattleStatusType.DamageRate_Curse:
                case BattleStatusType.DamageRate_Shock:
                case BattleStatusType.DamageRate_Stun:
                case BattleStatusType.DamageRate_Bleed:
                case BattleStatusType.DamageRate_Poison:
                case BattleStatusType.DamageRate_Burn:
                case BattleStatusType.DamageRate_Chill:
                case BattleStatusType.DamageRate_Freeze:
                case BattleStatusType.DamageRate_Confuse:
                case BattleStatusType.DamageRate_Blind:
                case BattleStatusType.DamageRate_Silence:
                case BattleStatusType.DamageRate_Bind:
                case BattleStatusType.DamageRate_Groggy:
                case BattleStatusType.DamageRate_Debuff:
                case BattleStatusType.AntiCriticalRate_Melee:
                case BattleStatusType.AntiCriticalRate_Spell:
                case BattleStatusType.AntiDamageRate:
                case BattleStatusType.AntiDamageRate_Melee:
                case BattleStatusType.AntiDamageRate_Spell:
                case BattleStatusType.AntiDamageRate_Critical:
                case BattleStatusType.ResistRate_Curse:
                case BattleStatusType.ResistRate_Shock:
                case BattleStatusType.ResistRate_Stun:
                case BattleStatusType.ResistRate_Bleed:
                case BattleStatusType.ResistRate_Poison:
                case BattleStatusType.ResistRate_Burn:
                case BattleStatusType.ResistRate_Chill:
                case BattleStatusType.ResistRate_Freeze:
                case BattleStatusType.ResistRate_Confuse:
                case BattleStatusType.ResistRate_Blind:
                case BattleStatusType.ResistRate_Silence:
                case BattleStatusType.ResistRate_Bind:
                case BattleStatusType.ResistRate_Groggy:
                case BattleStatusType.ResistRate_Debuff: 
                case BattleStatusType.HP_Rate_Recovery:
                case BattleStatusType.MP_Rate_Recovery:
                case BattleStatusType.AttackSpeedRate:
                case BattleStatusType.SpellCastSpeedRate:
                case BattleStatusType.MoveSpeedRate:
                case BattleStatusType.HitRate:
                case BattleStatusType.DodgeRate:
                case BattleStatusType.CostRate:
                case BattleStatusType.CooldownRecoverySpeedRate:
                case BattleStatusType.HitMotionRecoverySpeedRate:
                case BattleStatusType.ExpChanceExtraRate:
                case BattleStatusType.GoldChanceExtraRate:
                case BattleStatusType.ItemChanceExtraRate:
                    return StatusTool.PropertyValueType.SimpleRateValue;
                case BattleStatusType.CooldownRate:
                    return StatusTool.PropertyValueType.CompoundRateValue;
            }
        }
        
        public static StatusTool.PropertyValueType GetPropertyValueType(this BattleStatusType1 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType1.None:
                    return StatusTool.PropertyValueType.None;
                case BattleStatusType1.Attack_Melee:
                case BattleStatusType1.Attack_Spell:
                case BattleStatusType1.ElementEnhance_Fire:
                case BattleStatusType1.ElementEnhance_Water:
                case BattleStatusType1.ElementEnhance_Ground:
                case BattleStatusType1.ElementEnhance_Wind:
                case BattleStatusType1.ElementEnhance_Light:
                case BattleStatusType1.ElementEnhance_Darkness:
                case BattleStatusType1.ElementEnhance_All:
                    return StatusTool.PropertyValueType.FixedValue;
                case BattleStatusType1.CriticalRate_Melee:
                case BattleStatusType1.CriticalRate_Spell:
                case BattleStatusType1.DamageRate:
                case BattleStatusType1.DamageRate_Melee:
                case BattleStatusType1.DamageRate_Spell:
                case BattleStatusType1.DamageRate_Critical:
                case BattleStatusType1.DamageRate_Pierce:
                case BattleStatusType1.DamageRate_Boss:
                case BattleStatusType1.DamageRate_Curse:
                case BattleStatusType1.DamageRate_Shock:
                case BattleStatusType1.DamageRate_Stun:
                case BattleStatusType1.DamageRate_Bleed:
                case BattleStatusType1.DamageRate_Poison:
                case BattleStatusType1.DamageRate_Burn:
                case BattleStatusType1.DamageRate_Chill:
                case BattleStatusType1.DamageRate_Freeze:
                case BattleStatusType1.DamageRate_Confuse:
                case BattleStatusType1.DamageRate_Blind:
                case BattleStatusType1.DamageRate_Silence:
                case BattleStatusType1.DamageRate_Bind:
                case BattleStatusType1.DamageRate_Groggy:
                case BattleStatusType1.DamageRate_Debuff:
                    return StatusTool.PropertyValueType.SimpleRateValue;
            }
        }
        
        public static StatusTool.PropertyValueType GetPropertyValueType(this BattleStatusType2 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType2.None:
                    return StatusTool.PropertyValueType.None;
                case BattleStatusType2.Defense_Melee:
                case BattleStatusType2.Defense_Spell:
                case BattleStatusType2.ElementRegist_Fire:
                case BattleStatusType2.ElementRegist_Water:
                case BattleStatusType2.ElementRegist_Ground:
                case BattleStatusType2.ElementRegist_Wind:
                case BattleStatusType2.ElementRegist_Light:
                case BattleStatusType2.ElementRegist_Darkness:
                case BattleStatusType2.ElementRegist_All:
                    return StatusTool.PropertyValueType.FixedValue;
                case BattleStatusType2.AntiCriticalRate_Melee:
                case BattleStatusType2.AntiCriticalRate_Spell:
                case BattleStatusType2.AntiDamageRate:
                case BattleStatusType2.AntiDamageRate_Melee:
                case BattleStatusType2.AntiDamageRate_Spell:
                case BattleStatusType2.AntiDamageRate_Critical:
                case BattleStatusType2.ResistRate_Curse:
                case BattleStatusType2.ResistRate_Shock:
                case BattleStatusType2.ResistRate_Stun:
                case BattleStatusType2.ResistRate_Bleed:
                case BattleStatusType2.ResistRate_Poison:
                case BattleStatusType2.ResistRate_Burn:
                case BattleStatusType2.ResistRate_Chill:
                case BattleStatusType2.ResistRate_Freeze:
                case BattleStatusType2.ResistRate_Confuse:
                case BattleStatusType2.ResistRate_Blind:
                case BattleStatusType2.ResistRate_Silence:
                case BattleStatusType2.ResistRate_Bind:
                case BattleStatusType2.ResistRate_Groggy:
                case BattleStatusType2.ResistRate_Debuff:
                    return StatusTool.PropertyValueType.SimpleRateValue;
            }
        }
        
        public static StatusTool.PropertyValueType GetPropertyValueType(this BattleStatusType3 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType3.None:
                    return StatusTool.PropertyValueType.None;
                case BattleStatusType3.HP_Base:
                case BattleStatusType3.MP_Base:
                case BattleStatusType3.HP_Fix_Recovery:
                case BattleStatusType3.MP_Fix_Recovery:
                case BattleStatusType3.AttackSpeedBasis:
                case BattleStatusType3.SpellCastSpeedBasis:
                case BattleStatusType3.MoveSpeedBasis:
                case BattleStatusType3.JumpForce:
                case BattleStatusType3.JumpCount:
                case BattleStatusType3.SightRange:
                case BattleStatusType3.Absorb:
                    return StatusTool.PropertyValueType.FixedValue;
                case BattleStatusType3.HP_Rate_Recovery:
                case BattleStatusType3.MP_Rate_Recovery:
                case BattleStatusType3.AttackSpeedRate:
                case BattleStatusType3.SpellCastSpeedRate:
                case BattleStatusType3.MoveSpeedRate:
                case BattleStatusType3.HitRate:
                case BattleStatusType3.DodgeRate:
                case BattleStatusType3.CostRate:
                case BattleStatusType3.CooldownRecoverySpeedRate:
                case BattleStatusType3.HitMotionRecoverySpeedRate:
                case BattleStatusType3.ExpChanceExtraRate:
                case BattleStatusType3.GoldChanceExtraRate:
                case BattleStatusType3.ItemChanceExtraRate:
                    return StatusTool.PropertyValueType.SimpleRateValue;
                case BattleStatusType3.CooldownRate:
                    return StatusTool.PropertyValueType.CompoundRateValue;
            }
        }
        
        public static GameContentsLanguageDataTable.TableRecord GetLanguage(this BattleStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType.None:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(140000);
                case BattleStatusType.Attack_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145100);
                case BattleStatusType.Attack_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145101);
                case BattleStatusType.CriticalRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145102);
                case BattleStatusType.CriticalRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145103);
                case BattleStatusType.DamageRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145104);
                case BattleStatusType.DamageRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145105);
                case BattleStatusType.DamageRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145106);
                case BattleStatusType.DamageRate_Critical:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145107);
                case BattleStatusType.DamageRate_Pierce:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145108);
                case BattleStatusType.DamageRate_Boss:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145109);
                case BattleStatusType.DamageRate_Curse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145110);
                case BattleStatusType.DamageRate_Shock:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145111);
                case BattleStatusType.DamageRate_Stun:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145112);
                case BattleStatusType.DamageRate_Bleed:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145113);
                case BattleStatusType.DamageRate_Poison:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145114);
                case BattleStatusType.DamageRate_Burn:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145115);
                case BattleStatusType.DamageRate_Chill:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145116);
                case BattleStatusType.DamageRate_Freeze:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145117);
                case BattleStatusType.DamageRate_Confuse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145118);
                case BattleStatusType.DamageRate_Blind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145119);
                case BattleStatusType.DamageRate_Silence:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145120);
                case BattleStatusType.DamageRate_Bind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145121);
                case BattleStatusType.DamageRate_Groggy:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145122);
                case BattleStatusType.DamageRate_Debuff:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145123);
                case BattleStatusType.ElementEnhance_Fire:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145125);
                case BattleStatusType.ElementEnhance_Water:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145126);
                case BattleStatusType.ElementEnhance_Ground:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145127);
                case BattleStatusType.ElementEnhance_Wind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145128);
                case BattleStatusType.ElementEnhance_Light:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145129);
                case BattleStatusType.ElementEnhance_Darkness:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145130);
                case BattleStatusType.ElementEnhance_All:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145131);
                case BattleStatusType.Defense_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145200);
                case BattleStatusType.Defense_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145201);
                case BattleStatusType.AntiCriticalRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145202);
                case BattleStatusType.AntiCriticalRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145203);
                case BattleStatusType.AntiDamageRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145204);
                case BattleStatusType.AntiDamageRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145205);
                case BattleStatusType.AntiDamageRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145206);
                case BattleStatusType.AntiDamageRate_Critical:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145207);
                case BattleStatusType.ResistRate_Curse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145208);
                case BattleStatusType.ResistRate_Shock:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145209);
                case BattleStatusType.ResistRate_Stun:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145210);
                case BattleStatusType.ResistRate_Bleed:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145211);
                case BattleStatusType.ResistRate_Poison:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145212);
                case BattleStatusType.ResistRate_Burn:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145213);
                case BattleStatusType.ResistRate_Chill:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145214);
                case BattleStatusType.ResistRate_Freeze:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145215);
                case BattleStatusType.ResistRate_Confuse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145216);
                case BattleStatusType.ResistRate_Blind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145217);
                case BattleStatusType.ResistRate_Silence:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145218);
                case BattleStatusType.ResistRate_Bind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145219);
                case BattleStatusType.ResistRate_Groggy:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145220);
                case BattleStatusType.ResistRate_Debuff:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145221);
                case BattleStatusType.ElementRegist_Fire:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145225);
                case BattleStatusType.ElementRegist_Water:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145226);
                case BattleStatusType.ElementRegist_Ground:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145227);
                case BattleStatusType.ElementRegist_Wind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145228);
                case BattleStatusType.ElementRegist_Light:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145229);
                case BattleStatusType.ElementRegist_Darkness:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145230);
                case BattleStatusType.ElementRegist_All:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145231);
                case BattleStatusType.HP_Base:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145300);
                case BattleStatusType.MP_Base:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145301);
                case BattleStatusType.HP_Fix_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145302);
                case BattleStatusType.MP_Fix_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145303);
                case BattleStatusType.HP_Rate_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145304);
                case BattleStatusType.MP_Rate_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145305);
                case BattleStatusType.AttackSpeedBasis:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145306);
                case BattleStatusType.SpellCastSpeedBasis:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145307);
                case BattleStatusType.MoveSpeedBasis:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145308);
                case BattleStatusType.AttackSpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145309);
                case BattleStatusType.SpellCastSpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145310);
                case BattleStatusType.MoveSpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145311);
                case BattleStatusType.JumpForce:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145312);
                case BattleStatusType.JumpCount:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145313);
                case BattleStatusType.SightRange:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145314);
                case BattleStatusType.Absorb:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145315);
                case BattleStatusType.HitRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145316);
                case BattleStatusType.DodgeRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145317);
                case BattleStatusType.CostRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145318);
                case BattleStatusType.CooldownRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145319);
                case BattleStatusType.CooldownRecoverySpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145320);
                case BattleStatusType.HitMotionRecoverySpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145321);
                case BattleStatusType.ExpChanceExtraRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145322);
                case BattleStatusType.GoldChanceExtraRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145323);
                case BattleStatusType.ItemChanceExtraRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145324);
            }
        }
        
        public static string GetPropertyName(this BattleStatusType p_Type)
        {
#if UNITY_EDITOR
            var tryLang = GetLanguage(p_Type);
            if (ReferenceEquals(null, tryLang))
            {
                CustomDebug.LogError(($"BattleStatusType [{p_Type}] Language Load Fail!", UnityEngine.Color.red));
                return string.Empty;
            }
            else
            {
                return GetLanguage(p_Type).Text;
            }
#else
            return GetLanguage(p_Type).Text;
#endif
        }
        
        public static GameContentsLanguageDataTable.TableRecord GetLanguage(this BattleStatusType1 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType1.None:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(140000);
                case BattleStatusType1.Attack_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145100);
                case BattleStatusType1.Attack_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145101);
                case BattleStatusType1.CriticalRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145102);
                case BattleStatusType1.CriticalRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145103);
                case BattleStatusType1.DamageRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145104);
                case BattleStatusType1.DamageRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145105);
                case BattleStatusType1.DamageRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145106);
                case BattleStatusType1.DamageRate_Critical:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145107);
                case BattleStatusType1.DamageRate_Pierce:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145108);
                case BattleStatusType1.DamageRate_Boss:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145109);
                case BattleStatusType1.DamageRate_Curse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145110);
                case BattleStatusType1.DamageRate_Shock:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145111);
                case BattleStatusType1.DamageRate_Stun:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145112);
                case BattleStatusType1.DamageRate_Bleed:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145113);
                case BattleStatusType1.DamageRate_Poison:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145114);
                case BattleStatusType1.DamageRate_Burn:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145115);
                case BattleStatusType1.DamageRate_Chill:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145116);
                case BattleStatusType1.DamageRate_Freeze:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145117);
                case BattleStatusType1.DamageRate_Confuse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145118);
                case BattleStatusType1.DamageRate_Blind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145119);
                case BattleStatusType1.DamageRate_Silence:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145120);
                case BattleStatusType1.DamageRate_Bind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145121);
                case BattleStatusType1.DamageRate_Groggy:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145122);
                case BattleStatusType1.DamageRate_Debuff:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145123);
                case BattleStatusType1.ElementEnhance_Fire:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145125);
                case BattleStatusType1.ElementEnhance_Water:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145126);
                case BattleStatusType1.ElementEnhance_Ground:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145127);
                case BattleStatusType1.ElementEnhance_Wind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145128);
                case BattleStatusType1.ElementEnhance_Light:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145129);
                case BattleStatusType1.ElementEnhance_Darkness:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145130);
                case BattleStatusType1.ElementEnhance_All:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145131);
            }
        }
        
        public static string GetPropertyName(this BattleStatusType1 p_Type)
        {
            return GetLanguage(p_Type).Text;
        }
        
        public static GameContentsLanguageDataTable.TableRecord GetLanguage(this BattleStatusType2 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType2.None:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(140000);
                case BattleStatusType2.Defense_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145200);
                case BattleStatusType2.Defense_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145201);
                case BattleStatusType2.AntiCriticalRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145202);
                case BattleStatusType2.AntiCriticalRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145203);
                case BattleStatusType2.AntiDamageRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145204);
                case BattleStatusType2.AntiDamageRate_Melee:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145205);
                case BattleStatusType2.AntiDamageRate_Spell:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145206);
                case BattleStatusType2.AntiDamageRate_Critical:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145207);
                case BattleStatusType2.ResistRate_Curse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145208);
                case BattleStatusType2.ResistRate_Shock:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145209);
                case BattleStatusType2.ResistRate_Stun:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145210);
                case BattleStatusType2.ResistRate_Bleed:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145211);
                case BattleStatusType2.ResistRate_Poison:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145212);
                case BattleStatusType2.ResistRate_Burn:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145213);
                case BattleStatusType2.ResistRate_Chill:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145214);
                case BattleStatusType2.ResistRate_Freeze:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145215);
                case BattleStatusType2.ResistRate_Confuse:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145216);
                case BattleStatusType2.ResistRate_Blind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145217);
                case BattleStatusType2.ResistRate_Silence:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145218);
                case BattleStatusType2.ResistRate_Bind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145219);
                case BattleStatusType2.ResistRate_Groggy:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145220);
                case BattleStatusType2.ResistRate_Debuff:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145221);
                case BattleStatusType2.ElementRegist_Fire:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145225);
                case BattleStatusType2.ElementRegist_Water:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145226);
                case BattleStatusType2.ElementRegist_Ground:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145227);
                case BattleStatusType2.ElementRegist_Wind:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145228);
                case BattleStatusType2.ElementRegist_Light:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145229);
                case BattleStatusType2.ElementRegist_Darkness:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145230);
                case BattleStatusType2.ElementRegist_All:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145231);
            }
        }
        
        public static string GetPropertyName(this BattleStatusType2 p_Type)
        {
            return GetLanguage(p_Type).Text;
        }
        
        public static GameContentsLanguageDataTable.TableRecord GetLanguage(this BattleStatusType3 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusType3.None:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(140000);
                case BattleStatusType3.HP_Base:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145300);
                case BattleStatusType3.MP_Base:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145301);
                case BattleStatusType3.HP_Fix_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145302);
                case BattleStatusType3.MP_Fix_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145303);
                case BattleStatusType3.HP_Rate_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145304);
                case BattleStatusType3.MP_Rate_Recovery:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145305);
                case BattleStatusType3.AttackSpeedBasis:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145306);
                case BattleStatusType3.SpellCastSpeedBasis:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145307);
                case BattleStatusType3.MoveSpeedBasis:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145308);
                case BattleStatusType3.AttackSpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145309);
                case BattleStatusType3.SpellCastSpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145310);
                case BattleStatusType3.MoveSpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145311);
                case BattleStatusType3.JumpForce:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145312);
                case BattleStatusType3.JumpCount:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145313);
                case BattleStatusType3.SightRange:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145314);
                case BattleStatusType3.Absorb:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145315);
                case BattleStatusType3.HitRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145316);
                case BattleStatusType3.DodgeRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145317);
                case BattleStatusType3.CostRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145318);
                case BattleStatusType3.CooldownRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145319);
                case BattleStatusType3.CooldownRecoverySpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145320);
                case BattleStatusType3.HitMotionRecoverySpeedRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145321);
                case BattleStatusType3.ExpChanceExtraRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145322);
                case BattleStatusType3.GoldChanceExtraRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145323);
                case BattleStatusType3.ItemChanceExtraRate:
                    return GameContentsLanguageDataTable.GetInstanceUnsafe.GetRecord(145324);
            }
        }   
        
        public static string GetPropertyName(this BattleStatusType3 p_Type)
        {
            return GetLanguage(p_Type).Text;
        }

        #endregion
        
        #region <Struct>

        [Serializable]
        public struct BattleStatusFlag
        {
            #region <Fields>

            /// <summary>
            /// 포함된 능력치 타입 플래그 마스크
            /// </summary>
            public BattleStatusType1 FlagMask1;
            
            /// <summary>
            /// 포함된 능력치 타입 플래그 마스크2
            /// </summary>
            public BattleStatusType2 FlagMask2;
                        
            /// <summary>
            /// 포함된 능력치 타입 플래그 마스크3
            /// </summary>
            public BattleStatusType3 FlagMask3;

            #endregion

            #region <Constructor>

            public BattleStatusFlag(BattleStatusType1 p_FlagMask, BattleStatusType2 p_FlagMask2, BattleStatusType3 p_FlagMask3)
            {
                FlagMask1 = p_FlagMask;
                FlagMask2 = p_FlagMask2;
                FlagMask3 = p_FlagMask3;
            }

            #endregion

            #region <Methods>

            public bool HasAnyFlag()
            {
                return HasAnyType1Flag() || HasAnyType2Flag() || HasAnyType3Flag();
            }
            
            public bool HasAnyType1Flag()
            {
                return FlagMask1 != BattleStatusType1.None;
            }
            
            public bool HasAnyType2Flag()
            {
                return FlagMask2 != BattleStatusType2.None;
            }
            
            public bool HasAnyType3Flag()
            {
                return FlagMask3 != BattleStatusType3.None;
            }
            
            public bool HasFlag(BattleStatusType p_Type)
            {
                switch (p_Type)
                {
                    default:
                    case BattleStatusType.None:
                        return false;
                    case BattleStatusType.Attack_Melee:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.Attack_Melee);
                    case BattleStatusType.Attack_Spell:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.Attack_Spell);
                    case BattleStatusType.CriticalRate_Melee:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.CriticalRate_Melee);
                    case BattleStatusType.CriticalRate_Spell:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.CriticalRate_Spell);
                    case BattleStatusType.DamageRate:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate);
                    case BattleStatusType.DamageRate_Melee:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Melee);
                    case BattleStatusType.DamageRate_Spell:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Spell);
                    case BattleStatusType.DamageRate_Critical:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Critical);
                    case BattleStatusType.DamageRate_Pierce:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Pierce);
                    case BattleStatusType.DamageRate_Boss:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Boss);
                    case BattleStatusType.DamageRate_Curse:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Curse);
                    case BattleStatusType.DamageRate_Shock:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Shock);
                    case BattleStatusType.DamageRate_Stun:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Stun);
                    case BattleStatusType.DamageRate_Bleed:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Bleed);
                    case BattleStatusType.DamageRate_Poison:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Poison);
                    case BattleStatusType.DamageRate_Burn:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Burn);
                    case BattleStatusType.DamageRate_Chill:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Chill);
                    case BattleStatusType.DamageRate_Freeze:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Freeze);
                    case BattleStatusType.DamageRate_Confuse:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Confuse);
                    case BattleStatusType.DamageRate_Blind:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Blind);
                    case BattleStatusType.DamageRate_Silence:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Silence);
                    case BattleStatusType.DamageRate_Bind:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Bind);
                    case BattleStatusType.DamageRate_Groggy:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Groggy);
                    case BattleStatusType.DamageRate_Debuff:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.DamageRate_Debuff);
                    case BattleStatusType.ElementEnhance_Water:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.ElementEnhance_Water);
                    case BattleStatusType.ElementEnhance_Ground:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.ElementEnhance_Ground);
                    case BattleStatusType.ElementEnhance_Wind:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.ElementEnhance_Wind);
                    case BattleStatusType.ElementEnhance_Light:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.ElementEnhance_Light);
                    case BattleStatusType.ElementEnhance_Darkness:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.ElementEnhance_Darkness);
                    case BattleStatusType.ElementEnhance_All:
                        return FlagMask1.HasAnyFlagExceptNone(BattleStatusType1.ElementEnhance_All);
                    case BattleStatusType.Defense_Melee:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.Defense_Melee);
                    case BattleStatusType.Defense_Spell:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.Defense_Spell);
                    case BattleStatusType.AntiCriticalRate_Melee:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.AntiCriticalRate_Melee);
                    case BattleStatusType.AntiCriticalRate_Spell:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.AntiCriticalRate_Spell);
                    case BattleStatusType.AntiDamageRate:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.AntiDamageRate);
                    case BattleStatusType.AntiDamageRate_Melee:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.AntiDamageRate_Melee);
                    case BattleStatusType.AntiDamageRate_Spell:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.AntiDamageRate_Spell);
                    case BattleStatusType.AntiDamageRate_Critical:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.AntiDamageRate_Critical);
                    case BattleStatusType.ResistRate_Curse:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Curse);
                    case BattleStatusType.ResistRate_Shock:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Shock);
                    case BattleStatusType.ResistRate_Stun:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Stun);
                    case BattleStatusType.ResistRate_Bleed:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Bleed);
                    case BattleStatusType.ResistRate_Poison:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Poison);
                    case BattleStatusType.ResistRate_Burn:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Burn);
                    case BattleStatusType.ResistRate_Chill:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Chill);
                    case BattleStatusType.ResistRate_Freeze:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Freeze);
                    case BattleStatusType.ResistRate_Confuse:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Confuse);
                    case BattleStatusType.ResistRate_Blind:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Blind);
                    case BattleStatusType.ResistRate_Silence:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Silence);
                    case BattleStatusType.ResistRate_Bind:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Bind);
                    case BattleStatusType.ResistRate_Groggy:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Groggy);
                    case BattleStatusType.ResistRate_Debuff:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ResistRate_Debuff);
                    case BattleStatusType.ElementRegist_Fire:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_Fire);
                    case BattleStatusType.ElementRegist_Water:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_Water);
                    case BattleStatusType.ElementRegist_Ground:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_Ground);
                    case BattleStatusType.ElementRegist_Wind:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_Wind);
                    case BattleStatusType.ElementRegist_Light:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_Light);
                    case BattleStatusType.ElementRegist_Darkness:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_Darkness);
                    case BattleStatusType.ElementRegist_All:
                        return FlagMask2.HasAnyFlagExceptNone(BattleStatusType2.ElementRegist_All);
                    case BattleStatusType.HP_Base:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.HP_Base);
                    case BattleStatusType.MP_Base:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.MP_Base);
                    case BattleStatusType.HP_Fix_Recovery:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.HP_Fix_Recovery);
                    case BattleStatusType.MP_Fix_Recovery:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.MP_Fix_Recovery);
                    case BattleStatusType.HP_Rate_Recovery:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.HP_Rate_Recovery);
                    case BattleStatusType.MP_Rate_Recovery:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.MP_Rate_Recovery);
                    case BattleStatusType.AttackSpeedBasis:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.AttackSpeedBasis);
                    case BattleStatusType.SpellCastSpeedBasis:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.SpellCastSpeedBasis);
                    case BattleStatusType.MoveSpeedBasis:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.MoveSpeedBasis);
                    case BattleStatusType.AttackSpeedRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.AttackSpeedRate);
                    case BattleStatusType.SpellCastSpeedRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.SpellCastSpeedRate);
                    case BattleStatusType.MoveSpeedRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.MoveSpeedRate);
                    case BattleStatusType.JumpForce:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.JumpForce);
                    case BattleStatusType.JumpCount:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.JumpCount);
                    case BattleStatusType.SightRange:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.SightRange);
                    case BattleStatusType.Absorb:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.Absorb);
                    case BattleStatusType.HitRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.HitRate);
                    case BattleStatusType.DodgeRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.DodgeRate);
                    case BattleStatusType.CostRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.CostRate);
                    case BattleStatusType.CooldownRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.CooldownRate);
                    case BattleStatusType.CooldownRecoverySpeedRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.CooldownRecoverySpeedRate);
                    case BattleStatusType.HitMotionRecoverySpeedRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.HitMotionRecoverySpeedRate);
                    case BattleStatusType.ExpChanceExtraRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.ExpChanceExtraRate);
                    case BattleStatusType.GoldChanceExtraRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.GoldChanceExtraRate);
                    case BattleStatusType.ItemChanceExtraRate:
                        return FlagMask3.HasAnyFlagExceptNone(BattleStatusType3.ItemChanceExtraRate);
                }
            }
            
            public bool HasFlag(BattleStatusType1 p_Type)
            {
                return FlagMask1.HasAnyFlagExceptNone(p_Type);
            }
            
            public bool HasFlag(BattleStatusType2 p_Type)
            {
                return FlagMask2.HasAnyFlagExceptNone(p_Type);
            }
            
            public bool HasFlag(BattleStatusType3 p_Type)
            {
                return FlagMask3.HasAnyFlagExceptNone(p_Type);
            }
            
            public void AddFlag(BattleStatusType p_Type)
            {
                switch (p_Type)
                {
                    default:
                    case BattleStatusType.None:
                        break;
                    case BattleStatusType.Attack_Melee:
                        FlagMask1.AddFlag(BattleStatusType1.Attack_Melee);
                        break;
                    case BattleStatusType.Attack_Spell:
                        FlagMask1.AddFlag(BattleStatusType1.Attack_Spell);
                        break;
                    case BattleStatusType.CriticalRate_Melee:
                        FlagMask1.AddFlag(BattleStatusType1.CriticalRate_Melee);
                        break;
                    case BattleStatusType.CriticalRate_Spell:
                        FlagMask1.AddFlag(BattleStatusType1.CriticalRate_Spell);
                        break;
                    case BattleStatusType.DamageRate:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate);
                        break;
                    case BattleStatusType.DamageRate_Melee:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Melee);
                        break;
                    case BattleStatusType.DamageRate_Spell:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Spell);
                        break;
                    case BattleStatusType.DamageRate_Critical:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Critical);
                        break;
                    case BattleStatusType.DamageRate_Pierce:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Pierce);
                        break;
                    case BattleStatusType.DamageRate_Boss:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Boss);
                        break;
                    case BattleStatusType.DamageRate_Curse:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Curse);
                        break;
                    case BattleStatusType.DamageRate_Shock:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Shock);
                        break;
                    case BattleStatusType.DamageRate_Stun:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Stun);
                        break;
                    case BattleStatusType.DamageRate_Bleed:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Bleed);
                        break;
                    case BattleStatusType.DamageRate_Poison:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Poison);
                        break;
                    case BattleStatusType.DamageRate_Burn:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Burn);
                        break;
                    case BattleStatusType.DamageRate_Chill:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Chill);
                        break;
                    case BattleStatusType.DamageRate_Freeze:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Freeze);
                        break;
                    case BattleStatusType.DamageRate_Confuse:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Confuse);
                        break;
                    case BattleStatusType.DamageRate_Blind:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Blind);
                        break;
                    case BattleStatusType.DamageRate_Silence:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Silence);
                        break;
                    case BattleStatusType.DamageRate_Bind:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Bind);
                        break;
                    case BattleStatusType.DamageRate_Groggy:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Groggy);
                        break;
                    case BattleStatusType.DamageRate_Debuff:
                        FlagMask1.AddFlag(BattleStatusType1.DamageRate_Debuff);
                        break;
                    case BattleStatusType.ElementEnhance_Fire:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_Fire);
                        break;
                    case BattleStatusType.ElementEnhance_Water:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_Water);
                        break;
                    case BattleStatusType.ElementEnhance_Ground:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_Ground);
                        break;
                    case BattleStatusType.ElementEnhance_Wind:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_Wind);
                        break;
                    case BattleStatusType.ElementEnhance_Light:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_Light);
                        break;
                    case BattleStatusType.ElementEnhance_Darkness:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_Darkness);
                        break;
                    case BattleStatusType.ElementEnhance_All:
                        FlagMask1.AddFlag(BattleStatusType1.ElementEnhance_All);
                        break;
                    case BattleStatusType.Defense_Melee:
                        FlagMask2.AddFlag(BattleStatusType2.Defense_Melee);
                        break;
                    case BattleStatusType.Defense_Spell:
                        FlagMask2.AddFlag(BattleStatusType2.Defense_Spell);
                        break;
                    case BattleStatusType.AntiCriticalRate_Melee:
                        FlagMask2.AddFlag(BattleStatusType2.AntiCriticalRate_Melee);
                        break;
                    case BattleStatusType.AntiCriticalRate_Spell:
                        FlagMask2.AddFlag(BattleStatusType2.AntiCriticalRate_Spell);
                        break;
                    case BattleStatusType.AntiDamageRate:
                        FlagMask2.AddFlag(BattleStatusType2.AntiDamageRate);
                        break;
                    case BattleStatusType.AntiDamageRate_Melee:
                        FlagMask2.AddFlag(BattleStatusType2.AntiDamageRate_Melee);
                        break;
                    case BattleStatusType.AntiDamageRate_Spell:
                        FlagMask2.AddFlag(BattleStatusType2.AntiDamageRate_Spell);
                        break;
                    case BattleStatusType.AntiDamageRate_Critical:
                        FlagMask2.AddFlag(BattleStatusType2.AntiDamageRate_Critical);
                        break;
                    case BattleStatusType.ResistRate_Curse:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Curse);
                        break;
                    case BattleStatusType.ResistRate_Shock:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Shock);
                        break;
                    case BattleStatusType.ResistRate_Stun:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Stun);
                        break;
                    case BattleStatusType.ResistRate_Bleed:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Bleed);
                        break;
                    case BattleStatusType.ResistRate_Poison:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Poison);
                        break;
                    case BattleStatusType.ResistRate_Burn:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Burn);
                        break;
                    case BattleStatusType.ResistRate_Chill:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Chill);
                        break;
                    case BattleStatusType.ResistRate_Freeze:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Freeze);
                        break;
                    case BattleStatusType.ResistRate_Confuse:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Confuse);
                        break;
                    case BattleStatusType.ResistRate_Blind:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Blind);
                        break;
                    case BattleStatusType.ResistRate_Silence:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Silence);
                        break;
                    case BattleStatusType.ResistRate_Bind:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Bind);
                        break;
                    case BattleStatusType.ResistRate_Groggy:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Groggy);
                        break;
                    case BattleStatusType.ResistRate_Debuff:
                        FlagMask2.AddFlag(BattleStatusType2.ResistRate_Debuff);
                        break;
                    case BattleStatusType.ElementRegist_Fire:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_Fire);
                        break;
                    case BattleStatusType.ElementRegist_Water:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_Water);
                        break;
                    case BattleStatusType.ElementRegist_Ground:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_Ground);
                        break;
                    case BattleStatusType.ElementRegist_Wind:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_Wind);
                        break;
                    case BattleStatusType.ElementRegist_Light:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_Light);
                        break;
                    case BattleStatusType.ElementRegist_Darkness:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_Darkness);
                        break;
                    case BattleStatusType.ElementRegist_All:
                        FlagMask2.AddFlag(BattleStatusType2.ElementRegist_All);
                        break;
                    case BattleStatusType.HP_Base:
                        FlagMask3.AddFlag(BattleStatusType3.HP_Base);
                        break;
                    case BattleStatusType.MP_Base:
                        FlagMask3.AddFlag(BattleStatusType3.MP_Base);
                        break;
                    case BattleStatusType.HP_Fix_Recovery:
                        FlagMask3.AddFlag(BattleStatusType3.HP_Fix_Recovery);
                        break;
                    case BattleStatusType.MP_Fix_Recovery:
                        FlagMask3.AddFlag(BattleStatusType3.MP_Fix_Recovery);
                        break;
                    case BattleStatusType.HP_Rate_Recovery:
                        FlagMask3.AddFlag(BattleStatusType3.HP_Rate_Recovery);
                        break;
                    case BattleStatusType.MP_Rate_Recovery:
                        FlagMask3.AddFlag(BattleStatusType3.MP_Rate_Recovery);
                        break;
                    case BattleStatusType.AttackSpeedBasis:
                        FlagMask3.AddFlag(BattleStatusType3.AttackSpeedBasis);
                        break;
                    case BattleStatusType.SpellCastSpeedBasis:
                        FlagMask3.AddFlag(BattleStatusType3.SpellCastSpeedBasis);
                        break;
                    case BattleStatusType.MoveSpeedBasis:
                        FlagMask3.AddFlag(BattleStatusType3.MoveSpeedBasis);
                        break;
                    case BattleStatusType.AttackSpeedRate:
                        FlagMask3.AddFlag(BattleStatusType3.AttackSpeedRate);
                        break;
                    case BattleStatusType.SpellCastSpeedRate:
                        FlagMask3.AddFlag(BattleStatusType3.SpellCastSpeedRate);
                        break;
                    case BattleStatusType.MoveSpeedRate:
                        FlagMask3.AddFlag(BattleStatusType3.MoveSpeedRate);
                        break;
                    case BattleStatusType.JumpForce:
                        FlagMask3.AddFlag(BattleStatusType3.JumpForce);
                        break;
                    case BattleStatusType.JumpCount:
                        FlagMask3.AddFlag(BattleStatusType3.JumpCount);
                        break;
                    case BattleStatusType.SightRange:
                        FlagMask3.AddFlag(BattleStatusType3.SightRange);
                        break;
                    case BattleStatusType.Absorb:
                        FlagMask3.AddFlag(BattleStatusType3.Absorb);
                        break;
                    case BattleStatusType.HitRate:
                        FlagMask3.AddFlag(BattleStatusType3.HitRate);
                        break;
                    case BattleStatusType.DodgeRate:
                        FlagMask3.AddFlag(BattleStatusType3.DodgeRate);
                        break;
                    case BattleStatusType.CostRate:
                        FlagMask3.AddFlag(BattleStatusType3.CostRate);
                        break;
                    case BattleStatusType.CooldownRate:
                        FlagMask3.AddFlag(BattleStatusType3.CooldownRate);
                        break;
                    case BattleStatusType.CooldownRecoverySpeedRate:
                        FlagMask3.AddFlag(BattleStatusType3.CooldownRecoverySpeedRate);
                        break;
                    case BattleStatusType.HitMotionRecoverySpeedRate:
                        FlagMask3.AddFlag(BattleStatusType3.HitMotionRecoverySpeedRate);
                        break;
                    case BattleStatusType.ExpChanceExtraRate:
                        FlagMask3.AddFlag(BattleStatusType3.ExpChanceExtraRate);
                        break;
                    case BattleStatusType.GoldChanceExtraRate:
                        FlagMask3.AddFlag(BattleStatusType3.GoldChanceExtraRate);
                        break;
                    case BattleStatusType.ItemChanceExtraRate:
                        FlagMask3.AddFlag(BattleStatusType3.ItemChanceExtraRate);
                        break;
                }
            }
            
            public void AddFlag(BattleStatusType1 p_Type)
            {
                FlagMask1.AddFlag(p_Type);
            }
                        
            public void AddFlag(BattleStatusType2 p_Type)
            {
                FlagMask2.AddFlag(p_Type);
            }
                        
            public void AddFlag(BattleStatusType3 p_Type)
            {
                FlagMask3.AddFlag(p_Type);
            }

            public void RemoveFlag(BattleStatusType p_Type)
            {
                switch (p_Type)
                {
                    default:
                    case BattleStatusType.None:
                        break;
                    case BattleStatusType.Attack_Melee:
                        FlagMask1.RemoveFlag(BattleStatusType1.Attack_Melee);
                        break;
                    case BattleStatusType.Attack_Spell:
                        FlagMask1.RemoveFlag(BattleStatusType1.Attack_Spell);
                        break;
                    case BattleStatusType.CriticalRate_Melee:
                        FlagMask1.RemoveFlag(BattleStatusType1.CriticalRate_Melee);
                        break;
                    case BattleStatusType.CriticalRate_Spell:
                        FlagMask1.RemoveFlag(BattleStatusType1.CriticalRate_Spell);
                        break;
                    case BattleStatusType.DamageRate:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate);
                        break;
                    case BattleStatusType.DamageRate_Melee:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Melee);
                        break;
                    case BattleStatusType.DamageRate_Spell:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Spell);
                        break;
                    case BattleStatusType.DamageRate_Critical:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Critical);
                        break;
                    case BattleStatusType.DamageRate_Pierce:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Pierce);
                        break;
                    case BattleStatusType.DamageRate_Boss:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Boss);
                        break;
                    case BattleStatusType.DamageRate_Curse:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Curse);
                        break;
                    case BattleStatusType.DamageRate_Shock:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Shock);
                        break;
                    case BattleStatusType.DamageRate_Stun:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Stun);
                        break;
                    case BattleStatusType.DamageRate_Bleed:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Bleed);
                        break;
                    case BattleStatusType.DamageRate_Poison:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Poison);
                        break;
                    case BattleStatusType.DamageRate_Burn:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Burn);
                        break;
                    case BattleStatusType.DamageRate_Chill:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Chill);
                        break;
                    case BattleStatusType.DamageRate_Freeze:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Freeze);
                        break;
                    case BattleStatusType.DamageRate_Confuse:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Confuse);
                        break;
                    case BattleStatusType.DamageRate_Blind:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Blind);
                        break;
                    case BattleStatusType.DamageRate_Silence:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Silence);
                        break;
                    case BattleStatusType.DamageRate_Bind:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Bind);
                        break;
                    case BattleStatusType.DamageRate_Groggy:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Groggy);
                        break;
                    case BattleStatusType.DamageRate_Debuff:
                        FlagMask1.RemoveFlag(BattleStatusType1.DamageRate_Debuff);
                        break;
                    case BattleStatusType.ElementEnhance_Fire:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_Fire);
                        break;
                    case BattleStatusType.ElementEnhance_Water:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_Water);
                        break;
                    case BattleStatusType.ElementEnhance_Ground:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_Ground);
                        break;
                    case BattleStatusType.ElementEnhance_Wind:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_Wind);
                        break;
                    case BattleStatusType.ElementEnhance_Light:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_Light);
                        break;
                    case BattleStatusType.ElementEnhance_Darkness:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_Darkness);
                        break;
                    case BattleStatusType.ElementEnhance_All:
                        FlagMask1.RemoveFlag(BattleStatusType1.ElementEnhance_All);
                        break;
                    case BattleStatusType.Defense_Melee:
                        FlagMask2.RemoveFlag(BattleStatusType2.Defense_Melee);
                        break;
                    case BattleStatusType.Defense_Spell:
                        FlagMask2.RemoveFlag(BattleStatusType2.Defense_Spell);
                        break;
                    case BattleStatusType.AntiCriticalRate_Melee:
                        FlagMask2.RemoveFlag(BattleStatusType2.AntiCriticalRate_Melee);
                        break;
                    case BattleStatusType.AntiCriticalRate_Spell:
                        FlagMask2.RemoveFlag(BattleStatusType2.AntiCriticalRate_Spell);
                        break;
                    case BattleStatusType.AntiDamageRate:
                        FlagMask2.RemoveFlag(BattleStatusType2.AntiDamageRate);
                        break;
                    case BattleStatusType.AntiDamageRate_Melee:
                        FlagMask2.RemoveFlag(BattleStatusType2.AntiDamageRate_Melee);
                        break;
                    case BattleStatusType.AntiDamageRate_Spell:
                        FlagMask2.RemoveFlag(BattleStatusType2.AntiDamageRate_Spell);
                        break;
                    case BattleStatusType.AntiDamageRate_Critical:
                        FlagMask2.RemoveFlag(BattleStatusType2.AntiDamageRate_Critical);
                        break;
                    case BattleStatusType.ResistRate_Curse:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Curse);
                        break;
                    case BattleStatusType.ResistRate_Shock:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Shock);
                        break;
                    case BattleStatusType.ResistRate_Stun:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Stun);
                        break;
                    case BattleStatusType.ResistRate_Bleed:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Bleed);
                        break;
                    case BattleStatusType.ResistRate_Poison:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Poison);
                        break;
                    case BattleStatusType.ResistRate_Burn:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Burn);
                        break;
                    case BattleStatusType.ResistRate_Chill:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Chill);
                        break;
                    case BattleStatusType.ResistRate_Freeze:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Freeze);
                        break;
                    case BattleStatusType.ResistRate_Confuse:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Confuse);
                        break;
                    case BattleStatusType.ResistRate_Blind:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Blind);
                        break;
                    case BattleStatusType.ResistRate_Silence:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Silence);
                        break;
                    case BattleStatusType.ResistRate_Bind:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Bind);
                        break;
                    case BattleStatusType.ResistRate_Groggy:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Groggy);
                        break;
                    case BattleStatusType.ResistRate_Debuff:
                        FlagMask2.RemoveFlag(BattleStatusType2.ResistRate_Debuff);
                        break;
                    case BattleStatusType.ElementRegist_Fire:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_Fire);
                        break;
                    case BattleStatusType.ElementRegist_Water:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_Water);
                        break;
                    case BattleStatusType.ElementRegist_Ground:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_Ground);
                        break;
                    case BattleStatusType.ElementRegist_Wind:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_Wind);
                        break;
                    case BattleStatusType.ElementRegist_Light:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_Light);
                        break;
                    case BattleStatusType.ElementRegist_Darkness:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_Darkness);
                        break;
                    case BattleStatusType.ElementRegist_All:
                        FlagMask2.RemoveFlag(BattleStatusType2.ElementRegist_All);
                        break;
                    case BattleStatusType.HP_Base:
                        FlagMask3.RemoveFlag(BattleStatusType3.HP_Base);
                        break;
                    case BattleStatusType.MP_Base:
                        FlagMask3.RemoveFlag(BattleStatusType3.MP_Base);
                        break;
                    case BattleStatusType.HP_Fix_Recovery:
                        FlagMask3.RemoveFlag(BattleStatusType3.HP_Fix_Recovery);
                        break;
                    case BattleStatusType.MP_Fix_Recovery:
                        FlagMask3.RemoveFlag(BattleStatusType3.MP_Fix_Recovery);
                        break;
                    case BattleStatusType.HP_Rate_Recovery:
                        FlagMask3.RemoveFlag(BattleStatusType3.HP_Rate_Recovery);
                        break;
                    case BattleStatusType.MP_Rate_Recovery:
                        FlagMask3.RemoveFlag(BattleStatusType3.MP_Rate_Recovery);
                        break;
                    case BattleStatusType.AttackSpeedBasis:
                        FlagMask3.RemoveFlag(BattleStatusType3.AttackSpeedBasis);
                        break;
                    case BattleStatusType.SpellCastSpeedBasis:
                        FlagMask3.RemoveFlag(BattleStatusType3.SpellCastSpeedBasis);
                        break;
                    case BattleStatusType.MoveSpeedBasis:
                        FlagMask3.RemoveFlag(BattleStatusType3.MoveSpeedBasis);
                        break;
                    case BattleStatusType.AttackSpeedRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.AttackSpeedRate);
                        break;
                    case BattleStatusType.SpellCastSpeedRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.SpellCastSpeedRate);
                        break;
                    case BattleStatusType.MoveSpeedRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.MoveSpeedRate);
                        break;
                    case BattleStatusType.JumpForce:
                        FlagMask3.RemoveFlag(BattleStatusType3.JumpForce);
                        break;
                    case BattleStatusType.JumpCount:
                        FlagMask3.RemoveFlag(BattleStatusType3.JumpCount);
                        break;
                    case BattleStatusType.SightRange:
                        FlagMask3.RemoveFlag(BattleStatusType3.SightRange);
                        break;
                    case BattleStatusType.Absorb:
                        FlagMask3.RemoveFlag(BattleStatusType3.Absorb);
                        break;
                    case BattleStatusType.HitRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.HitRate);
                        break;
                    case BattleStatusType.DodgeRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.DodgeRate);
                        break;
                    case BattleStatusType.CostRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.CostRate);
                        break;
                    case BattleStatusType.CooldownRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.CooldownRate);
                        break;
                    case BattleStatusType.CooldownRecoverySpeedRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.CooldownRecoverySpeedRate);
                        break;
                    case BattleStatusType.HitMotionRecoverySpeedRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.HitMotionRecoverySpeedRate);
                        break;
                    case BattleStatusType.ExpChanceExtraRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.ExpChanceExtraRate);
                        break;
                    case BattleStatusType.GoldChanceExtraRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.GoldChanceExtraRate);
                        break;
                    case BattleStatusType.ItemChanceExtraRate:
                        FlagMask3.RemoveFlag(BattleStatusType3.ItemChanceExtraRate);
                        break;
                }
            }
            
            public void RemoveFlag(BattleStatusType1 p_Type)
            {
                FlagMask1.RemoveFlag(p_Type);
            }
            
            public void RemoveFlag(BattleStatusType2 p_Type)
            {
                FlagMask2.RemoveFlag(p_Type);
            }
            
            public void RemoveFlag(BattleStatusType3 p_Type)
            {
                FlagMask3.RemoveFlag(p_Type);
            }
            
            #endregion
        }

        #endregion
    }
}