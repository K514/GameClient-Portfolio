using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 싱글톤과 ScripatableObject를 상속하여, 에셋으로서 동작하는 싱글톤 추상 클래스.
    /// </summary>
    public abstract class ScriptableObjectAsyncSingleton<Me> : ScriptableObjectSingletonBase<Me>, ICancellationTokenSource where Me : ScriptableObjectAsyncSingleton<Me>
    {
        #region <Consts>
        
        /// <summary>
        /// 싱글톤 생성 및 초기화 메서드
        /// </summary>
        private static async UniTask<Me> GetInstanceSafe(CancellationToken p_CancellationToken)
        {
            p_CancellationToken.ThrowIfCancellationRequested();
            
            switch (_CurrentSingletonPhase)
            {
                case SingletonTool.SingletonInitializePhase.None:
                {
                    if (SystemBoot.IsSingletonAvailable)
                    {
                        try
                        {
                            await CreateSingletonInstance(p_CancellationToken);
                        }
#if APPLY_PRINT_LOG
                        catch(Exception e)
                        {
                            if (CustomDebug.CustomDebugLogFlag.PrintSingletonLog.HasOpen())
                            {
                                CustomDebug.LogError(($"* Fail to Initiate Singleton [{typeof(Me).Name}]", e, Color.red));
                            }
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
                case SingletonTool.SingletonInitializePhase.PreloadDependencies:
                case SingletonTool.SingletonInitializePhase.CreateSingletonInstance:
                case SingletonTool.SingletonInitializePhase.ProcessCreatedCallback:
                case SingletonTool.SingletonInitializePhase.ProcessInitializeCallback:
                {
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintSingletonLog.HasOpen())
                    {
                        CustomDebug.LogError((typeof(Me), "Yield", Color.red));
                    }
#endif
                    await UniTask.WaitUntil
                    (
                        () => _CurrentSingletonPhase switch
                        {
                            SingletonTool.SingletonInitializePhase.None => true,
                            SingletonTool.SingletonInitializePhase.InitializeOver => true,
                            _ => false,
                        }, 
                        cancellationToken: p_CancellationToken
                    );
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintSingletonLog.HasOpen())
                    {
                        CustomDebug.LogError((typeof(Me), "Yield Over", Color.red));
                    }
#endif
                    break;
                }
            }

            return _instance;
        }

        /// <summary>
        /// 싱글톤 생성 메서드
        /// </summary>
        private static async UniTask<Me> SpawnSingletonInstance(CancellationToken p_CancellationToken)
        {
            var result = FindAnyObjectByType<Me>();
            if (result == null)
            {
                result = (Me) await Resources.LoadAsync<Me>($"{SystemMaintenance.GetResourcePath(ResourceLoadType.FromUnityResource, ResourceType.ScriptableObject, AssetPathType.RelativePath)}/{typeof(Me)}").WithCancellation(p_CancellationToken);
            }
            return result;
        }

        /// <summary>
        /// 싱글톤 초기화 메서드
        /// </summary>
        private static async UniTask CreateSingletonInstance(CancellationToken p_CancellationToken)
        {
            _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.CreateSingletonInstance;
            _instance = await SpawnSingletonInstance(p_CancellationToken);
            SystemBoot.GetSystemLinkedCancellationTokenSource(ref _instance._CancellationTokenSource);
            _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.PreloadDependencies;
            if (await _instance.OnLoadDependency(p_CancellationToken))
            {
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.ProcessCreatedCallback;
                await _instance.OnCreated(p_CancellationToken);
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.ProcessInitializeCallback;
                await _instance.OnInitiate(p_CancellationToken);
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.InitializeOver;
            }
            else
            {
                throw new Exception();
            }
        }
        
        protected static async UniTask<object> GetObject(CancellationToken p_CancellationToken)
        {
            return await GetInstanceSafe(p_CancellationToken);
        }
        
        #endregion

        #region <Fields>
        
        /// <summary>
        /// 비동기 테스크 취소 토큰
        /// </summary>
        private CancellationTokenSource _CancellationTokenSource;

        #endregion
        
        #region <Callbacks>

        /// <summary>
        /// 해당 싱글톤이 종속된 싱글톤을 로드하는 콜백
        /// </summary>
        private async UniTask<bool> OnLoadDependency(CancellationToken p_CancellationToken)
        {
            TryInitializeDependency();

            var (result, _) = await SingletonTool.CreateSingletonAsync(_Dependencies, MultiTaskMode.Sequence, p_CancellationToken);
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
        private async void Awake()
        {
            switch (_CurrentSingletonPhase)
            {
                case SingletonTool.SingletonInitializePhase.None:
                    await GetInstanceSafe(SystemBoot.GetSystemCancellationToken());
                    break;
            }
        }

        /// <summary>
        /// 싱글톤 초기화 콜백. 해당 싱글톤 생명주기 중에 단 한번만 호출되야함.
        /// </summary>
        protected abstract UniTask OnCreated(CancellationToken p_CancellationToken);

        /// <summary>
        /// 싱글톤 초기화 콜백. OnCreated 이후에 호출된다.
        /// </summary>
        protected abstract UniTask OnInitiate(CancellationToken p_CancellationToken);

        /// <summary>
        /// 싱글톤이 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected override void OnDisposeSingleton()
        {
            AsyncTaskTool.Dispose(ref _CancellationTokenSource);
            
            base.OnDisposeSingleton();
        }

        #endregion

        #region <Methods>

        public async UniTask Reset(CancellationToken p_CancellationToken)
        {
            SystemBoot.GetSystemLinkedCancellationTokenSource(ref _CancellationTokenSource);
            await OnInitiate(p_CancellationToken);
        }
        
        public CancellationToken GetCancellationToken()
        {
            return _CancellationTokenSource.Token;
        }

        public void GetLinkedCancellationTokenSource(ref CancellationTokenSource r_Token)
        {
            if (r_Token.IsValid())
            {
                r_Token.Cancel();
            }

            r_Token = CancellationTokenSource.CreateLinkedTokenSource(_CancellationTokenSource.Token);
        }

        #endregion
    }
}