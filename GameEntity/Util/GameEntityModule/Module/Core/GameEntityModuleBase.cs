using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GameEntityModuleBase : IGameEntityModule
    {
        #region <Fields>

        /// <summary>
        /// 모듈 타입
        /// </summary>
        protected GameEntityModuleTool.ModuleType _ModuleType;
        
        /// <summary>
        /// 모듈 레코드
        /// </summary>
        protected IGameEntityModuleDataTableRecordBridge _ModuleRecord;
        
        /// <summary>
        /// 현재 모듈이 활성화 상태인지 표시하는 플래그
        /// </summary>
        protected bool _IsValid;

        #endregion

        #region <Constructor>

        protected GameEntityModuleBase(GameEntityModuleTool.ModuleType p_ModuleType, IGameEntityModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            _ModuleType = p_ModuleType;
            _ModuleRecord = p_ModuleRecord;
            Entity = p_Entity;
            Affine = Entity?.Affine;
        }

        #endregion
        
        #region <Destructor>

        ~GameEntityModuleBase()
        {
            Dispose();
        }

        #endregion
        
        #region <Callbacks>
        
        public void AwakeModule()
        {
            if (!_IsValid)
            {
                _IsValid = true;
                _OnAwakeModule();
            }
        }

        public void SleepModule()
        {
            if (_IsValid)
            {
                _IsValid = false;
                _OnSleepModule();
            }
        }

        public void ResetModule()
        {
            SleepModule();
            
            _OnResetModule();
        }

        /// <summary>
        /// 모듈이 비활성화 상태에서 활성화되는 경우 호출되는 콜백
        /// </summary>
        protected abstract void _OnAwakeModule();
        
        /// <summary>
        /// 모듈이 활성화 상태에서 비활성화되는 경우 호출되는 콜백
        /// </summary>
        protected abstract void _OnSleepModule();
        
        /// <summary>
        /// 모듈이 리셋될 때 호출되는 콜백
        /// </summary>
        protected abstract void _OnResetModule();
        
        /// <summary>
        /// 모듈이 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected virtual void OnDisposeUnmanaged()
        {
        }

        #endregion

        #region <Methods>

        public GameEntityModuleTool.ModuleType GetModuleType()
        {
            return _ModuleType;
        }

        public int GetRecordKey()
        {
            return _ModuleRecord.KEY;
        }

        public IGameEntityModuleDataTableRecordBridge GetRecord()
        {
            return _ModuleRecord;
        }

        #endregion
        
        #region <Disposable>
        
        /// <summary>
        /// dispose 패턴 onceFlag
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// dispose 플래그를 초기화 시키는 메서드
        /// </summary>
        public void Rejuvenate()
        {
            IsDisposed = false;
        }
        
        /// <summary>
        /// 인스턴스 파기 메서드
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            else
            {
                IsDisposed = true;
                OnDisposeUnmanaged();
            }
        }

        #endregion
    }
}