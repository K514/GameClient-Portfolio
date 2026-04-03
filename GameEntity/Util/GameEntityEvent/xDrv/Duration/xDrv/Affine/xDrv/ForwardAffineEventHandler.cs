using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 일정한 시간동안 특정한 방향으로 가속하는 아핀 이벤트 핸들러
    /// </summary>
    public class ForwardAffineEventHandler : AffineEventHandlerBase<ForwardAffineEventHandler>
    {
        #region <Fields>

        private Vector3 _Acceleration;

        #endregion
        
        #region <Callbacks>

        public override bool OnActivate(AffineEventHandlerCreateParams p_CreateParams, AffineEventHandlerActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                _Acceleration = p_ActivateParams.Vector;
                
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnEventStart()
        {
            Debug.LogError("Forward Affine Start !");
        }

        protected override void OnEventProgress(float p_DeltaTime, float p_Rate)
        {
            Entity.PhysicsModule.AddAcceleration(PhysicsTool.ForceType.Default, _Acceleration);
        }

        protected override void OnEventTerminate()
        {
            Debug.LogError("Forward Affine Over !");
        }

        #endregion
        
        #region <Methods>

        public override void PreloadEvent()
        {
        }

        #endregion
    }
}