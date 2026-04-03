namespace k514.Mono.Common
{
    public interface ITriggerEventHandler<out Key, CreateParams, in ActivateParams, TriggerRecord> : IGameEntityEventHandler<Key, CreateParams, ActivateParams>
        where CreateParams : ITriggerEventHandlerCreateParams<Key, TriggerRecord>
        where ActivateParams : ITriggerEventHandlerActivateParams
        where TriggerRecord : ITriggerEventHandlerRecord
    {
        /* Cost */
        bool HasEnoughCost();
        void PayCost();
        
        /* Cooldown */
        bool IsCooldown();
        float GetCooldownDuration();
        float GetCooldownElapsed();
        float GetCooldownRemained();
        float GetCooldownProgressRate();
        void SetCooldown(float p_Value);
        void RunCooldown();
        void ProgressCooldown(float p_DeltaTime);
        void ResetCooldown(bool p_CastEventFlag);
        
        /* Level */
        int Level { get; }
        bool IsMasterLevel { get; }
        bool AddLevel(int p_Value);
    }
    
    public interface ITriggerEventHandlerCreateParams<out Key, TriggerRecord> : IGameEntityEventHandlerCreateParams<Key>
        where TriggerRecord : ITriggerEventHandlerRecord
    {
        TriggerRecord Record { get; }
    }
    
    public interface ITriggerEventHandlerActivateParams : IGameEntityEventHandlerActivateParams
    {
        public int StartLevel { get; }
    }
    
    public abstract partial class TriggerEventHandlerBase<This, Key, CreateParams, ActivateParams, TriggerRecord> : GameEntityEventHandlerBase<This, Key, CreateParams, ActivateParams>, ITriggerEventHandler<Key, CreateParams, ActivateParams, TriggerRecord>
        where This : TriggerEventHandlerBase<This, Key, CreateParams, ActivateParams, TriggerRecord>, new()
        where CreateParams : ITriggerEventHandlerCreateParams<Key, TriggerRecord>
        where ActivateParams : ITriggerEventHandlerActivateParams
        where TriggerRecord : ITriggerEventHandlerRecord
    {
        #region <Fields>

        public TriggerRecord Record { get; private set; }

        #endregion

        #region <Callbacks>

        public override void OnCreate(IObjectContent<CreateParams> p_Wrapper, CreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);
            
            Record = p_CreateParams.Record;
            
            OnCreateCost();
            OnCreateCooldown();
        }
        
        public override bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                AddLevel(p_ActivateParams.StartLevel);
                
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public override void OnRetrieve(CreateParams p_CreateParams)
        {
            ResetCooldown(false);
            AddLevel(-Record.LevelUpperBound);

            base.OnRetrieve(p_CreateParams);
        }
 
        #endregion
    }
}