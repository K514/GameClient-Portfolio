using System;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    /// <summary>
    /// 어떤 입력이 발생했을때, 해당 입력은 하나지만 그 입력을 인식하는 이벤트는
    /// 유닛을 컨트롤하는 입력일 수도 있고 UI를 조작하는 입력일 수도 있다.
    ///
    /// 즉 입력과 이벤트는 1:N의 관계를 가진다.
    /// 
    /// 어떤 입력이 어떤 이벤트에 대한 것인지에 대한 정보를 레이어 라고 하여,
    /// 해당 클래스는 그 레이어를 기술한다.
    /// </summary>
    public class InputLayer
    {
        #region <Fields>

        /// <summary>
        /// 이벤트 레이어 타입
        /// </summary>
        private readonly InputEventTool.InputLayerType _LayerType;
        
        /// <summary>
        /// 키 입력 상태 클러스터
        /// </summary>
        private readonly KeyInputStateCluster _KeyInputStateCluster;
        
        /// <summary>
        /// 커맨드 캐스터
        /// </summary>
        public readonly CommandCaster CommandCaster;
        
        /// <summary>
        /// 해당 프레임에서 입력된 키 타입 플래그 마스크
        /// </summary>
        private InputEventTool.InputKeyType _KeyTypeDirtyMask;
        
        /// <summary>
        /// 해당 프레임에서 입력된 커맨드 타입 플래그 마스크
        /// </summary>
        private InputEventTool.TriggerKeyType _CommandCodeDirtyMask;
        
        /// <summary>
        /// 입력된 ArrowKey들의 KeyInputState 버퍼
        /// </summary>
        private readonly List<KeyInputState> _ArrowKeyInputStateDirtyBuffer;
        
        /// <summary>
        /// 입력된 TriggerKey들의 KeyInputState 버퍼
        /// </summary>
        private readonly List<KeyInputState> _TriggerKeyInputStateDirtyBuffer;
        
        /// <summary>
        /// 입력된 FunctionKey들의 KeyInputState 버퍼
        /// </summary>
        private readonly List<KeyInputState> _FunctionKeyInputStateDirtyBuffer;

        /// <summary>
        /// 블록 상태에서 입력이 발생했을 때, 저장하기 위한 버퍼
        /// </summary>
        private readonly Dictionary<KeyCode, InputLayerEventParams> _BlockEventBuffer;

        /// <summary>
        /// 입력을 블록하는 플래그
        /// </summary>
        private bool _BlockFlag;
        
        #endregion

        #region <Constructors>

        public InputLayer(InputEventTool.InputLayerType p_LayerType)
        {
            _LayerType = p_LayerType;
            _KeyInputStateCluster = new KeyInputStateCluster();
            CommandCaster = new CommandCaster(_LayerType);
            
            _KeyTypeDirtyMask = InputEventTool.InputKeyType.None;
            _CommandCodeDirtyMask = InputEventTool.TriggerKeyType.None;
            
            _ArrowKeyInputStateDirtyBuffer = new List<KeyInputState>();
            _TriggerKeyInputStateDirtyBuffer = new List<KeyInputState>();
            _FunctionKeyInputStateDirtyBuffer = new List<KeyInputState>();
            _BlockEventBuffer = new Dictionary<KeyCode, InputLayerEventParams>();
         
            UpdateMapping();
        }

        #endregion

        #region <Callbacks>

        public void OnInitiate()
        {
        }

        public void OnScenePreload()
        {
        }

        public void OnSceneStarted()
        {
        }

        public void OnSceneTerminated()
        {
        }

        public void OnSceneTransition()
        {
        }

        public void OnHandleEvent(InputLayerEventParams p_Params)
        {
            if (_BlockFlag)
            {
                var keyCode = p_Params.KeyCode;
                var inputState = p_Params.InputStateType;
                switch (inputState)
                {
                    case InputEventTool.InputStateType.Holding:
                    case InputEventTool.InputStateType.Release:
                    {
                        if (_BlockEventBuffer.TryGetValue(keyCode, out var o_PrevParams))
                        {
                            var prevInputState = o_PrevParams.InputStateType;
                            switch (prevInputState)
                            {
                                case InputEventTool.InputStateType.Press:
                                case InputEventTool.InputStateType.Release:
                                    break;
                                case InputEventTool.InputStateType.Holding:
                                    _BlockEventBuffer[keyCode] = p_Params;
                                    break;
                            }
                        }
                        else
                        {
                            _BlockEventBuffer.Add(keyCode, p_Params);
                        }
                        break;
                    }
                }
            }
            else
            {
                var targetKeyInputState = _KeyInputStateCluster.OnUpdateInput(p_Params);
                var keyType = targetKeyInputState.KeyType;
                var commandType = targetKeyInputState.TriggerKeyType;
            
                if (!_CommandCodeDirtyMask.HasAnyFlagExceptNone(commandType))
                {
                    _CommandCodeDirtyMask.AddFlag(commandType);

                    switch (keyType)
                    {
                        case InputEventTool.InputKeyType.ArrowKey:
                            _ArrowKeyInputStateDirtyBuffer.Add(targetKeyInputState);
                            _KeyTypeDirtyMask.AddFlag(InputEventTool.InputKeyType.ArrowKey);
                            break;
                        case InputEventTool.InputKeyType.TriggerKey:
                            _TriggerKeyInputStateDirtyBuffer.Add(targetKeyInputState);
                            _KeyTypeDirtyMask.AddFlag(InputEventTool.InputKeyType.TriggerKey);
                            break;
                        case InputEventTool.InputKeyType.FunctionKey:
                            _FunctionKeyInputStateDirtyBuffer.Add(targetKeyInputState);
                            _KeyTypeDirtyMask.AddFlag(InputEventTool.InputKeyType.FunctionKey);
                            break;
                    }
                }
            }
        }
            
        public void OnUpdate(float p_DeltaTime)
        {
            var hasArrowDirty = _KeyTypeDirtyMask.HasAnyFlagExceptNone(InputEventTool.InputKeyType.ArrowKey);
            var hasActionDirty = _KeyTypeDirtyMask.HasAnyFlagExceptNone(InputEventTool.InputKeyType.TriggerKey);
            var hasFunctionDirty = _KeyTypeDirtyMask.HasAnyFlagExceptNone(InputEventTool.InputKeyType.FunctionKey);
            var hasAnyDirty = _KeyTypeDirtyMask != InputEventTool.InputKeyType.None;

            if (hasAnyDirty)
            {
                CommandCaster.ClearInputMask(_KeyTypeDirtyMask);
            }
            if (hasArrowDirty)
            {
                CommandCaster.CastArrowKeyCommandEvent(_ArrowKeyInputStateDirtyBuffer, p_DeltaTime);
            }
            if (hasActionDirty)
            {
                CommandCaster.CastTriggerKeyCommandEvent(_TriggerKeyInputStateDirtyBuffer, p_DeltaTime);
            }
            if (hasFunctionDirty)
            {
                CommandCaster.CastFunctionKeyCommandEvent(_FunctionKeyInputStateDirtyBuffer, p_DeltaTime);
            }
            if (hasArrowDirty)
            {
                _ArrowKeyInputStateDirtyBuffer.Clear();
            }
            if (hasActionDirty)
            {
                _TriggerKeyInputStateDirtyBuffer.Clear();
            }
            if (hasFunctionDirty)
            {
                _FunctionKeyInputStateDirtyBuffer.Clear();
            }

            _KeyTypeDirtyMask = InputEventTool.InputKeyType.None;
            _CommandCodeDirtyMask = InputEventTool.TriggerKeyType.None;
        }
        
        public void OnUpdateFunctionKeyOnly(float p_DeltaTime)
        {
            if (_KeyTypeDirtyMask.HasAnyFlagExceptNone(InputEventTool.InputKeyType.FunctionKey))
            {
                CommandCaster.ClearInputMask(_KeyTypeDirtyMask);
                CommandCaster.CastFunctionKeyCommandEvent(_FunctionKeyInputStateDirtyBuffer, p_DeltaTime);
            }

            _ArrowKeyInputStateDirtyBuffer.Clear();
            _TriggerKeyInputStateDirtyBuffer.Clear();
            _FunctionKeyInputStateDirtyBuffer.Clear();
            _KeyTypeDirtyMask = InputEventTool.InputKeyType.None;
            _CommandCodeDirtyMask = InputEventTool.TriggerKeyType.None;
        }

        #endregion

        #region <Methods>

        public void UpdateMapping()
        {
            _KeyInputStateCluster.UpdateMapping();
        }

        public void SetBlock(bool p_Flag)
        {
            _BlockFlag = p_Flag;

            if (!_BlockFlag)
            {
                foreach (var paramsKV in _BlockEventBuffer)
                {
                    OnHandleEvent(paramsKV.Value);
                }
                _BlockEventBuffer.Clear();
            }
        }

        #endregion
    }
}