using UnityEngine;

namespace k514.Mono.Common
{
    public struct GameEntityAffineControlEventContainerActivateParams : IGameEntityEventContainerActivateParams
    {
        #region <Fields>

        public IGameEntityBridge Caster { get; }
        public int EventId { get; }
        public GameEntityAffineControlTool.GameEntityAffineControlParams Params { get; }
        public GameEntityEventCommonParams CommonParams { get; }

        #endregion
        
        #region <Constructors>

        public GameEntityAffineControlEventContainerActivateParams(IGameEntityBridge p_Caster, int p_Id, GameEntityAffineControlTool.GameEntityAffineControlParams p_Params, GameEntityEventCommonParams p_CommonParams = default)
        {
            Caster = p_Caster;
            EventId = p_Id;
            Params = p_Params;
            CommonParams = p_CommonParams;
        }

        #endregion
    }
    
    public class GameEntityAffineControlEventContainer : GameEntityEventContainer<GameEntityAffineControlEventContainer, ObjectCreateParams, GameEntityAffineControlEventContainerActivateParams, ITableRecord>
    {
        #region <Fields>

        public Transform Affine { get; private set; }
        public float Threshold { get; private set; }
        public bool Flag { get; private set; }
        private GameEntityAffineControlEventBase _AffineControl;
        
        #endregion

        #region <Callbacks>

        protected override bool OnActivate(ObjectCreateParams p_CreateParams, GameEntityAffineControlEventContainerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                Affine = Caster.Affine;
                Threshold = 0f;
                Flag = false;
            
                GameEntityAffineControlStorage.GetInstanceUnsafe.TryGetAffineControlEvent(EventId, out _AffineControl);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnRetrieve(ObjectCreateParams p_CreateParams)
        {
            base.OnRetrieve(p_CreateParams);

            _AffineControl = default;
            Affine = default;
        }

        protected override void OnContainerTerminate()
        {
        }

        protected override bool CheckContainerTerminate()
        {
            return false;
        }
        
        public override bool OnUpdate(float p_DeltaTime)
        {
            return base.OnUpdate(p_DeltaTime) || _AffineControl.UpdateAffineControl(this, p_DeltaTime);
        }

        #endregion

        #region <Methods>

        protected override void InitCancellationToken()
        {
            GameEntityAffineControlStorage.GetInstanceUnsafe.GetLinkedCancellationTokenSource(ref _CancellationTokenSource);
            _CancellationToken = _CancellationTokenSource.Token;
        }
        
        public bool InitializeAffineControl()
        {
            if (ReferenceEquals(null, _AffineControl))
            {
                return false;
            }
            else
            {
                return _AffineControl.InitializeAffineControl(this);
            }
        }
        
        public void UpdateThreshold(float p_Delta)
        {
            Threshold += p_Delta;
        }
        
        public void ResetThreshold()
        {
            Threshold = 0f;
        }
        
        public bool ToggleFlag()
        {
            Flag = !Flag;

            return Flag;
        }

        #endregion
    }

}