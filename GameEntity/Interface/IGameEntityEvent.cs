using System.Collections.Generic;

namespace k514.Mono.Common
{
    public interface IGameEntityEvent : IAnimatorEventReceiver, IMotionEventReceiver, 
        IGameEntityUpdateEvent, IGameEntityEventReceiver, IGameEntityEventHandler
    {
        
    }

    public interface IGameEntityUpdateEvent : IUpdateEvent
    {
        void ReserveUpdatePosition();
        void ReserveSkillChange();
        void ReservePooling();
        void ReserveInventoryUpdate();
    }

    public interface IGameEntityEventReceiver
    {
        void AddReceiver(GameEntityBaseEventReceiver p_EventReceiver);
        void AddReceiver(GameEntityUIEventReceiver p_EventReceiver);
        void AddReceiver(GameEntityModuleEventReceiver p_EventReceiver);
        void SendEvent(GameEntityTool.GameEntityBaseEventType p_Type, GameEntityBaseEventParams p_Params);
        void SendEvent(GameEntityTool.GameEntityUIEventType p_Type, GameEntityUIEventParams p_Params);
        void SendEvent(GameEntityTool.GameEntityModuleEventType p_Type, GameEntityModuleEventParams p_Params);
        void RemoveReceiver(GameEntityBaseEventReceiver p_EventReceiver);
        void RemoveReceiver(GameEntityUIEventReceiver p_EventReceiver);
        void RemoveReceiver(GameEntityModuleEventReceiver p_EventReceiver);
#if !SERVER_DRIVE
        void AddReceiver(GameEntityRenderEventReceiver p_EventReceiver);
        void SendEvent(GameEntityTool.GameEntityRenderType p_Type, GameEntityRenderEventParams p_Params);
        void RemoveReceiver(GameEntityRenderEventReceiver p_EventReceiver);
#endif
    }

    public interface IGameEntityEventHandler
    {
        /* EventHandler */
        void TerminateAllEventHandler();
        
        /* Affine EventHandler */
        List<IAffineEventHandler> GetAffineEventHandlerList();
        bool TryRunAffineEvent(AffineEventTool.AffineEventType p_Key, AffineEventHandlerActivateParams p_Params);
        void TerminateAllAffineHandler();
#if UNITY_EDITOR
        void PrintAffineHandlerList();
#endif
        /* Instance EventHandler */
        List<IInstanceEventHandler> GetInstanceEventHandlerList();
        bool TryRunInstanceEvent(InstanceEventTool.InstanceEventType p_Key, InstanceEventHandlerActivateParams p_Params);
        bool TryRunReservedInstanceEvent();
        void ResetReservedInstanceEvent();
        void TerminateAllInstanceHandler();
#if UNITY_EDITOR
        void PrintInstanceHandlerList();
#endif
        /* Filter EventHandler */
        List<IFilterEventHandler> GetFilterEventHandlerList();
        bool TryRunFilterEvent(FilterEventTool.FilterEventType p_Key, FilterEventHandlerActivateParams p_Params);
        void TerminateAllFilterHandler();
    #if UNITY_EDITOR
        void PrintFilterHandlerList();
#endif
        /* Enchant EventHandler */
        List<IEnchantEventHandler> GetEnchantEventHandlerList();
        bool TryRunEnchantEvent(int p_Key, EnchantEventHandlerActivateParams p_Params);
        void TerminateAllEnchantHandler();
#if UNITY_EDITOR
        void PrintEnchantHandlerList();
#endif
        /* Item EventHandler */
        List<IItemEventHandler> GetItemEventHandlerList();
        bool TryRunItemEvent(int p_Key, ItemEventHandlerActivateParams p_Params);
        void TerminateAllItemHandler();
#if UNITY_EDITOR
        void PrintItemHandlerList();
#endif
    }
}