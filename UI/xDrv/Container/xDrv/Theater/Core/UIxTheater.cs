#if !SERVER_DRIVE

using k514.Mono.Common;
using UnityEngine;

namespace k514.Mono.Feature
{
    public abstract class UIxTheater<TheaterElement> : UIxContainerBase where TheaterElement : UIxPanelBase
    {
        #region <Const>

        private const int __PreloadCounter = 64;

        #endregion

        #region <Fields>

        protected UIxTool.UIxElementType _SpawnKey;
        private UIPoolManager.CreateParams _CreateParams;
        
        #endregion

        #region <Callbacks>

        protected override void OnCreate(UIPoolManager.CreateParams p_CreateParams)
        {
            base.OnCreate(p_CreateParams);

            _CreateParams = 
                UIPoolManager.GetInstanceUnsafe
                    .GetCreateParams
                    (
                        _SpawnKey, ResourceLifeCycleType.ManualUnload, __PreloadCounter
                    );
        }
        
        protected override void OnDispose()
        {
            base.OnDispose();
            
            UIPoolManager.GetInstanceUnsafe?.RemovePool(_CreateParams);
            _CreateParams = default;
        }
        
        #endregion
        
        #region <Methods>
        
        protected TheaterElement GetTheaterElement(IGameEntityBridge p_Entity)
        {
            var spawned = UIPoolManager.GetInstanceUnsafe.Pop<TheaterElement>(_CreateParams, new UIPoolManager.ActivateParams(CanvasPreset, RectTransform, p_Entity));
            AddElement(spawned);

            return spawned;
        }
        
        protected TheaterElement GetTheaterElement(Vector3 p_PivotPosition)
        {
            var spawned = UIPoolManager.GetInstanceUnsafe.Pop<TheaterElement>(_CreateParams, new UIPoolManager.ActivateParams(CanvasPreset, RectTransform, p_PivotPosition));
            AddElement(spawned);

            return spawned;
        }
        
        #endregion
    }
}
#endif