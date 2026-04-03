using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    /// <summary>
    /// 입력된 키를 기반으로 커맨드 이벤트를 전파하는 클래스
    /// </summary>
    public partial class CommandCaster
    {
        #region <Fields>

        /// <summary>
        /// 해당 커맨드 시스템이 다루는 입력 이벤트 타입
        /// </summary>
        private readonly InputEventTool.InputLayerType _LayerType;

        /// <summary>
        /// 해당 프레임에서 입력됬거나 현재 입력중인 입력 플래그 마스크
        ///
        /// 즉, 위 2개 플래그마스크의 합집합 플래그
        /// </summary>
        private InputEventTool.TriggerKeyType _CurrentInputFlagMask;
        
        #endregion

        #region <Constructor>

        public CommandCaster(InputEventTool.InputLayerType p_LayerType)
        {
            _LayerType = p_LayerType;
            
            OnCreateQueue();
            ClearController();
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 컨트롤러 상태를 초기화 시키는 메서드
        /// </summary>
        private void ClearController()
        {
            var dt = Time.deltaTime;
            OnArrowKeyEventReleased(dt);
            ClearInputMask(InputEventTool.InputKeyType.Whole);
            ClearArrowCommandQueue();
            ResetCancellationToken();
        }

        /// <summary>
        /// 현재 프레임에서 발생한 입력을 갱신하기 전에 입력 마스크를 초기화 시키는 콜백
        /// </summary>
        public void ClearInputMask(InputEventTool.InputKeyType p_InputtedKeyTypeMask)
        {
            // 입력 플래그 마스크를 초기화 해준다.
            _CurrentInputFlagMask = InputEventTool.TriggerKeyType.None;

            // 커맨드 이벤트 안에는 현재 방향키가 어떤식으로 입력되고 있는지에 대한 정보도 필요하므로
            // 방향키 이외의 입력에 대해 방향키 코드를 추가해준다.
            if (p_InputtedKeyTypeMask.HasAnyFlagExceptNone(InputEventTool.InputKeyType.ArrowKey))
            {
                _CurrentArrowSequenceCode = 0;
            }
            else
            {
                _CurrentArrowSequenceCode = GetArrowCommandCode();
            }
        }

#if APPLY_PRINT_LOG
        /// <summary>
        /// 현재 커맨드 큐에 등록되어 있는 값을 콘솔에 출력하는 메서드
        /// </summary>
        private void OpenCommand()
        {
            CustomDebug.Log($"*********************************");
            var command = "[ ";
            var onceFlag = false;
            foreach (var VARIABLE in _ArrowCommandQueue)
            {
                if (onceFlag)
                {
                    command += ($", {VARIABLE}");
                }
                else
                {
                    onceFlag = true;
                    command += ($"{VARIABLE}");
                }
            }

            command += (" ]");
            CustomDebug.Log(command);
            CustomDebug.Log($"*********************************");
        }
#endif

        #endregion
    }
}