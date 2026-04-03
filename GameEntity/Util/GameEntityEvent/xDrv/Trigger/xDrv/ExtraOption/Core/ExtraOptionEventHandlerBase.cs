using System;

namespace k514.Mono.Common
{
    public interface IExtraOptionEventHandler : ITriggerEventHandler<int, ExtraOptionEventHandlerCreateParams, ExtraOptionEventHandlerActivateParams, ExtraOptionDataTable.TableRecord>
    {
        IExtraOptionContainer ExtraOptionContainer { get; }
    }
    
    public readonly struct ExtraOptionEventHandlerCreateParams : ITriggerEventHandlerCreateParams<int, ExtraOptionDataTable.TableRecord>
    {
        public int EventId { get; }
        public ExtraOptionDataTable.TableRecord Record { get; }
        public Type HandlerType => Record.EventHandlerType;

        public ExtraOptionEventHandlerCreateParams(int p_EventId)
        {
            EventId = p_EventId;
            Record = ExtraOptionDataTable.GetInstanceUnsafe[EventId];
        }
    }

    public readonly struct ExtraOptionEventHandlerActivateParams : ITriggerEventHandlerActivateParams
    {
        public IGameEntityBridge Entity { get; }
        public int StartLevel { get; }
        public readonly IExtraOptionContainer ExtraOptionContainer;
        
        public ExtraOptionEventHandlerActivateParams(IExtraOptionContainer p_ExtraOptionContainer, int p_Level)
        {
            ExtraOptionContainer = p_ExtraOptionContainer;
            Entity = ExtraOptionContainer.Entity;
            StartLevel = p_Level;
        }
    }
    
    public abstract partial class ExtraOptionEventHandlerBase<This> : TriggerEventHandlerBase<This, int, ExtraOptionEventHandlerCreateParams, ExtraOptionEventHandlerActivateParams, ExtraOptionDataTable.TableRecord>, IExtraOptionEventHandler
        where This : ExtraOptionEventHandlerBase<This>, new()
    {
        #region <Fields>

        public IExtraOptionContainer ExtraOptionContainer { get; private set; }

        #endregion

        #region <Callbacks>

        public override bool OnActivate(ExtraOptionEventHandlerCreateParams p_CreateParams, ExtraOptionEventHandlerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                ExtraOptionContainer = p_ActivateParams.ExtraOptionContainer;
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        #endregion

        #region <Methods>

        public override bool IsEnterable()
        {
            return !IsCooldown() && HasEnoughCost();
        }
    
        #endregion
    }
}