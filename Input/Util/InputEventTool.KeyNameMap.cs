using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class InputEventTool
    {
        public static readonly Dictionary<KeyCode, string> DefaultKeyCodeNameTable
            = new Dictionary<KeyCode, string>
            {
                {KeyCode.None, string.Empty},
                {KeyCode.UpArrow, "↑"}, {KeyCode.LeftArrow, "←"}, {KeyCode.DownArrow, "↓"}, {KeyCode.RightArrow, "←"},
                
                {KeyCode.Z, "Z"}, {KeyCode.X, "X"}, {KeyCode.C, "C"}, {KeyCode.V, "V"}, {KeyCode.B, "B"}, {KeyCode.Space, "Space"},
                {KeyCode.A, "A"}, {KeyCode.S, "S"}, {KeyCode.D, "D"}, {KeyCode.F, "F"}, {KeyCode.G, "G"}, {KeyCode.H, "H"},
                {KeyCode.Q, "Q"}, {KeyCode.W, "W"}, {KeyCode.E, "E"}, {KeyCode.R, "R"}, {KeyCode.T, "T"},
                
                {KeyCode.LeftControl, "LC"}, {KeyCode.LeftShift, "LS"}, {KeyCode.LeftAlt, "LA"}, 
                {KeyCode.RightControl, "RC"}, {KeyCode.RightShift, "RS"},
                {KeyCode.Delete, "Del"}, {KeyCode.End, "End"}, {KeyCode.PageDown, "PD"},
                
                {KeyCode.Alpha0, "0"}, {KeyCode.Alpha1, "1"}, {KeyCode.Alpha2, "2"}, {KeyCode.Alpha3, "3"}, {KeyCode.Alpha4, "4"},
                {KeyCode.Alpha5, "5"}, {KeyCode.Alpha6, "6"}, {KeyCode.Alpha7, "7"}, {KeyCode.Alpha8, "8"}, {KeyCode.Alpha9, "9"},
                
                {KeyCode.F1, "F1"}, {KeyCode.F2, "F2"}, {KeyCode.F3, "F3"}, {KeyCode.F4, "F4"}, {KeyCode.F5, "F5"},
                {KeyCode.F6, "F6"}, {KeyCode.F7, "F7"}, {KeyCode.F8, "F8"}, {KeyCode.F9, "F9"}, {KeyCode.F10, "F10"},
                {KeyCode.F11, "F11"}, {KeyCode.F12, "F12"},
                
                {KeyCode.KeypadEnter, "Enter"}, {KeyCode.Escape, "Esc"}, {KeyCode.Backspace, "BS"}, {KeyCode.Print, "Print"},
                {KeyCode.Y, "Y"}, {KeyCode.U, "U"}, {KeyCode.I, "I"}, {KeyCode.K, "K"}, {KeyCode.M, "M"}, 
            };
    }
}