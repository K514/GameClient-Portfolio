using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class PatchScene
    {
        public Slider _Slider;
        public TextMeshProUGUI _ProgressLabel;
        private StringBuilder _SB = new StringBuilder(20);


        public void SetProgressValue(float p_CurrentValue, float p_MaxValue)
        {
            SetProgressValue( p_CurrentValue / p_MaxValue);
        }

        public void SetProgressValue(float p_Value)
        {
            _Slider.value = p_Value;
        }

        public void SetProgressText(string p_CurrentText, string p_TotalText)
        {
            _SB.Clear();
            _SB.Append(p_CurrentText);
            _SB.Append("/");
            _SB.Append(p_TotalText);

            _ProgressLabel.text = _SB.ToString();
        }
    }
}
