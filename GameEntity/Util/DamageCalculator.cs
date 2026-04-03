using UnityEngine;

namespace k514.Mono.Common
{
    public static class DamageCalculator
    {
        #region <Consts>

        private const float DefaultCriticalDamageRate = 1.5f;
        private const float DamageLowerBound = 0;
        private const int AttributeEnhance10PercentBoundValue = 255;
        private const float _Inverse_AttributeEnhance10PercentBoundValue = 1f / AttributeEnhance10PercentBoundValue;
        
        /// <summary>
        /// 기본 경직 발생 확률
        /// </summary>
        private const float DefaultStuckOccurRate = 1f;
        
        /// <summary>
        /// 기본 크리티컬 확률
        /// </summary>
        private const float DefaultCriticalRate = 0.03f;
        
        /// <summary>
        /// 기본 적중률
        /// </summary>
        private const float DefaultHitRate = 0.97f;
        
        /// <summary>
        /// 크리티컬 확률 상한
        /// </summary>
        private const float CriticalRateUpperBound = 1f;
        
        /// <summary>
        /// 적중률 하한
        /// </summary>
        private const float HitRateLowerBound = 0.1f;
        
        #endregion

        #region <Enums>

        /// <summary>
        /// 공격 계산 타입
        /// </summary>
        public enum DamageCalcType
        {
            None,
            
            /// <summary>
            /// 물리 공격
            /// </summary>
            Melee,

            /// <summary>
            /// 마법 공격
            /// </summary>
            Spell
        }
        
        public enum HitResultType
        {
            HitNoOne,
            HitFail,
            HitNormal,
            HitCritical,
            HitMissed,
        }

        #endregion

        #region <Methods>

        public static (GameEntityTool.ElementType, float) CalcAttributeDamageMultiplyRate(IGameEntityBridge p_Trigger, IGameEntityBridge p_Target)
        {
            // 공격 타입을 합쳐준다.
            var triggerAttribute = GameEntityTool.ElementType.Fire;

            // 합쳐진 공격 타입이 무속성 공격이었던 경우
            if (triggerAttribute == GameEntityTool.ElementType.None)
            {
                // 무속성 공격에, 공격력 보정은 100퍼센트가 된다.
                return (GameEntityTool.ElementType.None, 1f);
            }
            // 공격 타입이 무속성 이외였던 경우
            else
            {
                // 복합속성일지도 모르므로, 각 속성의 강화/저항값을 고려하여 가장 강력한 속성 타입 및 그 데미지 배율을 선정하여
                // 리턴해준다.
                var attributeEnumerator = GameEntityTool.ElementTypeEnumerator;
                var (maxGabAttribute, maxGab) = (GameEntityTool.ElementType.None, float.MinValue);
                foreach (var attributeType in attributeEnumerator)
                {
                    var gab = 0f;

                    switch (attributeType)
                    {
                        case GameEntityTool.ElementType.Fire:
                        {
                            var enhance = p_Trigger[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementEnhance_Fire];
                            var resist = p_Target[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementRegist_Fire];
                            gab = enhance - resist;
                            break;
                        }
                        case GameEntityTool.ElementType.Water:
                        {
                            var enhance = p_Trigger[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementEnhance_Water];
                            var resist = p_Target[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementRegist_Water];
                            gab = enhance - resist;
                            break;
                        }
                        case GameEntityTool.ElementType.Wind:
                        {
                            var enhance = p_Trigger[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementEnhance_Wind];
                            var resist = p_Target[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementRegist_Wind];
                            gab = enhance - resist;
                            break;
                        }
                        case GameEntityTool.ElementType.Ground:
                        {
                            var enhance = p_Trigger[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementEnhance_Ground];
                            var resist = p_Target[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementRegist_Ground];
                            gab = enhance - resist;
                            break;
                        }
                        case GameEntityTool.ElementType.Darkness:
                        {
                            var enhance = p_Trigger[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementEnhance_Darkness];
                            var resist = p_Target[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementRegist_Darkness];
                            gab = enhance - resist;
                            break;
                        }
                        case GameEntityTool.ElementType.Light:
                        {
                            var enhance = p_Trigger[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementEnhance_Light];
                            var resist = p_Target[StatusTool.BattleStatusGroupType.Total, BattleStatusTool.BattleStatusType.ElementRegist_Light];
                            gab = enhance - resist;
                            break;
                        }
                    }

                    // 가장 높은 속성 값으로 보정해준다.
                    if (gab > maxGab)
                    {
                        maxGab = gab;
                        maxGabAttribute = attributeType;
                    }
                }

                return (maxGabAttribute, 1f + maxGab * _Inverse_AttributeEnhance10PercentBoundValue);
            }
        }

        public static HitResult CalcDamage(DamageCalcType p_Type, float p_DamageRate, IGameEntityBridge p_Trigger, IGameEntityBridge p_Target)
        {
            var resultType = HitResultType.HitFail;
            var resultAttribute = GameEntityTool.ElementType.None;
            var resultDamage = 0f;

            // 타격 판정 참가 유닛들의 전투치
            var triggerBattleStatus = p_Trigger[StatusTool.BattleStatusGroupType.Total];
            var targetBattleStatus = p_Target[StatusTool.BattleStatusGroupType.Total];

            /* 회피 계산 */
            // 적중률 계산 = 기본 적중률 + 공격자 적중률 * 공격자 적중률 보정치 - 기본 회피율 - 피격자 회피율 * 피격자 회피율 보정치
            var hitRate = triggerBattleStatus.HitRate - targetBattleStatus.DodgeRate;

            // 적중률 하한을 보정해준다.
            hitRate = Mathf.Max(hitRate, HitRateLowerBound);

            if (hitRate < Random.value)
            {
                resultType = HitResultType.HitMissed;
            }
            else
            {
                /* 타격 계산 */
                // 크리티컬 확률 계산 = 기본 크리티컬 확률 + 공격자 크리티컬 확률 * 공격자 크리티컬 확률 보정치 + 타격 크리티컬 값 - 피격자 안티 크리티컬 확률
                var criticalRate = triggerBattleStatus.GetCriticalRate(p_Type) - targetBattleStatus.GetAntiCriticalRate(p_Type);
                
                // 크리티컬 확률 상한을 보정해준다.
                criticalRate = Mathf.Min(criticalRate, CriticalRateUpperBound);
                
                if (criticalRate > Random.value)
                {
                    resultType = HitResultType.HitCritical;
                    p_DamageRate *= (DefaultCriticalDamageRate * triggerBattleStatus[BattleStatusTool.BattleStatusType.DamageRate_Critical] * (1f - targetBattleStatus[BattleStatusTool.BattleStatusType.AntiDamageRate_Critical]));
                }
                else
                {
                    resultType = HitResultType.HitNormal;
                }
                
                // 기본 공격력, 방어력을 계산해준다.
                resultDamage = p_DamageRate * triggerBattleStatus.GetAttackBase(p_Type) - targetBattleStatus.GetDefenseBase(p_Type);
            
                // 곱연산을 하기 전에, 양수 보정을 해준다.
                resultDamage = Mathf.Max(0, resultDamage);

                // 피해 증가량을 적용시켜준다.
                resultDamage *= triggerBattleStatus.GetDamageRate(p_Type);
                resultDamage *= targetBattleStatus.GetAntiDamageRate(p_Type);
                
                // 속성 증가량을 적용시켜준다.
                var hitAttributeTuple = CalcAttributeDamageMultiplyRate(p_Trigger, p_Target);
                resultAttribute = hitAttributeTuple.Item1;
                resultDamage *= hitAttributeTuple.Item2;
            }

            return new HitResult(resultType, resultAttribute, resultDamage);
        }

        #endregion

        #region <Structs>

        public readonly struct HitResult
        {
            public readonly HitResultType HitResultType;
            public readonly GameEntityTool.ElementType ElementType;
            public readonly float ResultDamage;

            public HitResult(HitResultType p_ResultType, GameEntityTool.ElementType p_ElementType, float p_ResultDamage)
            {
                HitResultType = p_ResultType;
                ElementType = p_ElementType;
                ResultDamage = p_ResultDamage;
            }
        }

        #endregion
    }
}