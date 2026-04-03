#if UNITY_EDITOR

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using xk514;

namespace k514
{
    /// <summary>
    /// 에디터 싱글톤 싱글톤 기저 클래스
    /// </summary>
    public abstract class EditorSingletonBase<Me> : Editor, ISingleton where Me : EditorSingletonBase<Me>
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
        /// GUI가 아직 표시될 수 없을 때를 나타내는 플래그
        /// </summary>
        protected bool _GUI_Not_Valid_Flag;

        #endregion
        
        #region <Callbacks>

        protected abstract void _Awake();

        private void OnDisable()
        {
            if (ReferenceEquals(_instance, this))
            {
                _OnDisable();
            }
        }

        /// <summary>
        /// 에디터 스크립트의 OnDestroy콜백은 OnDisable보다 늦게 호출되며 그 기능을 완전히 동일한듯.
        /// 해당 콜백이 호출되어도 인스턴스 자체가 즉시 릴리스되는 것은 아니다.
        /// </summary>
        protected void OnDestroy()
        {
            if (ReferenceEquals(_instance, this))
            {
                _OnDestroy();
            }
            
            Dispose();
        }

        protected virtual void _OnDisable()
        {
        }

        protected virtual void _OnDestroy()
        {
        }

        public override void OnInspectorGUI()
        {
            if (_GUI_Not_Valid_Flag)
            {
                _OnInspectorGUI_NotValid();
            }
            else
            {
                if (ReferenceEquals(_instance, this))
                {
                    switch (_CurrentSingletonPhase)
                    {
                        case SingletonTool.SingletonInitializePhase.InitializeOver:
                            _OnInspectorGUI();
                            break;
                        default:
                            _OnInspectorGUI_NotValid();
                            break;
                    }
                }
                else
                {
                    if (ReferenceEquals(_instance, null))
                    {
                        _Awake();
                    }
                }
            }
        }

        protected virtual void _OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        protected virtual void _OnInspectorGUI_NotValid()
        {
            EditorGUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Processing...");
            EditorGUILayout.EndVertical();
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
            _GUI_Not_Valid_Flag = false;
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
        
        protected void _Repaint()
        {
            if (ReferenceEquals(_instance, this))
            {
                Repaint();
            }
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

#endif