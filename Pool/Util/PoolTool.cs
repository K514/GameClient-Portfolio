using System.Threading;
using Cysharp.Threading.Tasks;
using xk514;

namespace k514
{
    public static class PoolTool
    {
        #region <Enums>
            
        /// <summary>
        /// 풀 내부데이터 Content 상태를 기술하는 열거형 상수
        /// </summary>
        public enum ContentState
        {
            None,
        
            /// <summary>
            /// 생성됬지만 초기화되기 이전의 상태
            /// </summary>
            Created,
        
            /// <summary>
            /// 초기화된 이후 활성화된 상태
            /// </summary>
            Active, 
        
            /// <summary>
            /// 비활성화되어 풀에 격납된 상태
            /// </summary>
            Pooled,
        
            /// <summary>
            /// 활성화 상태에서 비활성화 상태로 회수 작업중인 상태
            /// </summary>
            Retrieving,
        
            /// <summary>
            /// 파기된 상태
            /// </summary>
            Disposed,
        }

        #endregion
        
        #region <Methods>

        public static bool IsContentValid(this IContent p_This)
        {
            return p_This is { ContentState: ContentState.Active };
        }

        /// <summary>
        /// 해당 Content의 버전 스냅샷을 리턴하는 메서드
        /// </summary>
        public static ContentValidation GetContentValidation(this IContent p_This)
        {
            return p_This.IsContentValid() ? new ContentValidation(p_This) : default;
        }
        
#if APPLY_PRINT_LOG
        public static void PrintPoolInfo()
        {
            var multiPoolSubTypeSet = typeof(IMultiPool<>).GetSubTypeSet();
            foreach (var multiPoolSubType in multiPoolSubTypeSet)
            {
                var multiPool = SingletonTool.GetSingletonUnsafe<IPoolCommon>(multiPoolSubType);
                if (!ReferenceEquals(null, multiPool))
                {
                    CustomDebug.LogError($"[MultiPool : {multiPoolSubType}]");
                    multiPool.PrintPool();
                }
            }
        }
#endif

        #endregion

        #region <Structs>

        public struct ContentValidation
        {
            #region <Fields>

            public readonly IContent Content;
            public readonly int VersionSnapshot;
            public readonly bool ValidFlag;

            #endregion

            #region <Constructor>

            public ContentValidation(IContent p_Entity)
            {
                ValidFlag = !ReferenceEquals(null, p_Entity);
                Content = ValidFlag ? p_Entity : null;
                VersionSnapshot = ValidFlag ? Content.GetPooledCount() : 0;
            }

            #endregion

            #region <Methods>
   
            public bool IsValid()
            {
                return ValidFlag 
                       && Content.IsContentValid()
                       && VersionSnapshot == Content.GetPooledCount();
            }

            #endregion
        }

        #endregion
    }
}