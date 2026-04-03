using System;
using UnityEngine;

namespace k514
{
    public class AnimationClipNameTable : ResourceNameMapTable<AnimationClipNameTable, int, AnimationClipNameTable.TableRecord, AnimationClip>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
        }

        #endregion
    }
}