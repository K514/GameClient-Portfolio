using System;
using System.Collections.Generic;
using System.Threading;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public struct GameEntityExtraOptionHandlerCreateParam : IObjectCreateParams
    {
        #region <Fields>

        public IGameEntityEventContainer Container { get; private set; }

        #endregion

        #region <Constructors>

        public GameEntityExtraOptionHandlerCreateParam(IGameEntityEventContainer p_Container)
        {
            this = default;

            Container = p_Container;
        }

        #endregion
        
        #region <Callbacks>

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        private void OnDisposeUnmanaged()
        {
        }

        #endregion
    
        #region <Disposable>

        /// <summary>
        /// dispose 패턴 onceFlag
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// dispose 플래그를 초기화 시키는 메서드
        /// </summary>
        public void Rejuvenate()
        {
            IsDisposed = false;
        }

        /// <summary>
        /// 인스턴스 파기 메서드
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            else
            {
                IsDisposed = true;
                OnDisposeUnmanaged();
            }
        }
        
        #endregion
    }
    
    public struct GameEntityExtraOptionHandlerActivateParam : IObjectActivateParams
    {
        #region <Fields>

        public ExtraOptionDataTable.TableRecord Record { get; }
        public GameEntityExtraOptionBase Option { get; }
        public GameEntityExtraOptionTool.GameEntityExtraOptionParams Params { get; }

        #endregion

        #region <Constructors>

        public GameEntityExtraOptionHandlerActivateParam(GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params = default)
        {
            Record = default;
            Option = default;
            Params = p_Params;
        }
        
        public GameEntityExtraOptionHandlerActivateParam(ExtraOptionDataTable.TableRecord p_Record, GameEntityExtraOptionBase p_Option, GameEntityExtraOptionTool.GameEntityExtraOptionParams p_Params = default)
        {
            Record = p_Record;
            Option = p_Option;
            Params = p_Params;
        }

        #endregion
    }
    
    public class GameEntityExtraOptionHandler : ObjectPoolContent<GameEntityExtraOptionHandler, GameEntityExtraOptionHandlerCreateParam, GameEntityExtraOptionHandlerActivateParam>
    {
        #region <Fields>

        public IGameEntityEventContainer Container { get; private set; }
        public GameEntityExtraOptionBase Option { get; private set; }
        public IGameEntityBridge Caster => Container.Caster;
        public GameEntityEventCommonParams CommonParams => Container.CommonParams;
        private GameEntityExtraOptionHandlerActivateParam _ActivateParams;
        public ExtraOptionDataTable.TableRecord Record => _ActivateParams.Record;
        public GameEntityExtraOptionTool.GameEntityExtraOptionParams ExtraOptionParams => _ActivateParams.Params;
        public ProgressTimerWrap LifeSpanTimer { get; private set; }
        public ProgressTimerWrap CoolDownTimer { get; private set; }
        public ProgressTimerWrap TickTimer { get; private set; }
        private GameEntityBaseEventReceiver _CasterEventReceiver;
        private Dictionary<GameEntityTool.GameEntityBaseEventType, Action<GameEntityExtraOptionHandler, GameEntityBaseEventParams, GameEntityExtraOptionTool.GameEntityExtraOptionParams>> _EventActionTable;
        private Action<GameEntityExtraOptionHandler, GameEntityExtraOptionTool.GameEntityExtraOptionParams> _TickEvent;
        private GameEntityExtraOptionTool.HandlerAttributeType _AttributeMask;
        private BattleStatusPreset _AdditiveBattleStatus;
        private BattleStatusPreset _SimpleMultiplyBattleStatus;
        private BattleStatusPreset _CompoundMultiplyBattleStatus;
        private ShotStatusPreset _AdditiveShotStatus;
        private ShotStatusPreset _SimpleMultiplyShotStatus;
        private ShotStatusPreset _CompoundMultiplyShotStatus;
        private GameEntityTool.EntityStateType _EnchantStateMask;
        private Dictionary<int, int> _ActionLevelTable;
        protected CancellationToken _CancellationToken;
        private CancellationTokenSource _CancellationTokenSource;
        private PhaseType _CurrentPhase;
        public bool IsCooldown => _AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.Cooldown);
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(GameEntityExtraOptionHandlerCreateParam p_CreateParams)
        {
            Container = p_CreateParams.Container;
            LifeSpanTimer = new ProgressTimerWrap();
            CoolDownTimer = new ProgressTimerWrap();
            TickTimer = new ProgressTimerWrap();
            _EventActionTable = new Dictionary<GameEntityTool.GameEntityBaseEventType, Action<GameEntityExtraOptionHandler, GameEntityBaseEventParams, GameEntityExtraOptionTool.GameEntityExtraOptionParams>>();
            _ActionLevelTable = new Dictionary<int, int>();
        }

        protected override bool OnActivate(GameEntityExtraOptionHandlerCreateParam p_CreateParams, GameEntityExtraOptionHandlerActivateParam p_ActivateParams, bool p_IsPooled)
        {
            _ActivateParams = p_ActivateParams;
            Option = p_ActivateParams.Option;
            
            LifeSpanTimer.Reset();
            CoolDownTimer.Reset();
            TickTimer.Reset();
            
            _AdditiveBattleStatus = default;
            _SimpleMultiplyBattleStatus = default;
            _CompoundMultiplyBattleStatus = 1f;
            
            _AdditiveShotStatus = default;
            _SimpleMultiplyShotStatus = default;
            _CompoundMultiplyShotStatus = 1f;

            Container.GetLinkedCancellationTokenSource(ref _CancellationTokenSource);
            _CancellationToken = _CancellationTokenSource.Token;
            
            return true;
        }

        protected override void OnRetrieve(GameEntityExtraOptionHandlerCreateParam p_CreateParams)
        {
            CancelHandler(false);

            _CurrentPhase = PhaseType.None;
            Option = default;
            _ActivateParams = default;
            _CancellationTokenSource.Cancel();
            _CancellationTokenSource = default;
            _CancellationToken = default;
        }

        protected override void OnDispose()
        {
        }

        public bool OnUpdate(float p_DeltaTime)
        {
            switch (_CurrentPhase)
            {
                default:
                {
                    return false;
                }
                case PhaseType.Terminate:
                {
                    return true;
                }
                case PhaseType.Running:
                {
                    if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.TickEvent))
                    {
                        if (TickTimer.IsOver())
                        {
                            _TickEvent(this, default);
                            TickTimer.Reset();
                        }
                        else
                        {
                            TickTimer.Progress(p_DeltaTime);
                        }
                    }
                
                    if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.Cooldown))
                    {
                        if (CoolDownTimer.IsOver())
                        {
                            _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.Cooldown);
                            CoolDownTimer.Reset();
                        }
                        else
                        {
                            CoolDownTimer.Progress(p_DeltaTime);
                        }
                    }
                
                    if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.LifeSpan))
                    {
                        if (LifeSpanTimer.IsOver())
                        {
                            return true;
                        }
                        else
                        {
                            LifeSpanTimer.Progress(p_DeltaTime);
                        
                            return false;
                        }
                    }
                    else
                    {
                        return _AttributeMask == GameEntityExtraOptionTool.HandlerAttributeType.None;
                    }   
                }
            }
        }
        
        private void OnHandleGameEntityBaseEvent(GameEntityTool.GameEntityBaseEventType p_Type, GameEntityBaseEventParams p_Params)
        {
            if (!IsCooldown && _EventActionTable.TryGetValue(p_Type, out var o_Action))
            {
                o_Action(this, p_Params, ExtraOptionParams);
            }
        }

        #endregion

        #region <Methods>

        public void PreloadHandler()
        {
            Option?.PreloadOption(this);
        }
        
        public bool RunHandler()
        {
            switch (_CurrentPhase)
            {
                default:
                {
                    return false;
                }
                case PhaseType.None:
                {
                    _CurrentPhase = PhaseType.Running;

                    /* 옵션 발동 */
                    Option?.ActivateOption(this);

                    /* 이벤트 연결 */
                    var caster = Caster;
                    var mask = GameEntityTool.GameEntityBaseEventType.None;
                    foreach (var eventKV in _EventActionTable)
                    {
                        var eventType = eventKV.Key;
                        mask.AddFlag(eventType);
                    }

                    if (mask != GameEntityTool.GameEntityBaseEventType.None)
                    {
                        _CasterEventReceiver = new GameEntityBaseEventReceiver(mask, OnHandleGameEntityBaseEvent);
                        caster.AddReceiver(_CasterEventReceiver);
                    }
                    
                    return _AttributeMask != GameEntityExtraOptionTool.HandlerAttributeType.None;
                }
            }
        }

        public bool CancelHandler(bool p_InstantPooling)
        {
            switch (_CurrentPhase)
            {
                default:
                {                   
                    _CurrentPhase = PhaseType.Terminate;

                    if (p_InstantPooling)
                    {
                        Pooling();
                    }
                    
                    return false;
                }
                case PhaseType.Running:
                {
                    _CurrentPhase = PhaseType.Terminate;

                    Option?.DeactivateOption(this);
      
                    ResetEvent();
                    ResetTickEvent();
                    ResetStatus();
                    ResetState();
                    ResetLeveling();
                    ResetAttribute();

                    if (p_InstantPooling)
                    {
                        Pooling();
                    }
                    
                    return true;
                }
            }
        }

        public void SetLifeSpan(float p_Duration)
        {
            if (p_Duration > 0f)
            {
                LifeSpanTimer.SetDuration(p_Duration, false);
                _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.LifeSpan);
            }
            else
            {
                LifeSpanTimer.SetDuration(0f, false);
                _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.LifeSpan);
            }
        }
        
        public void SetCooldown(float p_Duration)
        {
            if (p_Duration > 0f)
            {
                CoolDownTimer.SetDuration(p_Duration, false);
                _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.Cooldown);
            }
            else
            {
                CoolDownTimer.SetDuration(0f, false);
                _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.Cooldown);
            }
        }
 
        public void SetHandlerRemain(bool p_Flag)
        {
            if (p_Flag)
            {
                _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.RemainHandler);
            }
            else
            {
                _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.RemainHandler);
            }
        }
        
        public void ResetAttribute()
        {
            _AttributeMask = GameEntityExtraOptionTool.HandlerAttributeType.None;
        }

        public void AddEvent(GameEntityTool.GameEntityBaseEventType p_Type, Action<GameEntityExtraOptionHandler, GameEntityBaseEventParams, GameEntityExtraOptionTool.GameEntityExtraOptionParams> p_Event)
        {
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.EntityEvent);
            if (_EventActionTable.TryGetValue(p_Type, out var o_Action))
            {
                o_Action += p_Event;
                _EventActionTable[p_Type] = o_Action;
            }
            else
            {
                _EventActionTable.Add(p_Type, p_Event);
            }   
        }

        public void ResetEvent()
        {
            _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.EntityEvent);
            _EventActionTable.Clear();
            
            if (!ReferenceEquals(null, _CasterEventReceiver))
            {
                _CasterEventReceiver.Dispose();
                _CasterEventReceiver = default;
            }
        }
        
        public void AddTickEvent(float p_Interval, Action<GameEntityExtraOptionHandler, GameEntityExtraOptionTool.GameEntityExtraOptionParams> p_Event, bool p_Instant)
        {
            TickTimer.SetDuration(p_Interval, false);
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.TickEvent);
            _TickEvent = p_Event;

            if (p_Instant)
            {
                _TickEvent(this, default);
            }
        }
        
        public void ResetTickEvent()
        {
            _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.TickEvent);
            _TickEvent = default;
        }
        
        public void AddLeveling(int p_ActionIndex, int p_Level)
        {
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasLeveling);
            Caster.ActionModule.BindAction(new ActionTool.ActionBindPreset(p_ActionIndex, p_Level));
            
            if (_ActionLevelTable.TryGetValue(p_ActionIndex, out var o_Level))
            {
                _ActionLevelTable[p_ActionIndex] = o_Level + p_Level;
            }
            else
            {
                _ActionLevelTable.Add(p_ActionIndex, p_Level);
            }   
        }

        public void ResetLeveling()
        {
            _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasLeveling);

            foreach (var kv in _ActionLevelTable)
            {
                var actionIndex = kv.Key;
                var level = kv.Value;
                // Caster.ActionModule.ReleaseAction(new ActionTool.ActionBindPreset(actionIndex, level));
            }
            
            _ActionLevelTable.Clear();
        }

        public void AddAdditiveStatus(BattleStatusPreset p_Preset)
        {
            var caster = Caster;
            caster.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, p_Preset, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
            _AdditiveBattleStatus += p_Preset;
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasAdditiveBattleStatus);
        }

        public void AddSimpleMultiplyStatus(BattleStatusPreset p_Preset)
        {
            var caster = Caster;
            caster.AddStatus(StatusTool.BattleStatusGroupType.SimpleMul, p_Preset, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
            _SimpleMultiplyBattleStatus += p_Preset;
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasSimpleMultiplyBattleStatus);
        }
        
        public void AddCompoundMultiplyStatus(BattleStatusPreset p_Preset)
        {
            var caster = Caster;
            var corrected = 1f + p_Preset;
            caster.AddStatus(StatusTool.BattleStatusGroupType.CompoundMul, corrected, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
            _CompoundMultiplyBattleStatus *= corrected;
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasCompoundMultiplyBattleStatus);
        }
        
        public void AddAdditiveStatus(ShotStatusPreset p_Preset)
        {
            var caster = Caster;
            caster.AddStatus(StatusTool.ShotStatusGroupType.ExtraAdd, p_Preset, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
            _AdditiveShotStatus += p_Preset;
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasAdditiveShotStatus);
        }

        public void AddSimpleMultiplyStatus(ShotStatusPreset p_Preset)
        {
            var caster = Caster;
            caster.AddStatus(StatusTool.ShotStatusGroupType.SimpleMul, p_Preset, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
            _SimpleMultiplyShotStatus += p_Preset;
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasSimpleMultiplyShotStatus);
        }
        
        public void AddCompoundMultiplyStatus(ShotStatusPreset p_Preset)
        {
            var caster = Caster;
            var corrected = 1f + p_Preset;
            caster.AddStatus(StatusTool.ShotStatusGroupType.CompoundMul, p_Preset, new StatusTool.StatusChangeParams(StatusTool.StatusChangeAttribute.ApplyCurrent));
            _CompoundMultiplyShotStatus *= corrected;
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasCompoundMultiplyShotStatus);
        }

        public void ResetStatus()
        {
            var caster = Caster;
            if (!ReferenceEquals(null, caster))
            {
                if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.HasAdditiveBattleStatus))
                {
                    _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasAdditiveBattleStatus);
                    caster.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, -_AdditiveBattleStatus);
                    _AdditiveBattleStatus = default;
                }
                if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.HasSimpleMultiplyBattleStatus))
                {
                    _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasSimpleMultiplyBattleStatus);
                    caster.AddStatus(StatusTool.BattleStatusGroupType.SimpleMul, -_SimpleMultiplyBattleStatus);
                    _SimpleMultiplyBattleStatus = default;
                }
                if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.HasCompoundMultiplyBattleStatus))
                {
                    _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasCompoundMultiplyBattleStatus);
                    caster.AddStatus(StatusTool.BattleStatusGroupType.CompoundMul, 1f / _CompoundMultiplyBattleStatus);
                    _CompoundMultiplyBattleStatus = 1f;
                }
                if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.HasAdditiveShotStatus))
                {
                    _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasAdditiveShotStatus);
                    caster.AddStatus(StatusTool.ShotStatusGroupType.ExtraAdd, -_AdditiveShotStatus);
                    _AdditiveShotStatus = default;
                }
                if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.HasSimpleMultiplyShotStatus))
                {
                    _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasSimpleMultiplyShotStatus);
                    caster.AddStatus(StatusTool.ShotStatusGroupType.SimpleMul, -_SimpleMultiplyShotStatus);
                    _SimpleMultiplyShotStatus = default;
                }
                if (_AttributeMask.HasAnyFlagExceptNone(GameEntityExtraOptionTool.HandlerAttributeType.HasCompoundMultiplyShotStatus))
                {
                    _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasCompoundMultiplyShotStatus);
                    caster.AddStatus(StatusTool.ShotStatusGroupType.CompoundMul, 1f / _CompoundMultiplyShotStatus);
                    _CompoundMultiplyShotStatus = 1f;
                }
            }
        }
        
        public void AddState(GameEntityTool.EntityStateType p_Mask)
        {
            _AttributeMask.AddFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasState);
            Caster.AddStateMask(p_Mask);
            _EnchantStateMask.AddFlag(p_Mask);
        }
        
        public void ResetState()
        {
            _AttributeMask.RemoveFlag(GameEntityExtraOptionTool.HandlerAttributeType.HasState);
            Caster.RemoveStateMask(_EnchantStateMask);
            _EnchantStateMask = default;
        }
        
        #endregion
    }
}