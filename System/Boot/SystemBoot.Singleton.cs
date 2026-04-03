using System;
using System.Collections.Generic;
using UnityEngine;
using xk514;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Fields>
                        
        /// <summary>
        /// 현재 활성화된 싱글톤 그룹
        /// </summary>
        private static List<ISingleton> _activeSingletonSet;

        #endregion

        #region <Callbacks>

        private static void OnInitSystemSingleton()
        {
            _activeSingletonSet = new List<ISingleton>();
            _systemState.AddFlag(SystemStateFlag.SingletonAvailable);
        }

        private static void OnReleaseSystemSingleton()
        {
            ClearActiveSingleton();
        }
        
        /// <summary>
        /// 싱글톤이 생성된 경우 호출되는 콜백
        /// </summary>
        public static void OnSingletonSpawned(ISingleton p_Singleton)
        {
            _activeSingletonSet.Add(p_Singleton);
            
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSystemManagerLog.HasOpen())
            {
                CustomDebug.LogError($"{p_Singleton} Singleton Spawned (Now Count : {_activeSingletonSet.Count})");
            }
#endif
        }
        
        /// <summary>
        /// 싱글톤이 파기된 경우 호출되는 콜백
        /// </summary>
        public static void OnSingletonDisposed(ISingleton p_Singleton)
        {
            _activeSingletonSet.Remove(p_Singleton);
            
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSystemManagerLog.HasOpen())
            {
                CustomDebug.LogError($"{p_Singleton} Singleton Dead (Now Count : {_activeSingletonSet.Count})");
            }
#endif
        }
        
        /// <summary>
        /// 싱글톤이 다른 오브젝트에 의해 수명을 제어받는 경우 호출되는 콜백
        /// </summary>
        public static void OnSingletonControlInterrupted(ISingleton p_Singleton)
        {
            _activeSingletonSet.Remove(p_Singleton);
            
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSystemManagerLog.HasOpen())
            {
                CustomDebug.LogError($"{p_Singleton} Singleton Snapped (Now Count : {_activeSingletonSet.Count})");
            }
#endif
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 현재 활성화된 싱글톤이 있는지 검증하는 메서드
        /// </summary>
        public static bool HasActiveSingleton()
        {
            return _activeSingletonSet.Count > 0;
        }

        /// <summary>
        /// 현재 활성화된 게임 테이블 싱글톤을 일괄 파기시키는 메서드
        /// 활성화된 순서의 역순으로 제거한다.
        /// </summary>
        public static void ClearActiveSingleton()
        {
#if APPLY_PRINT_LOG
            if (CustomDebug.CustomDebugLogFlag.PrintSystemManagerLog.HasOpen())
            {
                CustomDebug.LogError($"[SystemBoot] Unload All Singleton (Now Count : {_activeSingletonSet.Count})");
            }
#endif
            _systemState.RemoveFlag(SystemStateFlag.SingletonAvailable);

            // 게임 매니저를 특정 그룹 순서로 제거하기 위해 먼저 파기해준다.
            DisposeGameManager();
            
            var removeList = new List<ISingleton>(_activeSingletonSet);
            var currentSingletonNumber = removeList.Count;
            for (var i = currentSingletonNumber - 1; i > -1; i--)
            {
                var targetSingleton = removeList[i];
#if APPLY_PRINT_LOG
                try
                {
                    targetSingleton.Dispose();
                }
                catch(Exception e)
                {
                    if (CustomDebug.CustomDebugLogFlag.PrintSystemManagerLog.HasOpen())
                    {
                        CustomDebug.LogError(($"[SystemBoot] Release All Singleton Failed", e));
                    }
                }
#else
                try
                {
                    targetSingleton.Dispose();
                }
                catch
                {
                    // do nothing
                }
#endif
            }
            
            _systemState.AddFlag(SystemStateFlag.SingletonAvailable);
        }

#if APPLY_PRINT_LOG
        /// <summary>
        /// 현재 활성화된 싱글톤 정보를 출력하는 메서드
        /// </summary>
        public static void PrintActiveSingleton()
        {
            var currentSingletonNumber = _activeSingletonSet.Count;
            if (currentSingletonNumber > 0)
            {
                CustomDebug.LogError($"[SystemBoot] Singleton Remaind : {currentSingletonNumber}");
                for (int i = currentSingletonNumber - 1; i > -1; i--)
                {
                    var targetSingleton = _activeSingletonSet[i];
                    CustomDebug.LogError(targetSingleton.GetType().Name);
                }
            }
            else
            {
                CustomDebug.LogWarning($"[SystemBoot] Singleton Released Successfully");
            }
        }
#endif

        #endregion
    }
}