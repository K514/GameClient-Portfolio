namespace k514.Mono.Common
{
    public interface IGameEntityGroup
    {
        GameEntityGroupPreset GroupPreset { get; }
        void SetAllyMask(GameEntityTool.GameEntityGroupType p_AllyTypeMask);
        void SetEnemyMask(GameEntityTool.GameEntityGroupType p_EnemyTypeMask);
        GameEntityTool.GameEntityGroupType GetAllyMask();
        GameEntityTool.GameEntityGroupType GetEnemyMask();
        GameEntityTool.GameEntityGroupRelateType GetGroupRelate(IGameEntityBridge p_TargetUnit);
    }
}