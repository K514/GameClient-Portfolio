#if !SERVER_DRIVE

using System;
using k514.Mono.Common;
using UnityEngine;

namespace k514
{
    public class ShaderNameTable : ResourceNameMapTable<ShaderNameTable, RenderableTool.ShaderControlType, ShaderNameTable.TableRecord, Shader>
    {
        #region <Record>

        [Serializable]
        public class TableRecord : ResourceNameTableRecord
        {
        }

        #endregion
    }
}

#endif