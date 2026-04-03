namespace k514.Mono.Common
{
    public partial class InputEventTool
    {
        /// <summary>
        /// 입력 커맨드 기본 우선도
        /// </summary>
        public const int __INPUT_COMMAND_PRIORITY_DEFAULT = 100;
        
        /// <summary>
        /// UI 입력 스택 갱신 제한시간 상한
        /// </summary>
        public const float UIInputStackUpdateIntervalUpperBound = 0.5f;
        
        /// <summary>
        /// 키보드 입력 스택 갱신 제한시간 상한
        /// </summary>
        public const float KeyboardInputStackUpdateIntervalUpperBound = 0.3f;
        
        /// <summary>
        ///  방향키는 0 ~ 3의 인덱스를 지님
        /// </summary>
        public const int ARROW_MKEYCODE_UPPERBOUND = 4;

        /// <summary>
        /// 액션 키는 4 ~ 31 인덱스를 지님
        /// </summary>
        public const int TRIGGER_MKEYCODE_UPPERBOUND = 32;
        
        /// <summary>
        /// 방향키코드를 필터링하는 마스크
        /// </summary>
        public const int ArrowKeyCodeFilterMask = 0b1111;

        /// <summary>
        /// 액션키코드를 필터링하는 마스크
        /// </summary>
        public const int TriggerKeyCodeFilterMask = int.MaxValue ^ ArrowKeyCodeFilterMask;
    }
}