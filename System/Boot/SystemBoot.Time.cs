using System.Collections.Generic;
using Unity.Core;
using UnityEngine;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Consts>
        
        /// <summary>
        /// 기본 FPS
        /// </summary>
        public const int __Default_GAME_FRAME_PER_SECOND = 60;

        /// <summary>
        /// 고정 갱신 함수 주기
        /// </summary>
        public const float __Default_FIXED_TIMESTEP = 0.02f;

        /// <summary>
        /// 월드 시간 스케일 테이블
        /// </summary>
        private static Dictionary<WorldTimeScaleType, float> _WorldTimeScaleTable;
        
        /// <summary>
        /// 월드 시간 스케일 타입 반복자
        /// </summary>
        private static WorldTimeScaleType[] _Enumerator;
        
        /// <summary>
        /// UI 시간 스케일
        /// </summary>
        public static float UI_TimeScale;
        
        static SystemBoot()
        {
            _WorldTimeScaleTable = new Dictionary<WorldTimeScaleType, float>();
            _Enumerator = EnumFlag.GetEnumEnumerator<WorldTimeScaleType>(EnumFlag.GetEnumeratorType.GetAll);

            foreach (var timeScaleType in _Enumerator)
            {
                _WorldTimeScaleTable.Add(timeScaleType, 1f);
            }

            UI_TimeScale = 1f;
            
            OnTimeScaleChanged();
        }
        
        #endregion

        #region <Property>
        
        /// <summary>
        /// 고정 갱신 함수 주기 밀리세컨드 값
        /// </summary>
        public static uint __FIXED_TIMESTEP_MSEC { get; private set; }

        /// <summary>
        /// UI용 DeltaTime
        /// </summary>
        public static float UI_DeltaTime => UI_TimeScale * Time.unscaledDeltaTime;
        
        #endregion
        
        #region <Enums>

        public enum WorldTimeScaleType
        {
            Default,
            UIxControl,
            RewardSelect,
        }

        #endregion
        
        #region <Callbacks>

        private static void OnInitSystemTime()
        {
            SetFPS();
            SetFixedTimeStep();
            SetWorldTimeScale();
        }

        private static void OnReleaseSystemTime()
        {
        }

        private static void OnTimeScaleChanged()
        {
            var result = 1f;
            foreach (var timeScaleType in _Enumerator)
            {
                result *= _WorldTimeScaleTable[timeScaleType];
            }
            
            SetWorldTimeScale(result);
        }
        
        #endregion

        #region <Methods>

        public static void SetFPS(int p_FPS = __Default_GAME_FRAME_PER_SECOND)
        {
            Application.targetFrameRate = p_FPS;
        }

        public static void SetFixedTimeStep(float p_TimeStep = __Default_FIXED_TIMESTEP)
        {
            Time.fixedDeltaTime = p_TimeStep;
            __FIXED_TIMESTEP_MSEC = (uint) (1000 * p_TimeStep);
        }
        
        /// <summary>
        /// 시간 가속율을 지정하는 메서드
        ///
        /// todo<415k> : 타이머에 예약된 이벤트들은 이미 큐에 타임스탬프가 예약된 상황이므로
        /// 타이머 큐 자체가 갱신되는 속도를 해당 메서드와 동기시켜 조정하는 것으로 [전체 이벤트를 배속]시킬 수 있음.
        /// </summary>
        public static void SetWorldTimeScale(float p_TimeScale = 1f)
        {
            Time.timeScale = p_TimeScale;
        }
        
        public static void SetWorldTimeScale(WorldTimeScaleType p_Type, float p_TimeScale)
        {
            _WorldTimeScaleTable[p_Type] = p_TimeScale;

            OnTimeScaleChanged();
        }
        
        public static void StopWorldTimeScale(WorldTimeScaleType p_Type)
        {
            SetWorldTimeScale(p_Type, 0f);
        }
        
        public static void ResetWorldTimeScale(WorldTimeScaleType p_Type)
        {
            SetWorldTimeScale(p_Type, 1f);
        }
        
        public static void ResetWorldTimeScale()
        {
            foreach (var timeScaleType in _Enumerator)
            {
                ResetWorldTimeScale(timeScaleType);
            }
        }

        public static void ResetTimeScale()
        {
            ResetWorldTimeScale();
            UI_TimeScale = 1f;
        }

        #endregion
    }
}