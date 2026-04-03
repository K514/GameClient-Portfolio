namespace k514
{
    /// <summary>
    /// IPool, IMultiPool 공용 인터페이스
    /// </summary>
    public interface IPoolCommon : _IDisposable
    {
        void RetrieveAll();
        void ClearPool();
#if APPLY_PRINT_LOG
        /// <summary>
        /// 현재 풀 정보를 출력하는 메서드
        /// </summary>
        void PrintPool();
#endif
    }
}