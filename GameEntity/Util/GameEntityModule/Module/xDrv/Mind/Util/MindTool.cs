using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public static class MindTool
    {
        #region <Consts>

        public const int __MIND_COMMAND_PRIORITY_DEFAULT = 50;

        #endregion
        
        #region <Enums>

        public enum MindOrderType
        {
            Idle,
            Navigate,
            InputCommand,
            Delay,
            FreeDelay,
            Disable,
        }

        public enum MindOrderPhase
        {
            None,
            Init,
            PreDelay,
            Ready,
            Progressing,
            Fail,
            Success,
        }

        public enum MindOrderReserveType
        {
            /// <summary>
            /// 명령 추가
            /// </summary>
            Add,
            
            /// <summary>
            /// 명령 추가, 앞의 명령이 실패한 경우 해당 명령을 실행하지 않고 따라서 실패 처리함
            /// </summary>
            Add_RelayCancel,
            
            /// <summary>
            /// 명령 큐를 비우고 추가, 현재 진행중인 오더 유지
            /// </summary>
            Overlap,
            
            /// <summary>
            /// 명령 큐를 비우고 추가, 현재 진행중인 오더 취소
            /// </summary>
            Instant,
        }

        [Flags]
        public enum MoveOrderAttributeType
        {
            None = 0,
            
            MoveMotionType = 1 << 0,
        }
        
        #endregion

        #region <Methods>

        public static bool IsAutonomy(this MindModuleDataTableQuery.TableLabel p_Type)
        {
            switch (p_Type)
            {
                case MindModuleDataTableQuery.TableLabel.Autonomy:
                {
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }

        #endregion
        
        #region <Preset>

        public struct MoveOrderParams
        {
            #region <Fields>

            public readonly MoveOrderAttributeType AttributeMask;
            public readonly GeometryTool.NavigateDestinationPreset DestinationPreset;
            public readonly AnimationTool.MoveMotionType MoveMotionType;

            #endregion

            #region <Constructor>
            
            public MoveOrderParams(Vector3 p_TargetPositon, GeometryTool.NavigationAttributeFlag p_AttributeFlag, float p_MinDistance = GeometryTool.__Default_Navigation_MinDistance, float p_MaxDistance = GeometryTool.__Default_Navigation_MaxDistance)
                : this(new GeometryTool.NavigateDestinationPreset(p_TargetPositon, p_AttributeFlag, p_MinDistance, p_MaxDistance))
            {
            }
            
            public MoveOrderParams(IGameEntityBridge p_TargetEntity, GeometryTool.NavigationAttributeFlag p_AttributeFlag, float p_MinDistance = GeometryTool.__Default_Navigation_MinDistance, float p_MaxDistance = GeometryTool.__Default_Navigation_MaxDistance)
                : this(new GeometryTool.NavigateDestinationPreset(p_TargetEntity, p_AttributeFlag, p_MinDistance, p_MaxDistance))
            {
            }

            public MoveOrderParams(Vector3 p_TargetPositon, GeometryTool.NavigationAttributeFlag p_AttributeFlag, AnimationTool.MoveMotionType p_MoveMotionType, float p_MinDistance = GeometryTool.__Default_Navigation_MinDistance, float p_MaxDistance = GeometryTool.__Default_Navigation_MaxDistance)
                : this(new GeometryTool.NavigateDestinationPreset(p_TargetPositon, p_AttributeFlag, p_MinDistance, p_MaxDistance), p_MoveMotionType)
            {
            }
            
            public MoveOrderParams(IGameEntityBridge p_TargetEntity, GeometryTool.NavigationAttributeFlag p_AttributeFlag, AnimationTool.MoveMotionType p_MoveMotionType, float p_MinDistance = GeometryTool.__Default_Navigation_MinDistance, float p_MaxDistance = GeometryTool.__Default_Navigation_MaxDistance)
                : this(new GeometryTool.NavigateDestinationPreset(p_TargetEntity, p_AttributeFlag, p_MinDistance, p_MaxDistance), p_MoveMotionType)
            {
            }

            public MoveOrderParams(GeometryTool.NavigateDestinationPreset p_Destination, AnimationTool.MoveMotionType p_MoveMotionType) 
                : this(p_Destination)
            {
                MoveMotionType = p_MoveMotionType;
                AttributeMask = MoveOrderAttributeType.MoveMotionType;
            }

            public MoveOrderParams(GeometryTool.NavigateDestinationPreset p_Destination)
            {
                this = default;
                DestinationPreset = p_Destination;
            }

            #endregion
        }
        
        public struct MindOrderCreateParams : IObjectCreateParams
        {
            #region <Fields>

            public readonly IGameEntityBridge Entity;
            public readonly IMindModule MindModule;

            #endregion

            #region <Constructors>

            public MindOrderCreateParams(IGameEntityBridge p_Entity, IMindModule p_MindModule)
            {
                Entity = p_Entity;
                MindModule = p_MindModule;
                IsDisposed = false;
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
                
        public struct MindOrderActivateParams : IObjectActivateParams
        {
            #region <Fields>

            public readonly MindOrderType MindOrderType;
            public readonly AnimationTool.IdleMotionType IdleState;
            public readonly MoveOrderParams MoveOrderParams;
            public readonly InputEventTool.TriggerKeyType ReservedInputCommand;
            public readonly float Delay;
            
            #endregion
            
            #region <Constructors>

            public MindOrderActivateParams(MindOrderType p_Type, float p_Delay)
            {
                MindOrderType = p_Type;
                IdleState = default;
                MoveOrderParams = default;
                ReservedInputCommand = default;
                Delay = p_Delay;
            }
            
            public MindOrderActivateParams(AnimationTool.IdleMotionType p_Type, float p_Delay)
            {
                MindOrderType = MindOrderType.Idle;
                IdleState = p_Type;
                MoveOrderParams = default;
                ReservedInputCommand = default;
                Delay = p_Delay;
            }
            
            public MindOrderActivateParams(MoveOrderParams p_Params, float p_Delay)
            {
                MindOrderType = MindOrderType.Navigate;
                IdleState = default;
                MoveOrderParams = p_Params;
                ReservedInputCommand = default;
                Delay = p_Delay;
            }
            
            public MindOrderActivateParams(InputEventTool.TriggerKeyType p_Type, float p_Delay)
            {
                MindOrderType = MindOrderType.InputCommand;
                IdleState = default;
                MoveOrderParams = default;
                ReservedInputCommand = p_Type;
                Delay = p_Delay;
            }

            #endregion
        }
        
        public class MindOrder : ObjectPoolContent<MindOrder, MindOrderCreateParams, MindOrderActivateParams>
        {
            #region <Fields>

            private IGameEntityBridge _Entity;
            public MindOrderActivateParams MindOrderParams;
            public MindOrderPhase MindOrderPhase;
            public ProgressTimer DelayTimer;
            private bool _RelayCancelFlag;
            public MindOrderType MindOrderType => MindOrderParams.MindOrderType;
            
            #endregion

            #region <Callbacks>
            
            protected override void OnCreate(MindOrderCreateParams p_CreateParams)
            {
            }

            protected override bool OnActivate(MindOrderCreateParams p_CreateParams, MindOrderActivateParams p_ActivateParams, bool p_IsPooled)
            {
                _Entity = p_CreateParams.Entity;
                MindOrderParams = p_ActivateParams;
                DelayTimer = MindOrderParams.Delay;
                MindOrderPhase = MindOrderPhase.Init;
                
                return true;
            }

            protected override void OnRetrieve(MindOrderCreateParams p_CreateParams)
            {
                _Entity = default;
                MindOrderParams = default;
                DelayTimer = default;
                MindOrderPhase = default;
            }

            public void OnAdded(MindOrderReserveType p_Type)
            {
                switch (p_Type)
                {
                    default:
                    case MindOrderReserveType.Add:
                    case MindOrderReserveType.Overlap:
                    case MindOrderReserveType.Instant:
                    {
                        _RelayCancelFlag = false;
                        break;
                    }
                    case MindOrderReserveType.Add_RelayCancel:
                    {
                        _RelayCancelFlag = true;
                        break;
                    }
                }
            }
            
            public void OnSelected(MindOrderPhase p_LatestOrderPhase)
            {
                switch (p_LatestOrderPhase)
                {
                    default:
                    case MindOrderPhase.None:
                    case MindOrderPhase.Success:
                    {
                        break;
                    }
                    case MindOrderPhase.Fail:
                    {
                        if (_RelayCancelFlag)
                        {
                            Terminate(false);
                        }
                        break;
                    }
                }
            }
            
            /// <summary>
            /// 해당 오더가 파기된 경우에는 false를 리턴한다.
            /// </summary>
            public MindOrderPhase OnUpdate(float p_DeltaTime)
            {
                switch (MindOrderPhase)
                {
                    case MindOrderPhase.Init:
                    {
                        Init();
                        break;
                    }
                    case MindOrderPhase.PreDelay:
                    {
                        if (DelayTimer.IsOver())
                        {
                            MindOrderPhase = MindOrderPhase.Ready;
                        }
                        else
                        {
                            DelayTimer.Progress(p_DeltaTime);
                        }   
                        break;
                    }
                    case MindOrderPhase.Ready:
                    {
                        Run();
                        
                        break;
                    }
                }

                return MindOrderPhase;
            }

            protected override void OnDispose()
            {
            }
            
            #endregion
            
            #region <Methods>

            public bool IsOrderType(MindOrderType p_Type)
            {
                return MindOrderType == p_Type;
            }

            public void SetOrderParam(MindOrderActivateParams p_Param)
            {
                MindOrderParams = p_Param;
            }
            
            private void Init()
            {
                MindOrderPhase = DelayTimer.IsOver() ? MindOrderPhase.Ready : MindOrderPhase.PreDelay;
            }

            private void Run()
            {
                switch (MindOrderType)
                {
                    case MindOrderType.Idle:
                    {
                        _Entity.GeometryModule.StopNavigate();
                        _Entity.ActionModule.ReleaseAllInput();
                        _Entity.AnimationModule.SwitchToIdleMotion(MindOrderParams.IdleState, AnimationTool.MotionTransitionType.Bypass_StateMachine);
                      
                        MindOrderPhase = MindOrderPhase.Success;
                        break;
                    }
                    case MindOrderType.Navigate:
                    {
                        var moveOrderParams = MindOrderParams.MoveOrderParams;
                        
                        if (_Entity.GeometryModule.NavigateTo(moveOrderParams.DestinationPreset))
                        {
                            MindOrderPhase = MindOrderPhase.Progressing;
                        }
                        else
                        {
                            MindOrderPhase = MindOrderPhase.Fail;
                        }
                        break;
                    }
                    case MindOrderType.InputCommand:
                    {
                        var actionModule = _Entity.ActionModule;
                        actionModule.ReleaseAllInput();
                        var result = actionModule.InputCommand(CommandEventParams.GetActionCommandParams(MindOrderParams.ReservedInputCommand, InputEventTool.InputStateType.Press, __MIND_COMMAND_PRIORITY_DEFAULT));
                        
                        if (result)
                        {
                            MindOrderPhase = MindOrderPhase.Progressing;
                        }
                        else
                        {
                            MindOrderPhase = MindOrderPhase.Fail;
                        }
                        break;
                    }
                    case MindOrderType.Delay:
                    {
                        MindOrderPhase = MindOrderPhase.Success;
                        break;
                    }
                    case MindOrderType.FreeDelay:
                    {
                        _Entity.MindModule.ResetOrderInterval();
                        
                        MindOrderPhase = MindOrderPhase.Success;
                        break;
                    }
                    case MindOrderType.Disable:
                    {
                        _Entity.SetDisable(true);
                        
                        MindOrderPhase = MindOrderPhase.Success;
                        break;
                    }
                    default:
                    {
                        MindOrderPhase = MindOrderPhase.Success;
                        break;
                    }
                }
            }
            
            public void Terminate(bool p_TerminateSuccessFlag)
            {
                MindOrderPhase = p_TerminateSuccessFlag ? MindOrderPhase.Success : MindOrderPhase.Fail;
            }

            #endregion
        }

        #endregion
    }
}