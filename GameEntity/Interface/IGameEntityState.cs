namespace k514.Mono.Common
{
    public interface IGameEntityState : IGameEntityAttribute, IGameEntityState<GameEntityTool.EntityStateType>
    {
    }

    public interface IGameEntityAttribute
    {
        bool HasAttribute(GameEntityTool.GameEntityAttributeType p_Type);
        void AddAttribute(GameEntityTool.GameEntityAttributeType p_Type);
        void RemoveAttribute(GameEntityTool.GameEntityAttributeType p_Type);
    }       
    
    public interface IGameEntityState<State> where State : System.Enum
    {
        State EntityStateMask { get; }
        State GetDefaultState();
        int GetStateStackCount(GameEntityTool.EntityStateType p_Type);
        bool HasState_And(State p_CompareMask);  
        bool HasState_Or(State p_CompareMask);
        bool HasState_Only(State p_AvailableStateMask);
        void AddState(State p_Type);
        void RemoveState(State p_Type);
        void AddStateMask(GameEntityTool.EntityStateType p_Mask);
        void RemoveStateMask(GameEntityTool.EntityStateType p_Mask);
        void TurnState(State p_Type);
        void ClearState();
#if APPLY_PRINT_LOG
        void PrintState();
#endif
    }
}