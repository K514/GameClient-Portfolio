using System;

namespace k514.Mono.Common
{
    [Serializable]
    public struct GameEntityGroupPreset
    {
        #region <Fields>

        /// <summary>
        /// 해당 유닛이 포함된 세력
        /// </summary>
        public readonly GameEntityTool.GameEntityGroupType MainGroupType;
        
        /// <summary>
        /// 해당 유닛이 우호하는 세력
        /// </summary>
        public GameEntityTool.GameEntityGroupType AllyMask { get; private set; }

        /// <summary>
        /// 해당 유닛이 적대하는 세력
        /// </summary>
        public GameEntityTool.GameEntityGroupType EnemyMask { get; private set; }

        #endregion

        #region <Constructors>

        public GameEntityGroupPreset(GameEntityTool.GameEntityGroupType p_MainGroupType, GameEntityTool.GameEntityGroupType p_AllyMask, GameEntityTool.GameEntityGroupType p_EnemyMask)
        {
            MainGroupType = p_MainGroupType;
            AllyMask = MainGroupType | p_AllyMask;
            EnemyMask = p_EnemyMask;
        }

        #endregion

        #region <Methods>

        public void SetAllyMask(GameEntityTool.GameEntityGroupType p_AllyMask)
        {
            AllyMask = MainGroupType | p_AllyMask;
        }

        public void SetEnemyMask(GameEntityTool.GameEntityGroupType p_EnemyMask)
        {
            EnemyMask = p_EnemyMask;
        }

        #endregion
    }
}