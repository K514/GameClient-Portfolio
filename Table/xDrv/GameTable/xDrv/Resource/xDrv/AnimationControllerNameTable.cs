using System;
using UnityEngine;

namespace k514
{
    public class AnimationControllerNameTable : ResourceNameMapTable<AnimationControllerNameTable, int, AnimationControllerNameTable.TableRecord, RuntimeAnimatorController>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
        }

        #endregion
    }
}