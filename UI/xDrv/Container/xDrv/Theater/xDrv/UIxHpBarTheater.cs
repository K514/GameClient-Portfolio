using k514.Mono.Common;
using UnityEngine;

#if !SERVER_DRIVE

namespace k514.Mono.Feature
{
    public class UIxHpBarTheater : UIxTheater<UIxHpBarPanel>
    {
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            _SpawnKey = UIxTool.UIxElementType.HpBarPanel;
            
            base.OnCreate(p_CreateParams);
        }

        #endregion

        #region <Methods>

        public void PopTheaterElement(IGameEntityBridge p_Entity)
        {
            var spawned = GetTheaterElement(p_Entity);
            spawned.SetFadeIn(UIxTool.UIAfterFadeType.None, true);
        }

        #endregion
    }
}
#endif