using UnityEngine;

namespace k514.Mono.Common
{
    public interface IMoveEventHandler : IActionEventHandler
    {
    }
    
    public abstract class MoveEventHandlerBase<This> : ActionEventHandlerBase<This>, IMoveEventHandler
        where This : MoveEventHandlerBase<This>, new()
    {
        #region <Fields>

        protected new MoveActionDataTable.TableRecord Record { get; private set; }
        private CommandEventParams _LastHandledCommandEventParams;

        #endregion
        
        #region <Callbacks>

        public override void OnCreate(IObjectContent<ActionEventHandlerCreateParams> p_Wrapper, ActionEventHandlerCreateParams p_CreateParams)
        {
            base.OnCreate(p_Wrapper, p_CreateParams);

            Record = base.Record as MoveActionDataTable.TableRecord;
            ActionEventType = ActionEventTool.ActionEventType.Move;
            InterruptableMask = ActionEventTool.ActionEventType.All;
        }

        protected override void OnActionLevelChanged(int p_Prev, int p_Cur)
        {
        }
        
        public override void OnInterruptSuccess()
        {
            base.OnInterruptSuccess();            
            
            Entity.AddState(GameEntityTool.EntityStateType.DRIVE_MOVE);
            Entity.AnimationModule.SwitchToMoveMotion(AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }

        public override void OnInterrupted()
        {
            base.OnInterrupted();
            
            Entity.RemoveState(GameEntityTool.EntityStateType.DRIVE_MOVE);
            Entity.AnimationModule.SwitchToIdleMotion(AnimationTool.MotionTransitionType.Bypass_StateMachine);
        }
        
        protected override void OnPress(CommandEventParams p_CommandPreset)
        {
            Entity.AnimationModule.SetMoveRunOrWalk(_LastHandledCommandEventParams.CheckCommandScud(p_CommandPreset, ActionTool.__SCUD_COMMAND_INPUT_THRESHOLD));
            _LastHandledCommandEventParams = p_CommandPreset;
        }
        
        protected override void OnHolding(CommandEventParams p_CommandPreset)
        {
            if (IsSelected())
            {
                if (IsAvailableMove())
                {
                    if (_LastHandledCommandEventParams.ArrowGesture.ArrowType != p_CommandPreset.ArrowGesture.ArrowType)
                    {
                        _LastHandledCommandEventParams = p_CommandPreset;
                    }

                    OnMove(p_CommandPreset.UV);
                }
            }
            else
            {
                ActionModule.TryInterruptMainHandler(this);
            }
        }
        
        protected override void OnRelease(CommandEventParams p_CommandPreset)
        {
            Entity.AnimationModule.SetMoveState(AnimationTool.MoveMotionType.Idle);
            _LastHandledCommandEventParams = p_CommandPreset;
            ActionModule.TryReleaseMainHandler(this);
        }
        
        protected abstract void OnMove(Vector3 p_UV);
        
        #endregion

        #region <Methods>

        /// <summary>
        /// 이동 입력은 누르고 있는한 취소되지 않음
        /// </summary>
        protected override bool IsManualReleasable()
        {
            return false;
        }

        private bool IsAvailableMove()
        {
            var animationModule = Entity.AnimationModule;
            return !animationModule.IsCurrentMotion(AnimationTool.MotionType.JumpDown)
                   && !animationModule.IsCurrentMotion(AnimationTool.MotionType.Hit)
                   && Entity.HasState_Only(GameEntityTool.EntityStateType.MovePassMask);
        }

        #endregion
    }
}