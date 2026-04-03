using System;

namespace k514.Mono.Common
{
    public interface IGameEntityStatus : 
        IGameEntityStatusIndexer<StatusTool.BaseStatusGroupType, BaseStatusTool.BaseStatusType>,
        IGameEntityStatusIndexer<StatusTool.BattleStatusGroupType, BattleStatusTool.BattleStatusType>,
        IGameEntityStatusIndexer<StatusTool.BattleStatusGroupType, BattleStatusTool.BattleStatusType1>,
        IGameEntityStatusIndexer<StatusTool.BattleStatusGroupType, BattleStatusTool.BattleStatusType2>,
        IGameEntityStatusIndexer<StatusTool.BattleStatusGroupType, BattleStatusTool.BattleStatusType3>,
        IGameEntityStatusIndexer<StatusTool.ShotStatusGroupType, ShotStatusTool.ShotStatusType>,
        IGameEntityStatusQuery<StatusTool.BaseStatusGroupType, BaseStatusTool.BaseStatusType, BaseStatusPreset>,
        IGameEntityStatusQuery<StatusTool.BattleStatusGroupType, BattleStatusTool.BattleStatusType, BattleStatusPreset>,
        IGameEntityStatusQuery<StatusTool.ShotStatusGroupType, ShotStatusTool.ShotStatusType, ShotStatusPreset>
    {
    }
    
    public interface IGameEntityStatusIndexer<GroupType, StatusType>
        where GroupType : struct, Enum 
        where StatusType : struct, Enum
    {
        float this[GroupType p_GroupType, StatusType p_StatusType] { get; }
        float this[GroupType p_GroupType, StatusType p_StatusType, float p_Rate] { get; }
    }
    
    public interface IGameEntityStatusQuery<GroupType, StatusType, StatusPreset>
        where GroupType : struct, Enum 
        where StatusType : struct, Enum
        where StatusPreset : struct
    {
        StatusPreset this[GroupType p_GroupType] { get; }
        void AddStatus(GroupType p_GroupType, StatusPreset p_Preset, StatusTool.StatusChangeParams p_Params = default);
        void AddStatus(GroupType p_GroupType, StatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default);
        void SetStatus(GroupType p_GroupType, StatusType p_StatusType, float p_Value, StatusTool.StatusChangeParams p_Params = default);
        void AddStatusRate(GroupType p_GroupType, StatusType p_StatusType, float p_Rate, StatusTool.StatusChangeParams p_Params = default);
    }
}