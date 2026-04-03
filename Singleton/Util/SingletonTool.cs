using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514
{
    /// <summary>
    /// ISingleton의 구현체가 Terminal Generic이기 때문에 추상화할 수 없었던 싱글톤 공통 기능을
    /// 제공하는 정적 클래스
    /// </summary>
    public static class SingletonTool
    {
        #region <Enums>

        public enum SingletonInitializePhase
        {
            None,
            CreateSingletonInstance,
            PreloadDependencies,
            ProcessCreatedCallback,
            ProcessInitializeCallback,
            InitializeOver,
        }

        #endregion

        #region <Method/Reflection/Create>
        
        /// <summary>
        /// 싱글톤을 생성하는 메서드
        /// </summary>
        public static object CreateSingleton(Type p_Type)
        {
            switch (p_Type)
            {
                case var _ when p_Type.IsSingleton():
                case var _ when p_Type.IsUnitySingleton():
                {
                    var method = p_Type
                        .GetMethod
                        (
                            "GetObject",
                            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy
                        );
                    return method?.Invoke(null, null);
                }
                default :
#if APPLY_PRINT_LOG
                    CustomDebug.LogError((p_Type, "생성할 수 없는 싱글톤 타입", Color.red));
#endif
                    return null;
            }
        }
        
        /// <summary>
        /// 싱글톤을 생성하는 메서드 
        /// </summary>
        public static (bool, object) TryCreateSingleton(Type p_Type)
        {
            var spawned = CreateSingleton(p_Type);
            return (!ReferenceEquals(null, spawned), spawned);
        }

        /// <summary>
        /// 싱글톤을 생성하는 메서드
        ///
        /// 1번 파라미터가 참일 때 싱글톤을 순차적으로 생성한다. 거짓일 때 싱글톤을 동시 생성하여 모든 작업이 끝날때까지 대기한다.
        /// 2번 파라미터가 참일 때 싱글톤 생성에 실패한 경우 즉시 루프를 중단한다. 거짓일 때는 생성 성공/실패에 상관없이 루프를 끝까지 수행한다.
        /// </summary>
        public static (bool, List<object>) CreateSingleton(HashSet<Type> p_TypeSet, MultiTaskMode p_Mode)
        {
            var resultSet = new List<object>();

            switch (p_Mode)
            {
                default:
                case MultiTaskMode.Sequence:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = TryCreateSingleton(type);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                        else
                        {
                            return (false, resultSet);
                        }
                    }

                    return (true, resultSet);
                }
                case MultiTaskMode.SequenceKeepFail:
                case MultiTaskMode.Simultaneous:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = TryCreateSingleton(type);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                    }

                    return (true, resultSet);
                }
            }
        }
        
        /// <summary>
        /// 싱글톤을 생성하는 메서드
        ///
        /// 1번 파라미터가 참일 때 싱글톤을 순차적으로 생성한다. 거짓일 때 싱글톤을 동시 생성하여 모든 작업이 끝날때까지 대기한다.
        /// 2번 파라미터가 참일 때 싱글톤 생성에 실패한 경우 즉시 루프를 중단한다. 거짓일 때는 생성 성공/실패에 상관없이 루프를 끝까지 수행한다.
        /// </summary>
        public static (bool, List<object>) CreateSingleton(Type[] p_TypeSet, MultiTaskMode p_Mode)
        {
            var resultSet = new List<object>();

            switch (p_Mode)
            {
                default:
                case MultiTaskMode.Sequence:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = TryCreateSingleton(type);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                        else
                        {
                            return (false, resultSet);
                        }
                    }

                    return (true, resultSet);
                }
                case MultiTaskMode.SequenceKeepFail:
                case MultiTaskMode.Simultaneous:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = TryCreateSingleton(type);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                    }

                    return (true, resultSet);
                }
            }
        }
        
        /// <summary>
        /// 싱글톤을 생성하는 메서드
        /// </summary>
        public static async UniTask<object>CreateSingletonAsync(Type p_Type, CancellationToken p_Cancellation)
        {
            switch (p_Type)
            {
                case var _ when p_Type.IsSingleton():
                case var _ when p_Type.IsUnitySingleton():
                {
                    var method = p_Type
                        .GetMethod
                        (
                            "GetObject",
                            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy
                        );
                    return method?.Invoke(null, null);
                }
                case var _ when p_Type.IsAsyncSingleton():
                case var _ when p_Type.IsUnityAsyncSingleton():
                {
                    var method = p_Type
                        .GetMethod
                        (
                            "GetObject",
                            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy
                        );
                    var task = (UniTask<object>)method?.Invoke(null, new object[]{ p_Cancellation });
                    return await task.AttachExternalCancellation(p_Cancellation);
                }
                default :
#if APPLY_PRINT_LOG
                    CustomDebug.LogError((p_Type, "생성할 수 없는 싱글톤 타입", Color.red));
#endif
                    return null;
            }
        }
        
        /// <summary>
        /// 싱글톤을 생성하는 메서드 
        /// </summary>
        public static async UniTask<(bool, object)> TryCreateSingletonAsync(Type p_Type, CancellationToken p_Cancellation)
        {
            var spawned = await CreateSingletonAsync(p_Type, p_Cancellation);
            return (!ReferenceEquals(null, spawned), spawned);
        }

        /// <summary>
        /// 싱글톤을 생성하는 메서드
        ///
        /// 1번 파라미터가 참일 때 싱글톤을 순차적으로 생성한다. 거짓일 때 싱글톤을 동시 생성하여 모든 작업이 끝날때까지 대기한다.
        /// 2번 파라미터가 참일 때 싱글톤 생성에 실패한 경우 즉시 루프를 중단한다. 거짓일 때는 생성 성공/실패에 상관없이 루프를 끝까지 수행한다.
        /// </summary>
        public static async UniTask<(bool, List<object>)> CreateSingletonAsync(Type[] p_TypeSet, MultiTaskMode p_Mode, CancellationToken p_Cancellation)
        {
            var resultSet = new List<object>();
            
            switch (p_Mode)
            {
                default:
                case MultiTaskMode.Sequence:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = await TryCreateSingletonAsync(type, p_Cancellation).AttachExternalCancellation(p_Cancellation);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                        else
                        {
                            return (false, resultSet);
                        }
                    }

                    return (true, resultSet);
                }
                case MultiTaskMode.SequenceKeepFail:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = await TryCreateSingletonAsync(type, p_Cancellation).AttachExternalCancellation(p_Cancellation);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                    }

                    return (true, resultSet);
                }
                case MultiTaskMode.Simultaneous:
                {
                    var asyncTaskList = new List<UniTask<(bool, object)>>();
                    foreach (var type in p_TypeSet)
                    {
                        asyncTaskList.Add(TryCreateSingletonAsync(type, p_Cancellation));
                    }

                    var result = true;
                    var resultTaskSet = await UniTask.WhenAll(asyncTaskList).AttachExternalCancellation(p_Cancellation);
                    foreach (var (success, singleton) in resultTaskSet)
                    {
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    return (result, resultSet);
                }
            }
        }

        /// <summary>
        /// 싱글톤을 생성하는 메서드
        ///
        /// 1번 파라미터가 참일 때 싱글톤을 순차적으로 생성한다. 거짓일 때 싱글톤을 동시 생성하여 모든 작업이 끝날때까지 대기한다.
        /// 2번 파라미터가 참일 때 싱글톤 생성에 실패한 경우 즉시 루프를 중단한다. 거짓일 때는 생성 성공/실패에 상관없이 루프를 끝까지 수행한다.
        /// </summary>
        public static async UniTask<(bool, List<object>)> CreateSingletonAsync(HashSet<Type> p_TypeSet, MultiTaskMode p_Mode, CancellationToken p_Cancellation)
        {
            var resultSet = new List<object>();
            
            switch (p_Mode)
            {
                default:
                case MultiTaskMode.Sequence:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = await TryCreateSingletonAsync(type, p_Cancellation).AttachExternalCancellation(p_Cancellation);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                        else
                        {
                            return (false, resultSet);
                        }
                    }

                    return (true, resultSet);
                }
                case MultiTaskMode.SequenceKeepFail:
                {
                    foreach (var type in p_TypeSet)
                    {
                        var (success, singleton) = await TryCreateSingletonAsync(type, p_Cancellation).AttachExternalCancellation(p_Cancellation);
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                    }

                    return (true, resultSet);
                }
                case MultiTaskMode.Simultaneous:
                {
                    var asyncTaskList = new List<UniTask<(bool, object)>>();
                    foreach (var type in p_TypeSet)
                    {
                        asyncTaskList.Add(TryCreateSingletonAsync(type, p_Cancellation));
                    }

                    var result = true;
                    var resultTaskSet = await UniTask.WhenAll(asyncTaskList).AttachExternalCancellation(p_Cancellation);
                    foreach (var (success, singleton) in resultTaskSet)
                    {
                        if (success)
                        {
                            resultSet.Add(singleton);
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    return (result, resultSet);
                }
            }
        }
             
        #endregion
        
        #region <Method/Reflection/Get>

        public static T GetSingletonUnsafe<T>() where T : class
        {
            return GetSingletonUnsafe<T>(typeof(T));
        }
        
        public static T GetSingletonUnsafe<T>(Type p_Type) where T : class
        {
            switch (true)
            {
                case var _ when p_Type.IsSingleton():
                case var _ when p_Type.IsUnitySingleton():
                case var _ when p_Type.IsAsyncSingleton():
                case var _ when p_Type.IsUnityAsyncSingleton():
                {
                    var property = p_Type
                        .GetProperty
                        (
                            "GetInstanceUnsafe",
                            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                        );
                    return (T) property?.GetValue(null);
                }
                default :
                    return null;
            }
        }

        #endregion
        
        #region <Method/Reflection/Dispose>
        
        /// <summary>
        /// 싱글톤을 파기하는 메서드
        /// </summary>
        public static void DisposeSingleton(Type p_Type)
        {
            switch (p_Type)
            {
                case var _ when p_Type.IsSingleton():
                case var _ when p_Type.IsUnitySingleton():
                case var _ when p_Type.IsAsyncSingleton():
                case var _ when p_Type.IsUnityAsyncSingleton():
                {
                    var method = p_Type
                        .GetMethod
                        (
                            "DisposeSingletonInstance",
                            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
                        );
                    method?.Invoke(null, null);
                    break;
                }
                default :
                    break;
            }
        }

        /// <summary>
        /// 싱글톤을 파기하는 메서드
        /// </summary>
        public static void DisposeSingleton(HashSet<Type> p_TypeSet)
        {
            foreach (var type in p_TypeSet)
            {
                DisposeSingleton(type);
            }
        }
        
        /// <summary>
        /// 싱글톤을 파기하는 메서드
        /// </summary>
        public static void DisposeSingleton(Type[] p_TypeSet)
        {
            foreach (var type in p_TypeSet)
            {
                DisposeSingleton(type);
            }
        }

        #endregion
    }
}