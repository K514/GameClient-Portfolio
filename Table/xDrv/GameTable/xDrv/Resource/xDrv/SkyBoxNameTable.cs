using System;
using UnityEngine;

namespace k514
{
    public class SkyBoxNameTable : ResourceNameMapTable<SkyBoxNameTable, int, SkyBoxNameTable.TableRecord, Material>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
        }

        #endregion
    }
}