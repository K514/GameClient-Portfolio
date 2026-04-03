using System;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    [Serializable]
    public struct AccountItemData
    {
        #region <Fields>

        public GameEntityItemTool.GameEntityItemPreset GameEntityItemPreset;

        #endregion

        #region <Constructor>

        public AccountItemData(int p_Index, int p_Value) : this(new GameEntityItemTool.GameEntityItemPreset(p_Index, p_Value))
        {
        }
        
        public AccountItemData(GameEntityItemTool.GameEntityItemPreset p_Preset)
        {
            GameEntityItemPreset = p_Preset;
        }

        #endregion
    }
}