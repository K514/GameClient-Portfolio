using System;
using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 스탯 관련 프리셋
    /// </summary>
    [Serializable]
    public struct BattleStatusPreset
    {
        #region <Fields>

        /// <summary>
        /// 3차 플래그 마스크
        /// </summary>
        public BattleStatusTool.BattleStatusFlag FlagMask;

        /* Type 1 */
        public float Attack_Melee;
        public float Attack_Spell;

        public float CriticalRate_Melee;
        public float CriticalRate_Spell;

        public float DamageRate;
        public float DamageRate_Melee;
        public float DamageRate_Spell;
        public float DamageRate_Critical;
        public float DamageRate_Pierce;
        public float DamageRate_Boss;
        public float DamageRate_Curse;
        public float DamageRate_Shock;
        public float DamageRate_Stun;
        public float DamageRate_Bleed;
        public float DamageRate_Poison;
        public float DamageRate_Burn;
        public float DamageRate_Chill;
        public float DamageRate_Freeze;
        public float DamageRate_Confuse;
        public float DamageRate_Blind;
        public float DamageRate_Silence;
        public float DamageRate_Bind;
        public float DamageRate_Groggy;
        public float DamageRate_Debuff;
                                                          
        public float ElementEnhance_Fire;          
        public float ElementEnhance_Water;         
        public float ElementEnhance_Ground;        
        public float ElementEnhance_Wind;          
        public float ElementEnhance_Light;         
        public float ElementEnhance_Darkness;      
        public float ElementEnhance_All;         

        /* Type 2 */
        public float Defense_Melee;
        public float Defense_Spell;

        public float AntiCriticalRate_Melee;
        public float AntiCriticalRate_Spell;

        public float AntiDamageRate;
        public float AntiDamageRate_Melee;
        public float AntiDamageRate_Spell;
        public float AntiDamageRate_Critical;

        public float ResistRate_Curse;
        public float ResistRate_Shock;
        public float ResistRate_Stun;
        public float ResistRate_Bleed;
        public float ResistRate_Poison;
        public float ResistRate_Burn;
        public float ResistRate_Chill;
        public float ResistRate_Freeze;
        public float ResistRate_Confuse;
        public float ResistRate_Blind;
        public float ResistRate_Silence;
        public float ResistRate_Bind;
        public float ResistRate_Groggy;
        public float ResistRate_Debuff;
        
        public float ElementRegist_Fire;       
        public float ElementRegist_Water;      
        public float ElementRegist_Ground;     
        public float ElementRegist_Wind;       
        public float ElementRegist_Light;      
        public float ElementRegist_Darkness;   
        public float ElementRegist_All;      

        /* Type 3 */
        public float HP_Base;
        public float MP_Base;
        public float HP_Fix_Recovery;
        public float MP_Fix_Recovery;
        public float HP_Rate_Recovery;
        public float MP_Rate_Recovery;

        public float AttackSpeedBasis;
        public float SpellCastSpeedBasis;
        public float MoveSpeedBasis;
        
        public float AttackSpeedRate;                                   
        public float SpellCastSpeedRate;                                
        public float MoveSpeedRate;   
        
        public float JumpForce;
        public float JumpCount;
        public float SightRange;
        public float Absorb;

        public float HitRate;                                           
        public float DodgeRate;
        public float CostRate;
        public float CooldownRate;                                      
        public float CooldownRecoverySpeedRate;  
        public float HitMotionRecoverySpeedRate;
        
        public float ExpChanceExtraRate;
        public float GoldChanceExtraRate;
        public float ItemChanceExtraRate;

        #endregion

        #region <Indexer>

        public float this[BattleStatusTool.BattleStatusType p_Type] => GetProperty(p_Type);
        public float this[BattleStatusTool.BattleStatusType1 p_Type] => GetProperty(p_Type);
        public float this[BattleStatusTool.BattleStatusType2 p_Type] => GetProperty(p_Type);
        public float this[BattleStatusTool.BattleStatusType3 p_Type] => GetProperty(p_Type);
        public float this[BattleStatusTool.BattleStatusType p_Type, float p_Rate] => GetProperty(p_Type, p_Rate);
        public float this[BattleStatusTool.BattleStatusType1 p_Type, float p_Rate] => GetProperty(p_Type, p_Rate);
        public float this[BattleStatusTool.BattleStatusType2 p_Type, float p_Rate] => GetProperty(p_Type, p_Rate);
        public float this[BattleStatusTool.BattleStatusType3 p_Type, float p_Rate] => GetProperty(p_Type, p_Rate);

        #endregion

        #region <Constructor>

        public BattleStatusPreset(BattleStatusTool.BattleStatusType p_Type, float p_Value)
        {
            this = default;

            SetProperty(p_Type, p_Value);
        }
        
        public BattleStatusPreset(BattleStatusTool.BattleStatusType1 p_Type, float p_Value)
        {
            this = default;

            SetProperty(p_Type, p_Value);
        }
        
        public BattleStatusPreset(BattleStatusTool.BattleStatusType2 p_Type, float p_Value)
        {
            this = default;

            SetProperty(p_Type, p_Value);
        }
                
        public BattleStatusPreset(BattleStatusTool.BattleStatusType3 p_Type, float p_Value)
        {
            this = default;

            SetProperty(p_Type, p_Value);
        }
        
        public BattleStatusPreset(Dictionary<BattleStatusTool.BattleStatusType, float> p_ValueSet)
        {
            this = default;

            if (p_ValueSet.CheckGenericCollectionSafe())
            {
                foreach (var battleStatusType in BattleStatusTool.BattleStatusTypeEnumerator)
                {
                    if (p_ValueSet.TryGetValue(battleStatusType, out var o_Value))
                    {
                        SetProperty(battleStatusType, o_Value);
                    }
                }
            }
        }

        public BattleStatusPreset(int p_Index)
        {
            this = BattleStatusTable.GetInstanceUnsafe.GetRecord(p_Index).BattleStatusPreset;
        }
        
        #endregion
        
        #region <Operator>

        public static implicit operator BattleStatusPreset(float p_Value)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Value;
            result.Attack_Spell = p_Value;
            result.ElementEnhance_Fire = p_Value;
            result.ElementEnhance_Water = p_Value;
            result.ElementEnhance_Ground = p_Value;
            result.ElementEnhance_Wind = p_Value;
            result.ElementEnhance_Light = p_Value;
            result.ElementEnhance_Darkness = p_Value;
            result.ElementEnhance_All = p_Value;
            result.Defense_Melee = p_Value;
            result.Defense_Spell = p_Value;
            result.ElementRegist_Fire = p_Value;
            result.ElementRegist_Water = p_Value;
            result.ElementRegist_Ground = p_Value;
            result.ElementRegist_Wind = p_Value;
            result.ElementRegist_Light = p_Value;
            result.ElementRegist_Darkness = p_Value;
            result.ElementRegist_All = p_Value;
            result.HP_Base = p_Value;
            result.MP_Base = p_Value;
            result.HP_Fix_Recovery = p_Value;
            result.MP_Fix_Recovery = p_Value;
            result.AttackSpeedBasis = p_Value;
            result.SpellCastSpeedBasis = p_Value;
            result.MoveSpeedBasis = p_Value;
            result.JumpForce = p_Value;
            result.JumpCount = p_Value;
            result.SightRange = p_Value;
            result.Absorb = p_Value;
            
            result.CriticalRate_Melee = p_Value;
            result.CriticalRate_Spell = p_Value;
            result.DamageRate = p_Value;
            result.DamageRate_Melee = p_Value;
            result.DamageRate_Spell = p_Value;
            result.DamageRate_Critical = p_Value;
            result.DamageRate_Pierce = p_Value;
            result.DamageRate_Boss = p_Value;
            result.DamageRate_Curse = p_Value;
            result.DamageRate_Shock = p_Value;
            result.DamageRate_Stun = p_Value;
            result.DamageRate_Bleed = p_Value;
            result.DamageRate_Poison = p_Value;
            result.DamageRate_Burn = p_Value;
            result.DamageRate_Chill = p_Value;
            result.DamageRate_Freeze = p_Value;
            result.DamageRate_Confuse = p_Value;
            result.DamageRate_Blind = p_Value;
            result.DamageRate_Silence = p_Value;
            result.DamageRate_Bind = p_Value;
            result.DamageRate_Groggy = p_Value;
            result.DamageRate_Debuff = p_Value;
            result.AntiCriticalRate_Melee = p_Value;
            result.AntiCriticalRate_Spell = p_Value;
            result.AntiDamageRate = p_Value;
            result.AntiDamageRate_Melee = p_Value;
            result.AntiDamageRate_Spell = p_Value;
            result.AntiDamageRate_Critical = p_Value;
            result.ResistRate_Curse = p_Value;
            result.ResistRate_Shock = p_Value;
            result.ResistRate_Stun = p_Value;
            result.ResistRate_Bleed = p_Value;
            result.ResistRate_Poison = p_Value;
            result.ResistRate_Burn = p_Value;
            result.ResistRate_Chill = p_Value;
            result.ResistRate_Freeze = p_Value;
            result.ResistRate_Confuse = p_Value;
            result.ResistRate_Blind = p_Value;
            result.ResistRate_Silence = p_Value;
            result.ResistRate_Bind = p_Value;
            result.ResistRate_Groggy = p_Value;
            result.ResistRate_Debuff = p_Value;
            result.HP_Rate_Recovery = p_Value;
            result.MP_Rate_Recovery = p_Value;
            result.AttackSpeedRate = p_Value;
            result.SpellCastSpeedRate = p_Value;
            result.MoveSpeedRate = p_Value;
            result.HitRate = p_Value;
            result.DodgeRate = p_Value;
            result.CostRate = p_Value;
            result.CooldownRecoverySpeedRate = p_Value;
            result.HitMotionRecoverySpeedRate = p_Value;
            result.ExpChanceExtraRate = p_Value;
            result.GoldChanceExtraRate = p_Value;
            result.ItemChanceExtraRate = p_Value;
            
            result.CooldownRate = p_Value;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BattleStatusPreset operator+(BattleStatusPreset p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee + p_Right.Attack_Melee;
            result.Attack_Spell = p_Left.Attack_Spell + p_Right.Attack_Spell;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire + p_Right.ElementEnhance_Fire;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water + p_Right.ElementEnhance_Water;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground + p_Right.ElementEnhance_Ground;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind + p_Right.ElementEnhance_Wind;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light + p_Right.ElementEnhance_Light;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness + p_Right.ElementEnhance_Darkness;
            result.ElementEnhance_All = p_Left.ElementEnhance_All + p_Right.ElementEnhance_All;
            result.Defense_Melee = p_Left.Defense_Melee + p_Right.Defense_Melee;
            result.Defense_Spell = p_Left.Defense_Spell + p_Right.Defense_Spell;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire + p_Right.ElementRegist_Fire;
            result.ElementRegist_Water = p_Left.ElementRegist_Water + p_Right.ElementRegist_Water;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground + p_Right.ElementRegist_Ground;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind + p_Right.ElementRegist_Wind;
            result.ElementRegist_Light = p_Left.ElementRegist_Light + p_Right.ElementRegist_Light;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness + p_Right.ElementRegist_Darkness;
            result.ElementRegist_All = p_Left.ElementRegist_All + p_Right.ElementRegist_All;
            result.HP_Base = p_Left.HP_Base + p_Right.HP_Base;
            result.MP_Base = p_Left.MP_Base + p_Right.MP_Base;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery + p_Right.HP_Fix_Recovery;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery + p_Right.MP_Fix_Recovery;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis + p_Right.AttackSpeedBasis;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis + p_Right.SpellCastSpeedBasis;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis + p_Right.MoveSpeedBasis;
            result.JumpForce = p_Left.JumpForce + p_Right.JumpForce;
            result.JumpCount = p_Left.JumpCount + p_Right.JumpCount;
            result.SightRange = p_Left.SightRange + p_Right.SightRange;
            result.Absorb = p_Left.Absorb + p_Right.Absorb;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee + p_Right.CriticalRate_Melee;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell + p_Right.CriticalRate_Spell;
            result.DamageRate = p_Left.DamageRate + p_Right.DamageRate;
            result.DamageRate_Melee = p_Left.DamageRate_Melee + p_Right.DamageRate_Melee;
            result.DamageRate_Spell = p_Left.DamageRate_Spell + p_Right.DamageRate_Spell;
            result.DamageRate_Critical = p_Left.DamageRate_Critical + p_Right.DamageRate_Critical;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce + p_Right.DamageRate_Pierce;
            result.DamageRate_Boss = p_Left.DamageRate_Boss + p_Right.DamageRate_Boss;
            result.DamageRate_Curse = p_Left.DamageRate_Curse + p_Right.DamageRate_Curse;
            result.DamageRate_Shock = p_Left.DamageRate_Shock + p_Right.DamageRate_Shock;
            result.DamageRate_Stun = p_Left.DamageRate_Stun + p_Right.DamageRate_Stun;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed + p_Right.DamageRate_Bleed;
            result.DamageRate_Poison = p_Left.DamageRate_Poison + p_Right.DamageRate_Poison;
            result.DamageRate_Burn = p_Left.DamageRate_Burn + p_Right.DamageRate_Burn;
            result.DamageRate_Chill = p_Left.DamageRate_Chill + p_Right.DamageRate_Chill;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze + p_Right.DamageRate_Freeze;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse + p_Right.DamageRate_Confuse;
            result.DamageRate_Blind = p_Left.DamageRate_Blind + p_Right.DamageRate_Blind;
            result.DamageRate_Silence = p_Left.DamageRate_Silence + p_Right.DamageRate_Silence;
            result.DamageRate_Bind = p_Left.DamageRate_Bind + p_Right.DamageRate_Bind;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy + p_Right.DamageRate_Groggy;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff + p_Right.DamageRate_Debuff;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee + p_Right.AntiCriticalRate_Melee;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell + p_Right.AntiCriticalRate_Spell;
            result.AntiDamageRate = p_Left.AntiDamageRate + p_Right.AntiDamageRate;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee + p_Right.AntiDamageRate_Melee;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell + p_Right.AntiDamageRate_Spell;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical + p_Right.AntiDamageRate_Critical;
            result.ResistRate_Curse = p_Left.ResistRate_Curse + p_Right.ResistRate_Curse;
            result.ResistRate_Shock = p_Left.ResistRate_Shock + p_Right.ResistRate_Shock;
            result.ResistRate_Stun = p_Left.ResistRate_Stun + p_Right.ResistRate_Stun;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed + p_Right.ResistRate_Bleed;
            result.ResistRate_Poison = p_Left.ResistRate_Poison + p_Right.ResistRate_Poison;
            result.ResistRate_Burn = p_Left.ResistRate_Burn + p_Right.ResistRate_Burn;
            result.ResistRate_Chill = p_Left.ResistRate_Chill + p_Right.ResistRate_Chill;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze + p_Right.ResistRate_Freeze;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse + p_Right.ResistRate_Confuse;
            result.ResistRate_Blind = p_Left.ResistRate_Blind + p_Right.ResistRate_Blind;
            result.ResistRate_Silence = p_Left.ResistRate_Silence + p_Right.ResistRate_Silence;
            result.ResistRate_Bind = p_Left.ResistRate_Bind + p_Right.ResistRate_Bind;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy + p_Right.ResistRate_Groggy;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff + p_Right.ResistRate_Debuff;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery + p_Right.HP_Rate_Recovery;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery + p_Right.MP_Rate_Recovery;
            result.AttackSpeedRate = p_Left.AttackSpeedRate + p_Right.AttackSpeedRate;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate + p_Right.SpellCastSpeedRate;
            result.MoveSpeedRate = p_Left.MoveSpeedRate + p_Right.MoveSpeedRate;
            result.HitRate = p_Left.HitRate + p_Right.HitRate;
            result.DodgeRate = p_Left.DodgeRate + p_Right.DodgeRate;
            result.CostRate = p_Left.CostRate + p_Right.CostRate;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate + p_Right.CooldownRecoverySpeedRate;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate + p_Right.HitMotionRecoverySpeedRate;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate + p_Right.ExpChanceExtraRate;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate + p_Right.GoldChanceExtraRate;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate + p_Right.ItemChanceExtraRate;
            
            result.CooldownRate = (1f + p_Left.CooldownRate) * (1f + p_Right.CooldownRate) - 1f;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BattleStatusPreset operator+(BattleStatusPreset p_Left, float p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee + p_Right;
            result.Attack_Spell = p_Left.Attack_Spell + p_Right;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire + p_Right;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water + p_Right;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground + p_Right;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind + p_Right;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light + p_Right;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness + p_Right;
            result.ElementEnhance_All = p_Left.ElementEnhance_All + p_Right;
            result.Defense_Melee = p_Left.Defense_Melee + p_Right;
            result.Defense_Spell = p_Left.Defense_Spell + p_Right;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire + p_Right;
            result.ElementRegist_Water = p_Left.ElementRegist_Water + p_Right;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground + p_Right;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind + p_Right;
            result.ElementRegist_Light = p_Left.ElementRegist_Light + p_Right;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness + p_Right;
            result.ElementRegist_All = p_Left.ElementRegist_All + p_Right;
            result.HP_Base = p_Left.HP_Base + p_Right;
            result.MP_Base = p_Left.MP_Base + p_Right;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery + p_Right;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery + p_Right;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis + p_Right;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis + p_Right;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis + p_Right;
            result.JumpForce = p_Left.JumpForce + p_Right;
            result.JumpCount = p_Left.JumpCount + p_Right;
            result.SightRange = p_Left.SightRange + p_Right;
            result.Absorb = p_Left.Absorb + p_Right;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee + p_Right;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell + p_Right;
            result.DamageRate = p_Left.DamageRate + p_Right;
            result.DamageRate_Melee = p_Left.DamageRate_Melee + p_Right;
            result.DamageRate_Spell = p_Left.DamageRate_Spell + p_Right;
            result.DamageRate_Critical = p_Left.DamageRate_Critical + p_Right;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce + p_Right;
            result.DamageRate_Boss = p_Left.DamageRate_Boss + p_Right;
            result.DamageRate_Curse = p_Left.DamageRate_Curse + p_Right;
            result.DamageRate_Shock = p_Left.DamageRate_Shock + p_Right;
            result.DamageRate_Stun = p_Left.DamageRate_Stun + p_Right;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed + p_Right;
            result.DamageRate_Poison = p_Left.DamageRate_Poison + p_Right;
            result.DamageRate_Burn = p_Left.DamageRate_Burn + p_Right;
            result.DamageRate_Chill = p_Left.DamageRate_Chill + p_Right;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze + p_Right;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse + p_Right;
            result.DamageRate_Blind = p_Left.DamageRate_Blind + p_Right;
            result.DamageRate_Silence = p_Left.DamageRate_Silence + p_Right;
            result.DamageRate_Bind = p_Left.DamageRate_Bind + p_Right;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy + p_Right;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff + p_Right;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee + p_Right;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell + p_Right;
            result.AntiDamageRate = p_Left.AntiDamageRate + p_Right;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee + p_Right;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell + p_Right;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical + p_Right;
            result.ResistRate_Curse = p_Left.ResistRate_Curse + p_Right;
            result.ResistRate_Shock = p_Left.ResistRate_Shock + p_Right;
            result.ResistRate_Stun = p_Left.ResistRate_Stun + p_Right;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed + p_Right;
            result.ResistRate_Poison = p_Left.ResistRate_Poison + p_Right;
            result.ResistRate_Burn = p_Left.ResistRate_Burn + p_Right;
            result.ResistRate_Chill = p_Left.ResistRate_Chill + p_Right;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze + p_Right;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse + p_Right;
            result.ResistRate_Blind = p_Left.ResistRate_Blind + p_Right;
            result.ResistRate_Silence = p_Left.ResistRate_Silence + p_Right;
            result.ResistRate_Bind = p_Left.ResistRate_Bind + p_Right;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy + p_Right;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff + p_Right;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery + p_Right;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery + p_Right;
            result.AttackSpeedRate = p_Left.AttackSpeedRate + p_Right;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate + p_Right;
            result.MoveSpeedRate = p_Left.MoveSpeedRate + p_Right;
            result.HitRate = p_Left.HitRate + p_Right;
            result.DodgeRate = p_Left.DodgeRate + p_Right;
            result.CostRate = p_Left.CostRate + p_Right;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate + p_Right;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate + p_Right;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate + p_Right;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate + p_Right;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate + p_Right;
            
            result.CooldownRate = (1f + p_Left.CooldownRate) * (1f + p_Right) - 1f;
            
            result.InitFlagMask();
            
            return result;
        }
                
        public static BattleStatusPreset operator+(float p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left + p_Right.Attack_Melee;
            result.Attack_Spell = p_Left + p_Right.Attack_Spell;
            result.ElementEnhance_Fire = p_Left + p_Right.ElementEnhance_Fire;
            result.ElementEnhance_Water = p_Left + p_Right.ElementEnhance_Water;
            result.ElementEnhance_Ground = p_Left + p_Right.ElementEnhance_Ground;
            result.ElementEnhance_Wind = p_Left + p_Right.ElementEnhance_Wind;
            result.ElementEnhance_Light = p_Left + p_Right.ElementEnhance_Light;
            result.ElementEnhance_Darkness = p_Left + p_Right.ElementEnhance_Darkness;
            result.ElementEnhance_All = p_Left + p_Right.ElementEnhance_All;
            result.Defense_Melee = p_Left + p_Right.Defense_Melee;
            result.Defense_Spell = p_Left + p_Right.Defense_Spell;
            result.ElementRegist_Fire = p_Left + p_Right.ElementRegist_Fire;
            result.ElementRegist_Water = p_Left + p_Right.ElementRegist_Water;
            result.ElementRegist_Ground = p_Left + p_Right.ElementRegist_Ground;
            result.ElementRegist_Wind = p_Left + p_Right.ElementRegist_Wind;
            result.ElementRegist_Light = p_Left + p_Right.ElementRegist_Light;
            result.ElementRegist_Darkness = p_Left + p_Right.ElementRegist_Darkness;
            result.ElementRegist_All = p_Left + p_Right.ElementRegist_All;
            result.HP_Base = p_Left + p_Right.HP_Base;
            result.MP_Base = p_Left + p_Right.MP_Base;
            result.HP_Fix_Recovery = p_Left + p_Right.HP_Fix_Recovery;
            result.MP_Fix_Recovery = p_Left + p_Right.MP_Fix_Recovery;
            result.AttackSpeedBasis = p_Left + p_Right.AttackSpeedBasis;
            result.SpellCastSpeedBasis = p_Left + p_Right.SpellCastSpeedBasis;
            result.MoveSpeedBasis = p_Left + p_Right.MoveSpeedBasis;
            result.JumpForce = p_Left + p_Right.JumpForce;
            result.JumpCount = p_Left + p_Right.JumpCount;
            result.SightRange = p_Left + p_Right.SightRange;
            result.Absorb = p_Left + p_Right.Absorb;
            
            result.CriticalRate_Melee = p_Left + p_Right.CriticalRate_Melee;
            result.CriticalRate_Spell = p_Left + p_Right.CriticalRate_Spell;
            result.DamageRate = p_Left + p_Right.DamageRate;
            result.DamageRate_Melee = p_Left + p_Right.DamageRate_Melee;
            result.DamageRate_Spell = p_Left + p_Right.DamageRate_Spell;
            result.DamageRate_Critical = p_Left + p_Right.DamageRate_Critical;
            result.DamageRate_Pierce = p_Left + p_Right.DamageRate_Pierce;
            result.DamageRate_Boss = p_Left + p_Right.DamageRate_Boss;
            result.DamageRate_Curse = p_Left + p_Right.DamageRate_Curse;
            result.DamageRate_Shock = p_Left + p_Right.DamageRate_Shock;
            result.DamageRate_Stun = p_Left + p_Right.DamageRate_Stun;
            result.DamageRate_Bleed = p_Left + p_Right.DamageRate_Bleed;
            result.DamageRate_Poison = p_Left + p_Right.DamageRate_Poison;
            result.DamageRate_Burn = p_Left + p_Right.DamageRate_Burn;
            result.DamageRate_Chill = p_Left + p_Right.DamageRate_Chill;
            result.DamageRate_Freeze = p_Left + p_Right.DamageRate_Freeze;
            result.DamageRate_Confuse = p_Left + p_Right.DamageRate_Confuse;
            result.DamageRate_Blind = p_Left + p_Right.DamageRate_Blind;
            result.DamageRate_Silence = p_Left + p_Right.DamageRate_Silence;
            result.DamageRate_Bind = p_Left + p_Right.DamageRate_Bind;
            result.DamageRate_Groggy = p_Left + p_Right.DamageRate_Groggy;
            result.DamageRate_Debuff = p_Left + p_Right.DamageRate_Debuff;
            result.AntiCriticalRate_Melee = p_Left + p_Right.AntiCriticalRate_Melee;
            result.AntiCriticalRate_Spell = p_Left + p_Right.AntiCriticalRate_Spell;
            result.AntiDamageRate = p_Left + p_Right.AntiDamageRate;
            result.AntiDamageRate_Melee = p_Left + p_Right.AntiDamageRate_Melee;
            result.AntiDamageRate_Spell = p_Left + p_Right.AntiDamageRate_Spell;
            result.AntiDamageRate_Critical = p_Left + p_Right.AntiDamageRate_Critical;
            result.ResistRate_Curse = p_Left + p_Right.ResistRate_Curse;
            result.ResistRate_Shock = p_Left + p_Right.ResistRate_Shock;
            result.ResistRate_Stun = p_Left + p_Right.ResistRate_Stun;
            result.ResistRate_Bleed = p_Left + p_Right.ResistRate_Bleed;
            result.ResistRate_Poison = p_Left + p_Right.ResistRate_Poison;
            result.ResistRate_Burn = p_Left + p_Right.ResistRate_Burn;
            result.ResistRate_Chill = p_Left + p_Right.ResistRate_Chill;
            result.ResistRate_Freeze = p_Left + p_Right.ResistRate_Freeze;
            result.ResistRate_Confuse = p_Left + p_Right.ResistRate_Confuse;
            result.ResistRate_Blind = p_Left + p_Right.ResistRate_Blind;
            result.ResistRate_Silence = p_Left + p_Right.ResistRate_Silence;
            result.ResistRate_Bind = p_Left + p_Right.ResistRate_Bind;
            result.ResistRate_Groggy = p_Left + p_Right.ResistRate_Groggy;
            result.ResistRate_Debuff = p_Left + p_Right.ResistRate_Debuff;
            result.HP_Rate_Recovery = p_Left + p_Right.HP_Rate_Recovery;
            result.MP_Rate_Recovery = p_Left + p_Right.MP_Rate_Recovery;
            result.AttackSpeedRate = p_Left + p_Right.AttackSpeedRate;
            result.SpellCastSpeedRate = p_Left + p_Right.SpellCastSpeedRate;
            result.MoveSpeedRate = p_Left + p_Right.MoveSpeedRate;
            result.HitRate = p_Left + p_Right.HitRate;
            result.DodgeRate = p_Left + p_Right.DodgeRate;
            result.CostRate = p_Left + p_Right.CostRate;
            result.CooldownRecoverySpeedRate = p_Left + p_Right.CooldownRecoverySpeedRate;
            result.HitMotionRecoverySpeedRate = p_Left + p_Right.HitMotionRecoverySpeedRate;
            result.ExpChanceExtraRate = p_Left + p_Right.ExpChanceExtraRate;
            result.GoldChanceExtraRate = p_Left + p_Right.GoldChanceExtraRate;
            result.ItemChanceExtraRate = p_Left + p_Right.ItemChanceExtraRate;
            
            result.CooldownRate = (1f + p_Left) * (1f + p_Right.CooldownRate) - 1f;
            
            result.InitFlagMask();
            
            return result;
        }

        public static BattleStatusPreset operator-(BattleStatusPreset p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee - p_Right.Attack_Melee;
            result.Attack_Spell = p_Left.Attack_Spell - p_Right.Attack_Spell;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire - p_Right.ElementEnhance_Fire;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water - p_Right.ElementEnhance_Water;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground - p_Right.ElementEnhance_Ground;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind - p_Right.ElementEnhance_Wind;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light - p_Right.ElementEnhance_Light;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness - p_Right.ElementEnhance_Darkness;
            result.ElementEnhance_All = p_Left.ElementEnhance_All - p_Right.ElementEnhance_All;
            result.Defense_Melee = p_Left.Defense_Melee - p_Right.Defense_Melee;
            result.Defense_Spell = p_Left.Defense_Spell - p_Right.Defense_Spell;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire - p_Right.ElementRegist_Fire;
            result.ElementRegist_Water = p_Left.ElementRegist_Water - p_Right.ElementRegist_Water;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground - p_Right.ElementRegist_Ground;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind - p_Right.ElementRegist_Wind;
            result.ElementRegist_Light = p_Left.ElementRegist_Light - p_Right.ElementRegist_Light;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness - p_Right.ElementRegist_Darkness;
            result.ElementRegist_All = p_Left.ElementRegist_All - p_Right.ElementRegist_All;
            result.HP_Base = p_Left.HP_Base - p_Right.HP_Base;
            result.MP_Base = p_Left.MP_Base - p_Right.MP_Base;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery - p_Right.HP_Fix_Recovery;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery - p_Right.MP_Fix_Recovery;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis - p_Right.AttackSpeedBasis;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis - p_Right.SpellCastSpeedBasis;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis - p_Right.MoveSpeedBasis;
            result.JumpForce = p_Left.JumpForce - p_Right.JumpForce;
            result.JumpCount = p_Left.JumpCount - p_Right.JumpCount;
            result.SightRange = p_Left.SightRange - p_Right.SightRange;
            result.Absorb = p_Left.Absorb - p_Right.Absorb;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee - p_Right.CriticalRate_Melee;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell - p_Right.CriticalRate_Spell;
            result.DamageRate = p_Left.DamageRate - p_Right.DamageRate;
            result.DamageRate_Melee = p_Left.DamageRate_Melee - p_Right.DamageRate_Melee;
            result.DamageRate_Spell = p_Left.DamageRate_Spell - p_Right.DamageRate_Spell;
            result.DamageRate_Critical = p_Left.DamageRate_Critical - p_Right.DamageRate_Critical;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce - p_Right.DamageRate_Pierce;
            result.DamageRate_Boss = p_Left.DamageRate_Boss - p_Right.DamageRate_Boss;
            result.DamageRate_Curse = p_Left.DamageRate_Curse - p_Right.DamageRate_Curse;
            result.DamageRate_Shock = p_Left.DamageRate_Shock - p_Right.DamageRate_Shock;
            result.DamageRate_Stun = p_Left.DamageRate_Stun - p_Right.DamageRate_Stun;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed - p_Right.DamageRate_Bleed;
            result.DamageRate_Poison = p_Left.DamageRate_Poison - p_Right.DamageRate_Poison;
            result.DamageRate_Burn = p_Left.DamageRate_Burn - p_Right.DamageRate_Burn;
            result.DamageRate_Chill = p_Left.DamageRate_Chill - p_Right.DamageRate_Chill;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze - p_Right.DamageRate_Freeze;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse - p_Right.DamageRate_Confuse;
            result.DamageRate_Blind = p_Left.DamageRate_Blind - p_Right.DamageRate_Blind;
            result.DamageRate_Silence = p_Left.DamageRate_Silence - p_Right.DamageRate_Silence;
            result.DamageRate_Bind = p_Left.DamageRate_Bind - p_Right.DamageRate_Bind;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy - p_Right.DamageRate_Groggy;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff - p_Right.DamageRate_Debuff;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee - p_Right.AntiCriticalRate_Melee;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell - p_Right.AntiCriticalRate_Spell;
            result.AntiDamageRate = p_Left.AntiDamageRate - p_Right.AntiDamageRate;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee - p_Right.AntiDamageRate_Melee;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell - p_Right.AntiDamageRate_Spell;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical - p_Right.AntiDamageRate_Critical;
            result.ResistRate_Curse = p_Left.ResistRate_Curse - p_Right.ResistRate_Curse;
            result.ResistRate_Shock = p_Left.ResistRate_Shock - p_Right.ResistRate_Shock;
            result.ResistRate_Stun = p_Left.ResistRate_Stun - p_Right.ResistRate_Stun;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed - p_Right.ResistRate_Bleed;
            result.ResistRate_Poison = p_Left.ResistRate_Poison - p_Right.ResistRate_Poison;
            result.ResistRate_Burn = p_Left.ResistRate_Burn - p_Right.ResistRate_Burn;
            result.ResistRate_Chill = p_Left.ResistRate_Chill - p_Right.ResistRate_Chill;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze - p_Right.ResistRate_Freeze;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse - p_Right.ResistRate_Confuse;
            result.ResistRate_Blind = p_Left.ResistRate_Blind - p_Right.ResistRate_Blind;
            result.ResistRate_Silence = p_Left.ResistRate_Silence - p_Right.ResistRate_Silence;
            result.ResistRate_Bind = p_Left.ResistRate_Bind - p_Right.ResistRate_Bind;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy - p_Right.ResistRate_Groggy;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff - p_Right.ResistRate_Debuff;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery - p_Right.HP_Rate_Recovery;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery - p_Right.MP_Rate_Recovery;
            result.AttackSpeedRate = p_Left.AttackSpeedRate - p_Right.AttackSpeedRate;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate - p_Right.SpellCastSpeedRate;
            result.MoveSpeedRate = p_Left.MoveSpeedRate - p_Right.MoveSpeedRate;
            result.HitRate = p_Left.HitRate - p_Right.HitRate;
            result.DodgeRate = p_Left.DodgeRate - p_Right.DodgeRate;
            result.CostRate = p_Left.CostRate - p_Right.CostRate;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate - p_Right.CooldownRecoverySpeedRate;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate - p_Right.HitMotionRecoverySpeedRate;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate - p_Right.ExpChanceExtraRate;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate - p_Right.GoldChanceExtraRate;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate - p_Right.ItemChanceExtraRate;
            
            result.CooldownRate = (1f + p_Left.CooldownRate) / (1f + p_Right.CooldownRate).ApplyLowerBound(BaseStatusTool.PropertyLowerBound) - 1f;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BattleStatusPreset operator-(BattleStatusPreset p_Left, float p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee - p_Right;
            result.Attack_Spell = p_Left.Attack_Spell - p_Right;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire - p_Right;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water - p_Right;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground - p_Right;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind - p_Right;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light - p_Right;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness - p_Right;
            result.ElementEnhance_All = p_Left.ElementEnhance_All - p_Right;
            result.Defense_Melee = p_Left.Defense_Melee - p_Right;
            result.Defense_Spell = p_Left.Defense_Spell - p_Right;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire - p_Right;
            result.ElementRegist_Water = p_Left.ElementRegist_Water - p_Right;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground - p_Right;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind - p_Right;
            result.ElementRegist_Light = p_Left.ElementRegist_Light - p_Right;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness - p_Right;
            result.ElementRegist_All = p_Left.ElementRegist_All - p_Right;
            result.HP_Base = p_Left.HP_Base - p_Right;
            result.MP_Base = p_Left.MP_Base - p_Right;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery - p_Right;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery - p_Right;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis - p_Right;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis - p_Right;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis - p_Right;
            result.JumpForce = p_Left.JumpForce - p_Right;
            result.JumpCount = p_Left.JumpCount - p_Right;
            result.SightRange = p_Left.SightRange - p_Right;
            result.Absorb = p_Left.Absorb - p_Right;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee - p_Right;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell - p_Right;
            result.DamageRate = p_Left.DamageRate - p_Right;
            result.DamageRate_Melee = p_Left.DamageRate_Melee - p_Right;
            result.DamageRate_Spell = p_Left.DamageRate_Spell - p_Right;
            result.DamageRate_Critical = p_Left.DamageRate_Critical - p_Right;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce - p_Right;
            result.DamageRate_Boss = p_Left.DamageRate_Boss - p_Right;
            result.DamageRate_Curse = p_Left.DamageRate_Curse - p_Right;
            result.DamageRate_Shock = p_Left.DamageRate_Shock - p_Right;
            result.DamageRate_Stun = p_Left.DamageRate_Stun - p_Right;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed - p_Right;
            result.DamageRate_Poison = p_Left.DamageRate_Poison - p_Right;
            result.DamageRate_Burn = p_Left.DamageRate_Burn - p_Right;
            result.DamageRate_Chill = p_Left.DamageRate_Chill - p_Right;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze - p_Right;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse - p_Right;
            result.DamageRate_Blind = p_Left.DamageRate_Blind - p_Right;
            result.DamageRate_Silence = p_Left.DamageRate_Silence - p_Right;
            result.DamageRate_Bind = p_Left.DamageRate_Bind - p_Right;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy - p_Right;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff - p_Right;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee - p_Right;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell - p_Right;
            result.AntiDamageRate = p_Left.AntiDamageRate - p_Right;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee - p_Right;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell - p_Right;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical - p_Right;
            result.ResistRate_Curse = p_Left.ResistRate_Curse - p_Right;
            result.ResistRate_Shock = p_Left.ResistRate_Shock - p_Right;
            result.ResistRate_Stun = p_Left.ResistRate_Stun - p_Right;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed - p_Right;
            result.ResistRate_Poison = p_Left.ResistRate_Poison - p_Right;
            result.ResistRate_Burn = p_Left.ResistRate_Burn - p_Right;
            result.ResistRate_Chill = p_Left.ResistRate_Chill - p_Right;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze - p_Right;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse - p_Right;
            result.ResistRate_Blind = p_Left.ResistRate_Blind - p_Right;
            result.ResistRate_Silence = p_Left.ResistRate_Silence - p_Right;
            result.ResistRate_Bind = p_Left.ResistRate_Bind - p_Right;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy - p_Right;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff - p_Right;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery - p_Right;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery - p_Right;
            result.AttackSpeedRate = p_Left.AttackSpeedRate - p_Right;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate - p_Right;
            result.MoveSpeedRate = p_Left.MoveSpeedRate - p_Right;
            result.HitRate = p_Left.HitRate - p_Right;
            result.DodgeRate = p_Left.DodgeRate - p_Right;
            result.CostRate = p_Left.CostRate - p_Right;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate - p_Right;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate - p_Right;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate - p_Right;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate - p_Right;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate - p_Right;
            
            result.CooldownRate = (1f + p_Left.CooldownRate) / (1f + p_Right).ApplyLowerBound(BaseStatusTool.PropertyLowerBound) - 1f;
            
            result.InitFlagMask();
            
            return result;
        }
                
        public static BattleStatusPreset operator-(float p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left - p_Right.Attack_Melee;
            result.Attack_Spell = p_Left - p_Right.Attack_Spell;
            result.ElementEnhance_Fire = p_Left - p_Right.ElementEnhance_Fire;
            result.ElementEnhance_Water = p_Left - p_Right.ElementEnhance_Water;
            result.ElementEnhance_Ground = p_Left - p_Right.ElementEnhance_Ground;
            result.ElementEnhance_Wind = p_Left - p_Right.ElementEnhance_Wind;
            result.ElementEnhance_Light = p_Left - p_Right.ElementEnhance_Light;
            result.ElementEnhance_Darkness = p_Left - p_Right.ElementEnhance_Darkness;
            result.ElementEnhance_All = p_Left - p_Right.ElementEnhance_All;
            result.Defense_Melee = p_Left - p_Right.Defense_Melee;
            result.Defense_Spell = p_Left - p_Right.Defense_Spell;
            result.ElementRegist_Fire = p_Left - p_Right.ElementRegist_Fire;
            result.ElementRegist_Water = p_Left - p_Right.ElementRegist_Water;
            result.ElementRegist_Ground = p_Left - p_Right.ElementRegist_Ground;
            result.ElementRegist_Wind = p_Left - p_Right.ElementRegist_Wind;
            result.ElementRegist_Light = p_Left - p_Right.ElementRegist_Light;
            result.ElementRegist_Darkness = p_Left - p_Right.ElementRegist_Darkness;
            result.ElementRegist_All = p_Left - p_Right.ElementRegist_All;
            result.HP_Base = p_Left - p_Right.HP_Base;
            result.MP_Base = p_Left - p_Right.MP_Base;
            result.HP_Fix_Recovery = p_Left - p_Right.HP_Fix_Recovery;
            result.MP_Fix_Recovery = p_Left - p_Right.MP_Fix_Recovery;
            result.AttackSpeedBasis = p_Left - p_Right.AttackSpeedBasis;
            result.SpellCastSpeedBasis = p_Left - p_Right.SpellCastSpeedBasis;
            result.MoveSpeedBasis = p_Left - p_Right.MoveSpeedBasis;
            result.JumpForce = p_Left - p_Right.JumpForce;
            result.JumpCount = p_Left - p_Right.JumpCount;
            result.SightRange = p_Left - p_Right.SightRange;
            result.Absorb = p_Left - p_Right.Absorb;
            
            result.CriticalRate_Melee = p_Left - p_Right.CriticalRate_Melee;
            result.CriticalRate_Spell = p_Left - p_Right.CriticalRate_Spell;
            result.DamageRate = p_Left - p_Right.DamageRate;
            result.DamageRate_Melee = p_Left - p_Right.DamageRate_Melee;
            result.DamageRate_Spell = p_Left - p_Right.DamageRate_Spell;
            result.DamageRate_Critical = p_Left - p_Right.DamageRate_Critical;
            result.DamageRate_Pierce = p_Left - p_Right.DamageRate_Pierce;
            result.DamageRate_Boss = p_Left - p_Right.DamageRate_Boss;
            result.DamageRate_Curse = p_Left - p_Right.DamageRate_Curse;
            result.DamageRate_Shock = p_Left - p_Right.DamageRate_Shock;
            result.DamageRate_Stun = p_Left - p_Right.DamageRate_Stun;
            result.DamageRate_Bleed = p_Left - p_Right.DamageRate_Bleed;
            result.DamageRate_Poison = p_Left - p_Right.DamageRate_Poison;
            result.DamageRate_Burn = p_Left - p_Right.DamageRate_Burn;
            result.DamageRate_Chill = p_Left - p_Right.DamageRate_Chill;
            result.DamageRate_Freeze = p_Left - p_Right.DamageRate_Freeze;
            result.DamageRate_Confuse = p_Left - p_Right.DamageRate_Confuse;
            result.DamageRate_Blind = p_Left - p_Right.DamageRate_Blind;
            result.DamageRate_Silence = p_Left - p_Right.DamageRate_Silence;
            result.DamageRate_Bind = p_Left - p_Right.DamageRate_Bind;
            result.DamageRate_Groggy = p_Left - p_Right.DamageRate_Groggy;
            result.DamageRate_Debuff = p_Left - p_Right.DamageRate_Debuff;
            result.AntiCriticalRate_Melee = p_Left - p_Right.AntiCriticalRate_Melee;
            result.AntiCriticalRate_Spell = p_Left - p_Right.AntiCriticalRate_Spell;
            result.AntiDamageRate = p_Left - p_Right.AntiDamageRate;
            result.AntiDamageRate_Melee = p_Left - p_Right.AntiDamageRate_Melee;
            result.AntiDamageRate_Spell = p_Left - p_Right.AntiDamageRate_Spell;
            result.AntiDamageRate_Critical = p_Left - p_Right.AntiDamageRate_Critical;
            result.ResistRate_Curse = p_Left - p_Right.ResistRate_Curse;
            result.ResistRate_Shock = p_Left - p_Right.ResistRate_Shock;
            result.ResistRate_Stun = p_Left - p_Right.ResistRate_Stun;
            result.ResistRate_Bleed = p_Left - p_Right.ResistRate_Bleed;
            result.ResistRate_Poison = p_Left - p_Right.ResistRate_Poison;
            result.ResistRate_Burn = p_Left - p_Right.ResistRate_Burn;
            result.ResistRate_Chill = p_Left - p_Right.ResistRate_Chill;
            result.ResistRate_Freeze = p_Left - p_Right.ResistRate_Freeze;
            result.ResistRate_Confuse = p_Left - p_Right.ResistRate_Confuse;
            result.ResistRate_Blind = p_Left - p_Right.ResistRate_Blind;
            result.ResistRate_Silence = p_Left - p_Right.ResistRate_Silence;
            result.ResistRate_Bind = p_Left - p_Right.ResistRate_Bind;
            result.ResistRate_Groggy = p_Left - p_Right.ResistRate_Groggy;
            result.ResistRate_Debuff = p_Left - p_Right.ResistRate_Debuff;
            result.HP_Rate_Recovery = p_Left - p_Right.HP_Rate_Recovery;
            result.MP_Rate_Recovery = p_Left - p_Right.MP_Rate_Recovery;
            result.AttackSpeedRate = p_Left - p_Right.AttackSpeedRate;
            result.SpellCastSpeedRate = p_Left - p_Right.SpellCastSpeedRate;
            result.MoveSpeedRate = p_Left - p_Right.MoveSpeedRate;
            result.HitRate = p_Left - p_Right.HitRate;
            result.DodgeRate = p_Left - p_Right.DodgeRate;
            result.CostRate = p_Left - p_Right.CostRate;
            result.CooldownRecoverySpeedRate = p_Left - p_Right.CooldownRecoverySpeedRate;
            result.HitMotionRecoverySpeedRate = p_Left - p_Right.HitMotionRecoverySpeedRate;
            result.ExpChanceExtraRate = p_Left - p_Right.ExpChanceExtraRate;
            result.GoldChanceExtraRate = p_Left - p_Right.GoldChanceExtraRate;
            result.ItemChanceExtraRate = p_Left - p_Right.ItemChanceExtraRate;
            
            result.CooldownRate = (1f + p_Left) / (1f + p_Right.CooldownRate).ApplyLowerBound(BaseStatusTool.PropertyLowerBound) - 1f;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BattleStatusPreset operator-(BattleStatusPreset p_Left)
        {
            return -1f * p_Left;
        }
        
        public static BattleStatusPreset operator*(BattleStatusPreset p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee * p_Right.Attack_Melee;
            result.Attack_Spell = p_Left.Attack_Spell * p_Right.Attack_Spell;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire * p_Right.ElementEnhance_Fire;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water * p_Right.ElementEnhance_Water;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground * p_Right.ElementEnhance_Ground;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind * p_Right.ElementEnhance_Wind;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light * p_Right.ElementEnhance_Light;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness * p_Right.ElementEnhance_Darkness;
            result.ElementEnhance_All = p_Left.ElementEnhance_All * p_Right.ElementEnhance_All;
            result.Defense_Melee = p_Left.Defense_Melee * p_Right.Defense_Melee;
            result.Defense_Spell = p_Left.Defense_Spell * p_Right.Defense_Spell;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire * p_Right.ElementRegist_Fire;
            result.ElementRegist_Water = p_Left.ElementRegist_Water * p_Right.ElementRegist_Water;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground * p_Right.ElementRegist_Ground;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind * p_Right.ElementRegist_Wind;
            result.ElementRegist_Light = p_Left.ElementRegist_Light * p_Right.ElementRegist_Light;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness * p_Right.ElementRegist_Darkness;
            result.ElementRegist_All = p_Left.ElementRegist_All * p_Right.ElementRegist_All;
            result.HP_Base = p_Left.HP_Base * p_Right.HP_Base;
            result.MP_Base = p_Left.MP_Base * p_Right.MP_Base;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery * p_Right.HP_Fix_Recovery;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery * p_Right.MP_Fix_Recovery;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis * p_Right.AttackSpeedBasis;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis * p_Right.SpellCastSpeedBasis;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis * p_Right.MoveSpeedBasis;
            result.JumpForce = p_Left.JumpForce * p_Right.JumpForce;
            result.JumpCount = p_Left.JumpCount * p_Right.JumpCount;
            result.SightRange = p_Left.SightRange * p_Right.SightRange;
            result.Absorb = p_Left.Absorb * p_Right.Absorb;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee * p_Right.CriticalRate_Melee;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell * p_Right.CriticalRate_Spell;
            result.DamageRate = p_Left.DamageRate * p_Right.DamageRate;
            result.DamageRate_Melee = p_Left.DamageRate_Melee * p_Right.DamageRate_Melee;
            result.DamageRate_Spell = p_Left.DamageRate_Spell * p_Right.DamageRate_Spell;
            result.DamageRate_Critical = p_Left.DamageRate_Critical * p_Right.DamageRate_Critical;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce * p_Right.DamageRate_Pierce;
            result.DamageRate_Boss = p_Left.DamageRate_Boss * p_Right.DamageRate_Boss;
            result.DamageRate_Curse = p_Left.DamageRate_Curse * p_Right.DamageRate_Curse;
            result.DamageRate_Shock = p_Left.DamageRate_Shock * p_Right.DamageRate_Shock;
            result.DamageRate_Stun = p_Left.DamageRate_Stun * p_Right.DamageRate_Stun;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed * p_Right.DamageRate_Bleed;
            result.DamageRate_Poison = p_Left.DamageRate_Poison * p_Right.DamageRate_Poison;
            result.DamageRate_Burn = p_Left.DamageRate_Burn * p_Right.DamageRate_Burn;
            result.DamageRate_Chill = p_Left.DamageRate_Chill * p_Right.DamageRate_Chill;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze * p_Right.DamageRate_Freeze;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse * p_Right.DamageRate_Confuse;
            result.DamageRate_Blind = p_Left.DamageRate_Blind * p_Right.DamageRate_Blind;
            result.DamageRate_Silence = p_Left.DamageRate_Silence * p_Right.DamageRate_Silence;
            result.DamageRate_Bind = p_Left.DamageRate_Bind * p_Right.DamageRate_Bind;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy * p_Right.DamageRate_Groggy;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff * p_Right.DamageRate_Debuff;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee * p_Right.AntiCriticalRate_Melee;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell * p_Right.AntiCriticalRate_Spell;
            result.AntiDamageRate = p_Left.AntiDamageRate * p_Right.AntiDamageRate;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee * p_Right.AntiDamageRate_Melee;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell * p_Right.AntiDamageRate_Spell;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical * p_Right.AntiDamageRate_Critical;
            result.ResistRate_Curse = p_Left.ResistRate_Curse * p_Right.ResistRate_Curse;
            result.ResistRate_Shock = p_Left.ResistRate_Shock * p_Right.ResistRate_Shock;
            result.ResistRate_Stun = p_Left.ResistRate_Stun * p_Right.ResistRate_Stun;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed * p_Right.ResistRate_Bleed;
            result.ResistRate_Poison = p_Left.ResistRate_Poison * p_Right.ResistRate_Poison;
            result.ResistRate_Burn = p_Left.ResistRate_Burn * p_Right.ResistRate_Burn;
            result.ResistRate_Chill = p_Left.ResistRate_Chill * p_Right.ResistRate_Chill;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze * p_Right.ResistRate_Freeze;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse * p_Right.ResistRate_Confuse;
            result.ResistRate_Blind = p_Left.ResistRate_Blind * p_Right.ResistRate_Blind;
            result.ResistRate_Silence = p_Left.ResistRate_Silence * p_Right.ResistRate_Silence;
            result.ResistRate_Bind = p_Left.ResistRate_Bind * p_Right.ResistRate_Bind;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy * p_Right.ResistRate_Groggy;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff * p_Right.ResistRate_Debuff;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery * p_Right.HP_Rate_Recovery;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery * p_Right.MP_Rate_Recovery;
            result.AttackSpeedRate = p_Left.AttackSpeedRate * p_Right.AttackSpeedRate;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate * p_Right.SpellCastSpeedRate;
            result.MoveSpeedRate = p_Left.MoveSpeedRate * p_Right.MoveSpeedRate;
            result.HitRate = p_Left.HitRate * p_Right.HitRate;
            result.DodgeRate = p_Left.DodgeRate * p_Right.DodgeRate;
            result.CostRate = p_Left.CostRate * p_Right.CostRate;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate * p_Right.CooldownRecoverySpeedRate;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate * p_Right.HitMotionRecoverySpeedRate;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate * p_Right.ExpChanceExtraRate;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate * p_Right.GoldChanceExtraRate;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate * p_Right.ItemChanceExtraRate;
            
            result.CooldownRate = p_Left.CooldownRate * p_Right.CooldownRate;
   
            result.InitFlagMask();

            return result;
        }
        
        public static BattleStatusPreset operator*(BattleStatusPreset p_Left, float p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee * p_Right;
            result.Attack_Spell = p_Left.Attack_Spell * p_Right;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire * p_Right;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water * p_Right;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground * p_Right;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind * p_Right;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light * p_Right;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness * p_Right;
            result.ElementEnhance_All = p_Left.ElementEnhance_All * p_Right;
            result.Defense_Melee = p_Left.Defense_Melee * p_Right;
            result.Defense_Spell = p_Left.Defense_Spell * p_Right;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire * p_Right;
            result.ElementRegist_Water = p_Left.ElementRegist_Water * p_Right;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground * p_Right;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind * p_Right;
            result.ElementRegist_Light = p_Left.ElementRegist_Light * p_Right;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness * p_Right;
            result.ElementRegist_All = p_Left.ElementRegist_All * p_Right;
            result.HP_Base = p_Left.HP_Base * p_Right;
            result.MP_Base = p_Left.MP_Base * p_Right;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery * p_Right;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery * p_Right;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis * p_Right;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis * p_Right;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis * p_Right;
            result.JumpForce = p_Left.JumpForce * p_Right;
            result.JumpCount = p_Left.JumpCount * p_Right;
            result.SightRange = p_Left.SightRange * p_Right;
            result.Absorb = p_Left.Absorb * p_Right;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee * p_Right;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell * p_Right;
            result.DamageRate = p_Left.DamageRate * p_Right;
            result.DamageRate_Melee = p_Left.DamageRate_Melee * p_Right;
            result.DamageRate_Spell = p_Left.DamageRate_Spell * p_Right;
            result.DamageRate_Critical = p_Left.DamageRate_Critical * p_Right;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce * p_Right;
            result.DamageRate_Boss = p_Left.DamageRate_Boss * p_Right;
            result.DamageRate_Curse = p_Left.DamageRate_Curse * p_Right;
            result.DamageRate_Shock = p_Left.DamageRate_Shock * p_Right;
            result.DamageRate_Stun = p_Left.DamageRate_Stun * p_Right;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed * p_Right;
            result.DamageRate_Poison = p_Left.DamageRate_Poison * p_Right;
            result.DamageRate_Burn = p_Left.DamageRate_Burn * p_Right;
            result.DamageRate_Chill = p_Left.DamageRate_Chill * p_Right;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze * p_Right;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse * p_Right;
            result.DamageRate_Blind = p_Left.DamageRate_Blind * p_Right;
            result.DamageRate_Silence = p_Left.DamageRate_Silence * p_Right;
            result.DamageRate_Bind = p_Left.DamageRate_Bind * p_Right;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy * p_Right;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff * p_Right;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee * p_Right;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell * p_Right;
            result.AntiDamageRate = p_Left.AntiDamageRate * p_Right;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee * p_Right;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell * p_Right;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical * p_Right;
            result.ResistRate_Curse = p_Left.ResistRate_Curse * p_Right;
            result.ResistRate_Shock = p_Left.ResistRate_Shock * p_Right;
            result.ResistRate_Stun = p_Left.ResistRate_Stun * p_Right;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed * p_Right;
            result.ResistRate_Poison = p_Left.ResistRate_Poison * p_Right;
            result.ResistRate_Burn = p_Left.ResistRate_Burn * p_Right;
            result.ResistRate_Chill = p_Left.ResistRate_Chill * p_Right;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze * p_Right;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse * p_Right;
            result.ResistRate_Blind = p_Left.ResistRate_Blind * p_Right;
            result.ResistRate_Silence = p_Left.ResistRate_Silence * p_Right;
            result.ResistRate_Bind = p_Left.ResistRate_Bind * p_Right;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy * p_Right;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff * p_Right;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery * p_Right;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery * p_Right;
            result.AttackSpeedRate = p_Left.AttackSpeedRate * p_Right;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate * p_Right;
            result.MoveSpeedRate = p_Left.MoveSpeedRate * p_Right;
            result.HitRate = p_Left.HitRate * p_Right;
            result.DodgeRate = p_Left.DodgeRate * p_Right;
            result.CostRate = p_Left.CostRate * p_Right;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate * p_Right;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate * p_Right;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate * p_Right;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate * p_Right;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate * p_Right;
            
            result.CooldownRate = p_Left.CooldownRate * p_Right;
            
            result.InitFlagMask();

            return result;
        }

        public static BattleStatusPreset operator*(float p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left * p_Right.Attack_Melee;
            result.Attack_Spell = p_Left * p_Right.Attack_Spell;
            result.ElementEnhance_Fire = p_Left * p_Right.ElementEnhance_Fire;
            result.ElementEnhance_Water = p_Left * p_Right.ElementEnhance_Water;
            result.ElementEnhance_Ground = p_Left * p_Right.ElementEnhance_Ground;
            result.ElementEnhance_Wind = p_Left * p_Right.ElementEnhance_Wind;
            result.ElementEnhance_Light = p_Left * p_Right.ElementEnhance_Light;
            result.ElementEnhance_Darkness = p_Left * p_Right.ElementEnhance_Darkness;
            result.ElementEnhance_All = p_Left * p_Right.ElementEnhance_All;
            result.Defense_Melee = p_Left * p_Right.Defense_Melee;
            result.Defense_Spell = p_Left * p_Right.Defense_Spell;
            result.ElementRegist_Fire = p_Left * p_Right.ElementRegist_Fire;
            result.ElementRegist_Water = p_Left * p_Right.ElementRegist_Water;
            result.ElementRegist_Ground = p_Left * p_Right.ElementRegist_Ground;
            result.ElementRegist_Wind = p_Left * p_Right.ElementRegist_Wind;
            result.ElementRegist_Light = p_Left * p_Right.ElementRegist_Light;
            result.ElementRegist_Darkness = p_Left * p_Right.ElementRegist_Darkness;
            result.ElementRegist_All = p_Left * p_Right.ElementRegist_All;
            result.HP_Base = p_Left * p_Right.HP_Base;
            result.MP_Base = p_Left * p_Right.MP_Base;
            result.HP_Fix_Recovery = p_Left * p_Right.HP_Fix_Recovery;
            result.MP_Fix_Recovery = p_Left * p_Right.MP_Fix_Recovery;
            result.AttackSpeedBasis = p_Left * p_Right.AttackSpeedBasis;
            result.SpellCastSpeedBasis = p_Left * p_Right.SpellCastSpeedBasis;
            result.MoveSpeedBasis = p_Left * p_Right.MoveSpeedBasis;
            result.JumpForce = p_Left * p_Right.JumpForce;
            result.JumpCount = p_Left * p_Right.JumpCount;
            result.SightRange = p_Left * p_Right.SightRange;
            result.Absorb = p_Left * p_Right.Absorb;
            
            result.CriticalRate_Melee = p_Left * p_Right.CriticalRate_Melee;
            result.CriticalRate_Spell = p_Left * p_Right.CriticalRate_Spell;
            result.DamageRate = p_Left * p_Right.DamageRate;
            result.DamageRate_Melee = p_Left * p_Right.DamageRate_Melee;
            result.DamageRate_Spell = p_Left * p_Right.DamageRate_Spell;
            result.DamageRate_Critical = p_Left * p_Right.DamageRate_Critical;
            result.DamageRate_Pierce = p_Left * p_Right.DamageRate_Pierce;
            result.DamageRate_Boss = p_Left * p_Right.DamageRate_Boss;
            result.DamageRate_Curse = p_Left * p_Right.DamageRate_Curse;
            result.DamageRate_Shock = p_Left * p_Right.DamageRate_Shock;
            result.DamageRate_Stun = p_Left * p_Right.DamageRate_Stun;
            result.DamageRate_Bleed = p_Left * p_Right.DamageRate_Bleed;
            result.DamageRate_Poison = p_Left * p_Right.DamageRate_Poison;
            result.DamageRate_Burn = p_Left * p_Right.DamageRate_Burn;
            result.DamageRate_Chill = p_Left * p_Right.DamageRate_Chill;
            result.DamageRate_Freeze = p_Left * p_Right.DamageRate_Freeze;
            result.DamageRate_Confuse = p_Left * p_Right.DamageRate_Confuse;
            result.DamageRate_Blind = p_Left * p_Right.DamageRate_Blind;
            result.DamageRate_Bind = p_Left * p_Right.DamageRate_Bind;
            result.DamageRate_Silence = p_Left * p_Right.DamageRate_Silence;
            result.DamageRate_Groggy = p_Left * p_Right.DamageRate_Groggy;
            result.DamageRate_Debuff = p_Left * p_Right.DamageRate_Debuff;
            result.AntiCriticalRate_Melee = p_Left * p_Right.AntiCriticalRate_Melee;
            result.AntiCriticalRate_Spell = p_Left * p_Right.AntiCriticalRate_Spell;
            result.AntiDamageRate = p_Left * p_Right.AntiDamageRate;
            result.AntiDamageRate_Melee = p_Left * p_Right.AntiDamageRate_Melee;
            result.AntiDamageRate_Spell = p_Left * p_Right.AntiDamageRate_Spell;
            result.AntiDamageRate_Critical = p_Left * p_Right.AntiDamageRate_Critical;
            result.ResistRate_Curse = p_Left * p_Right.ResistRate_Curse;
            result.ResistRate_Shock = p_Left * p_Right.ResistRate_Shock;
            result.ResistRate_Stun = p_Left * p_Right.ResistRate_Stun;
            result.ResistRate_Bleed = p_Left * p_Right.ResistRate_Bleed;
            result.ResistRate_Poison = p_Left * p_Right.ResistRate_Poison;
            result.ResistRate_Burn = p_Left * p_Right.ResistRate_Burn;
            result.ResistRate_Chill = p_Left * p_Right.ResistRate_Chill;
            result.ResistRate_Freeze = p_Left * p_Right.ResistRate_Freeze;
            result.ResistRate_Confuse = p_Left * p_Right.ResistRate_Confuse;
            result.ResistRate_Blind = p_Left * p_Right.ResistRate_Blind;
            result.ResistRate_Bind = p_Left * p_Right.ResistRate_Bind;
            result.ResistRate_Silence = p_Left * p_Right.ResistRate_Silence;
            result.ResistRate_Groggy = p_Left * p_Right.ResistRate_Groggy;
            result.ResistRate_Debuff = p_Left * p_Right.ResistRate_Debuff;
            result.HP_Rate_Recovery = p_Left * p_Right.HP_Rate_Recovery;
            result.MP_Rate_Recovery = p_Left * p_Right.MP_Rate_Recovery;
            result.AttackSpeedRate = p_Left * p_Right.AttackSpeedRate;
            result.SpellCastSpeedRate = p_Left * p_Right.SpellCastSpeedRate;
            result.MoveSpeedRate = p_Left * p_Right.MoveSpeedRate;
            result.HitRate = p_Left * p_Right.HitRate;
            result.DodgeRate = p_Left * p_Right.DodgeRate;
            result.CostRate = p_Left * p_Right.CostRate;
            result.CooldownRecoverySpeedRate = p_Left * p_Right.CooldownRecoverySpeedRate;
            result.HitMotionRecoverySpeedRate = p_Left * p_Right.HitMotionRecoverySpeedRate;
            result.ExpChanceExtraRate = p_Left * p_Right.ExpChanceExtraRate;
            result.GoldChanceExtraRate = p_Left * p_Right.GoldChanceExtraRate;
            result.ItemChanceExtraRate = p_Left * p_Right.ItemChanceExtraRate;
            
            result.CooldownRate = p_Left * p_Right.CooldownRate;
            
            result.InitFlagMask();

            return result;
        }

        public static BattleStatusPreset operator/(BattleStatusPreset p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left.Attack_Melee / p_Right.Attack_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Attack_Spell = p_Left.Attack_Spell / p_Right.Attack_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire / p_Right.ElementEnhance_Fire.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water / p_Right.ElementEnhance_Water.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground / p_Right.ElementEnhance_Ground.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind / p_Right.ElementEnhance_Wind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light / p_Right.ElementEnhance_Light.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness / p_Right.ElementEnhance_Darkness.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_All = p_Left.ElementEnhance_All / p_Right.ElementEnhance_All.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Defense_Melee = p_Left.Defense_Melee / p_Right.Defense_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Defense_Spell = p_Left.Defense_Spell / p_Right.Defense_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire / p_Right.ElementRegist_Fire.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Water = p_Left.ElementRegist_Water / p_Right.ElementRegist_Water.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground / p_Right.ElementRegist_Ground.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind / p_Right.ElementRegist_Wind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Light = p_Left.ElementRegist_Light / p_Right.ElementRegist_Light.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness / p_Right.ElementRegist_Darkness.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_All = p_Left.ElementRegist_All / p_Right.ElementRegist_All.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HP_Base = p_Left.HP_Base / p_Right.HP_Base.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MP_Base = p_Left.MP_Base / p_Right.MP_Base.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery / p_Right.HP_Fix_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery / p_Right.MP_Fix_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis / p_Right.AttackSpeedBasis.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis / p_Right.SpellCastSpeedBasis.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis / p_Right.MoveSpeedBasis.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.JumpForce = p_Left.JumpForce / p_Right.JumpForce.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.JumpCount = p_Left.JumpCount / p_Right.JumpCount.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.SightRange = p_Left.SightRange / p_Right.SightRange.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Absorb = p_Left.Absorb / p_Right.Absorb.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee / p_Right.CriticalRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell / p_Right.CriticalRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate = p_Left.DamageRate / p_Right.DamageRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Melee = p_Left.DamageRate_Melee / p_Right.DamageRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Spell = p_Left.DamageRate_Spell / p_Right.DamageRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Critical = p_Left.DamageRate_Critical / p_Right.DamageRate_Critical.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce / p_Right.DamageRate_Pierce.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Boss = p_Left.DamageRate_Boss / p_Right.DamageRate_Boss.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Curse = p_Left.DamageRate_Curse / p_Right.DamageRate_Curse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Shock = p_Left.DamageRate_Shock / p_Right.DamageRate_Shock.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Stun = p_Left.DamageRate_Stun / p_Right.DamageRate_Stun.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed / p_Right.DamageRate_Bleed.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Poison = p_Left.DamageRate_Poison / p_Right.DamageRate_Poison.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Burn = p_Left.DamageRate_Burn / p_Right.DamageRate_Burn.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Chill = p_Left.DamageRate_Chill / p_Right.DamageRate_Chill.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze / p_Right.DamageRate_Freeze.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse / p_Right.DamageRate_Confuse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Blind = p_Left.DamageRate_Blind / p_Right.DamageRate_Blind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Bind = p_Left.DamageRate_Bind / p_Right.DamageRate_Bind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Silence = p_Left.DamageRate_Silence / p_Right.DamageRate_Silence.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy / p_Right.DamageRate_Groggy.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff / p_Right.DamageRate_Debuff.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee / p_Right.AntiCriticalRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell / p_Right.AntiCriticalRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate = p_Left.AntiDamageRate / p_Right.AntiDamageRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee / p_Right.AntiDamageRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell / p_Right.AntiDamageRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical / p_Right.AntiDamageRate_Critical.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Curse = p_Left.ResistRate_Curse / p_Right.ResistRate_Curse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Shock = p_Left.ResistRate_Shock / p_Right.ResistRate_Shock.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Stun = p_Left.ResistRate_Stun / p_Right.ResistRate_Stun.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed / p_Right.ResistRate_Bleed.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Poison = p_Left.ResistRate_Poison / p_Right.ResistRate_Poison.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Burn = p_Left.ResistRate_Burn / p_Right.ResistRate_Burn.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Chill = p_Left.ResistRate_Chill / p_Right.ResistRate_Chill.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze / p_Right.ResistRate_Freeze.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse / p_Right.ResistRate_Confuse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Blind = p_Left.ResistRate_Blind / p_Right.ResistRate_Blind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Bind = p_Left.ResistRate_Bind / p_Right.ResistRate_Bind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Silence = p_Left.ResistRate_Silence / p_Right.ResistRate_Silence.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy / p_Right.ResistRate_Groggy.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff / p_Right.ResistRate_Debuff.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery / p_Right.HP_Rate_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery / p_Right.MP_Rate_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AttackSpeedRate = p_Left.AttackSpeedRate / p_Right.AttackSpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate / p_Right.SpellCastSpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MoveSpeedRate = p_Left.MoveSpeedRate / p_Right.MoveSpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HitRate = p_Left.HitRate / p_Right.HitRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DodgeRate = p_Left.DodgeRate / p_Right.DodgeRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.CostRate = p_Left.CostRate / p_Right.CostRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate / p_Right.CooldownRecoverySpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate / p_Right.HitMotionRecoverySpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate / p_Right.ExpChanceExtraRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate / p_Right.GoldChanceExtraRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate / p_Right.ItemChanceExtraRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            
            result.CooldownRate = p_Left.CooldownRate / p_Right.CooldownRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
   
            result.InitFlagMask();

            return result;
        }

        public static BattleStatusPreset operator/(BattleStatusPreset p_Left, float p_Right)
        {
            var result = new BattleStatusPreset();
            p_Right = p_Right.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);

            result.Attack_Melee = p_Left.Attack_Melee / p_Right;
            result.Attack_Spell = p_Left.Attack_Spell / p_Right;
            result.ElementEnhance_Fire = p_Left.ElementEnhance_Fire / p_Right;
            result.ElementEnhance_Water = p_Left.ElementEnhance_Water / p_Right;
            result.ElementEnhance_Ground = p_Left.ElementEnhance_Ground / p_Right;
            result.ElementEnhance_Wind = p_Left.ElementEnhance_Wind / p_Right;
            result.ElementEnhance_Light = p_Left.ElementEnhance_Light / p_Right;
            result.ElementEnhance_Darkness = p_Left.ElementEnhance_Darkness / p_Right;
            result.ElementEnhance_All = p_Left.ElementEnhance_All / p_Right;
            result.Defense_Melee = p_Left.Defense_Melee / p_Right;
            result.Defense_Spell = p_Left.Defense_Spell / p_Right;
            result.ElementRegist_Fire = p_Left.ElementRegist_Fire / p_Right;
            result.ElementRegist_Water = p_Left.ElementRegist_Water / p_Right;
            result.ElementRegist_Ground = p_Left.ElementRegist_Ground / p_Right;
            result.ElementRegist_Wind = p_Left.ElementRegist_Wind / p_Right;
            result.ElementRegist_Light = p_Left.ElementRegist_Light / p_Right;
            result.ElementRegist_Darkness = p_Left.ElementRegist_Darkness / p_Right;
            result.ElementRegist_All = p_Left.ElementRegist_All / p_Right;
            result.HP_Base = p_Left.HP_Base / p_Right;
            result.MP_Base = p_Left.MP_Base / p_Right;
            result.HP_Fix_Recovery = p_Left.HP_Fix_Recovery / p_Right;
            result.MP_Fix_Recovery = p_Left.MP_Fix_Recovery / p_Right;
            result.AttackSpeedBasis = p_Left.AttackSpeedBasis / p_Right;
            result.SpellCastSpeedBasis = p_Left.SpellCastSpeedBasis / p_Right;
            result.MoveSpeedBasis = p_Left.MoveSpeedBasis / p_Right;
            result.JumpForce = p_Left.JumpForce / p_Right;
            result.JumpCount = p_Left.JumpCount / p_Right;
            result.SightRange = p_Left.SightRange / p_Right;
            result.Absorb = p_Left.Absorb / p_Right;
            
            result.CriticalRate_Melee = p_Left.CriticalRate_Melee / p_Right;
            result.CriticalRate_Spell = p_Left.CriticalRate_Spell / p_Right;
            result.DamageRate = p_Left.DamageRate / p_Right;
            result.DamageRate_Melee = p_Left.DamageRate_Melee / p_Right;
            result.DamageRate_Spell = p_Left.DamageRate_Spell / p_Right;
            result.DamageRate_Critical = p_Left.DamageRate_Critical / p_Right;
            result.DamageRate_Pierce = p_Left.DamageRate_Pierce / p_Right;
            result.DamageRate_Boss = p_Left.DamageRate_Boss / p_Right;
            result.DamageRate_Curse = p_Left.DamageRate_Curse / p_Right;
            result.DamageRate_Shock = p_Left.DamageRate_Shock / p_Right;
            result.DamageRate_Stun = p_Left.DamageRate_Stun / p_Right;
            result.DamageRate_Bleed = p_Left.DamageRate_Bleed / p_Right;
            result.DamageRate_Poison = p_Left.DamageRate_Poison / p_Right;
            result.DamageRate_Burn = p_Left.DamageRate_Burn / p_Right;
            result.DamageRate_Chill = p_Left.DamageRate_Chill / p_Right;
            result.DamageRate_Freeze = p_Left.DamageRate_Freeze / p_Right;
            result.DamageRate_Confuse = p_Left.DamageRate_Confuse / p_Right;
            result.DamageRate_Blind = p_Left.DamageRate_Blind / p_Right;
            result.DamageRate_Bind = p_Left.DamageRate_Bind / p_Right;
            result.DamageRate_Silence = p_Left.DamageRate_Silence / p_Right;
            result.DamageRate_Groggy = p_Left.DamageRate_Groggy / p_Right;
            result.DamageRate_Debuff = p_Left.DamageRate_Debuff / p_Right;
            result.AntiCriticalRate_Melee = p_Left.AntiCriticalRate_Melee / p_Right;
            result.AntiCriticalRate_Spell = p_Left.AntiCriticalRate_Spell / p_Right;
            result.AntiDamageRate = p_Left.AntiDamageRate / p_Right;
            result.AntiDamageRate_Melee = p_Left.AntiDamageRate_Melee / p_Right;
            result.AntiDamageRate_Spell = p_Left.AntiDamageRate_Spell / p_Right;
            result.AntiDamageRate_Critical = p_Left.AntiDamageRate_Critical / p_Right;
            result.ResistRate_Curse = p_Left.ResistRate_Curse / p_Right;
            result.ResistRate_Shock = p_Left.ResistRate_Shock / p_Right;
            result.ResistRate_Stun = p_Left.ResistRate_Stun / p_Right;
            result.ResistRate_Bleed = p_Left.ResistRate_Bleed / p_Right;
            result.ResistRate_Poison = p_Left.ResistRate_Poison / p_Right;
            result.ResistRate_Burn = p_Left.ResistRate_Burn / p_Right;
            result.ResistRate_Chill = p_Left.ResistRate_Chill / p_Right;
            result.ResistRate_Freeze = p_Left.ResistRate_Freeze / p_Right;
            result.ResistRate_Confuse = p_Left.ResistRate_Confuse / p_Right;
            result.ResistRate_Blind = p_Left.ResistRate_Blind / p_Right;
            result.ResistRate_Bind = p_Left.ResistRate_Bind / p_Right;
            result.ResistRate_Silence = p_Left.ResistRate_Silence / p_Right;
            result.ResistRate_Groggy = p_Left.ResistRate_Groggy / p_Right;
            result.ResistRate_Debuff = p_Left.ResistRate_Debuff / p_Right;
            result.HP_Rate_Recovery = p_Left.HP_Rate_Recovery / p_Right;
            result.MP_Rate_Recovery = p_Left.MP_Rate_Recovery / p_Right;
            result.AttackSpeedRate = p_Left.AttackSpeedRate / p_Right;
            result.SpellCastSpeedRate = p_Left.SpellCastSpeedRate / p_Right;
            result.MoveSpeedRate = p_Left.MoveSpeedRate / p_Right;
            result.HitRate = p_Left.HitRate / p_Right;
            result.DodgeRate = p_Left.DodgeRate / p_Right;
            result.CostRate = p_Left.CostRate / p_Right;
            result.CooldownRecoverySpeedRate = p_Left.CooldownRecoverySpeedRate / p_Right;
            result.HitMotionRecoverySpeedRate = p_Left.HitMotionRecoverySpeedRate / p_Right;
            result.ExpChanceExtraRate = p_Left.ExpChanceExtraRate / p_Right;
            result.GoldChanceExtraRate = p_Left.GoldChanceExtraRate / p_Right;
            result.ItemChanceExtraRate = p_Left.ItemChanceExtraRate / p_Right;
            
            result.CooldownRate = p_Left.CooldownRate / p_Right;
            
            result.InitFlagMask();
            
            return result;
        }
        
        public static BattleStatusPreset operator/(float p_Left, BattleStatusPreset p_Right)
        {
            var result = new BattleStatusPreset();

            result.Attack_Melee = p_Left / p_Right.Attack_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Attack_Spell = p_Left / p_Right.Attack_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Fire = p_Left / p_Right.ElementEnhance_Fire.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Water = p_Left / p_Right.ElementEnhance_Water.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Ground = p_Left / p_Right.ElementEnhance_Ground.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Wind = p_Left / p_Right.ElementEnhance_Wind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Light = p_Left / p_Right.ElementEnhance_Light.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_Darkness = p_Left / p_Right.ElementEnhance_Darkness.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementEnhance_All = p_Left / p_Right.ElementEnhance_All.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Defense_Melee = p_Left / p_Right.Defense_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Defense_Spell = p_Left / p_Right.Defense_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Fire = p_Left / p_Right.ElementRegist_Fire.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Water = p_Left / p_Right.ElementRegist_Water.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Ground = p_Left / p_Right.ElementRegist_Ground.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Wind = p_Left / p_Right.ElementRegist_Wind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Light = p_Left / p_Right.ElementRegist_Light.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_Darkness = p_Left / p_Right.ElementRegist_Darkness.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ElementRegist_All = p_Left / p_Right.ElementRegist_All.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HP_Base = p_Left / p_Right.HP_Base.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MP_Base = p_Left / p_Right.MP_Base.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HP_Fix_Recovery = p_Left / p_Right.HP_Fix_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MP_Fix_Recovery = p_Left / p_Right.MP_Fix_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AttackSpeedBasis = p_Left / p_Right.AttackSpeedBasis.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.SpellCastSpeedBasis = p_Left / p_Right.SpellCastSpeedBasis.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MoveSpeedBasis = p_Left / p_Right.MoveSpeedBasis.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.JumpForce = p_Left / p_Right.JumpForce.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.JumpCount = p_Left / p_Right.JumpCount.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.SightRange = p_Left / p_Right.SightRange.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.Absorb = p_Left / p_Right.Absorb.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            
            result.CriticalRate_Melee = p_Left / p_Right.CriticalRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.CriticalRate_Spell = p_Left / p_Right.CriticalRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate = p_Left / p_Right.DamageRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Melee = p_Left / p_Right.DamageRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Spell = p_Left / p_Right.DamageRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Critical = p_Left / p_Right.DamageRate_Critical.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Pierce = p_Left / p_Right.DamageRate_Pierce.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Boss = p_Left / p_Right.DamageRate_Boss.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Curse = p_Left / p_Right.DamageRate_Curse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Shock = p_Left / p_Right.DamageRate_Shock.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Stun = p_Left / p_Right.DamageRate_Stun.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Bleed = p_Left / p_Right.DamageRate_Bleed.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Poison = p_Left / p_Right.DamageRate_Poison.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Burn = p_Left / p_Right.DamageRate_Burn.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Chill = p_Left / p_Right.DamageRate_Chill.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Freeze = p_Left / p_Right.DamageRate_Freeze.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Confuse = p_Left / p_Right.DamageRate_Confuse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Blind = p_Left / p_Right.DamageRate_Blind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Bind = p_Left / p_Right.DamageRate_Bind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Silence = p_Left / p_Right.DamageRate_Silence.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Groggy = p_Left / p_Right.DamageRate_Groggy.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DamageRate_Debuff = p_Left / p_Right.DamageRate_Debuff.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiCriticalRate_Melee = p_Left / p_Right.AntiCriticalRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiCriticalRate_Spell = p_Left / p_Right.AntiCriticalRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate = p_Left / p_Right.AntiDamageRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate_Melee = p_Left / p_Right.AntiDamageRate_Melee.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate_Spell = p_Left / p_Right.AntiDamageRate_Spell.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AntiDamageRate_Critical = p_Left / p_Right.AntiDamageRate_Critical.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Curse = p_Left / p_Right.ResistRate_Curse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Shock = p_Left / p_Right.ResistRate_Shock.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Stun = p_Left / p_Right.ResistRate_Stun.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Bleed = p_Left / p_Right.ResistRate_Bleed.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Poison = p_Left / p_Right.ResistRate_Poison.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Burn = p_Left / p_Right.ResistRate_Burn.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Chill = p_Left / p_Right.ResistRate_Chill.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Freeze = p_Left / p_Right.ResistRate_Freeze.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Confuse = p_Left / p_Right.ResistRate_Confuse.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Blind = p_Left / p_Right.ResistRate_Blind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Bind = p_Left / p_Right.ResistRate_Bind.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Silence = p_Left / p_Right.ResistRate_Silence.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Groggy = p_Left / p_Right.ResistRate_Groggy.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ResistRate_Debuff = p_Left / p_Right.ResistRate_Debuff.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HP_Rate_Recovery = p_Left / p_Right.HP_Rate_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MP_Rate_Recovery = p_Left / p_Right.MP_Rate_Recovery.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.AttackSpeedRate = p_Left / p_Right.AttackSpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.SpellCastSpeedRate = p_Left / p_Right.SpellCastSpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.MoveSpeedRate = p_Left / p_Right.MoveSpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HitRate = p_Left / p_Right.HitRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.DodgeRate = p_Left / p_Right.DodgeRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.CostRate = p_Left / p_Right.CostRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.CooldownRecoverySpeedRate = p_Left / p_Right.CooldownRecoverySpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.HitMotionRecoverySpeedRate = p_Left / p_Right.HitMotionRecoverySpeedRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ExpChanceExtraRate = p_Left / p_Right.ExpChanceExtraRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.GoldChanceExtraRate = p_Left / p_Right.GoldChanceExtraRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.ItemChanceExtraRate = p_Left / p_Right.ItemChanceExtraRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            
            result.CooldownRate = p_Left / p_Right.CooldownRate.ApplyLowerBound(BaseStatusTool.PropertyLowerBound);
            result.InitFlagMask();
            
            return result;
        }

        public static BattleStatusPreset operator!(BattleStatusPreset p_Left)
        {
            return -1f * p_Left;
        }
        
#if UNITY_EDITOR
        public override string ToString()
        {
            var result = "";
            foreach (var battleStatusType1 in BattleStatusTool.BattleStatusType1Enumerator)
            {
                if (FlagMask.HasFlag(battleStatusType1))
                {
                    var property = GetProperty(battleStatusType1);
                    result += $"[{battleStatusType1} : {property}]\n";
                }
            }
            
            foreach (var battleStatusType2 in BattleStatusTool.BattleStatusType2Enumerator)
            {
                if (FlagMask.HasFlag(battleStatusType2))
                {
                    var property = GetProperty(battleStatusType2);
                    result += $"[{battleStatusType2} : {property}]\n";
                }
            }
            
            foreach (var battleStatusType3 in BattleStatusTool.BattleStatusType3Enumerator)
            {
                if (FlagMask.HasFlag(battleStatusType3))
                {
                    var property = GetProperty(battleStatusType3);
                    result += $"[{battleStatusType3} : {property}]\n";
                }
            }
            
            return string.IsNullOrEmpty(result) ? "유효한 스탯 없음!" : result;
        }
#endif

        #endregion

        #region <Methods>
        
        private void InitFlagMask()
        {
            FlagMask = default;
            
            foreach (var battleStatusType1 in BattleStatusTool.BattleStatusType1Enumerator)
            {
                var property = GetProperty(battleStatusType1);
                if (property.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(battleStatusType1);
                }
                else
                {
                    FlagMask.AddFlag(battleStatusType1);
                }
            }
            foreach (var battleStatusType2 in BattleStatusTool.BattleStatusType2Enumerator)
            {
                var property = GetProperty(battleStatusType2);
                if (property.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(battleStatusType2);
                }
                else
                {
                    FlagMask.AddFlag(battleStatusType2);
                }
            }
            foreach (var battleStatusType3 in BattleStatusTool.BattleStatusType3Enumerator)
            {
                var property = GetProperty(battleStatusType3);
                if (property.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(battleStatusType3);
                }
                else
                {
                    FlagMask.AddFlag(battleStatusType3);
                }
            }
        }

        #endregion

        #region <Method/Query>

        public bool HasProperValue()
        {
            return FlagMask.HasAnyFlag();
        }
        
        public bool HasProperValue(BattleStatusTool.BattleStatusType p_Type)
        {
            return FlagMask.HasFlag(p_Type);
        }
        
        public bool HasProperValue(BattleStatusTool.BattleStatusType1 p_Type)
        {
            return FlagMask.HasFlag(p_Type);
        }
        
        public bool HasProperValue(BattleStatusTool.BattleStatusType2 p_Type)
        {
            return FlagMask.HasFlag(p_Type);
        }
        
        public bool HasProperValue(BattleStatusTool.BattleStatusType3 p_Type)
        {
            return FlagMask.HasFlag(p_Type);
        }

        #endregion

        #region <Method/Query/Add>
        
        public void AddProperty(BattleStatusTool.BattleStatusType p_Type, float p_Value)
        {
            var appliedProperty = GetProperty(p_Type) + p_Value;
            SetProperty(p_Type, appliedProperty);
        }
        
        public void AddProperty(BattleStatusTool.BattleStatusType1 p_Type, float p_Value)
        {
            var appliedProperty = GetProperty(p_Type) + p_Value;
            SetProperty(p_Type, appliedProperty);
        }
        
        public void AddProperty(BattleStatusTool.BattleStatusType2 p_Type, float p_Value)
        {
            var appliedProperty = GetProperty(p_Type) + p_Value;
            SetProperty(p_Type, appliedProperty);
        }
        
        public void AddProperty(BattleStatusTool.BattleStatusType3 p_Type, float p_Value)
        {
            var appliedProperty = GetProperty(p_Type) + p_Value;
            SetProperty(p_Type, appliedProperty);
        }

        #endregion

        #region <Method/Query/Set>
        
        public void SetProperty(BattleStatusTool.BattleStatusType p_Type, float p_Value)
        {
            if (p_Type != BattleStatusTool.BattleStatusType.None)
            {
                switch (p_Type)
                {
                    default:
                        break;
                    case BattleStatusTool.BattleStatusType.Attack_Melee:
                    {
                        Attack_Melee = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.Attack_Melee);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.Attack_Melee);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.Attack_Spell:
                    {
                        Attack_Spell = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.Attack_Spell);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.Attack_Spell);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.CriticalRate_Melee:
                    {
                        CriticalRate_Melee = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.CriticalRate_Melee);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.CriticalRate_Melee);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.CriticalRate_Spell:
                    {
                        CriticalRate_Spell = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.CriticalRate_Spell);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.CriticalRate_Spell);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate:
                    {
                        DamageRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Melee:
                    {
                        DamageRate_Melee = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Melee);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Melee);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Spell:
                    {
                        DamageRate_Spell = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Spell);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Spell);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Critical:
                    {
                        DamageRate_Critical = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Critical);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Critical);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Pierce:
                    {
                        DamageRate_Pierce = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Pierce);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Pierce);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Boss:
                    {
                        DamageRate_Boss = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Boss);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Boss);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Curse:
                    {
                        DamageRate_Curse = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Curse);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Curse);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Shock:
                    {
                        DamageRate_Shock = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Shock);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Shock);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Stun:
                    {
                        DamageRate_Stun = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Stun);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Stun);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Bleed:
                    {
                        DamageRate_Bleed = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Bleed);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Bleed);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Poison:
                    {
                        DamageRate_Poison = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Poison);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Poison);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Burn:
                    {
                        DamageRate_Burn = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Burn);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Burn);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Chill:
                    {
                        DamageRate_Chill = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Chill);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Chill);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Freeze:
                    {
                        DamageRate_Freeze = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Freeze);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Freeze);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Confuse:
                    {
                        DamageRate_Confuse = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Confuse);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Confuse);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Blind:
                    {
                        DamageRate_Blind = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Blind);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Blind);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Bind:
                    {
                        DamageRate_Bind = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Bind);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Bind);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Silence:
                    {
                        DamageRate_Silence = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Silence);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Silence);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Groggy:
                    {
                        DamageRate_Groggy = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Groggy);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Groggy);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DamageRate_Debuff:
                    {
                        DamageRate_Debuff = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.DamageRate_Debuff);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.DamageRate_Debuff);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_Fire:
                    {
                        ElementEnhance_Fire = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Fire);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Fire);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_Water:
                    {
                        ElementEnhance_Water = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Water);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Water);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_Ground:
                    {
                        ElementEnhance_Ground = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Ground);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Ground);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_Wind:
                    {
                        ElementEnhance_Wind = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Wind);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Wind);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_Light:
                    {
                        ElementEnhance_Light = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Light);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Light);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_Darkness:
                    {
                        ElementEnhance_Darkness = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Darkness);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_Darkness);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementEnhance_All:
                    {
                        ElementEnhance_All = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_All);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType1.ElementEnhance_All);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.Defense_Melee:
                    {
                        Defense_Melee = p_Value;
                        
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.Defense_Melee);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.Defense_Melee);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.Defense_Spell:
                    {
                        Defense_Spell = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.Defense_Spell);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.Defense_Spell);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AntiCriticalRate_Melee:
                    {
                        AntiCriticalRate_Melee = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.AntiCriticalRate_Melee);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.AntiCriticalRate_Melee);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AntiCriticalRate_Spell:
                    {
                        AntiCriticalRate_Spell = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.AntiCriticalRate_Spell);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.AntiCriticalRate_Spell);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AntiDamageRate:
                    {
                        AntiDamageRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AntiDamageRate_Melee:
                    {
                        AntiDamageRate_Melee = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate_Melee);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate_Melee);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AntiDamageRate_Spell:
                    {
                        AntiDamageRate_Spell = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate_Spell);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate_Spell);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AntiDamageRate_Critical:
                    {
                        AntiDamageRate_Critical = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate_Critical);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.AntiDamageRate_Critical);
                        }
                        break;
                    }
                    
                    case BattleStatusTool.BattleStatusType.ResistRate_Curse:
                    {
                        ResistRate_Curse = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Curse);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Curse);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Shock:
                    {
                        ResistRate_Shock = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Shock);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Shock);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Stun:
                    {
                        ResistRate_Stun = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Stun);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Stun);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Bleed:
                    {
                        ResistRate_Bleed = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Bleed);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Bleed);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Poison:
                    {
                        ResistRate_Poison = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Poison);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Poison);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Burn:
                    {
                        ResistRate_Burn = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Burn);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Burn);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Chill:
                    {
                        ResistRate_Chill = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Chill);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Chill);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Freeze:
                    {
                        ResistRate_Freeze = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Freeze);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Freeze);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Confuse:
                    {
                        ResistRate_Confuse = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Confuse);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Confuse);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Blind:
                    {
                        ResistRate_Blind = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Blind);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Blind);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Bind:
                    {
                        ResistRate_Bind = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Bind);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Bind);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Silence:
                    {
                        ResistRate_Silence = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Silence);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Silence);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Groggy:
                    {
                        ResistRate_Groggy = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Groggy);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Groggy);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ResistRate_Debuff:
                    {
                        ResistRate_Debuff = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ResistRate_Debuff);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ResistRate_Debuff);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_Fire:
                    {
                        ElementRegist_Fire = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Fire);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Fire);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_Water:
                    {
                        ElementRegist_Water = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Water);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Water);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_Ground:
                    {
                        ElementRegist_Ground = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Ground);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Ground);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_Wind:
                    {
                        ElementRegist_Wind = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Wind);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Wind);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_Light:
                    {
                        ElementRegist_Light = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Light);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Light);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_Darkness:
                    {
                        ElementRegist_Darkness = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Darkness);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_Darkness);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ElementRegist_All:
                    {
                        ElementRegist_All = p_Value;
                        
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType2.ElementRegist_All);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType2.ElementRegist_All);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.HP_Base:
                    {
                        HP_Base = p_Value;
                        
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.HP_Base);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.HP_Base);
                        }
                        
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.MP_Base:
                    {
                        MP_Base = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.MP_Base);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.MP_Base);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.HP_Fix_Recovery:
                    {
                        HP_Fix_Recovery = p_Value;
                        
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.HP_Fix_Recovery);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.HP_Fix_Recovery);
                        }
                        
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.MP_Fix_Recovery:
                    {
                        MP_Fix_Recovery = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.MP_Fix_Recovery);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.MP_Fix_Recovery);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.HP_Rate_Recovery:
                    {
                        HP_Rate_Recovery = p_Value;
                        
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.HP_Rate_Recovery);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.HP_Rate_Recovery);
                        }
                        
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.MP_Rate_Recovery:
                    {
                        MP_Rate_Recovery = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.MP_Rate_Recovery);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.MP_Rate_Recovery);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AttackSpeedBasis:
                    {
                        AttackSpeedBasis = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.AttackSpeedBasis);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.AttackSpeedBasis);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.SpellCastSpeedBasis:
                    {
                        SpellCastSpeedBasis = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.SpellCastSpeedBasis);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.SpellCastSpeedBasis);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.MoveSpeedBasis:
                    {
                        MoveSpeedBasis = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.MoveSpeedBasis);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.MoveSpeedBasis);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.AttackSpeedRate:
                    {
                        AttackSpeedRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.AttackSpeedRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.AttackSpeedRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.SpellCastSpeedRate:
                    {
                        SpellCastSpeedRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.SpellCastSpeedRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.SpellCastSpeedRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.MoveSpeedRate:
                    {
                        MoveSpeedRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.MoveSpeedRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.MoveSpeedRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.JumpForce:
                    {
                        JumpForce = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.JumpForce);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.JumpForce);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.JumpCount:
                    {
                        JumpCount = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.JumpCount);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.JumpCount);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.SightRange:
                    {
                        SightRange = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.SightRange);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.SightRange);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.Absorb:
                    {
                        Absorb = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.Absorb);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.Absorb);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.HitRate:
                    {
                        HitRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.HitRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.HitRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.DodgeRate:
                    {
                        DodgeRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.DodgeRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.DodgeRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.CostRate:
                    {
                        CostRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.CostRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.CostRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.CooldownRate:
                    {
                        CooldownRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.CooldownRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.CooldownRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.CooldownRecoverySpeedRate:
                    {
                        CooldownRecoverySpeedRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.CooldownRecoverySpeedRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.CooldownRecoverySpeedRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.HitMotionRecoverySpeedRate:
                    {
                        HitMotionRecoverySpeedRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.HitMotionRecoverySpeedRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.HitMotionRecoverySpeedRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ExpChanceExtraRate:
                    {
                        ExpChanceExtraRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.ExpChanceExtraRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.ExpChanceExtraRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.GoldChanceExtraRate:
                    {
                        GoldChanceExtraRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.GoldChanceExtraRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.GoldChanceExtraRate);
                        }
                        break;
                    }
                    case BattleStatusTool.BattleStatusType.ItemChanceExtraRate:
                    {
                        ItemChanceExtraRate = p_Value;
                                            
                        if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                        {
                            FlagMask.RemoveFlag(BattleStatusTool.BattleStatusType3.ItemChanceExtraRate);
                        }
                        else
                        {
                            FlagMask.AddFlag(BattleStatusTool.BattleStatusType3.ItemChanceExtraRate);
                        }
                        break;
                    }
                }
            }
        }
        
        public void SetProperty(BattleStatusTool.BattleStatusType1 p_Type, float p_Value)
        {
            if (p_Type != BattleStatusTool.BattleStatusType1.None)
            {
                switch (p_Type)
                {
                    case BattleStatusTool.BattleStatusType1.Attack_Melee:
                        Attack_Melee = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.Attack_Spell:
                        Attack_Spell = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.CriticalRate_Melee:
                        CriticalRate_Melee = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.CriticalRate_Spell:
                        CriticalRate_Spell = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate:
                        DamageRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Melee:
                        DamageRate_Melee = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Spell:
                        DamageRate_Spell = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Critical:
                        DamageRate_Critical = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Pierce:
                        DamageRate_Pierce = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Boss:
                        DamageRate_Boss = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Curse:
                        DamageRate_Curse = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Shock:
                        DamageRate_Shock = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Stun:
                        DamageRate_Stun = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Bleed:
                        DamageRate_Bleed = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Poison:
                        DamageRate_Poison = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Burn:
                        DamageRate_Burn = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Chill:
                        DamageRate_Chill = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Freeze:
                        DamageRate_Freeze = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Confuse:
                        DamageRate_Confuse = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Blind:
                        DamageRate_Blind = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Bind:
                        DamageRate_Bind = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Silence:
                        DamageRate_Silence = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Groggy:
                        DamageRate_Groggy = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.DamageRate_Debuff:
                        DamageRate_Debuff = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_Fire:
                        ElementEnhance_Fire = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_Water:
                        ElementEnhance_Water = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_Ground:
                        ElementEnhance_Ground = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_Wind:
                        ElementEnhance_Wind = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_Light:
                        ElementEnhance_Light = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_Darkness:
                        ElementEnhance_Darkness = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType1.ElementEnhance_All:
                        ElementEnhance_All = p_Value;
                        break;
                }
                
                if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(p_Type);
                }
                else
                {
                    FlagMask.AddFlag(p_Type);
                }
            }
        }
                
        public void SetProperty(BattleStatusTool.BattleStatusType2 p_Type, float p_Value)
        {
            if (p_Type != BattleStatusTool.BattleStatusType2.None)
            {
                switch (p_Type)
                {
                    case BattleStatusTool.BattleStatusType2.Defense_Melee:
                        Defense_Melee = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.Defense_Spell:
                        Defense_Spell = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.AntiCriticalRate_Melee:
                        AntiCriticalRate_Melee = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.AntiCriticalRate_Spell:
                        AntiCriticalRate_Spell = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.AntiDamageRate:
                        AntiDamageRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.AntiDamageRate_Melee:
                        AntiDamageRate_Melee = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.AntiDamageRate_Spell:
                        AntiDamageRate_Spell = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.AntiDamageRate_Critical:
                        AntiDamageRate_Critical = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Curse:
                        ResistRate_Curse = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Shock:
                        ResistRate_Shock = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Stun:
                        ResistRate_Stun = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Bleed:
                        ResistRate_Bleed = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Poison:
                        ResistRate_Poison = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Burn:
                        ResistRate_Burn = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Chill:
                        ResistRate_Chill = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Freeze:
                        ResistRate_Freeze = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Confuse:
                        ResistRate_Confuse = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Blind:
                        ResistRate_Blind = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Bind:
                        ResistRate_Bind = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Silence:
                        ResistRate_Silence = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Groggy:
                        ResistRate_Groggy = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ResistRate_Debuff:
                        ResistRate_Debuff = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_Fire:
                        ElementRegist_Fire = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_Water:
                        ElementRegist_Water = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_Ground:
                        ElementRegist_Ground = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_Wind:
                        ElementRegist_Wind = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_Light:
                        ElementRegist_Light = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_Darkness:
                        ElementRegist_Darkness = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType2.ElementRegist_All:
                        ElementRegist_All = p_Value;
                        break;
                }
                
                if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(p_Type);
                }
                else
                {
                    FlagMask.AddFlag(p_Type);
                }
            }
        }

        public void SetProperty(BattleStatusTool.BattleStatusType3 p_Type, float p_Value)
        {
            if (p_Type != BattleStatusTool.BattleStatusType3.None)
            {
                switch (p_Type)
                {
                    case BattleStatusTool.BattleStatusType3.HP_Base:
                        HP_Base = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.MP_Base:
                        MP_Base = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.HP_Fix_Recovery:
                        HP_Fix_Recovery = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.MP_Fix_Recovery:
                        MP_Fix_Recovery = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.HP_Rate_Recovery:
                        HP_Rate_Recovery = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.MP_Rate_Recovery:
                        MP_Rate_Recovery = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.AttackSpeedBasis:
                        AttackSpeedBasis = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.SpellCastSpeedBasis:
                        SpellCastSpeedBasis = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.MoveSpeedBasis:
                        MoveSpeedBasis = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.AttackSpeedRate:
                        AttackSpeedRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.SpellCastSpeedRate:
                        SpellCastSpeedRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.MoveSpeedRate:
                        MoveSpeedRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.JumpForce:
                        JumpForce = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.JumpCount:
                        JumpCount = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.SightRange:
                        SightRange = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.Absorb:
                        Absorb = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.HitRate:
                        HitRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.DodgeRate:
                        DodgeRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.CostRate:
                        CostRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.CooldownRate:
                        CooldownRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.CooldownRecoverySpeedRate:
                        CooldownRecoverySpeedRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.HitMotionRecoverySpeedRate:
                        HitMotionRecoverySpeedRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.ExpChanceExtraRate:
                        ExpChanceExtraRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.GoldChanceExtraRate:
                        GoldChanceExtraRate = p_Value;
                        break;
                    case BattleStatusTool.BattleStatusType3.ItemChanceExtraRate:
                        ItemChanceExtraRate = p_Value;
                        break;
                }

                if (p_Value.IsReachedZero(BattleStatusTool.PropertyLowerBound))
                {
                    FlagMask.RemoveFlag(p_Type);
                }
                else
                {
                    FlagMask.AddFlag(p_Type);
                }
            }
        }
             
        #endregion
        
        #region <Method/Query/Get>
        
        public readonly float GetProperty(BattleStatusTool.BattleStatusType p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusTool.BattleStatusType.None:
                    return 0;
                case BattleStatusTool.BattleStatusType.Attack_Melee:
                    return Attack_Melee;
                case BattleStatusTool.BattleStatusType.Attack_Spell:
                    return Attack_Spell;
                case BattleStatusTool.BattleStatusType.CriticalRate_Melee:
                    return CriticalRate_Melee;
                case BattleStatusTool.BattleStatusType.CriticalRate_Spell:
                    return CriticalRate_Spell;
                case BattleStatusTool.BattleStatusType.DamageRate:
                    return DamageRate;
                case BattleStatusTool.BattleStatusType.DamageRate_Melee:
                    return DamageRate_Melee;
                case BattleStatusTool.BattleStatusType.DamageRate_Spell:
                    return DamageRate_Spell;
                case BattleStatusTool.BattleStatusType.DamageRate_Critical:
                    return DamageRate_Critical;
                case BattleStatusTool.BattleStatusType.DamageRate_Pierce:
                    return DamageRate_Pierce;
                case BattleStatusTool.BattleStatusType.DamageRate_Boss:
                    return DamageRate_Boss;
                case BattleStatusTool.BattleStatusType.DamageRate_Curse:
                    return DamageRate_Curse;
                case BattleStatusTool.BattleStatusType.DamageRate_Shock:
                    return DamageRate_Shock;
                case BattleStatusTool.BattleStatusType.DamageRate_Stun:
                    return DamageRate_Stun;
                case BattleStatusTool.BattleStatusType.DamageRate_Bleed:
                    return DamageRate_Bleed;
                case BattleStatusTool.BattleStatusType.DamageRate_Poison:
                    return DamageRate_Poison;
                case BattleStatusTool.BattleStatusType.DamageRate_Burn:
                    return DamageRate_Burn;
                case BattleStatusTool.BattleStatusType.DamageRate_Chill:
                    return DamageRate_Chill;
                case BattleStatusTool.BattleStatusType.DamageRate_Freeze:
                    return DamageRate_Freeze;
                case BattleStatusTool.BattleStatusType.DamageRate_Confuse:
                    return DamageRate_Confuse;
                case BattleStatusTool.BattleStatusType.DamageRate_Blind:
                    return DamageRate_Blind;
                case BattleStatusTool.BattleStatusType.DamageRate_Bind:
                    return DamageRate_Bind;
                case BattleStatusTool.BattleStatusType.DamageRate_Silence:
                    return DamageRate_Silence;
                case BattleStatusTool.BattleStatusType.DamageRate_Groggy:
                    return DamageRate_Groggy;
                case BattleStatusTool.BattleStatusType.DamageRate_Debuff:
                    return DamageRate_Debuff;
                case BattleStatusTool.BattleStatusType.ElementEnhance_Fire:
                    return ElementEnhance_Fire;
                case BattleStatusTool.BattleStatusType.ElementEnhance_Water:
                    return ElementEnhance_Water;
                case BattleStatusTool.BattleStatusType.ElementEnhance_Ground:
                    return ElementEnhance_Ground;
                case BattleStatusTool.BattleStatusType.ElementEnhance_Wind:
                    return ElementEnhance_Wind;
                case BattleStatusTool.BattleStatusType.ElementEnhance_Light:
                    return ElementEnhance_Light;
                case BattleStatusTool.BattleStatusType.ElementEnhance_Darkness:
                    return ElementEnhance_Darkness;
                case BattleStatusTool.BattleStatusType.ElementEnhance_All:
                    return ElementEnhance_All;
                case BattleStatusTool.BattleStatusType.Defense_Melee:
                    return Defense_Melee;
                case BattleStatusTool.BattleStatusType.Defense_Spell:
                    return Defense_Spell;
                case BattleStatusTool.BattleStatusType.AntiCriticalRate_Melee:
                    return AntiCriticalRate_Melee;
                case BattleStatusTool.BattleStatusType.AntiCriticalRate_Spell:
                    return AntiCriticalRate_Spell;
                case BattleStatusTool.BattleStatusType.AntiDamageRate:
                    return AntiDamageRate;
                case BattleStatusTool.BattleStatusType.AntiDamageRate_Melee:
                    return AntiDamageRate_Melee;
                case BattleStatusTool.BattleStatusType.AntiDamageRate_Spell:
                    return AntiDamageRate_Spell;
                case BattleStatusTool.BattleStatusType.AntiDamageRate_Critical:
                    return AntiDamageRate_Critical;
                case BattleStatusTool.BattleStatusType.ResistRate_Curse:
                    return ResistRate_Curse;
                case BattleStatusTool.BattleStatusType.ResistRate_Shock:
                    return ResistRate_Shock;
                case BattleStatusTool.BattleStatusType.ResistRate_Stun:
                    return ResistRate_Stun;
                case BattleStatusTool.BattleStatusType.ResistRate_Bleed:
                    return ResistRate_Bleed;
                case BattleStatusTool.BattleStatusType.ResistRate_Poison:
                    return ResistRate_Poison;
                case BattleStatusTool.BattleStatusType.ResistRate_Burn:
                    return ResistRate_Burn;
                case BattleStatusTool.BattleStatusType.ResistRate_Chill:
                    return ResistRate_Chill;
                case BattleStatusTool.BattleStatusType.ResistRate_Freeze:
                    return ResistRate_Freeze;
                case BattleStatusTool.BattleStatusType.ResistRate_Confuse:
                    return ResistRate_Confuse;
                case BattleStatusTool.BattleStatusType.ResistRate_Blind:
                    return ResistRate_Blind;
                case BattleStatusTool.BattleStatusType.ResistRate_Bind:
                    return ResistRate_Bind;
                case BattleStatusTool.BattleStatusType.ResistRate_Silence:
                    return ResistRate_Silence;
                case BattleStatusTool.BattleStatusType.ResistRate_Groggy:
                    return ResistRate_Groggy;
                case BattleStatusTool.BattleStatusType.ResistRate_Debuff:
                    return ResistRate_Debuff;
                case BattleStatusTool.BattleStatusType.ElementRegist_Fire:
                    return ElementRegist_Fire;
                case BattleStatusTool.BattleStatusType.ElementRegist_Water:
                    return ElementRegist_Water;
                case BattleStatusTool.BattleStatusType.ElementRegist_Ground:
                    return ElementRegist_Ground;
                case BattleStatusTool.BattleStatusType.ElementRegist_Wind:
                    return ElementRegist_Wind;
                case BattleStatusTool.BattleStatusType.ElementRegist_Light:
                    return ElementRegist_Light;
                case BattleStatusTool.BattleStatusType.ElementRegist_Darkness:
                    return ElementRegist_Darkness;
                case BattleStatusTool.BattleStatusType.ElementRegist_All:
                    return ElementRegist_All;
                case BattleStatusTool.BattleStatusType.HP_Base:
                    return HP_Base;
                case BattleStatusTool.BattleStatusType.MP_Base:
                    return MP_Base;
                case BattleStatusTool.BattleStatusType.HP_Fix_Recovery:
                    return HP_Fix_Recovery;
                case BattleStatusTool.BattleStatusType.MP_Fix_Recovery:
                    return MP_Fix_Recovery;
                case BattleStatusTool.BattleStatusType.HP_Rate_Recovery:
                    return HP_Rate_Recovery;
                case BattleStatusTool.BattleStatusType.MP_Rate_Recovery:
                    return MP_Rate_Recovery;
                case BattleStatusTool.BattleStatusType.AttackSpeedBasis:
                    return AttackSpeedBasis;
                case BattleStatusTool.BattleStatusType.SpellCastSpeedBasis:
                    return SpellCastSpeedBasis;
                case BattleStatusTool.BattleStatusType.MoveSpeedBasis:
                    return MoveSpeedBasis;
                case BattleStatusTool.BattleStatusType.AttackSpeedRate:
                    return AttackSpeedRate;
                case BattleStatusTool.BattleStatusType.SpellCastSpeedRate:
                    return SpellCastSpeedRate;
                case BattleStatusTool.BattleStatusType.MoveSpeedRate:
                    return MoveSpeedRate;
                case BattleStatusTool.BattleStatusType.JumpForce:
                    return JumpForce;
                case BattleStatusTool.BattleStatusType.JumpCount:
                    return JumpCount;
                case BattleStatusTool.BattleStatusType.SightRange:
                    return SightRange;
                case BattleStatusTool.BattleStatusType.Absorb:
                    return Absorb;
                case BattleStatusTool.BattleStatusType.HitRate:
                    return HitRate;
                case BattleStatusTool.BattleStatusType.DodgeRate:
                    return DodgeRate;
                case BattleStatusTool.BattleStatusType.CostRate:
                    return CostRate;
                case BattleStatusTool.BattleStatusType.CooldownRate:
                    return CooldownRate;
                case BattleStatusTool.BattleStatusType.CooldownRecoverySpeedRate:
                    return CooldownRecoverySpeedRate;
                case BattleStatusTool.BattleStatusType.HitMotionRecoverySpeedRate:
                    return HitMotionRecoverySpeedRate;
                case BattleStatusTool.BattleStatusType.ExpChanceExtraRate:
                    return ExpChanceExtraRate;
                case BattleStatusTool.BattleStatusType.GoldChanceExtraRate:
                    return GoldChanceExtraRate;
                case BattleStatusTool.BattleStatusType.ItemChanceExtraRate:
                    return ItemChanceExtraRate;
            }
        }

        public readonly float GetProperty(BattleStatusTool.BattleStatusType1 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusTool.BattleStatusType1.None:
                    return 0;
                case BattleStatusTool.BattleStatusType1.Attack_Melee:
                    return Attack_Melee;
                case BattleStatusTool.BattleStatusType1.Attack_Spell:
                    return Attack_Spell;
                case BattleStatusTool.BattleStatusType1.CriticalRate_Melee:
                    return CriticalRate_Melee;
                case BattleStatusTool.BattleStatusType1.CriticalRate_Spell:
                    return CriticalRate_Spell;
                case BattleStatusTool.BattleStatusType1.DamageRate:
                    return DamageRate;
                case BattleStatusTool.BattleStatusType1.DamageRate_Melee:
                    return DamageRate_Melee;
                case BattleStatusTool.BattleStatusType1.DamageRate_Spell:
                    return DamageRate_Spell;
                case BattleStatusTool.BattleStatusType1.DamageRate_Critical:
                    return DamageRate_Critical;
                case BattleStatusTool.BattleStatusType1.DamageRate_Pierce:
                    return DamageRate_Pierce;
                case BattleStatusTool.BattleStatusType1.DamageRate_Boss:
                    return DamageRate_Boss;
                case BattleStatusTool.BattleStatusType1.DamageRate_Curse:
                    return DamageRate_Curse;
                case BattleStatusTool.BattleStatusType1.DamageRate_Shock:
                    return DamageRate_Shock;
                case BattleStatusTool.BattleStatusType1.DamageRate_Stun:
                    return DamageRate_Stun;
                case BattleStatusTool.BattleStatusType1.DamageRate_Bleed:
                    return DamageRate_Bleed;
                case BattleStatusTool.BattleStatusType1.DamageRate_Poison:
                    return DamageRate_Poison;
                case BattleStatusTool.BattleStatusType1.DamageRate_Burn:
                    return DamageRate_Burn;
                case BattleStatusTool.BattleStatusType1.DamageRate_Chill:
                    return DamageRate_Chill;
                case BattleStatusTool.BattleStatusType1.DamageRate_Freeze:
                    return DamageRate_Freeze;
                case BattleStatusTool.BattleStatusType1.DamageRate_Confuse:
                    return DamageRate_Confuse;
                case BattleStatusTool.BattleStatusType1.DamageRate_Blind:
                    return DamageRate_Blind;
                case BattleStatusTool.BattleStatusType1.DamageRate_Bind:
                    return DamageRate_Bind;
                case BattleStatusTool.BattleStatusType1.DamageRate_Silence:
                    return DamageRate_Silence;
                case BattleStatusTool.BattleStatusType1.DamageRate_Groggy:
                    return DamageRate_Groggy;
                case BattleStatusTool.BattleStatusType1.DamageRate_Debuff:
                    return DamageRate_Debuff;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_Fire:
                    return ElementEnhance_Fire;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_Water:
                    return ElementEnhance_Water;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_Ground:
                    return ElementEnhance_Ground;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_Wind:
                    return ElementEnhance_Wind;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_Light:
                    return ElementEnhance_Light;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_Darkness:
                    return ElementEnhance_Darkness;
                case BattleStatusTool.BattleStatusType1.ElementEnhance_All:
                    return ElementEnhance_All;
            }
        }

        public readonly float GetProperty(BattleStatusTool.BattleStatusType2 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusTool.BattleStatusType2.None:
                    return 0;
                case BattleStatusTool.BattleStatusType2.Defense_Melee:
                    return Defense_Melee;
                case BattleStatusTool.BattleStatusType2.Defense_Spell:
                    return Defense_Spell;
                case BattleStatusTool.BattleStatusType2.AntiCriticalRate_Melee:
                    return AntiCriticalRate_Melee;
                case BattleStatusTool.BattleStatusType2.AntiCriticalRate_Spell:
                    return AntiCriticalRate_Spell;
                case BattleStatusTool.BattleStatusType2.AntiDamageRate:
                    return AntiDamageRate;
                case BattleStatusTool.BattleStatusType2.AntiDamageRate_Melee:
                    return AntiDamageRate_Melee;
                case BattleStatusTool.BattleStatusType2.AntiDamageRate_Spell:
                    return AntiDamageRate_Spell;
                case BattleStatusTool.BattleStatusType2.AntiDamageRate_Critical:
                    return AntiDamageRate_Critical;
                case BattleStatusTool.BattleStatusType2.ResistRate_Curse:
                    return ResistRate_Curse;
                case BattleStatusTool.BattleStatusType2.ResistRate_Shock:
                    return ResistRate_Shock;
                case BattleStatusTool.BattleStatusType2.ResistRate_Stun:
                    return ResistRate_Stun;
                case BattleStatusTool.BattleStatusType2.ResistRate_Bleed:
                    return ResistRate_Bleed;
                case BattleStatusTool.BattleStatusType2.ResistRate_Poison:
                    return ResistRate_Poison;
                case BattleStatusTool.BattleStatusType2.ResistRate_Burn:
                    return ResistRate_Burn;
                case BattleStatusTool.BattleStatusType2.ResistRate_Chill:
                    return ResistRate_Chill;
                case BattleStatusTool.BattleStatusType2.ResistRate_Freeze:
                    return ResistRate_Freeze;
                case BattleStatusTool.BattleStatusType2.ResistRate_Confuse:
                    return ResistRate_Confuse;
                case BattleStatusTool.BattleStatusType2.ResistRate_Blind:
                    return ResistRate_Blind;
                case BattleStatusTool.BattleStatusType2.ResistRate_Bind:
                    return ResistRate_Bind;
                case BattleStatusTool.BattleStatusType2.ResistRate_Silence:
                    return ResistRate_Silence;
                case BattleStatusTool.BattleStatusType2.ResistRate_Groggy:
                    return ResistRate_Groggy;
                case BattleStatusTool.BattleStatusType2.ResistRate_Debuff:
                    return ResistRate_Debuff;
                case BattleStatusTool.BattleStatusType2.ElementRegist_Fire:
                    return ElementRegist_Fire;
                case BattleStatusTool.BattleStatusType2.ElementRegist_Water:
                    return ElementRegist_Water;
                case BattleStatusTool.BattleStatusType2.ElementRegist_Ground:
                    return ElementRegist_Ground;
                case BattleStatusTool.BattleStatusType2.ElementRegist_Wind:
                    return ElementRegist_Wind;
                case BattleStatusTool.BattleStatusType2.ElementRegist_Light:
                    return ElementRegist_Light;
                case BattleStatusTool.BattleStatusType2.ElementRegist_Darkness:
                    return ElementRegist_Darkness;
                case BattleStatusTool.BattleStatusType2.ElementRegist_All:
                    return ElementRegist_All;
            }
        }
        
        public readonly float GetProperty(BattleStatusTool.BattleStatusType3 p_Type)
        {
            switch (p_Type)
            {
                default:
                case BattleStatusTool.BattleStatusType3.None:
                    return 0;
                case BattleStatusTool.BattleStatusType3.HP_Base:
                    return HP_Base;
                case BattleStatusTool.BattleStatusType3.MP_Base:
                    return MP_Base;
                case BattleStatusTool.BattleStatusType3.HP_Fix_Recovery:
                    return HP_Fix_Recovery;
                case BattleStatusTool.BattleStatusType3.MP_Fix_Recovery:
                    return MP_Fix_Recovery;
                case BattleStatusTool.BattleStatusType3.HP_Rate_Recovery:
                    return HP_Rate_Recovery;
                case BattleStatusTool.BattleStatusType3.MP_Rate_Recovery:
                    return MP_Rate_Recovery;
                case BattleStatusTool.BattleStatusType3.AttackSpeedBasis:
                    return AttackSpeedBasis;
                case BattleStatusTool.BattleStatusType3.SpellCastSpeedBasis:
                    return SpellCastSpeedBasis;
                case BattleStatusTool.BattleStatusType3.MoveSpeedBasis:
                    return MoveSpeedBasis;
                case BattleStatusTool.BattleStatusType3.AttackSpeedRate:
                    return AttackSpeedRate;
                case BattleStatusTool.BattleStatusType3.SpellCastSpeedRate:
                    return SpellCastSpeedRate;
                case BattleStatusTool.BattleStatusType3.MoveSpeedRate:
                    return MoveSpeedRate;
                case BattleStatusTool.BattleStatusType3.JumpForce:
                    return JumpForce;
                case BattleStatusTool.BattleStatusType3.JumpCount:
                    return JumpCount;
                case BattleStatusTool.BattleStatusType3.SightRange:
                    return SightRange;
                case BattleStatusTool.BattleStatusType3.Absorb:
                    return Absorb;
                case BattleStatusTool.BattleStatusType3.HitRate:
                    return HitRate;
                case BattleStatusTool.BattleStatusType3.DodgeRate:
                    return DodgeRate;
                case BattleStatusTool.BattleStatusType3.CostRate:
                    return CostRate;
                case BattleStatusTool.BattleStatusType3.CooldownRate:
                    return CooldownRate;
                case BattleStatusTool.BattleStatusType3.CooldownRecoverySpeedRate:
                    return CooldownRecoverySpeedRate;
                case BattleStatusTool.BattleStatusType3.HitMotionRecoverySpeedRate:
                    return HitMotionRecoverySpeedRate;
                case BattleStatusTool.BattleStatusType3.ExpChanceExtraRate:
                    return ExpChanceExtraRate;
                case BattleStatusTool.BattleStatusType3.GoldChanceExtraRate:
                    return GoldChanceExtraRate;
                case BattleStatusTool.BattleStatusType3.ItemChanceExtraRate:
                    return ItemChanceExtraRate;
            }
        }
                
        public readonly float GetProperty(BattleStatusTool.BattleStatusType p_Type, float p_MultiplyRate)
        {
            return GetProperty(p_Type) * p_MultiplyRate;
        }
                
        public readonly float GetProperty(BattleStatusTool.BattleStatusType1 p_Type, float p_MultiplyRate)
        {
            return GetProperty(p_Type) * p_MultiplyRate;
        }
                
        public readonly float GetProperty(BattleStatusTool.BattleStatusType2 p_Type, float p_MultiplyRate)
        {
            return GetProperty(p_Type) * p_MultiplyRate;
        }
                
        public readonly float GetProperty(BattleStatusTool.BattleStatusType3 p_Type, float p_MultiplyRate)
        {
            return GetProperty(p_Type) * p_MultiplyRate;
        }
       
        public readonly float GetDamageRate(DamageCalculator.DamageCalcType p_CalcType)
        {
            switch (p_CalcType)
            {
                default:
                case DamageCalculator.DamageCalcType.None:
                    return 1f;
                case DamageCalculator.DamageCalcType.Melee:
                    return DamageRate_Melee;
                case DamageCalculator.DamageCalcType.Spell:
                    return DamageRate_Spell;
            }
        }
        
        public readonly float GetScaledDamageRate(DamageCalculator.DamageCalcType p_CalcType)
        {
            return DamageRate * GetDamageRate(p_CalcType);
        }
        
        public readonly float GetAntiDamageRate(DamageCalculator.DamageCalcType p_CalcType)
        {
            switch (p_CalcType)
            {
                default:
                case DamageCalculator.DamageCalcType.None:
                    return 1f;
                case DamageCalculator.DamageCalcType.Melee:
                    return 1f - AntiDamageRate_Melee;
                case DamageCalculator.DamageCalcType.Spell:
                    return 1f - AntiDamageRate_Spell;
            }
        }
        
        public readonly float GetScaledAntiDamageRate(DamageCalculator.DamageCalcType p_CalcType)
        {
            return (1f - AntiDamageRate) * GetAntiDamageRate(p_CalcType);
        }
        
        public readonly float GetAttackBase(DamageCalculator.DamageCalcType p_CalcType)
        {
            switch (p_CalcType)
            {
                default:
                case DamageCalculator.DamageCalcType.None:
                    return 0f;
                case DamageCalculator.DamageCalcType.Melee:
                    return Attack_Melee;
                case DamageCalculator.DamageCalcType.Spell:
                    return Attack_Spell;
            }
        }
        
        public readonly float GetScaledAttackBase(DamageCalculator.DamageCalcType p_CalcType)
        {
            return Attack_Melee * GetScaledDamageRate(p_CalcType);
        }
        
        public readonly float GetDefenseBase(DamageCalculator.DamageCalcType p_CalcType)
        {
            switch (p_CalcType)
            {
                default:
                case DamageCalculator.DamageCalcType.None:
                    return 0f;
                case DamageCalculator.DamageCalcType.Melee:
                    return Defense_Melee;
                case DamageCalculator.DamageCalcType.Spell:
                    return Defense_Spell;
            }
        }

        public readonly float GetCriticalRate(DamageCalculator.DamageCalcType p_CalcType)
        {
            switch (p_CalcType)
            {
                default:
                case DamageCalculator.DamageCalcType.None:
                    return 0f;
                case DamageCalculator.DamageCalcType.Melee:
                    return CriticalRate_Melee;
                case DamageCalculator.DamageCalcType.Spell:
                    return CriticalRate_Spell;
            }
        }
               
        public readonly float GetAntiCriticalRate(DamageCalculator.DamageCalcType p_CalcType)
        {
            switch (p_CalcType)
            {
                default:
                case DamageCalculator.DamageCalcType.None:
                    return 0f;
                case DamageCalculator.DamageCalcType.Melee:
                    return AntiCriticalRate_Melee;
                case DamageCalculator.DamageCalcType.Spell:
                    return AntiCriticalRate_Spell;
            }
        }

        public readonly float GetScaledAttackSpeed()
        {
            return AttackSpeedBasis * AttackSpeedRate;
        }
        
        public readonly float GetScaledMoveSpeed()
        {
            return MoveSpeedBasis * MoveSpeedRate;
        }
        
        public readonly float GetScaledSpellCastSpeed()
        {
            return SpellCastSpeedBasis * SpellCastSpeedRate;
        }

        #endregion

        #region <Method/Query/Text>

        public readonly string GetPropertyText(BattleStatusTool.BattleStatusType p_Type, float p_MultiplyRate = 1f)
        {
            var valueType = p_Type.GetPropertyValueType();
            var value = GetProperty(p_Type, p_MultiplyRate);

            switch (valueType)
            {
                default:
                case StatusTool.PropertyValueType.None:
                    return "NULL";
                case StatusTool.PropertyValueType.FixedValue:
                    return $"{Mathf.FloorToInt(value)}";
                case StatusTool.PropertyValueType.SimpleRateValue:
                case StatusTool.PropertyValueType.CompoundRateValue:
                    return $"{Mathf.FloorToInt(100f * value)} %";
            }
        }

        public readonly string GetPropertyText(BattleStatusTool.BattleStatusType1 p_Type, float p_MultiplyRate = 1f)
        {
            var valueType = p_Type.GetPropertyValueType();
            var value = GetProperty(p_Type, p_MultiplyRate);

            switch (valueType)
            {
                default:
                case StatusTool.PropertyValueType.None:
                    return "NULL";
                case StatusTool.PropertyValueType.FixedValue:
                    return $"{Mathf.FloorToInt(value)}";
                case StatusTool.PropertyValueType.SimpleRateValue:
                case StatusTool.PropertyValueType.CompoundRateValue:
                    return $"{Mathf.FloorToInt(100f * value)} %";
            }
        }
        
        public readonly string GetPropertyText(BattleStatusTool.BattleStatusType2 p_Type, float p_MultiplyRate = 1f)
        {
            var valueType = p_Type.GetPropertyValueType();
            var value = GetProperty(p_Type, p_MultiplyRate);

            switch (valueType)
            {
                default:
                case StatusTool.PropertyValueType.None:
                    return "NULL";
                case StatusTool.PropertyValueType.FixedValue:
                    return $"{Mathf.FloorToInt(value)}";
                case StatusTool.PropertyValueType.SimpleRateValue:
                case StatusTool.PropertyValueType.CompoundRateValue:
                    return $"{Mathf.FloorToInt(100f * value)} %";
            }
        }
        
        public readonly string GetPropertyText(BattleStatusTool.BattleStatusType3 p_Type, float p_MultiplyRate = 1f)
        {
            var valueType = p_Type.GetPropertyValueType();
            var value = GetProperty(p_Type, p_MultiplyRate);

            switch (valueType)
            {
                default:
                case StatusTool.PropertyValueType.None:
                    return "NULL";
                case StatusTool.PropertyValueType.FixedValue:
                    return $"{Mathf.FloorToInt(value)}";
                case StatusTool.PropertyValueType.SimpleRateValue:
                case StatusTool.PropertyValueType.CompoundRateValue:
                    return $"{Mathf.FloorToInt(100f * value)} %";
            }
        }

        #endregion
    }
    
    public class BattleStatusPresetWrapper
    {
        #region <Fields>

        public BattleStatusPreset BattleStatusPreset;

        #endregion
        
        #region <Indexer>

        public float this[BattleStatusTool.BattleStatusType p_Type] => BattleStatusPreset[p_Type];
        public float this[BattleStatusTool.BattleStatusType1 p_Type] => BattleStatusPreset[p_Type];
        public float this[BattleStatusTool.BattleStatusType2 p_Type] => BattleStatusPreset[p_Type];
        public float this[BattleStatusTool.BattleStatusType3 p_Type] => BattleStatusPreset[p_Type];
        public float this[BattleStatusTool.BattleStatusType p_Type, float p_Rate] => BattleStatusPreset[p_Type, p_Rate];
        public float this[BattleStatusTool.BattleStatusType1 p_Type, float p_Rate] => BattleStatusPreset[p_Type, p_Rate];
        public float this[BattleStatusTool.BattleStatusType2 p_Type, float p_Rate] => BattleStatusPreset[p_Type, p_Rate];
        public float this[BattleStatusTool.BattleStatusType3 p_Type, float p_Rate] => BattleStatusPreset[p_Type, p_Rate];
 
        #endregion
        
        #region <Constructors>

        public BattleStatusPresetWrapper(BattleStatusPreset p_Preset)
        {
            BattleStatusPreset = p_Preset;
        }

        #endregion

        #region <Operator>
        
        public static implicit operator BattleStatusPreset(BattleStatusPresetWrapper p_Wrapper)
        {
            return p_Wrapper.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator+(BattleStatusPresetWrapper p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left.BattleStatusPreset + p_Right.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator+(BattleStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BattleStatusPreset + p_Right;
        }
        
        public static BattleStatusPreset operator+(float p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left + p_Right.BattleStatusPreset;
        }

        public static BattleStatusPreset operator-(BattleStatusPresetWrapper p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left.BattleStatusPreset - p_Right.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator-(BattleStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BattleStatusPreset - p_Right;
        }
        
        public static BattleStatusPreset operator-(float p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left - p_Right.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator*(BattleStatusPresetWrapper p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left.BattleStatusPreset * p_Right.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator*(BattleStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BattleStatusPreset * p_Right;
        }
        
        public static BattleStatusPreset operator*(float p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left * p_Right.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator/(BattleStatusPresetWrapper p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left.BattleStatusPreset / p_Right.BattleStatusPreset;
        }
        
        public static BattleStatusPreset operator/(BattleStatusPresetWrapper p_Left, float p_Right) 
        {
            return p_Left.BattleStatusPreset / p_Right;
        }
        
        public static BattleStatusPreset operator/(float p_Left, BattleStatusPresetWrapper p_Right) 
        {
            return p_Left / p_Right.BattleStatusPreset;
        }
        
        #endregion
        
        #region <Methods>

        public void SetPreset(BattleStatusPreset p_Preset)
        {
            BattleStatusPreset = p_Preset;
        }

        public void AddProperty(BattleStatusTool.BattleStatusType p_Type, float p_Value)
        {
            BattleStatusPreset.AddProperty(p_Type, p_Value);
        }
        
        public void AddProperty(BattleStatusTool.BattleStatusType1 p_Type, float p_Value)
        {
            BattleStatusPreset.AddProperty(p_Type, p_Value);
        }
        
        public void AddProperty(BattleStatusTool.BattleStatusType2 p_Type, float p_Value)
        {
            BattleStatusPreset.AddProperty(p_Type, p_Value);
        }
        
        public void AddProperty(BattleStatusTool.BattleStatusType3 p_Type, float p_Value)
        {
            BattleStatusPreset.AddProperty(p_Type, p_Value);
        }

        public void SetProperty(BattleStatusTool.BattleStatusType p_Type, float p_Value)
        {
            BattleStatusPreset.SetProperty(p_Type, p_Value);
        }
        
        public void SetProperty(BattleStatusTool.BattleStatusType1 p_Type, float p_Value)
        {
            BattleStatusPreset.SetProperty(p_Type, p_Value);
        }
        
        public void SetProperty(BattleStatusTool.BattleStatusType2 p_Type, float p_Value)
        {
            BattleStatusPreset.SetProperty(p_Type, p_Value);
        }
        
        public void SetProperty(BattleStatusTool.BattleStatusType3 p_Type, float p_Value)
        {
            BattleStatusPreset.SetProperty(p_Type, p_Value);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType p_Type)
        {
            return BattleStatusPreset.GetProperty(p_Type);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType1 p_Type)
        {
            return BattleStatusPreset.GetProperty(p_Type);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType2 p_Type)
        {
            return BattleStatusPreset.GetProperty(p_Type);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType3 p_Type)
        {
            return BattleStatusPreset.GetProperty(p_Type);
        }

        public float GetProperty(BattleStatusTool.BattleStatusType p_Type, float p_MultiplyRate)
        {
            return BattleStatusPreset.GetProperty(p_Type, p_MultiplyRate);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType1 p_Type, float p_MultiplyRate)
        {
            return BattleStatusPreset.GetProperty(p_Type, p_MultiplyRate);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType2 p_Type, float p_MultiplyRate)
        {
            return BattleStatusPreset.GetProperty(p_Type, p_MultiplyRate);
        }
        
        public float GetProperty(BattleStatusTool.BattleStatusType3 p_Type, float p_MultiplyRate)
        {
            return BattleStatusPreset.GetProperty(p_Type, p_MultiplyRate);
        }

        #endregion
    }
}