using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private AssetLoadResult<Sprite> _PortraitSpritePreset;

        #endregion

        #region <Callbacks>

        private void OnDisposePortrait()
        {
            if (_PortraitSpritePreset)
            {
                AssetLoaderManager.GetInstanceUnsafe?.UnloadAsset(ref _PortraitSpritePreset);
            }
        }
        
        #endregion
        
        #region <Methods>

        public Sprite GetPortrait()
        {
            if (_PortraitSpritePreset)
            {
                return _PortraitSpritePreset.Asset;
            }
            else
            {
                return _PortraitSpritePreset.Asset;
            }
        }

        #endregion
    }
}