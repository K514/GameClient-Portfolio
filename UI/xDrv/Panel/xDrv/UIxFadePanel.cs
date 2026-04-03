#if !SERVER_DRIVE
using UnityEngine.UI;

namespace k514.Mono.Common
{
    /// <summary>
    /// 페이드 연출을 담당하는 전용 이미지가 추가된 판넬 컴포넌트 클래스
    /// </summary>
    public class UIxFadePanel : UIxPanelBase
    {
        #region <Consts>

        private static readonly string __FADER_NAME = "Fader";

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 페이더용 이미지
        /// </summary>
        protected Image _PanelFader;
        
        /// <summary>
        /// 페이더용 이미지 기본값
        /// </summary>
        protected UIxTool.ImagePreset _DefaultPanelPreset;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);
            
            _PanelFader = Affine.FindRecursive<Image>(__FADER_NAME).Item2;
            _DefaultPanelPreset = new UIxTool.ImagePreset(_PanelFader);
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetFadeDuration(0.3f, 0f, 0.3f);
                
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 페이드 판넬의 경우에는 투명하게 하는 것이 오히려 가리고 있던 것들을 불투명하게 해주는 효과가 있으므로
        /// 알파값에 역보간값을 적용시켜야 다른 컴포넌트와 일괄적으로 알파값 조정을 할때 불투명화 연출이 적절하게 나온다.
        ///
        /// 만약 '가려야 할' 페이드 판넬이 다른 '드러내야 할' 컴포넌트와 똑같이 보간값을 적용 받는다면
        /// 드러내야 할 컴포넌트가 오히려 가려지고, 가려져야할 컴포넌트가 오히려 드러나는 문제가 발생하기 때문이다.
        /// </summary>
        protected override void SetOpaque(float p_ProgressRate01)
        {
            _PanelFader.SetImageAlpha(_DefaultPanelPreset.DefaultColor.a * (1f - p_ProgressRate01));
        }
        
        /// <summary>
        /// 페이드 판넬의 경우에는 불투명하게 하는 것이 오히려 가리고 있던 것들을 투명하게 해주는 효과가 있으므로
        /// 알파값에 역보간값을 적용시켜야 다른 컴포넌트와 일괄적으로 알파값 조정을 할때 투명화 연출이 적절하게 나온다.
        ///
        /// 만약 '가려야 할' 페이드 판넬이 다른 '드러내야 할' 컴포넌트와 똑같이 보간값을 적용 받는다면
        /// 드러내야 할 컴포넌트가 오히려 가려지고, 가려져야할 컴포넌트가 오히려 드러나는 문제가 발생하기 때문이다.
        /// </summary>
        protected override void SetTransparent(float p_ProgressRate01)
        {
            _PanelFader.SetImageAlpha(_DefaultPanelPreset.DefaultColor.a * p_ProgressRate01);
        }
        
        #endregion
    }
}
#endif