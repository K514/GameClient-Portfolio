#if !SERVER_DRIVE

using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public class UIxIndicatorJoystick : UIxFloattingJoystick
    {
        #region <Fields>

        private RectTransform _ArrowBase;
        private RectTransform _UpArrow;
        private RectTransform _RightArrow;
        private RectTransform _DownArrow;
        private RectTransform _LeftArrow;
        
        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            {
                var (valid, arrowBase) = RectTransform.FindRecursive<RectTransform>("Indicator");
                if (valid)
                {
                    _ArrowBase = arrowBase;
                    _UpArrow = _ArrowBase.GetChild(0).GetComponent<RectTransform>();
                    _RightArrow = _ArrowBase.GetChild(1).GetComponent<RectTransform>();
                    _DownArrow = _ArrowBase.GetChild(2).GetComponent<RectTransform>();
                    _LeftArrow = _ArrowBase.GetChild(3).GetComponent<RectTransform>();
                }
            }
            
            SetStateFlag(UIxTool.UIxStaticStateType.FloatPivotWhenPress);
        }

        protected override bool OnActivate(UIPoolManager.CreateParams p_CreateParams, UIPoolManager.ActivateParams p_ActivateParams, bool p_IsPooled)
        {
            if (base.OnActivate(p_CreateParams, p_ActivateParams, p_IsPooled))
            {
                SetFadeDuration(0.5f, 0f, 0.1f);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region <Methods>

        protected override void CastInputPointerDownEvent()
        {
            base.CastInputPointerDownEvent();
            
            _ArrowBase.position = _PointerDownPosition;

            _UpArrow.gameObject.SetActive(false);
            _LeftArrow.gameObject.SetActive(false);
            _DownArrow.gameObject.SetActive(false);
            _RightArrow.gameObject.SetActive(false);
        }

        protected override void CastInputPointerHoldingEvent()
        {
            base.CastInputPointerHoldingEvent();
            
            _ArrowBase.position = _PointerDownPosition;
            
            var enumerator = EnumFlag.GetEnumEnumerator<ArrowType>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var arrowType in enumerator)
            {
                switch (arrowType)
                {
                    case ArrowType.Up:
                        _UpArrow.gameObject.SetActive(_PrevArrowType.HasAnyFlagExceptNone(arrowType));
                        break;
                    case ArrowType.Left:
                        _LeftArrow.gameObject.SetActive(_PrevArrowType.HasAnyFlagExceptNone(arrowType));
                        break;
                    case ArrowType.Down:
                        _DownArrow.gameObject.SetActive(_PrevArrowType.HasAnyFlagExceptNone(arrowType));
                        break;
                    case ArrowType.Right:
                        _RightArrow.gameObject.SetActive(_PrevArrowType.HasAnyFlagExceptNone(arrowType));
                        break;
                }
            }
        }

        #endregion
    }
}

#endif