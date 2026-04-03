using xk514;

namespace k514.Mono.Common
{
    /// <summary>
    /// 해당 개체의 사망 이벤트를 기술하는 부분 클래스
    /// </summary>
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Callbacks>

        private void OnCreateDead()
        {
        }

        private void OnActivateDead()
        {
        }
        
        private void OnRetrieveDead()
        {
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 해당 유닛을 파괴한다.
        /// </summary>
        public void SetDead(bool p_Instant)
        {
            if (IsAlive)
            {
#if UNITY_EDITOR
                if (CustomDebug.AIStateName)
                {
                    SetPostFix($"(Dead)");
                }
#endif
                // 사망 상태로 전환한다.
                if (IsDisable)
                {
                    TurnState(GameEntityTool.EntityStateType.DEAD | GameEntityTool.EntityStateType.DISABLE);
                }
                else
                {
                    TurnState(GameEntityTool.EntityStateType.DEAD);
                }

                // 컬라이더를 트리거로 변경한다.
                SetPhysicsCollideEnable(false);
                StopParticleSystem();
                TerminateAllEventHandler();
                
#if !SERVER_DRIVE
                if (IsFocused)
                {
                    CameraManager.GetInstanceUnsafe.OnFocusDead(this);
                }
#endif
                // 사망 연출을 스킵하는 경우
                if (p_Instant || IsDisable)
                {
                    SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.DeadSpan);
                    SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.LifeSpanTerminate);
                    
                    OnModule_Dead(true);
                }
                // 사망 연출을 스킵하지 않는 경우
                else
                {
                    // LifeSpan을 진행하는 경우, DeadSpan 페이즈로 넘겨주기만 하면 된다.
                    if (IsLifeSpanValid())
                    {
                        SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.DeadSpan);
                      
                        OnModule_Dead(false);
                    }
                    // LifeSpan을 진행하지 않는 경우, DeadSpan 페이즈로 넘겨도 그 이후로 진행되지 않으므로, LifeSpanTerminate 페이즈로 바로 넘겨준다.
                    else
                    {
                        SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.DeadSpan);
                        SetLifeSpanPhase(GameEntityTool.EntityLifeSpanPhase.LifeSpanTerminate);
                     
                        OnModule_Dead(false);
                    }
                }
            }
        }

        #endregion
    }
}