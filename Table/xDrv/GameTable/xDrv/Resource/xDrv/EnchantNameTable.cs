using System;
using UnityEngine;

namespace k514
{
    public class EnchantNameTable : ResourceNameMapTable<EnchantNameTable, string, EnchantNameTable.TableRecord, GameObject>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
            public int ImageIndex { get; private set; }
        }


        public AssetLoadResult<Sprite> GetImageSprite(string p_Index, ResourceLifeCycleType p_ResourceLifeCycleType = ResourceLifeCycleType.SceneUnload)
        {
            return ImageNameTable.GetInstanceUnsafe.GetResource(GetRecord(p_Index).ImageIndex, p_ResourceLifeCycleType);
        }
        #endregion
    }
}