namespace k514.Mono.Common
{
    public partial class PhysicsBase
    {
        #region <Fields>

        /// <summary>
        /// 중력 타입
        /// </summary>
        protected PhysicsTool.GravityType _GravityFlag;

        /// <summary>
        /// 연직 방향 타입
        /// </summary>
        public CustomMath.Significant Current_Y_VelocityType { get; private set; }

        /// <summary>
        /// 이번 프레임에서 속도에 기여한 가속도 타입 플래그마스크
        /// </summary>
        protected PhysicsTool.ForceType _AppliedForceFlagMask;
        
        /// <summary>
        /// 반중력 타이머
        /// </summary>
        protected ProgressTimer _AntiGravityTimer;
        
        #endregion

        #region <Callbacks>

        private void OnAwakeState()
        {
            Update_Y_VelocityType();

            var entityType = Entity.GameEntityType;
            switch (entityType)
            {
                case GameEntityTool.GameEntityType.Unit:
                {
                    SetGravityFlag(PhysicsTool.GravityType.Applied);
                    break;
                }
                default:
                {
                    SetGravityFlag(PhysicsTool.GravityType.Anti_Perfect);
                    break;
                }
            }
        }

        private void OnSleepState()
        {
            _AppliedForceFlagMask = PhysicsTool.ForceType.None;
            SetGravityFlag(PhysicsTool.GravityType.Anti_Perfect);
        }

        #endregion
        
        #region <Methods>

        public void SetGravityFlag(PhysicsTool.GravityType p_GravityFlag)
        {
            if (_GravityFlag != p_GravityFlag)
            {
                _GravityFlag = p_GravityFlag;
                switch (_GravityFlag)
                {
                    case PhysicsTool.GravityType.Applied:
                        break;
                    case PhysicsTool.GravityType.Anti_HitBreak:
                    case PhysicsTool.GravityType.Anti_Duration:
                    case PhysicsTool.GravityType.Anti_Perfect:
                        ClearGravityForce();
                        break;
                }
            }
        }

        public void SetAntiGravity(float p_Duration)
        {
            switch (_GravityFlag)
            {
                case PhysicsTool.GravityType.Applied:
                    _GravityFlag = PhysicsTool.GravityType.Anti_Duration;
                    _AntiGravityTimer = p_Duration;
                    ClearGravityForce();
                    break;
            }
        }

        private void UpdateGravity(float p_DeltaTime)
        {
            switch (_GravityFlag)
            {
                case PhysicsTool.GravityType.Applied:
                {
                    if (_IsGrounded)
                    {
                        _GravityPhysicsSystem.OverlapVelocity(EnvironmentManager.GravityVelocityOnGround);
                    }
                    else
                    {
                        _GravityPhysicsSystem.OverlapAcceleration(EnvironmentManager.GravityAcceleration);
                    }
                    break;
                }
                case PhysicsTool.GravityType.Anti_Duration:
                {
                    if (_AntiGravityTimer.IsOver())
                    {
                        _GravityFlag = PhysicsTool.GravityType.Applied;
                    }
                    else
                    {
                        _AntiGravityTimer.Progress(p_DeltaTime);
                    }
                    break;
                }
                case PhysicsTool.GravityType.Anti_HitBreak:
                case PhysicsTool.GravityType.Anti_Perfect:
                {
                    break;
                }
            }
        }
        
        public void Update_Y_VelocityType()
        {
            var prevYVelocityType = Current_Y_VelocityType;
            var currentYVelocity = _CurrentVelocity.y;
            
            if (currentYVelocity > 0f)
            {
                Current_Y_VelocityType = CustomMath.Significant.Plus;
            }
            else if (currentYVelocity < 0f)
            {
                Current_Y_VelocityType = CustomMath.Significant.Minus;
            }
            else
            {
                Current_Y_VelocityType = CustomMath.Significant.Zero;
            }
            
            if (prevYVelocityType != Current_Y_VelocityType)
            {
                OnVelocity_Y_Changed(_AppliedForceFlagMask, prevYVelocityType, Current_Y_VelocityType);
            }
        }
        
        #endregion
    }
}