using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public static class GameEntityModuleTool
    {
        #region <Enums>

        public enum ModuleType
        {
            None,
            Action,
            Mind,
            Animation,
            Physics,
            Geometry,
            Render,
            Role
        }

        public static ModuleType[] _ModuleTypeEnumerator;

        #endregion

        #region <Constructor>

        static GameEntityModuleTool()
        {
            _ModuleTypeEnumerator = EnumFlag.GetEnumEnumerator<ModuleType>(EnumFlag.GetEnumeratorType.ExceptNone);
        }

        #endregion
    }
}