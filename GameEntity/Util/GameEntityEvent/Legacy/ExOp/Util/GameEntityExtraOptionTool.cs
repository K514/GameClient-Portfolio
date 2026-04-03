using System;

namespace k514.Mono.Common
{
    public static class GameEntityExtraOptionTool
    {
        #region <Enums>
                
        [Flags]
        public enum HandlerAttributeType
        {
            None = 0,
            
            LifeSpan = 1 << 0,
            Cooldown = 1 << 1,
            EntityEvent = 1 << 2,
            TickEvent = 1 << 3,
            RemainHandler = 1 << 4,

            HasLeveling = 1 << 8,
            HasState = 1 << 9,
            HasAdditiveBattleStatus = 1 << 10,
            HasSimpleMultiplyBattleStatus = 1 << 11,
            HasCompoundMultiplyBattleStatus = 1 << 12,
            HasAdditiveShotStatus = 1 << 13,
            HasSimpleMultiplyShotStatus = 1 << 14,
            HasCompoundMultiplyShotStatus = 1 << 15,
            HasStatus = HasAdditiveBattleStatus | HasSimpleMultiplyBattleStatus | HasCompoundMultiplyBattleStatus | HasAdditiveShotStatus | HasSimpleMultiplyShotStatus | HasCompoundMultiplyShotStatus,
        }
        
        public enum ExtraOptionType
        {
            None,
            
            AddAdditiveBattleStatus,
            AddSimpleMultiplyBattleStatus,
            AddCompoundMultiplyBattleStatus,

            AddAdditiveShotStatus,
            AddSimpleMultiplyShotStatus,
            AddCompoundMultiplyShotStatus,

            AddActionLevel,
            CastEnchantToSelf,
            CastEnchantToHit,
            CastEnchantToStrike,
            UseItemWhenStartStage,
            
            HpRecoveryFix,
            HpRecoveryRate,
            MpRecoveryFix,
            MpRecoveryRate,
            
            ManaStoneGamble,
            GoldGamble,
            SkillGamble,
            StatusGamble,
            ConsumeGamble,
            
            ContractNPC,
            FirstRerollService,
            SummonSkeleton,
            PurifyZone,
            CallMeteor,
            BlastZone,
            MonsterReborn,
            
            Titanize,
            ExtraShot,
            BurstShot,
            AddVfx
        }

        #endregion

        #region <Preset>

        public struct GameEntityExtraOptionParams
        {
            #region <Fields>

            public readonly int Count;
            public readonly float Value;
            public readonly bool Flag;
            public readonly ITableRecord Record;
            public readonly bool ValidFlag;
            
            #endregion
            
            #region <Constructor>

            public GameEntityExtraOptionParams(int p_Count)
            {
                this = default;
                
                Count = p_Count;
                ValidFlag = true;
            }
            
            public GameEntityExtraOptionParams(float p_Value)
            {
                this = default;

                Value = p_Value;
                ValidFlag = true;
            }
            
            public GameEntityExtraOptionParams(bool p_Flag)
            {
                this = default;

                Flag = p_Flag;
                ValidFlag = true;
            }
            
            public GameEntityExtraOptionParams(ITableRecord p_Record)
            {
                this = default;

                Record = p_Record;
                ValidFlag = true;
            }

            #endregion
        }

        #endregion
    }
}