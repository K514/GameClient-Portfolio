using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514
{
    /// <summary>
    /// [입력 버튼, 인풋 식별자] 컬렉션
    /// </summary>
    public class KeyCodeTable : SystemValueIndexTable<KeyCodeTable, KeyCode, int>
    {
        #region <Consts>
        
        /// <summary>
        /// 기본 키 테이블
        ///
        /// [KeyCode, MIKeyCode]
        /// 
        /// 해당 시스템에서 사용할 유니티 키와 매핑된 값을 가진다.
        /// </summary>
        private Dictionary<KeyCode, int> _DefaultMIKeyCodeTable
            = new Dictionary<KeyCode, int>
            {
                {KeyCode.None, -1},
                {KeyCode.UpArrow, 0}, {KeyCode.LeftArrow, 1}, {KeyCode.DownArrow, 2}, {KeyCode.RightArrow, 3},
                
                {KeyCode.Z, 4}, {KeyCode.X, 5}, {KeyCode.C, 6}, {KeyCode.V, 7}, {KeyCode.B, 8}, {KeyCode.Space, 9},
                {KeyCode.A, 10}, {KeyCode.S, 11}, {KeyCode.D, 12}, {KeyCode.F, 13}, {KeyCode.G, 14}, {KeyCode.H, 15},
                {KeyCode.Q, 16}, {KeyCode.W, 17}, {KeyCode.E, 18}, {KeyCode.R, 19}, {KeyCode.T, 20},
                
                {KeyCode.LeftControl, 21}, {KeyCode.LeftShift, 22}, {KeyCode.LeftAlt, 23}, 
                {KeyCode.RightControl, 24}, {KeyCode.RightShift, 25},
                {KeyCode.Delete, 26}, {KeyCode.End, 27}, {KeyCode.PageDown, 28},
                
                {KeyCode.Alpha0, 40}, {KeyCode.Alpha1, 41}, {KeyCode.Alpha2, 42}, {KeyCode.Alpha3, 43}, {KeyCode.Alpha4, 44},
                {KeyCode.Alpha5, 45}, {KeyCode.Alpha6, 46}, {KeyCode.Alpha7, 47}, {KeyCode.Alpha8, 48}, {KeyCode.Alpha9, 49},
                
                {KeyCode.F1, 51}, {KeyCode.F2, 52}, {KeyCode.F3, 53}, {KeyCode.F4, 54}, {KeyCode.F5, 55},
                {KeyCode.F6, 56}, {KeyCode.F7, 57}, {KeyCode.F8, 58}, {KeyCode.F9, 69}, {KeyCode.F10, 60},
                {KeyCode.F11, 61}, {KeyCode.F12, 62},
                
                {KeyCode.KeypadEnter, 70}, {KeyCode.Escape, 71}, {KeyCode.Backspace, 72}, {KeyCode.Print, 73},
                {KeyCode.Y, 74}, {KeyCode.U, 75}, {KeyCode.I, 76}, {KeyCode.K, 77}, {KeyCode.M, 78}, 
            };
        
        #endregion
        
        #region <Fields>

        /// <summary>
        /// 시스템에서 사용할 유니티 키코드 셋
        /// </summary>
        public static KeyCode[] KeyCodeSet;

        /// <summary>
        /// 시스템에서 사용할 유니티 엔진 키코드 정수 셋
        /// </summary>
        public static int[] IKeyCodeSet;

        /// <summary>
        /// 시스템에서 사용할 유니티 엔진 키코드 최대값
        /// </summary>
        public static int IKeyCodeMax;

        /// <summary>
        /// 기본 키 테이블의 역 테이블
        ///
        /// [MKeyCode, KeyCode]
        /// </summary>
        public static Dictionary<int, KeyCode> InvMIKeyCodeTable;

        #endregion
        
        #region <Callbacks>
        
        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await base.OnInitiate(p_CancellationToken);
            
            var tryTable = GetTable();
            var tableCount = tryTable.Count;
            if (tableCount > 0)
            {
                KeyCodeSet = tryTable.Keys.ToArray();
                IKeyCodeSet = KeyCodeSet.Select(keycode => (int)keycode).ToArray();
                IKeyCodeMax = (int) KeyCodeSet.Max() + 1;
                InvMIKeyCodeTable = tryTable.ToDictionary(kv => kv.Value.Value, kv => kv.Key);
            }
        }

        #endregion

        #region <Methods>
        
        protected override async UniTask AddDefaultRecords(CancellationToken p_CancellationToken)
        {
            await base.AddDefaultRecords(p_CancellationToken);

            foreach (var defaultKeyValue in _DefaultMIKeyCodeTable)
            {
                await AddRecord(defaultKeyValue.Key, false, p_CancellationToken, defaultKeyValue.Value);
            }
        }

        #endregion
    }
}