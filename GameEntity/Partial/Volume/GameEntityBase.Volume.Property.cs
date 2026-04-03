using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 반경 프리셋
        /// </summary>
        public ScaleFloatInvSqr Radius { get; protected set; }
        
        /// <summary>
        /// 높이 프리셋
        /// </summary>
        public ScaleFloat Height { get; protected set; }
        
        /// <summary>
        /// 질량 프리셋
        /// </summary>
        public ScaleFloatInv Mass { get; protected set; }
        
        /// <summary>
        /// 컬라이더 중심 좌표 오프셋
        /// </summary>
        public ScaleVector ColliderCenterOffset { get; protected set; }
        
        #endregion

        #region <Callbacks> 

        private void OnCreateVolumePhysics()
        {
            var modelRecord = ModelDataRecord;
            Radius = new ScaleFloatInvSqr(modelRecord.PrefabColliderRadius, modelRecord.PrefabSkinOffset);
            Height = new ScaleFloat(modelRecord.PrefabColliderHeight, modelRecord.PrefabSkinOffset);
            Mass = new ScaleFloatInv(modelRecord.PrefabMass, 0f);
            ColliderCenterOffset = new ScaleVector(modelRecord.PrefabColliderCenterOffset);
        }

        private void OnActivateVolumePhysics()
        {
        }

        private void OnRetrieveVolumePhysics()
        {
        }

        private void OnUpdateVolumePhysics()
        {
            Radius.SetScale(Scale);
            Height.SetScale(Scale);
            Mass.SetScale(Scale);
            ColliderCenterOffset.SetScale(Scale);
        }

        #endregion

        #region <Methods>

        public Vector3 GetCenterOffset(float p_Factor)
        {
            return p_Factor * ColliderCenterOffset.CurrentValue;
        }
        
        public float GetRadiusSkinOffset()
        {
            return Radius.CurrentOffset;
        }
        
        public float GetHeightSkinOffset()
        {
            return Height.CurrentOffset;
        }
        
        public float GetRadius()
        {
            return Radius.CurrentValue;
        }

        public float GetRadius(float p_Factor)
        {
            return p_Factor * Radius.CurrentValue;
        }

        public float GetRadiusWithSkinOffset()
        {
            return Radius.CurrentValueWithOffset;
        }
       
        public float GetUnscaledRadius()
        {
            return Radius.DefaultValue;
        }

        
        public float GetUnscaledRadius(float p_Factor)
        {
            return p_Factor * Radius.DefaultValue;
        }

        public float GetHeight()
        {
            return Height.CurrentValue;
        }

        public float GetHeight(float p_Factor)
        {
            return p_Factor * Height.CurrentValue;
        }

        public float GetHeightWithSkinOffset()
        {
            return Height.CurrentValueWithOffset;
        }
        
        public float GetUnscaledHeight()
        {
            return Height.DefaultValue;
        }
        
        public float GetUnscaledHeight(float p_Factor)
        {
            return p_Factor * Height.DefaultValue;
        }

        public Vector3 GetHeightVector(float p_Factor)
        {
            return GetHeight(p_Factor) * Vector3.up;
        }

        public float GetLargerBetweenRadiusAndHalfHeight()
        {
            return Mathf.Max(GetRadius(), GetHeight(0.5f));
        }

        public Vector3 GetRandomAroundPosition(float p_Min, float p_Max)
        {
            var radius = GetRadius();
            return Affine.GetRandomPosition(XYZType.ZX, radius + p_Min, radius + p_Max);
        }

        public Vector3 GetBottomPosition()
        {
            return GetCenterPosition() - GetHeightVector(0.5f);
        }
        
        public Vector3 GetBottomRelativePosition(float p_Forward)
        {
            return GetBottomPosition() + GetRelativeVector(p_Forward);
        }
        
        public Vector3 GetBottomRelativePosition(float p_Forward, float p_Right)
        {
            return GetBottomPosition() + GetRelativeVector(p_Forward, p_Right);
        }
        
        public Vector3 GetBottomUpPosition(float p_YOffset)
        {
            return GetBottomPosition() + p_YOffset * Vector3.up;
        }
        
        public Vector3 GetCenterPosition()
        {
            return Affine.position + ColliderCenterOffset.CurrentValue;
        }
        
        public Vector3 GetCenterRelativePosition(float p_Forward)
        {
            return GetCenterPosition() + GetRelativeVector(p_Forward);
        }
        
        public Vector3 GetCenterRelativePosition(float p_Forward, float p_Right)
        {
            return GetCenterPosition() + GetRelativeVector(p_Forward, p_Right);
        }

        public Vector3 GetTopPosition()
        {
            return GetCenterPosition() + GetHeightVector(0.5f);
        }
        
        public Vector3 GetTopRelativePosition(float p_Forward)
        {
            return GetTopPosition() + GetRelativeVector(p_Forward);
        }
        
        public Vector3 GetTopRelativePosition(float p_Forward, float p_Right)
        {
            return GetTopPosition() + GetRelativeVector(p_Forward, p_Right);
        }
        
        public Vector3 GetTopUpPosition(float p_YOffset)
        {
            return GetTopPosition() + p_YOffset * Vector3.up;
        }

        #endregion
    }
}