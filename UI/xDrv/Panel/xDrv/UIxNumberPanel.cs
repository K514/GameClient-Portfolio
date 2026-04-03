#if !SERVER_DRIVE
using k514.Mono.Common;
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Feature
{
    public class UIxNumberPanel : UIxPanelBase
    {
        #region <Consts>

        private const int NumberPanelMaxLength = 8;
        private static readonly int UpperBound = 10.Pow(NumberPanelMaxLength) - 1;

        #endregion
        
        #region <Fields>

        private Image[] _NumberImageGroup;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            _NumberImageGroup = new Image[NumberPanelMaxLength];
            
            var wrapper = transform.FindRecursive("Align").Item2;
            for (int i = 0; i < NumberPanelMaxLength; i++)
            {
                var childOne = new GameObject($"N{i}");
                childOne.transform.SetParent(wrapper, false);
                _NumberImageGroup[i] = childOne.AddComponent<Image>();
            }

            base.OnCreate(p_CreateParams);
        }

        #endregion
        
        #region <Methods>

        public void SetDisableDamageSprite()
        {
            for (int i = 0; i < NumberPanelMaxLength; i++)
            {
                _NumberImageGroup[i].gameObject.SetActiveSafe(false);
            } 
        }

        public void SetDamageSprite(float p_Value)
        {
            var intVal = (int)Mathf.Abs(p_Value);
            if (intVal > UpperBound)
            {
                for (int i = 0; i < NumberPanelMaxLength; i++)
                {
                    _NumberImageGroup[i].gameObject.SetActiveSafe(true);
                    // _NumberImageGroup[i].sprite = UIxSpriteSheetManager.GetInstanceUnsafe.GetNumberSprite(9);
                } 
            }
            else if(intVal < 1)
            {
                _NumberImageGroup[0].gameObject.SetActiveSafe(true);
                // _NumberImageGroup[0].sprite = UIxSpriteSheetManager.GetInstanceUnsafe.GetNumberSprite(0);
                
                for (int i = 1; i < NumberPanelMaxLength; i++)
                {
                    _NumberImageGroup[i].gameObject.SetActiveSafe(false);
                } 
            }
            else
            {
                var prevPow = 1;
                for (int i = 0; i < NumberPanelMaxLength; i++)
                {
                    if (intVal > 0)
                    {
                        var baseValue = 10.Pow(i + 1);
                        var remaind = intVal % baseValue;
                        if (remaind > 0)
                        {
                            intVal -= remaind;
                            _NumberImageGroup[i].gameObject.SetActiveSafe(true);
                            if (prevPow > 1)
                            {
                                remaind /= prevPow;
                            }
                            // _NumberImageGroup[i].sprite = UIxSpriteSheetManager.GetInstanceUnsafe.GetNumberSprite(remaind);
                        }
                        else
                        {
                            _NumberImageGroup[i].gameObject.SetActiveSafe(true);
                            // _NumberImageGroup[i].sprite = UIxSpriteSheetManager.GetInstanceUnsafe.GetNumberSprite(0);
                        }

                        prevPow = baseValue;
                    }
                    else
                    {
                        _NumberImageGroup[i].gameObject.SetActiveSafe(false);
                    }
                }
            }
        }

        #endregion
    }
}
#endif