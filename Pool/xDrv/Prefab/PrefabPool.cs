using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using k514.Mono.Feature;
using Unity.Entities;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 프리팹 풀
    /// </summary>
    public class PrefabPool<Content, CreateParams, ActivateParams> : PoolBase<Content, CreateParams, ActivateParams> 
        where Content : PrefabPoolContent<Content, CreateParams, ActivateParams>
        where CreateParams : IPrefabCreateParams<CreateParams>
        where ActivateParams : IPrefabActivateParams
    {
        #region <Consts>

        public static PrefabPool<Content, CreateParams, ActivateParams> GetPool(CreateParams p_CreateParams, Transform p_Wrapper = null)
        {
            var pool = new PrefabPool<Content, CreateParams, ActivateParams>(p_CreateParams, p_Wrapper);
            pool.LoadPrefab();
            return pool;
        }
        
        public static async UniTask<PrefabPool<Content, CreateParams, ActivateParams>> GetPool(CreateParams p_CreateParams, Transform p_Wrapper = null, CancellationToken p_Token = default)
        {
            var pool = new PrefabPool<Content, CreateParams, ActivateParams>(p_CreateParams, p_Wrapper);
            await pool.LoadPrefab(p_Token);
            return pool;
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 프리팹 풀 래퍼
        /// </summary>
        public Transform Wrapper { get; private set; }

        /// <summary>
        /// 풀 프리팹 기본 타입
        /// </summary>
        public Type DefaultMainComponentType { get; private set; }

        /// <summary>
        ///풀 프리팹 타입
        /// </summary>
        public Type MainComponentType { get; private set; }

        /// <summary>
        /// 메인 컴포넌트 외에 프리팹에 추가해야할 컴포넌트 리스트
        /// </summary>
        public List<Type> SubComponentList { get; private set; }

        /// <summary>
        /// 프리팹 에셋
        /// </summary>
        private AssetLoadResult<GameObject> Prefab;

        #endregion
        
        #region <Constructors>

        private PrefabPool(CreateParams p_CreateParams, Transform p_Wrapper) : base(p_CreateParams)
        {
#if UNITY_EDITOR
            if (p_CreateParams is null)
            {
                throw new ArgumentNullException(nameof(p_CreateParams));
            }
#endif
            Wrapper = p_Wrapper;
            DefaultMainComponentType = typeof(Content);
            SubComponentList = new List<Type>();
            
            var componentType = p_CreateParams.GetComponent();
            
            // 컴포넌트 데이터 컴포넌트가 없는 경우
            if (ReferenceEquals(null, componentType))
            {
                MainComponentType = DefaultMainComponentType;
            }
            else
            {
                switch (componentType)
                {
                    // 컴포넌트 데이터 컴포넌트와 메인 컴포넌트 타입이 같은 경우
                    case var _ when ReferenceEquals(componentType, DefaultMainComponentType):
                        MainComponentType = DefaultMainComponentType;
                        break;
                    // 컴포넌트 데이터 컴포넌트보다 풀 플레이스홀더 타입의 계층이 더 아래었던 경우
                    case var _ when DefaultMainComponentType.IsSubclassOf(componentType):
                        MainComponentType = DefaultMainComponentType;
                        break;
                    // 컴포넌트 데이터 컴포넌트보다 풀 플레이스홀더 타입의 계층이 더 위였던 경우
                    case var _ when componentType.IsSubclassOf(DefaultMainComponentType):
                        MainComponentType = componentType;
                        break;
                    // 플레이스 홀더 타입과 컴포넌트 데이터 컴포넌트가 아무런 상속관계가 없는 경우
                    default:
                        MainComponentType = DefaultMainComponentType;
                        SubComponentList.Add(componentType);
                        break;
                }
            }
        }

        #endregion

        #region <Callbacks>

        protected sealed override void OnCreateContent(Content p_Content)
        {
            p_Content.OnCreate(this);
        }
        
        protected sealed override bool OnActivateContent(Content p_Content, ActivateParams p_ActivateParams)
        {
            var wrapper = p_ActivateParams.Wrapper;
            if (ReferenceEquals(null, wrapper))
            {
                p_Content.Affine.SetParent(Wrapper, false);
            }
            else
            {
                p_Content.Affine.SetParent(wrapper, false);
            }

            return p_Content.OnActivate(this, p_ActivateParams);
        }
        
        protected sealed override void OnRetrieveBegin(Content p_Content)
        {
            p_Content.OnRetrieveBegin(this);
        }

        protected sealed override void OnRetrieving(Content p_Content)
        {
            p_Content.OnRetrieving(this);
        }

        protected sealed override void OnRetrieveOver(Content p_Content)
        {
            p_Content.OnRetrieveOver(this);

            if (Wrapper == null)
            {
                if (IsDisposed)
                {
                }
                else
                {
                    p_Content.Affine.SetParent(null, false);
                }
            }
            else
            {
                p_Content.Affine.SetParent(Wrapper, false);
            }
        }

        protected override void OnDisposeUnmanaged()
        {
            Wrapper = null;
            if (Prefab.ValidFlag)
            {
                AssetLoaderManager.GetInstanceUnsafe?.UnloadAsset(ref Prefab);
            }
            
            base.OnDisposeUnmanaged();
        }
        
        #endregion
        
        #region <Methods>

        protected sealed override Content CreateContent(CreateParams p_CreateParams)
        {
            return PrefabPoolContent<Content, CreateParams, ActivateParams>.CreateContent(this);
        }

        public void LoadPrefab()
        {
            if (AssetLoaderManager.GetInstanceUnsafe is not null)
            {
                Prefab = AssetLoaderManager.GetInstanceUnsafe.LoadAsset<GameObject>(GetCreateParams().AssetLoadKey);
            }
        }
        
        public async UniTask LoadPrefab(CancellationToken p_Token)
        {
            if (AssetLoaderManager.GetInstanceUnsafe is not null)
            {
                Prefab = await AssetLoaderManager.GetInstanceUnsafe.LoadAssetAsync<GameObject>(GetCreateParams().AssetLoadKey, p_Token);
            }
        }

        public GameObject GetPrefab()
        {
            return Prefab.Asset;
        }

        #endregion
    }

    /// <summary>
    /// 풀 프리팹
    /// </summary>
    public abstract class PrefabPoolContent<Content, CreateParams, ActivateParams> : MonoBehaviour, IPrefabContent<CreateParams>
        where Content : PrefabPoolContent<Content, CreateParams, ActivateParams>
        where CreateParams : IPrefabCreateParams<CreateParams>
        where ActivateParams : IPrefabActivateParams
    {
        #region <Finalizer>

        protected void OnDestroy()
        {
            Dispose();
        }

        #endregion

        #region <Consts>

        public static Content CreateContent(PrefabPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            var poolKey = p_Pool.GetCreateParams();
            var mainComponent = p_Pool.MainComponentType;
            var subComponentSet = p_Pool.SubComponentList;
            var prefab = p_Pool.GetPrefab();
            var gameObject = default(GameObject);
            
            if (ReferenceEquals(null, prefab))
            {
#if UNITY_EDITOR
                CustomDebug.LogError($"Model Key '{poolKey?.ModelData.TableRecord?.KEY??-1}' not found");
                gameObject = new GameObject($"Fallback_({mainComponent.Name})");
                var model = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;
                model.name = "Model";
                model.SetParent(gameObject.transform);
                DestroyImmediate(model.GetComponent<CapsuleCollider>());
                
                var modelRecord = poolKey.ModelData.TableRecord;
                if (!ReferenceEquals(null, modelRecord) && modelRecord is IGameEntityModelDataTableRecordBridge c_ModelRecord)
                {
                    var diameter = 2f * c_ModelRecord.PrefabColliderRadius;
                    var halfHeight = 0.5f * c_ModelRecord.PrefabColliderHeight;
                    model.localScale = new Vector3(diameter, halfHeight, diameter);
                }
#else
                gameObject = new GameObject();
#endif
            }
            else
            {
                gameObject = Instantiate(prefab);
            }

            var result = gameObject.AddComponent(mainComponent) as Content;
            foreach (var subComponent in subComponentSet)
            {
                gameObject.AddComponent(subComponent);
            }        
            
            // fake null
            if (result == null)
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError((p_Pool, $"오브젝트 생성에 실패했습니다. : [{poolKey}], [Component : {mainComponent}]"));
#endif
                Destroy(gameObject);
            }
            else 
            {
                result._Pool = p_Pool;
                result.OnInitializeModel(poolKey.GetDefaultScale());
            }

            return result;
        }

        #endregion

        #region <Fields>

        public PoolTool.ContentState ContentState { get; private set; }
        public bool IsContentActive => ContentState == PoolTool.ContentState.Active;
        public bool IsContentPooled => ContentState == PoolTool.ContentState.Pooled;
        private PrefabPool<Content, CreateParams, ActivateParams> _Pool;
        private int _pooledCount;
        public Transform Affine { get; private set; }
        public ScaleFloatInvSqrSqrt ObjectScale { get; private set; }
        public float Scale => ObjectScale.CurrentValue;
        public float HalfScale => 0.5f * ObjectScale.CurrentValue;
        public float DoubleScale => 2f * ObjectScale.CurrentValue;
        public float SqrtScale => ObjectScale.CurrentValueSqrt;

        #endregion

        #region <Callbacks>

        protected virtual void OnInitializeModel(float p_ModelScale)
        {
            Affine = transform;
            ObjectScale = new ScaleFloatInvSqrSqrt(p_ModelScale * Affine.localScale.x, 0f);
            Affine.localScale = Scale * Vector3.one;
        }
                
        protected virtual void OnScaleChanged(float p_DeltaRatio)
        {
            Affine.localScale *= p_DeltaRatio;
        }
        
        public void OnCreate(PrefabPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool) && ContentState == PoolTool.ContentState.None)
            {
                ContentState = PoolTool.ContentState.Created;
                
                try
                {
                    // 해당 풀링 인스턴스의 수명은, 해당 인스턴스의 원본 프리팹의 ResourceLifeCycleType에 의해 결정되며
                    // 씬 전환으로는 해당 인스턴스는 파기되지 않는다.
                    // 파기하기 위해서는 PrefabPoolingManager의 Release 계열 메서드를 통해 파기해야한다.
                    Affine.DontDestroyOnLoadSafe();
                    
                    OnCreate(_Pool.GetCreateParams());
                }
#if APPLY_PRINT_LOG
                catch (Exception e)
                {
                    CustomDebug.LogError((this, "OnCreateError!", e, Color.red));
                    Dispose();
                }
#else
                catch
                {
                    Dispose();
                }
#endif
            }
        }

        public bool OnActivate(PrefabPool<Content, CreateParams, ActivateParams> p_Pool, ActivateParams p_ActivateParams)
        {
            if (ReferenceEquals(_Pool, p_Pool))
            {
                switch (ContentState)
                {
                    default:
                    {
                        return false;
                    }
                    case PoolTool.ContentState.Created:
                    {
                        ContentState = PoolTool.ContentState.Active;
                        _pooledCount++;

                        try
                        {
                            gameObject.SetActiveSafe(true);
                            return OnActivate(_Pool.GetCreateParams(), p_ActivateParams, false);
                        }
#if APPLY_PRINT_LOG
                        catch (Exception e)
                        {
                            CustomDebug.LogError((this, "OnActivateError!", e, Color.red));
                            return false;
                        }
#else
                        catch
                        {
                            return false;
                        }
#endif
                        break;
                    }
                    case PoolTool.ContentState.Pooled:
                    {
                        ContentState = PoolTool.ContentState.Active;
                        _pooledCount++;

                        try
                        {
                            gameObject.SetActiveSafe(true);
                            return OnActivate(_Pool.GetCreateParams(), p_ActivateParams, true);
                        }
#if APPLY_PRINT_LOG
                        catch (Exception e)
                        {
                            CustomDebug.LogError((this, "OnActivateError!", e, Color.red));
                            return false;
                        }
#else
                        catch
                        {
                            return false;
                        }
#endif
                        break;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public void OnRetrieveBegin(PrefabPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool))
            {
                switch (ContentState)
                {
                    case PoolTool.ContentState.Created:
                    case PoolTool.ContentState.Active:
                        ContentState = PoolTool.ContentState.Retrieving;
                        break;
                }
            }
        }

        public void OnRetrieving(PrefabPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool) && ContentState == PoolTool.ContentState.Retrieving)
            {
                try
                {
                    gameObject.SetActiveSafe(false);
                    OnRetrieve(_Pool.GetCreateParams(), _pooledCount != 0, p_Pool.IsDisposed);
                }
#if APPLY_PRINT_LOG
                catch (Exception e)
                {
                    CustomDebug.LogError((this, "OnRetrieveError!", e, Color.red));
                    Dispose();
                }
#else
                catch
                {
                    Dispose();
                }
#endif
            }
        }

        public void OnRetrieveOver(PrefabPool<Content, CreateParams, ActivateParams> p_Pool)
        {
            if (ReferenceEquals(_Pool, p_Pool) && ContentState == PoolTool.ContentState.Retrieving)
            {
                ContentState = PoolTool.ContentState.Pooled;
            }
        }

        protected abstract void OnCreate(CreateParams p_CreateParams);
        protected abstract bool OnActivate(CreateParams p_CreateParams, ActivateParams p_ActivateParams, bool p_IsPooled);
        protected abstract void OnRetrieve(CreateParams p_CreateParams, bool p_IsPooled, bool p_IsDisposed);
        protected abstract void OnDispose();

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        private void OnDisposeUnmanaged()
        {
            switch (ContentState)
            {
                case PoolTool.ContentState.None:
                case PoolTool.ContentState.Created:
                case PoolTool.ContentState.Active:
                case PoolTool.ContentState.Pooled:
                case PoolTool.ContentState.Retrieving:
                {
                    ContentState = PoolTool.ContentState.Disposed;

                    Pooling();
                    OnDispose();

                    if (this != null)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
                case PoolTool.ContentState.Disposed :
                    break;
            }
        }
        
        #endregion

        #region <Methods>

        /// <summary>
        /// 생성 파라미터를 리턴하는 메서드
        /// </summary>
        public CreateParams GetCreateParams() => _Pool.GetCreateParams();

        /// <summary>
        /// 해당 오브젝트가 오브젝트 풀러 이외의 방법으로 생성된 경우, 생성 콜백을 수동 호출해주는 메서드
        /// </summary>
        public void CheckAwake(CreateParams p_CreateParams = default)
        {
            if (ContentState == PoolTool.ContentState.None)
            {
                OnInitializeModel(1f);
                ContentState = PoolTool.ContentState.Active;
                OnCreate(p_CreateParams);
                OnActivate(p_CreateParams, default, false);
            }
        }
        
        public int GetPooledCount()
        {
            return _pooledCount;
        }
        
        public void DisconnectPool()
        {
            if (!ReferenceEquals(null, _Pool))
            {
                var _pool = _Pool;
                _Pool = null;
                _pool.DisconnectContent(this as Content);
            }
        }
        
        public void Pooling()
        {
            _Pool?.Retrieve(this as Content);
        }

        /// <summary>
        /// 게임 오브젝트의 스케일을 변경하는 메서드
        /// </summary>
        public void SetScaleFactor(float p_ScaleFactor)
        {
            p_ScaleFactor = Mathf.Max(0.01f, p_ScaleFactor);
            
            var prevObjectScaleInv = ObjectScale.CurrentValueInverse;
            ObjectScale.SetScale(p_ScaleFactor);
            
            var scaleDeltaRatio = prevObjectScaleInv * Scale;
            OnScaleChanged(scaleDeltaRatio);
        }
        
        /// <summary>
        /// 게임 오브젝트의 스케일을 곱하는 메서드
        /// </summary>
        public void MulScaleFactor(float p_ScaleFactor)
        {
            SetScaleFactor(p_ScaleFactor * ObjectScale.CurrentValue);
        }

        /// <summary>
        /// 게임 오브젝트의 레이어를 변경하는 메서드
        /// </summary>
        public void TurnLayerTo(GameConst.GameLayerType p_LayerType)
        {
            Affine.TurnLayerTo(p_LayerType, false);
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