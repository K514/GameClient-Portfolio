using System;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 싱글톤 추상 클래스.
    /// </summary>
    public abstract class Singleton<Me> : SingletonBase<Me> where Me : Singleton<Me>, new()
    {
        #region <Consts>

        /// <summary>
        /// 싱글톤 생성 및 초기화 메서드
        /// </summary>
        private static Me GetInstanceSafe()
        {
            switch (_CurrentSingletonPhase)
            {
                case SingletonTool.SingletonInitializePhase.None:
                {
                    if (SystemBoot.IsSingletonAvailable)
                    {
                        try
                        {
                            CreateSingletonInstance();
                        }
#if APPLY_PRINT_LOG
                        catch(Exception e)
                        {
                            CustomDebug.LogError(($"* Fail to Initiate Singleton [{typeof(Me).Name}]", e, Color.red));
#else
                        catch
                        {
#endif
                            DisposeSingletonInstance();
                        }
                    }
                    break;
                }
            }

            return _instance;
        }

        /// <summary>
        /// 싱글톤 생성 메서드
        /// </summary>
        private static void CreateSingletonInstance()
        {
            _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.CreateSingletonInstance;
            _instance = new Me();
            _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.PreloadDependencies;
            if (_instance.OnLoadDependency())
            {
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.ProcessCreatedCallback;
                _instance.OnCreated();
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.ProcessInitializeCallback;
                _instance.OnInitiate();
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.InitializeOver;
            }
            else
            {
                throw new Exception();
            }
        }

        protected static object GetObject()
        {
            return GetInstanceSafe();
        }
        
        #endregion

        #region <Callbacks>

        /// <summary>
        /// 해당 싱글톤이 종속된 싱글톤을 로드하는 콜백
        /// </summary>
        private bool OnLoadDependency()
        {
            TryInitializeDependency();

            var (result, _) = SingletonTool.CreateSingleton(_Dependencies, MultiTaskMode.Sequence);
            if (result)
            {
                SystemBoot.OnSingletonSpawned(_instance);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 싱글톤 초기화 콜백. 해당 싱글톤 생명주기 중에 단 한번만 호출되야함.
        /// </summary>
        protected abstract void OnCreated();

        /// <summary>
        /// 싱글톤 초기화 콜백. OnCreated 이후에 호출된다.
        /// </summary>
        protected abstract void OnInitiate();

        #endregion

        #region <Methods>

        public void Reset()
        {
            OnInitiate();
        }

        #endregion
    }
}