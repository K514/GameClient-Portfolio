using System;
using k514.Mono.Common;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 싱글톤과 MonoBehaviour를 상속하여, 유니티 컴포넌트로서 동작하는 싱글톤 추상 클래스.
    /// </summary>
    public abstract class UnitySingleton<Me> : UnitySingletonBase<Me> where Me : UnitySingleton<Me>
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
            var tryFind = FindAnyObjectByType<Me>();
            if (tryFind == null)
            {
                var prefabNameTable = ScriptPrefabNameTable.GetInstanceUnsafe;
                var prefabNameTableValid = !ReferenceEquals(null, prefabNameTable);
                if (prefabNameTableValid)
                {
                    var assetLoadResult = ScriptPrefabNameTable.GetInstanceUnsafe.GetResource(typeof(Me), ResourceLifeCycleType.ManualUnload);
                    if (assetLoadResult)
                    {
                        var spawned = Instantiate(assetLoadResult.Asset);
                        var tryResult = spawned.GetComponent<Me>();
                        var result = tryResult == null ? spawned.AddComponent<Me>() : tryResult;
                        result._AssetPreset = assetLoadResult;
                        result.Affine = result.transform;
                        return result;
                    }
                    else
                    {
                        var spawned = new GameObject(typeof(Me).Name);
                        var result = spawned.AddComponent<Me>();
                        result.Affine = result.transform;
                        return result;
                    } 
                }
                else
                {
                    var spawned = new GameObject(typeof(Me).Name);
                    var result = spawned.AddComponent<Me>();
                    result.Affine = result.transform;
                    return result;
                }
            }
            else
            {
                tryFind.Affine = tryFind.transform;
                return tryFind;
            }
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
                // Singleton 초기화보다 먼저 Awake 함수에 의해 초기화 되는 경우
                case SingletonTool.SingletonInitializePhase.None:
                {
                    GetInstanceSafe();
                    goto default;
                }
                // Singleton 초기화에 의해 AddComponent된 경우
                case SingletonTool.SingletonInitializePhase.CreateSingletonInstance:
                {
                    gameObject.DontDestroyOnLoadSafe();
                    break;
                }
                // InitializeOver 페이즈일 때 진입하며 Sinlgeton 초기화가 완료된 이후 Awake가 호출된 경우
                default:
                {
                    // Awake 이전에 _instance가 할당됬다면 isDestroy가 true 상태인 별도의 인스턴스가 할당되기 때문에 == 연산자로 비교한다.
                    if (this == _instance)
                    {
                        gameObject.DontDestroyOnLoadSafe();
                    }
                    else
                    {
                        if ( this != null)
                        {
                            Destroy(gameObject);
                        }
                    }
                    break;
                }
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