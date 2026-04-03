namespace k514
{
    /// <summary>
    /// 싱글톤 공통 인터페이스
    /// </summary>
    public interface ISingleton : _IDisposable
    {
    }
    
    /// <summary>
    /// 유니티 싱글톤 공통 인터페이스
    /// </summary>
    public interface IUnitySingleton : ISingleton
    {
    }
}