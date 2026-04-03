#if !SERVER_DRIVE

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    public class ImageNameTable : ResourceNameMapTable<ImageNameTable, int, ImageNameTable.TableRecord, Sprite>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
            #region <Fields>

            public bool IsSprite { get; private set; }

            #endregion

            #region <Methods>

            public AssetLoadResult<Texture> GetTexture(ResourceLifeCycleType p_ResourceLifeCycleType)
            {
                var resourceName = ResourceName;
                if (IsSprite)
                {
                    var spriteResult = AssetLoaderManager.GetInstanceUnsafe.LoadAsset<Sprite>((p_ResourceLifeCycleType, resourceName));
                    return new AssetLoadResult<Texture>(spriteResult.AssetLoadKey, spriteResult.Asset.texture);
                }
                else
                {
                    return AssetLoaderManager.GetInstanceUnsafe.LoadAsset<Texture>((p_ResourceLifeCycleType, resourceName));
                }
            }
        
            public async UniTask<AssetLoadResult<Texture>> GetTextureAsync(ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken)
            {
                var resourceName = ResourceName;
                if (IsSprite)
                {
                    var spriteResult = await AssetLoaderManager.GetInstanceUnsafe.LoadAssetAsync<Sprite>((p_ResourceLifeCycleType, resourceName), p_CancellationToken);
                    return new AssetLoadResult<Texture>(spriteResult.AssetLoadKey, spriteResult.Asset.texture);
                }
                else
                {
                    return await AssetLoaderManager.GetInstanceUnsafe.LoadAssetAsync<Texture>((p_ResourceLifeCycleType, resourceName), p_CancellationToken);
                }
            }

            #endregion
        }

        #endregion

        #region <Methods>
        
        public AssetLoadResult<Texture> GetTexture(int p_Key, ResourceLifeCycleType p_ResourceLifeCycleType)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return o_Record.GetTexture(p_ResourceLifeCycleType);
            }
            else
            {
                return default;
            }
        }
        
        public async UniTask<AssetLoadResult<Texture>> GetTextureAsync(int p_Key, ResourceLifeCycleType p_ResourceLifeCycleType, CancellationToken p_CancellationToken)
        {
            if (TryGetRecord(p_Key, out var o_Record))
            {
                return await o_Record.GetTextureAsync(p_ResourceLifeCycleType, p_CancellationToken);
            }
            else
            {
                return default;
            }
        }

        #endregion
    }
}

#endif