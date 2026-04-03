using System;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 싱글톤과 ScripatableObject를 상속하여, 에셋으로서 동작하는 싱글톤 추상 클래스.
    /// </summary>
    public abstract class ScriptableObjectSingleton<Me> : ScriptableObjectSingletonBase<Me> where Me : ScriptableObjectSingleton<Me>
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
                            throw;
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
        private static Me SpawnSingletonInstance()
        {
            var result = FindAnyObjectByType<Me>();
            if (result == null)
            {
                result = Resources.Load<Me>($"{SystemMaintenance.GetResourcePath(ResourceLoadType.FromUnityResource, ResourceType.ScriptableObject, AssetPathType.RelativePath)}/{typeof(Me)}");
            }
            return result;
        }

        /// <summary>
        /// 싱글톤 초기화 메서드
        /// </summary>
        private static void CreateSingletonInstance()
        {
            _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.CreateSingletonInstance;
            _instance = SpawnSingletonInstance();
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
        /// 초기화 콜백
        /// </summary>
        private void Awake()
        {
            switch (_CurrentSingletonPhase)
            {
                case SingletonTool.SingletonInitializePhase.None:
                    GetInstanceSafe();
                    break;
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