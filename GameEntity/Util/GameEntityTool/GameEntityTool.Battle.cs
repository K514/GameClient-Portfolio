namespace k514.Mono.Common
{
    public static partial class GameEntityTool
    {
        #region <Consts>

        public const int LevelUpperBound = 50;
        public const float LevelUpStatusBonusRate = 1.014f;
        public const float LevelUpStatusBonusRateInv = 1f / LevelUpStatusBonusRate;
        public static readonly BattleStatusPreset LevelUpBattleStatusBonus;
        
        #endregion

        #region <Methods>

        public static float GetLevelUpStatusBonusRate(int p_Delta)
        {
            if (p_Delta < 0)
            {
                return LevelUpStatusBonusRateInv.Pow(-p_Delta);
            }
            else
            {
                return LevelUpStatusBonusRate.Pow(p_Delta);
            }
        }

        #endregion
    }
}