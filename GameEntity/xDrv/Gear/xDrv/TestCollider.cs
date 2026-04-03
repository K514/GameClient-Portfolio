using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public class TestCollider : GearEntityBase
    {
        private async void Awake()
        {
            await SystemBoot.WaitGameManagerLoad();
            
            CheckAwake(new GearPoolManager.CreateParams());
        }

        protected override void OnUpdateEntity(float p_DeltaTime)
        {
            base.OnUpdateEntity(p_DeltaTime);
#if UNITY_EDITOR
            DrawCollider(Color.red, 0f);
#endif
            var player = PlayerManager.GetInstanceUnsafe.Player;
            if (player.IsEntityValid())
            {
#if UNITY_EDITOR
                player.DrawCollider(Color.blue, 0f);
#endif
                var playerRange = CustomCircle.GetXZCircle(player.CapsuleColliderGroup[0]);
                foreach (var boxCollider in BoxColliderGroup)
                {
                    var plane = CustomPlane.GetBasisLocation(boxCollider);
                    if (playerRange.IsIntersectWith(plane))
                    {
                        Debug.LogError("Touch Box");
                        return;
                    }
                }
                foreach (var capsuleCollider in CapsuleColliderGroup)
                {
                    var circle = CustomCircle.GetXZCircle(capsuleCollider);
                    if (playerRange.IsIntersectWith(circle))
                    {
                        Debug.LogError("Touch Circle");
                        return;
                    }
                }
            }
        }
    }
}