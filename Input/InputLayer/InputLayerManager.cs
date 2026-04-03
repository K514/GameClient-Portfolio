using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    /// <summary>
    /// 입력 이벤트를 수신받고 해당 이벤트에 적합한 입력 레이어에 이벤트를 전파하는 가능을 수행하는 매니저 클래스
    /// </summary>
    public class InputLayerManager : SceneChangeEventReceiveAsyncSingleton<InputLayerManager>
    {
        #region <Fields>

        /// <summary>
        /// [입력 레이어 타입, 입력 레이어] 테이블
        /// </summary>
        private Dictionary<InputEventTool.InputLayerType, InputLayer> _InputLayerTable;

        /// <summary>
        /// 입력 이벤트 수신자
        /// </summary>
        private InputLayerEventReceiver _InputLayerEventReceiver;

        #endregion

        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);
            
            _InputLayerTable = new Dictionary<InputEventTool.InputLayerType, InputLayer>();
            _InputLayerEventReceiver = new InputLayerEventReceiver(InputEventTool.InputLayerType.WholeMask, OnHandleEvent);

            var enumerator = EnumFlag.GetEnumEnumerator<InputEventTool.InputLayerType>(EnumFlag.GetEnumeratorType.ExceptMaskNone);
            foreach (var layerType in enumerator)
            {
                _InputLayerTable.Add(layerType, new InputLayer(layerType));
            }
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {    
                inputEventPreset.Value.OnInitiate();
            }

            await UniTask.CompletedTask;
        }

        protected override void OnDisposeSingleton()
        {
            _InputLayerEventReceiver?.Dispose();
            _InputLayerEventReceiver = null;
            
            base.OnDisposeSingleton();
        }
        
        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {
                inputEventPreset.Value.OnScenePreload();
            }
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {
                inputEventPreset.Value.OnSceneStarted();
            }
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {
                inputEventPreset.Value.OnSceneTerminated();
            }
            
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {
                inputEventPreset.Value.OnSceneTransition();
            }
            
            await UniTask.CompletedTask;
        }

        private void OnHandleEvent(InputEventTool.InputLayerType p_LayerType, InputLayerEventParams p_Params)
        {
            _InputLayerTable[p_LayerType].OnHandleEvent(p_Params);
        }

        public void OnUpdate(float p_DeltaTime)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {
                inputEventPreset.Value.OnUpdate(p_DeltaTime);
            }
        }
        
        public void OnUpdateFunctionKeyOnly(float p_DeltaTime)
        {
            foreach (var inputEventPreset in _InputLayerTable)
            {
                inputEventPreset.Value.OnUpdateFunctionKeyOnly(p_DeltaTime);
            }
        }

        #endregion

        #region <Methods>

        public void SetLayerBlock(InputEventTool.InputLayerType p_Type, bool p_Flag)
        {
            _InputLayerTable[p_Type].SetBlock(p_Flag);
        }

        #endregion
    }
}