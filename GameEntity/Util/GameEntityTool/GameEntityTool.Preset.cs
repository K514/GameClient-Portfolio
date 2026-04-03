using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityTool
    {
        public class TriggerColliderWrapper : MonoBehaviour
        {
            #region <Consts>

            private const float __TriggerColliderSizeOffset = 0.08f;

            #endregion

            #region <Fields>

            private IGameEntityBridge _Entity;
            private CapsuleCollider _Collider;
            
            #endregion

            #region <Callbacks>

            public void OnCreateTriggerCollider(IGameEntityBridge p_Entity)
            {
                _Entity = p_Entity;
                transform.SetParent(_Entity.Affine, false);
                _Collider = gameObject.AddComponent<CapsuleCollider>();
                _Collider.radius = p_Entity.Radius.DefaultValue + __TriggerColliderSizeOffset;
                _Collider.height = p_Entity.Height.DefaultValue + __TriggerColliderSizeOffset;
                _Collider.center = p_Entity.ColliderCenterOffset.DefaultValue;
                _Collider.isTrigger = true;
            }

            private void OnTriggerEnter(Collider other)
            {
                if (_Entity.IsLaunched)
                {
                    _Entity.OnTriggerEnterFrom(other);
                }
            }

            #endregion

            #region <Methods>

            public CapsuleCollider GetCollider()
            {
                return _Collider;
            }

            #endregion
        }
    }
}