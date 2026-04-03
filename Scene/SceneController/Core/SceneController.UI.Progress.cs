#if !SERVER_DRIVE
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class SceneController<Me, Phase, Sequence, Handler, Result>
    {
        #region <Consts>

        private const string _MainLabelName = "PhaseText";

        #endregion

        #region <Fields>

        private UIxTool.SliderPreset _ProgressBar;
        private TextMeshProUGUI _Label;

        #endregion

        #region <Callbacks>

        private void OnCreateProgress()
        {
            _ProgressBar = transform.SetSliderPreset();
  
            var (labelValid, labelAffine) = transform.FindRecursive<TextMeshProUGUI>(_MainLabelName);
            if (labelValid)
            {
                _Label = labelAffine;
            }
        }

        #endregion

        #region <Methods>

        private void SetProgress(float p_Rate)
        {
            //  UnityEngine.Debug.LogError(SceneLoader.GetInstanceUnsafe._CurrentPhase);
            if (_ProgressBar.ValidFlag && _ProgressBar.CheckGetProgressValue(p_Rate))
            {
                var iRate = (int) (p_Rate * 100);
                _ProgressBar.SetProgress(p_Rate);
                _ProgressBar.SetText($"{iRate}%");
            }
        }

        protected async UniTaskVoid SetLabelText(SystemMessageTable.KeyType p_Type)
        {
            if (!ReferenceEquals(null, _Label))
            {
                var content = SystemMessageTable.GetValue(p_Type);
                _Label.text = content;
            }
        }
        
        protected async UniTaskVoid SetLabelText(string p_Text)
        {
            if (!ReferenceEquals(null, _Label))
            {
                _Label.text = p_Text;
            }
        }

        #endregion
    }
}
#endif
