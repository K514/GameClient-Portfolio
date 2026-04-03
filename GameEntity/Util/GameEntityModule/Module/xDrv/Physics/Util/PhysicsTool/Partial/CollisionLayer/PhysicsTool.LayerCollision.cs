using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public static partial class PhysicsTool
    {
        public static void SetIgnoreLayerCollision(this GameConst.GameLayerType p_Left, GameConst.GameLayerType p_Right, bool p_Flag)
        {
            Physics.IgnoreLayerCollision((int) p_Left, (int) p_Right, p_Flag);
        }
        
        public static async UniTask LoadLayerBaseCollisionDetection(CancellationToken p_CancellationToken)
        {
            var tryTable = (await PhysicsCollisionLayerTable.GetInstanceSafe(p_CancellationToken)).GetTable();
            var layerTypeEnumerator = EnumFlag.GetEnumEnumerator<GameConst.GameLayerType>(EnumFlag.GetEnumeratorType.GetAll);

            foreach (var tryLayerType in layerTypeEnumerator)
            {
                if (tryTable.ContainsKey(tryLayerType))
                {
                    foreach (var targetLayerType in layerTypeEnumerator)
                    {
                        if (targetLayerType <= tryLayerType)
                        {
                            tryLayerType.SetIgnoreLayerCollision(targetLayerType, true);
                        }
                    }
                }
                else
                {
                    foreach (var targetLayerType in layerTypeEnumerator)
                    {
                        if (targetLayerType <= tryLayerType)
                        {
                            tryLayerType.SetIgnoreLayerCollision(targetLayerType, false);
                        }
                    }
                }
            }
            
            foreach (var tryLayerType in layerTypeEnumerator)
            {
                if (tryTable.TryGetValue(tryLayerType, out var o_Record))
                {
                    var layerList = o_Record.CollisionGameLayerSet;
                    if (layerList.CheckCollectionSafe())
                    {
                        foreach (var targetLayerType in layerList)
                        {
                            tryLayerType.SetIgnoreLayerCollision(targetLayerType, false);
                        }
                    }
                }
            }
        }
    }
}