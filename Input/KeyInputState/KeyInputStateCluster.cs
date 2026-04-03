namespace k514.Mono.Common
{
    /// <summary>
    /// 시스템에서 허용하는 키의 종류만큼, KeyInputState를 생성하여 보유하는 클래스
    /// </summary>
    public class KeyInputStateCluster
    {
        #region <Fields>

        private readonly KeyInputState[] _KeyInputStateTable;
        
        #endregion

        #region <Constructor>

        public KeyInputStateCluster()
        {
            var inputEventKeyCodeSet = KeyCodeTable.IKeyCodeSet;
            var inputEventKeyCodeScale = KeyCodeTable.IKeyCodeMax;
            
            _KeyInputStateTable = new KeyInputState[inputEventKeyCodeScale];
            foreach (var iKeyCode in inputEventKeyCodeSet)
            {
                _KeyInputStateTable[iKeyCode] = new KeyInputState(iKeyCode);
            }
        }

        #endregion

        #region <Callbacks>

        /// <summary>
        /// 어떤 키에 대해 입력이 발생하였을때, 해당 이벤트 정보를 그 키와 매핑된 KeyInputState에 전파하는 콜백
        /// </summary>
        public KeyInputState OnUpdateInput(InputLayerEventParams p_Params)
        {
            var targetKeyCodeState = _KeyInputStateTable[p_Params.IKeyCode];
            targetKeyCodeState.OnUpdateKeyState(p_Params);

            return targetKeyCodeState;
        }

        #endregion

        #region <Methods>

        public void UpdateMapping()
        {
            var inputEventKeyCodeSet = KeyCodeTable.IKeyCodeSet;
            foreach (var iKeyCode in inputEventKeyCodeSet)
            {
                _KeyInputStateTable[iKeyCode].UpdateMapping();
            }
        }

        #endregion
    }
}