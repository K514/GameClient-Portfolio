using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameEntityModuleClusterBase<This, ModuleLabelType, TableBridge, RecordBridge, ModuleBase>
    {
        /// <summary>
        /// 지정한 인덱스를 가지는 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public bool TryAddModule(int p_Index, out ModuleBase o_Module)
        {
            return !ReferenceEquals(_NullModule, o_Module = AddModule(p_Index));
        }

        /// <summary>
        /// 지정한 라벨 타입을 가지는 첫번째 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public bool TryAddModule(ModuleLabelType p_ModuleType, out ModuleBase o_Module)
        {
            return !ReferenceEquals(_NullModule, o_Module = AddModule(p_ModuleType));
        }
        
        /// <summary>
        /// 지정한 라벨/서브 라벨 타입을 가지는 첫번째 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public bool TryAddModule<SubModuleLabelType>(ModuleLabelType p_ModuleType, SubModuleLabelType p_SubModuleType, out ModuleBase o_Module) where SubModuleLabelType : struct, Enum
        {
            return !ReferenceEquals(_NullModule, o_Module = AddModule(p_ModuleType, p_SubModuleType));
        }
        
        /// <summary>
        /// 지정한 인덱스를 가지는 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public ModuleBase AddModule(int p_Index)
        {
            if (_ModuleIndexTable.TryGetValue(p_Index, out var o_Module))
            {
                return o_Module;
            }
            else
            {
                var (valid, moduleLabelType, spawnedModule) = SpawnModule(p_Index);
                if (valid)
                {
                    if (_ModuleLabelTable.TryGetValue(moduleLabelType, out var o_List))
                    {
                        o_List.Add(spawnedModule);
                    }
                    else
                    {
                        o_List = new List<ModuleBase>{ spawnedModule };
                        _ModuleLabelTable.Add(moduleLabelType, o_List);
                    }
                    _ModuleIndexTable.Add(p_Index, spawnedModule);

                    return spawnedModule;
                }
                else
                {
                    return _NullModule;
                }
            }
        }

        /// <summary>
        /// 지정한 라벨 타입을 가지는 첫번째 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public ModuleBase AddModule(ModuleLabelType p_ModuleType)
        {
            if (_ModuleLabelTable.TryGetValue(p_ModuleType, out var o_List) && o_List.CheckCollectionSafe())
            {
                return o_List[0];
            }
            else
            {
                if (_ModuleTableQuery.TryGetLabelTable(p_ModuleType, out var o_Table) && o_Table.TryGetFirstKey(out var o_Key))
                {
                    return AddModule(o_Key);    
                }
                else
                {
                    return _NullModule;
                }
            }
        }
        
        /// <summary>
        /// 지정한 라벨/서브 라벨 타입을 가지는 첫번째 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public ModuleBase AddModule<SubModuleLabelType>(ModuleLabelType p_ModuleType, SubModuleLabelType p_SubModuleType) where SubModuleLabelType : struct, Enum
        {
            if (_ModuleLabelTable.TryGetValue(p_ModuleType, out var o_List) && o_List.CheckCollectionSafe())
            {
                foreach (var module in o_List)
                {
                    var moduleKey = module.GetRecordKey();
                    if (_ModuleTableQuery.IsSubLabel(moduleKey, p_SubModuleType))
                    {
                        return module;
                    }
                }
            }
            
            if (_ModuleTableQuery.TryGetSubLabelTableKey(p_ModuleType, p_SubModuleType, out var o_Key))
            {
                return AddModule(o_Key);
            }
            else
            {
                return _NullModule;
            }
        }
        
        /// <summary>
        /// 지정한 인덱스를 가지는 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public async UniTask<ModuleBase> AddModule(int p_Index, CancellationToken p_Token)
        {
            if (_ModuleIndexTable.TryGetValue(p_Index, out var o_Module))
            {
                return o_Module;
            }
            else
            {
                var (valid, moduleLabelType, spawnedModule) = await SpawnModule(p_Index, p_Token);
                if (valid)
                {
                    if (_ModuleLabelTable.TryGetValue(moduleLabelType, out var o_List))
                    {
                        o_List.Add(spawnedModule);
                    }
                    else
                    {
                        o_List = new List<ModuleBase>{ spawnedModule };
                        _ModuleLabelTable.Add(moduleLabelType, o_List);
                    }
                    _ModuleIndexTable.Add(p_Index, spawnedModule);
             
                    return spawnedModule;
                }
                else
                {
                    return _NullModule;
                }
            }
        }
        
        /// <summary>
        /// 지정한 라벨 타입을 가지는 첫번째 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public async UniTask<ModuleBase> AddModule(ModuleLabelType p_ModuleType, CancellationToken p_Token)
        {
            if (_ModuleLabelTable.TryGetValue(p_ModuleType, out var o_List) && o_List.CheckCollectionSafe())
            {
                return o_List[0];
            }
            else
            {
                if (_ModuleTableQuery.TryGetLabelTable(p_ModuleType, out var o_Table) && o_Table.TryGetFirstKey(out var o_Key))
                {
                    return await AddModule(o_Key, p_Token);
                }
                else
                {
                    return _NullModule;
                }
            }
        }
        
        /// <summary>
        /// 지정한 라벨/서브 라벨 타입을 가지는 첫번째 모듈 레코드로부터 모듈을 생성하여 등록하는 메서드
        /// </summary>
        public async UniTask<ModuleBase> AddModule<SubModuleLabelType>(ModuleLabelType p_ModuleType, SubModuleLabelType p_SubModuleType, CancellationToken p_Token) where SubModuleLabelType : struct, Enum
        {
            if (_ModuleLabelTable.TryGetValue(p_ModuleType, out var o_List) && o_List.CheckCollectionSafe())
            {
                foreach (var module in o_List)
                {
                    var moduleKey = module.GetRecordKey();
                    if (_ModuleTableQuery.IsSubLabel(moduleKey, p_SubModuleType))
                    {
                        return module;
                    }
                }
            }
        
            if (_ModuleTableQuery.TryGetSubLabelTableKey(p_ModuleType, p_SubModuleType, out var o_Key))
            {
                return await AddModule(o_Key, p_Token);
            }
            else
            {
                return _NullModule;
            }
        }
    }
}