namespace k514.Mono.Common
{
    /// <summary>
    /// 카메라가 포커싱할 수 있는 오브젝트를 기술하는 인터페이스
    /// </summary>
    public interface ICameraFocusable : ICameraFocusEventReceiver, IPhysicsVolume, IAffineControl, IAffineTracker
    {
    }
}