using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>
        
        /// <summary>
        /// 컬라이더 그룹
        /// </summary>
        public List<Collider> ColliderGroup { get; private set; }
        
        /// <summary>
        /// 박스컬라이더 그룹
        /// </summary>
        public List<BoxCollider> BoxColliderGroup { get; private set; }
        
        /// <summary>
        /// 캡슐컬라이더 그룹
        /// </summary>
        public List<CapsuleCollider> CapsuleColliderGroup { get; private set; }

        /// <summary>
        /// 트리거 컬라이더 아핀
        /// </summary>
        private GameEntityTool.TriggerColliderWrapper _TriggerCollider;

        #endregion

        #region <Callbacks>

        private void OnCreateVolumeCollider()
        {
            Affine.GetComponentsInChildren(ColliderGroup = new List<Collider>());
            BoxColliderGroup = new List<BoxCollider>();
            CapsuleColliderGroup = new List<CapsuleCollider>();
            
            foreach (var collider in ColliderGroup)
            {
                collider.isTrigger = false;
                
                switch (collider)
                {
                    case var _ when collider is BoxCollider c_Collider :
                        BoxColliderGroup.Add(c_Collider);
                        break;
                    case var _ when collider is CapsuleCollider c_Collider:
                        CapsuleColliderGroup.Add(c_Collider);
                        break;
                }
            }

            _TriggerCollider = new GameObject("TriggerCollider").AddComponent<GameEntityTool.TriggerColliderWrapper>();
            _TriggerCollider.OnCreateTriggerCollider(this);

            var triggerCollider = _TriggerCollider.GetCollider();
            ColliderGroup.Add(triggerCollider);
            CapsuleColliderGroup.Add(triggerCollider);
        }

        private void OnActivateVolumeCollider()
        {
            SetPhysicsCollideEnable(true);
        }

        private void OnRetrieveVolumeCollider()
        {
            SetPhysicsCollideEnable(false);
        }

        private void OnUpdateVolumeCollider()
        {
        }

        #endregion
        
        #region <Methods>

        public void SetPhysicsCollideEnable(bool p_Flag)
        {
            foreach (var collider in ColliderGroup)
            {
                collider.enabled = p_Flag;
            }
        }

        public bool IsIntersectWith(CustomCircle p_Circle)
        {
            foreach (var boxCollider in BoxColliderGroup)
            {
                var customPlane = CustomPlane.GetBasisLocation(boxCollider);
                if (customPlane.IsIntersectWith(p_Circle))
                {
                    return true;
                }
            }

            foreach (var capsuleCollider in CapsuleColliderGroup)
            {
                var customCircle = CustomCircle.GetXZCircle(capsuleCollider);
                if (customCircle.IsIntersectWith(p_Circle))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool IsIntersectWith(CustomPlane p_Plane)
        {
            foreach (var boxCollider in BoxColliderGroup)
            {
                var customPlane = CustomPlane.GetBasisLocation(boxCollider);
                if (customPlane.IsIntersectWith(p_Plane, false))
                {
                    return true;
                }
            }

            foreach (var capsuleCollider in CapsuleColliderGroup)
            {
                var customCircle = CustomCircle.GetXZCircle(capsuleCollider);
                if (customCircle.IsIntersectWith(p_Plane))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool IsRayCastHitting(RaycastHit[] p_RaycastHits, int p_Count)
        {
            foreach (var collider in ColliderGroup)
            {
                for (var i = 0; i < p_Count; i++)
                {
                    var hit = p_RaycastHits[i];
                    var hitCollider = hit.collider;
                    if (!ReferenceEquals(collider, hitCollider))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

#if UNITY_EDITOR
        public void DrawCollider(Color p_Color, float p_Duration)
        {
            foreach (var boxCollider in BoxColliderGroup)
            {
                var customPlane = CustomPlane.GetBasisLocation(boxCollider);
                customPlane.DrawPlane(p_Color, p_Duration);
            }
            
            foreach (var capsuleCollider in CapsuleColliderGroup)
            {
                var customCircle = CustomCircle.GetXZCircle(capsuleCollider);
                customCircle.DrawCircle(p_Color, p_Duration);
            }
        }
#endif

        #endregion
    }
}