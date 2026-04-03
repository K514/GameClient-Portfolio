using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameEntityModuleClusterBase<This, ModuleLabelType, TableBridge, RecordBridge, ModuleBase>
    {
        /// <summary>
        /// 기본 모듈로 전환시키는 메서드
        /// </summary>
        public void SwitchDefaultModule()
        {
            OnModuleSwitched(_DefaultModule);
        }
        
        /// <summary>
        /// Null 모듈로 전환시키는 메서드
        /// </summary>
        public void SwitchNullModule()
        {
            OnModuleSwitched(_NullModule);
        }

        /// <summary>
        /// 지정한 인덱스의 모듈로 전환시키는 메서드
        ///
        /// 지정한 인덱스가 존재하지 않는 경우, Null 모듈로 전환시킨다.
        /// </summary>
        public void SwitchModule(int p_Index)
        {
            if (TryAddModule(p_Index, out var o_Module))
            {
                OnModuleSwitched(o_Module);
            }   
        }

        /// <summary>
        /// 지정한 라벨을 가진 모듈로 전환시키는 메서드
        ///
        /// 해당하는 모듈이 없는 경우, 해당 라벨의 가장 첫번째 레코드로부터 모듈을 생성하여 전환시킨다.
        /// </summary>
        public void SwitchModule(ModuleLabelType p_ModuleType)
        {
            if (TryAddModule(p_ModuleType, out var o_Module))
            {
                OnModuleSwitched(o_Module);
            }
        }
        
        /// <summary>
        /// 지정한 라벨/서브라벨을 가진 모듈로 전환시키는 메서드
        ///
        /// 해당하는 모듈이 없는 경우, 해당 라벨/서브라벨의 가장 첫번째 레코드로부터 모듈을 생성하여 전환시킨다.
        /// </summary>
        public void SwitchModule<SubModuleLabelType>(ModuleLabelType p_ModuleType, SubModuleLabelType p_SubModuleType) where SubModuleLabelType : struct, Enum
        {
            if (TryAddModule(p_ModuleType, p_SubModuleType, out var o_Module))
            {
                OnModuleSwitched(o_Module);
            }
        }
    }
}