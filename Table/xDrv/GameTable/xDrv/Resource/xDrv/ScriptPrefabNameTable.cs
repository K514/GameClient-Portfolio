using System;
using UnityEngine;

namespace k514
{
    public class ScriptPrefabNameTable : ResourceNameMapTable<ScriptPrefabNameTable, Type, ScriptPrefabNameTable.TableRecord, GameObject>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
        }

        #endregion
    }
}