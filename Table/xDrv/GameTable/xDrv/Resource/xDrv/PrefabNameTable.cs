using System;
using UnityEngine;

namespace k514
{
    public class PrefabNameTable : ResourceNameMapTable<PrefabNameTable, int, PrefabNameTable.TableRecord, GameObject>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
        }

        #endregion
    }
}