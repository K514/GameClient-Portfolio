using Cysharp.Threading.Tasks;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Fields>

        /// <summary>
        /// 현재 시스템 상태 플래그마스크
        /// </summary>
        private static SystemStateFlag _systemState;

        /// <summary>
        /// 시스템 부팅 중인지 표시하는 플래그
        /// </summary>
        public static bool IsSystemOpenProgressing => !_systemState.HasAnyFlagExceptNone(SystemStateFlag.SystemValid) && IsSingletonAvailable;
        
        /// <summary>
        /// 시스템 동작 중인지 표시하는 플래그
        /// </summary>
        public static bool IsSystemOpen => _systemState.HasAllFlagExceptNone(SystemStateFlag.SystemOpen);
        
        /// <summary>
        /// 시스템 정지를 표시하는 플래그
        /// </summary>
        public static bool IsSystemClose => !IsSystemOpen;
        
        /// <summary>
        /// 게임 매니저 로드를 표시하는 플래그
        /// </summary>
        public static bool IsGameManagerLoaded => _systemState.HasAllFlagExceptNone(SystemStateFlag.GameManagerLoaded);
        
        /// <summary>
        /// 싱글톤 생성을 제한하는 플래그
        /// </summary>
        public static bool IsSingletonAvailable => _systemState.HasAnyFlagExceptNone(SystemStateFlag.SingletonAvailable);
        
        #endregion

        #region <Callbacks>

        private static void OnBeginStartSystem()
        {
            _systemState.AddFlag(SystemStateFlag.SingletonAvailable);
        }
        
        private static void OnSuccessStartSystem()
        {
            _systemState.AddFlag(SystemStateFlag.SystemValid);
#if UNITY_EDITOR
            OnLoadMethodOver?.Invoke();
#endif
        }
        
        private static void OnFailStartSystem()
        {
            _systemState.RemoveFlag(SystemStateFlag.SystemOpen);
        }

        private static void OnSuccessReleaseSystem()
        {
            _systemState.RemoveFlag(SystemStateFlag.SystemOpen);
        }

        #endregion

        #region <Methods>

        public static async UniTask WaitSystemOpen()
        {
            await UniTask.WaitUntil(() => IsSystemOpen);
        }

        public static async UniTask WaitGameManagerLoad()
        {
            await UniTask.WaitUntil(() => IsGameManagerLoaded);
        }

        #endregion
    }
}