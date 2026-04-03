#if !SERVER_DRIVE
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxContainerBase
    {
        #region <Callbacks>

        protected virtual void OnElementAdded(UIxElementBase p_Element)
        {
        }

        #endregion
        
        #region <Methods>

        public void AddElement(UIxElementBase p_Element)
        {
            if (!_Elements.Contains(p_Element))
            {
                _Elements.Add(p_Element);
                p_Element.SetContainer(this);
                OnElementAdded(p_Element);
            }
        }
        
        public M AddElement<M>(Component p_Component) where M : UIxElementBase
        {
            var spawned = p_Component.gameObject.AddComponent<M>();
            if (spawned != null)
            {
                spawned.CheckAwake();
                AddElement(spawned);

                return spawned;
            }
            else
            {
                return null;
            }
        }
   
        public M AddElement<M>(Transform p_Root, string p_Name) where M : UIxElementBase
        {
            var (valid, affine) = p_Root?.FindRecursive(p_Name) ?? default;
            if (valid)
            {
                return AddElement<M>(affine);
            }
            else
            {
                return null;
            }
        }   
        
        public M AddElement<M>(string p_Name) where M : UIxElementBase
        {
            return AddElement<M>(Affine, p_Name);
        }
        
        public M AddElement<M>(Component p_Component, UIxTool.UIInputEventParams p_EventParams) where M : UIxElementBase
        {
            var spawned = p_Component.gameObject.AddComponent<M>();
            if (spawned != null)
            {
                spawned.SetInputEvent(p_EventParams);
                spawned.CheckAwake();
                AddElement(spawned);

                return spawned;
            }
            else
            {
                return null;
            }
        }

        public M AddElement<M>(Transform p_Root, string p_Name, UIxTool.UIInputEventParams p_EventParams) where M : UIxElementBase
        {
            var (valid, affine) = p_Root?.FindRecursive(p_Name) ?? default;
            if (valid)
            {
                return AddElement<M>(affine, p_EventParams);
            }
            else
            {
                return null;
            }
        }

        public M AddElement<M>(string p_Name, UIxTool.UIInputEventParams p_EventParams) where M : UIxElementBase
        {
            return AddElement<M>(Affine, p_Name, p_EventParams);
        }
        
        public M AddElement<M>(Component p_Component, UIxTool.UITouchEventParams p_EventParams) where M : UIxElementBase
        {
            var spawned = p_Component.gameObject.AddComponent<M>();
            if (spawned != null)
            {
                spawned.SetTouchEvent(p_EventParams);
                spawned.CheckAwake();
                AddElement(spawned);

                return spawned;
            }
            else
            {
                return null;
            }
        }
        
        public M AddElement<M>(Transform p_Root, string p_Name, UIxTool.UITouchEventParams p_EventParams) where M : UIxElementBase
        {
            var (valid, affine) = p_Root?.FindRecursive(p_Name) ?? default;
            if (valid)
            {
                return AddElement<M>(affine, p_EventParams);
            }
            else
            {
                return null;
            }
        }
        
        public M AddElement<M>(string p_Name, UIxTool.UITouchEventParams p_EventParams) where M : UIxElementBase
        {
            return AddElement<M>(Affine, p_Name, p_EventParams);
        }

        public void RemoveElement(UIxElementBase p_Element)
        {
            if (_Elements.Contains(p_Element))
            {
                _Elements.Remove(p_Element);
                p_Element.ResetContainer();
            }
        }
        
        public void RemoveAllElements(bool p_RetrieveFlag)
        {
            if (p_RetrieveFlag)
            {
                for (var i = _Elements.Count - 1; i > -1; i--)
                {
                    var tryElement = _Elements[i];
                    _Elements.Remove(tryElement);
                    tryElement.LateEventMask.AddFlag(UIxTool.UIxLateEventType.TurnFadeOut | UIxTool.UIxLateEventType.Retrieve);
                }
            }
            else
            {
                for (var i = _Elements.Count - 1; i > -1; i--)
                {
                    RemoveElement(_Elements[i]);
                }   
            }
        }

        #endregion
    }
}
#endif