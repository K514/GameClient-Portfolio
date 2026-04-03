using System;
using System.Collections.Generic;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// 유니티 싱글톤 기저 클래스
    /// </summary>
    public abstract class UnitySingletonBase<Me> : MonoBehaviour, IUnitySingleton where Me : UnitySingletonBase<Me>
    {
        #region <Consts>

        /// <summary>
        /// 싱글톤 인스턴스
        /// </summary>
        protected static Me _instance;

        /// <summary>
        /// 싱글톤이 초기화 진행 페이즈
        /// </summary>
        protected static SingletonTool.SingletonInitializePhase _CurrentSingletonPhase;

        /// <summary>
        /// 싱글톤 인스턴스 접근 프로퍼티
        /// </summary>
        public static Me GetInstanceUnsafe => _CurrentSingletonPhase switch
        {
            SingletonTool.SingletonInitializePhase.InitializeOver => _instance,
            _ => null
        };
        
        /// <summary>
        /// 싱글톤 파기 메서드
        /// </summary>
        public static void DisposeSingletonInstance()
        {
            if (!ReferenceEquals(null, _instance))
            {
                _instance.Dispose();
                _instance = null;
            }
        }
        
        #endregion

        #region <Fields>

        /// <summary>
        /// 해당 싱글톤이 초기화되는데 필요한 싱글톤 타입 리스트
        /// </summary>
        protected HashSet<Type> _Dependencies;

        /// <summary>
        /// 해당 싱글톤의 래퍼 오브젝트 에셋프리셋
        /// </summary>
        protected AssetLoadResult<GameObject> _AssetPreset;
        
        /// <summary>
        /// 아핀 오브젝트
        /// </summary>
        public Transform Affine { get; protected set; }

        #endregion
        
        #region <Callbacks>

        /// <summary>
        /// 게임 오브젝트가 파괴된 경우, 소멸 메서드를 호출해준다.
        /// </summary>
        protected void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        private void OnDisposeUnmanaged()
        {
            OnDisposeSingleton();
            
            if (!ReferenceEquals(null, _Dependencies))
            {
                _Dependencies.Clear();
                _Dependencies = null;
            }
            
            if (ReferenceEquals(_instance, this))
            {
                SystemBoot.OnSingletonDisposed(_instance);
                _CurrentSingletonPhase = SingletonTool.SingletonInitializePhase.None;
                _instance = null;
            }
        }

        /// <summary>
        /// 싱글톤이 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected virtual void OnDisposeSingleton()
        {
            if (this != null)
            {
                Destroy(gameObject);
            }
            
            Affine = null;
            AssetLoaderManager.GetInstanceUnsafe?.UnloadAsset(ref _AssetPreset);
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 해당 싱글톤이 종속된 싱글톤을 특정하는 콜백
        /// </summary>
        protected virtual void TryInitializeDependency()
        {
            _Dependencies = new HashSet<Type>();
        }

#if APPLY_PRINT_LOG
        public override string ToString()
        {
            return $"[{typeof(Me).Name}]";
        }
#endif

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
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintSingletonLog.HasOpen())
                {
                    CustomDebug.Log((this, "Dispose Start"));
                }
#endif
                try
                {
                    OnDisposeUnmanaged();
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintSingletonLog.HasOpen())
                    {
                        CustomDebug.Log((this, "Dispose Complete"));
                    }
#endif
                }
#if APPLY_PRINT_LOG
                catch (Exception e)
                {
                    if (CustomDebug.CustomDebugLogFlag.PrintSingletonLog.HasOpen())
                    {
                        CustomDebug.LogError((this, "Dispose Failed", e, Color.red));
                    }
                    throw;
                }
#else
                catch
                {
                    throw;
                }
#endif
            }
        }

        #endregion
    }
}