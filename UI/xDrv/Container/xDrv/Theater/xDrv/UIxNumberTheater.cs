using DamageNumbersPro;
using k514.Mono.Common;
using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Feature
{
    public class UIxNumberTheater : UIxTheater<UIxDamagePanel>
    {
        #region <Enums>

        public enum NumberEventType
        {
            None,
            Invincible,
            Critical,
        }

        #endregion
        
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            _SpawnKey = UIxTool.UIxElementType.NumberPanel;
            base.OnCreate(p_CreateParams);
        }

        #endregion

        #region <Methods>

        public void PopTheaterElement(Vector3 p_Position, float p_Radius, Color p_Color, string p_Content, NumberEventType p_Type)
        {
            var randomizePosition = p_Position.GetRandomPosition(XYZType.ZX, 0f, p_Radius);
            var spawned = GetTheaterElement(randomizePosition);
            var headString = p_Type switch
            {
                NumberEventType.None => string.Empty,
                NumberEventType.Invincible => "Miss",
                NumberEventType.Critical => string.Empty,
            };
            spawned.SetMainTextColor(p_Color);
            spawned.SetMainText(p_Content, headString);
            spawned.SetMainTextSize(p_Type == NumberEventType.Critical ? 68 : 48);
            spawned.SetFadeDuration(1f,1f, 1f);
            spawned.SetFadeInOut(UIxTool.UIAfterFadeType.Retrieve, true);
            spawned.UpdateText();
        }

        
        public void PopTheaterElement(Vector3 p_Position, float p_Radius, Color p_Color, float p_Value, NumberEventType p_Type)
        {
            PopTheaterElement(p_Position, p_Radius, p_Color, ((int) Mathf.Abs(p_Value)).ToString(), p_Type);
        }
        #endregion
    }
}
#endif