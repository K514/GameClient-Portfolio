using System.Collections.Generic;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 유닛이 게임에서 수행할 역할을 기술하는 모듈
    /// </summary>
    public interface IRoleModule : IGameEntityModule
    {
        RoleModuleDataTableQuery.TableLabel GetRoleModuleType();
    }
}