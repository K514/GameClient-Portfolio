using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class CommandCaster
    {
        #region <Fields>

        /// <summary>
        /// 비동기 작업을 취소하기 위한 토큰
        /// </summary>
        private CancellationTokenSource _CancellationTokenSource;
        
        /// <summary>
        /// 방향키가 눌린 경우, 해당 순서를 일정시간 저장하는 커맨드 큐
        /// </summary>
        private List<int> _ArrowCommandQueue;
        
        #endregion

        #region <Callbacks>

        private void OnCreateQueue()
        {
            _ArrowCommandQueue = new List<int>();
        }

        #endregion
        
        #region <Methods>

        /// <summary>
        /// 비동기 작업 취소 토큰을 파기하고 새로 발급하는 메서드
        /// </summary>
        private void ResetCancellationToken()
        {
            _CancellationTokenSource?.Cancel(false);
            _CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// 커맨드 큐에 방향 커맨드를 등록하고, 일정시간 후에 등록한 커맨드를 삭제하게 하는 메서드
        /// </summary>
        private async UniTaskVoid AddArrowCommandQueue(int p_ArrowMKey)
        {
            _ArrowCommandQueue.Add(p_ArrowMKey);
            
            if (_ArrowCommandQueue.Count > InputEventTool.CommandMaxCapacity)
            {
                _ArrowCommandQueue.RemoveAt(0);
            }
            else
            {
                await UniTask.Delay(InputEventTool.CommandExpireTime, cancellationToken: _CancellationTokenSource.Token);
                
                if (_ArrowCommandQueue.Count > 0)
                {
                    _ArrowCommandQueue.RemoveAt(0);
                }
            }
        }
    
        /// <summary>
        /// 커맨드 큐를 비우는 메서드
        /// </summary>
        private void ClearArrowCommandQueue()
        {
            _ArrowCommandQueue.Clear();
        }
        
        /// <summary>
        /// 현재 방향키 커맨드 큐에 등록된 값을 하나의 코드로 바꿔서 리턴하는 메서드
        /// 예를들어 Q[2, 3, 0, 1(new!)] =역순=> 1032 =(+1111)=> 2143을 리턴한다.
        /// </summary>
        private int GetArrowCommandCode()
        {
            var resultCode = 0;
            var currentCommandNumber = _ArrowCommandQueue.Count;
            for (var i = 0; i < currentCommandNumber; i++)
            {
                resultCode += (_ArrowCommandQueue[i] + 1) * 10.Pow(i);
            }

            return resultCode;
        }

        #endregion
    }
}