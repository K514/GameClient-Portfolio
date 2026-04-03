using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public interface IDurationEventHandler<out Key, CreateParams, in ActivateParams> : IGameEntityEventHandler<Key, CreateParams, ActivateParams>
        where CreateParams : IDurationEventHandlerCreateParams<Key>
        where ActivateParams : IDurationEventHandlerActivateParams
    {
        bool Progress(float p_DeltaTime);
    }
    
    public interface IDurationEventHandlerCreateParams<out Key> : IGameEntityEventHandlerCreateParams<Key>
    {
    }
    
    public interface IDurationEventHandlerActivateParams : IGameEntityEventHandlerActivateParams
    {
        public float Duration { get; }
    }
    
    public abstract class DurationEventHandlerBase<This, Key, CreateParams, ActivateParams> : GameEntityEventHandlerBase<This, Key, CreateParams, ActivateParams>, IDurationEventHandler<Key, CreateParams, ActivateParams>
        where This : DurationEventHandlerBase<This, Key, CreateParams, ActivateParams>, new()
        where CreateParams : IDurationEventHandlerCreateParams<Key>
        where ActivateParams : IDurationEventHandlerActivateParams
    {
        #region <Fields>

        protected ProgressTimer _EventTimer;
        private DurationEventTool.EventPhase _EventPhase;

        #endregion

        #region <Enums>

  
        #endregion
        
        #region <Callbacks>

        public override bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _EventTimer = p_ActivateParams.Duration;

                return true;
            }
            else
            {
                return false;
            }
        }

        protected abstract void OnEventStart();
        protected abstract void OnEventProgress(float p_DeltaTime, float p_Rate);
        protected abstract void OnEventTerminate();
        
        public override void OnRetrieve(CreateParams p_CreateParams)
        {
            _EventPhase = DurationEventTool.EventPhase.None;
            
            base.OnRetrieve(p_CreateParams);
        }

        #endregion

        #region <Methods>

        protected void TerminateEvent()
        {
            switch (_EventPhase)
            {
                case DurationEventTool.EventPhase.None:
                case DurationEventTool.EventPhase.Progress:
                    _EventPhase = DurationEventTool.EventPhase.Terminate;
                    break;
            }
        }
        
        public virtual bool Progress(float p_DeltaTime)
        {
            if (IsHandlerValid())
            {
                switch (_EventPhase)
                {
                    default:
                    case DurationEventTool.EventPhase.None:
                    {
                        _EventPhase = DurationEventTool.EventPhase.Progress;   
                        OnEventStart();
                    
                        if (_EventTimer.IsOver())
                        {
                            TerminateEvent();
                        }
                        
                        return true;
                    }
                    case DurationEventTool.EventPhase.Progress:
                    {
                        if (_EventTimer.IsOver())
                        {
                            TerminateEvent();
                        }
                        else
                        {
                            _EventTimer.Progress(p_DeltaTime);
                            OnEventProgress(p_DeltaTime, _EventTimer.ProgressRate);
                        }
                        
                        return true;
                    }
                    case DurationEventTool.EventPhase.Terminate:
                    {
                        OnEventTerminate();
                        Pooling();
                        
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}