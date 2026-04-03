using k514.Mono.Common;

#if !SERVER_DRIVE

namespace k514.Mono.Feature
{
    public class UIxGaugeTheater : UIxTheater<UIxGaugePanel>
    {
        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            _SpawnKey = UIxTool.UIxElementType.GaugePanel;

            base.OnCreate(p_CreateParams);
        }

        #endregion

        #region <Methods>

        public UIxGaugePanel PopTheaterElement(IGameEntityBridge p_Entity, float p_InitialRate)
        {
            var spawned = GetTheaterElement(p_Entity);
            spawned.SetFadeIn(UIxTool.UIAfterFadeType.None, true);
            spawned.SetGaugeBar(p_InitialRate);
            
            return spawned;
        }

        #endregion
    }
}
#endif