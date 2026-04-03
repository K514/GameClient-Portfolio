using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 위치 추적 프리셋
        /// </summary>
        public PositionTracer PositionTrace { get; private set; }

        #endregion
        
        #region <Callbacks>

        protected override void OnCreateAffine()
        {
            PositionTrace = new PositionTracer();
        }
        
        protected override void OnActivateAffine(ActivateParams p_ActivateParams)
        {
            _PivotPosition = new AffineWrapperPreset(Affine);
          
            var correctedAffine = p_ActivateParams.AffineCorrection.GetCorrection(this);
            SetAffineValue(correctedAffine);
        }
        
        protected override void OnRetrieveAffine()
        {
            base.OnRetrieveAffine();
            
            ResetPositionTrace();
        }
                
        protected override void OnScaleChanged(float p_DeltaRatio)
        {
            base.OnScaleChanged(p_DeltaRatio);

            OnUpdateVolumeScale(p_DeltaRatio);
            OnUpdateParticleSystemScale(p_DeltaRatio);
            
            OnModule_Update_Scale();
        }
        
        protected override void OnPositionChanged(Vector3 p_Prev, Vector3 p_Current)
        {
            base.OnPositionChanged(p_Prev, p_Current); 

            UpdatePositionTrace();

            switch (this)
            {
                case var _ when IsMovedOverThreshold():
                {
                    OnModule_PositionChanged(p_Prev, p_Current);
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.PositionChanged, new GameEntityBaseEventParams(this, true));
#if !SERVER_DRIVE
                    ReserveUpdateCameraInteract();
#endif
                    break;
                }
                case var _ when IsMoved():
                {
                    OnModule_PositionChanged(p_Prev, p_Current);
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.PositionChanged, new GameEntityBaseEventParams(this, false));
#if !SERVER_DRIVE
                    ReserveUpdateCameraInteract();
#endif
                    break;
                }
            }
        }
        
        protected override void OnPivotChanged()
        {
            OnModule_PivotChanged(PositionTrace);
        }

        private bool OnPositionMoved()
        {
            UpdatePositionTrace();

            switch (this)
            {
                case var _ when IsMovedOverThreshold():
                {
                    OnModule_PositionMoved();
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.PositionMoved, new GameEntityBaseEventParams(this, true));
#if !SERVER_DRIVE
                    ReserveUpdateCameraInteract();
#endif
                    return true;
                }
                case var _ when IsMoved():
                {
                    OnModule_PositionMoved();
                    GameEntityBaseEventSender.SendEvent(GameEntityTool.GameEntityBaseEventType.PositionMoved, new GameEntityBaseEventParams(this, false));
#if !SERVER_DRIVE
                    ReserveUpdateCameraInteract();
#endif
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }
        
        #endregion

        #region <Methods>
        
        public override void SetLookUV(Vector3 p_TargetVector)
        {
            if (!HasState_Or(GameEntityTool.EntityStateType.BlockMoveStateGroupMask))
            {
                base.SetLookUV(p_TargetVector);
            }
        }

        public bool IsMoved()
        {
            return PositionTrace._IsMoved;
        }
                
        public bool IsMovedOverThreshold()
        {
            return PositionTrace._IsMovedOverThreshold;
        }

        public bool IsMovedVertical()
        {
            return PositionTrace._IsMovedVertical;
        }

        public void UpdatePositionTrace()
        {
            PositionTrace.Update(Affine.position);
        }

        public void ResetPositionTrace()
        {
            PositionTrace.Reset(Affine.position);
        }

        #endregion
    }
}