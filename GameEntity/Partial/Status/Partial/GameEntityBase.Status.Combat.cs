using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Methods>

        public bool HasManaEnough(float p_Mana)
        {
            var costRate = this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.CostRate];
            return costRate * p_Mana <= this[StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.MP_Base];

        }

        public void CostMana(float p_Mana)
        {
            var costRate = this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.CostRate];
            p_Mana *= costRate;
            AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.MP_Base, -p_Mana, default);
        }
          
        public void GiveDamage(StatusTool.StatusChangeParams p_Params, float p_DamageRate = 1f)
        {
            var trigger = p_Params.Trigger;
            if (trigger.IsEntityValid())
            {
                var damage = p_DamageRate * trigger[StatusTool.BattleStatusGroupType.Total].GetScaledAttackBase(DamageCalculator.DamageCalcType.Melee);
                GiveDamage(damage, p_Params);
            }
        }
        
        public void GiveDamage(float p_Damage, StatusTool.StatusChangeParams p_Params)
        {
            if (IsImmortal || IsInvincible)
            {
                if(IsUnitEntity)
                {
                    UIxControlRoot.GetInstanceUnsafe?
                        .NumberTheater?
                        .PopTheaterElement
                        (
                            p_Params.TargetPosition, p_Params.RandomizeRadius, 
                            Color.white, string.Empty, UIxNumberTheater.NumberEventType.Invincible
                        ); 
                }
            }
            else
            {
                // 최소데미지 보정
                p_Damage = Mathf.Max(1f, p_Damage);
                
                // 크리티컬 계산
                var criticalRate = p_Params.Trigger?[StatusTool.BattleStatusGroupType.Total].GetCriticalRate(DamageCalculator.DamageCalcType.Melee) ?? 0f;
                if (criticalRate > Random.value)
                {
                    p_Params.AddAttribute(StatusTool.StatusChangeAttribute.Critical);
                    p_Damage *= this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.DamageRate_Critical];
                }
                
                // 방어력 계산
                p_Damage *= Mathf.Min(BattleStatusTool.AntiDamageRateUpperBound, this[StatusTool.BattleStatusGroupType.Total].GetScaledAntiDamageRate(DamageCalculator.DamageCalcType.Melee));

                // 소수점 제거(반올림)
                p_Damage = Mathf.Round(p_Damage);
                
                AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, -p_Damage, p_Params);
            }
        }
        
        public void Absorb()
        {
            GetMaster().HealHP(this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.Absorb]);
        }
        
        public void HealHP(float p_Value)
        {
            AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, p_Value, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.HealHP));
        }
        
        public void HealRateHP(float p_Rate)
        {
            HealHP(this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.HP_Base, p_Rate]);
        }
        
        public void HealMP(float p_Value)
        {
            AddStatus(StatusTool.BattleStatusGroupType.Current, BattleStatusTool.BattleStatusType.HP_Base, p_Value, new StatusTool.StatusChangeParams(StatusTool.StatusChangeEventType.HealMP));
        }
        
        public void HealRateMP(float p_Rate)
        {
            HealHP(this[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.MP_Base, p_Rate]);
        }

        #endregion
    }
}