namespace k514.Mono.Common
{
    /// <summary>
    /// 게임 엔터티 초기화 파라미터 인터페이스
    /// </summary>
    public interface IGameEntityActivateParams : IWorldObjectActivateParams
    {
        /// <summary>
        /// 초기화시 부여할 개체 속성
        /// </summary>
        public GameEntityTool.ActivateParamsAttributeType GameEntityActivateParamsAttributeMask { get; }

        /// <summary>
        /// 해당 개체를 생성한 개체
        /// </summary>
        public IGameEntityBridge Spawner { get; }
        
        /// <summary>
        /// 개체 초기화와 동시에 예약할 배치 이벤트 프리셋
        /// </summary>
        public InstanceEventTool.InstanceEventPreset ReservedEventPreset { get; }

        /// <summary>
        /// 오브젝트에 불일 이름
        /// </summary>
        public string Alias { get; }
    }
}