#if !SERVER_DRIVE

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        #region <Classes>

        public class UIxRootPreset : _IDisposable
        {
            ~UIxRootPreset()
            {
                Dispose();
            }
            
            #region <Fields>

            public RenderMode RenderMode { get; private set; }
            public RectTransform RootRect { get; private set; }
            public Dictionary<int, UIxCanvasPreset> UIxCanvasCollection { get; private set; }

            #endregion

            #region <Indexer>

            public UIxCanvasPreset this[int p_LayerIndex]
            {
                get => UIxCanvasCollection[p_LayerIndex];
            }

            #endregion        
            
            #region <Constructors>

            public UIxRootPreset(RenderMode p_RenderMode, RectTransform p_RootRect)
            {
                RenderMode = p_RenderMode;
                RootRect = p_RootRect;
                UIxCanvasCollection = new Dictionary<int, UIxCanvasPreset>();
            }

            #endregion

            #region <Callbacks>

            public void OnCanvasReleased(UIxCanvasPreset p_CanvasPreset)
            {
                if (!ReferenceEquals(null, UIxCanvasCollection))
                {
                    var tryLayerIndex = p_CanvasPreset.LayerIndex;
                    UIxCanvasCollection.Remove(tryLayerIndex);
                }
            }

            /// <summary>
            /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
            /// </summary>
            private void OnDisposeUnmanaged()
            {
                if (!ReferenceEquals(null, UIxCanvasCollection))
                {
                    var enumerator = UIxCanvasCollection.Keys.ToList();
                    foreach (var layerIndex in enumerator)
                    {
                        var uiCanvasPreset = UIxCanvasCollection[layerIndex];
                        uiCanvasPreset.Dispose();
                    }
                    
                    UIxCanvasCollection.Clear();
                    UIxCanvasCollection = default;
                }
                
                var tryObjectRoot = UIxObjectRoot.GetInstanceUnsafe;
                if (!ReferenceEquals(null, tryObjectRoot))
                {
                    tryObjectRoot.OnRootReleased(this);
                }
                
                Object.Destroy(RootRect.gameObject);
                RootRect = default;
                RenderMode = default;
            }

            #endregion

            #region <Methods>

            public UIxCanvasPreset AddCanvas(int p_LayerIndex)
            {
                if (!UIxCanvasCollection.TryGetValue(p_LayerIndex, out var o_CanvasPreset))
                {
                    var spawnedCanvas = RootRect.CreateDefaultCanvas();
                    spawnedCanvas.name = $"Canvas [Layer_{p_LayerIndex}]";
                    spawnedCanvas.renderMode = RenderMode;

                    switch (RenderMode)
                    {
                        case RenderMode.ScreenSpaceCamera:
                            spawnedCanvas.worldCamera = CameraManager.GetInstanceUnsafe.MainCamera;
                            spawnedCanvas.sortingLayerName = ((GameConst.GameSortingLayerType)p_LayerIndex).ToString();
                            break;
                        case RenderMode.ScreenSpaceOverlay:
                            spawnedCanvas.sortingOrder = p_LayerIndex;
                            break;
                        case RenderMode.WorldSpace:
                            spawnedCanvas.worldCamera = CameraManager.GetInstanceUnsafe.MainCamera;
                            spawnedCanvas.sortingOrder = p_LayerIndex;
                            spawnedCanvas.gameObject.TurnLayerTo(GameConst.GameLayerType.WorldUI, false);
                            break;
                    }

                    UIxCanvasCollection.Add(p_LayerIndex, o_CanvasPreset = new UIxCanvasPreset(this, spawnedCanvas));
                }

                return o_CanvasPreset;
            }

            #endregion
            
            #region <Disposable>
        
            /// <summary>
            /// dispose 패턴 onceFlag
            /// </summary>
            public bool IsDisposed { get; private set; }
        
            /// <summary>
            /// dispose 플래그를 초기화 시키는 메서드
            /// </summary>
            public void Rejuvenate()
            {
                IsDisposed = false;
            }

            /// <summary>
            /// 인스턴스 파기 메서드
            /// </summary>
            public void Dispose()
            {
                if (IsDisposed)
                {
                    return;
                }
                else
                {
                    IsDisposed = true;
                    OnDisposeUnmanaged();
                }
            }

            #endregion
        }

        /// <summary>
        /// 어떤 Canvas 컴포넌트에 대한 참조 정보 및 랜더 모드, 정렬을 위한 캔버스 레이어 정보를 가지며
        /// 해당 Canvas에 포함된 UIxElement 컴포넌트를 컬렉션으로 기술하는 클래스
        /// </summary>
        public class UIxCanvasPreset : _IDisposable
        {
            ~UIxCanvasPreset()
            {
                Dispose();
            }
            
            #region <Fields>

            public UIxRootPreset RootPreset { get; private set; }
            public int LayerIndex { get; private set; }
            public Canvas Canvas { get; private set; }
            public RectTransform CanvasRect;
            public Dictionary<UIxElementType, UIxElementBase> UIxElementCollection { get; private set; }

            #endregion

            #region <Indexer>

            public UIxElementBase this[UIxElementType p_Type]
            {
                get => UIxElementCollection[p_Type]; 
            }
            
            #endregion        
            
            #region <Constructors>
            
            public UIxCanvasPreset(UIxRootPreset p_RootPreset, Canvas p_Canvas)
            {
                RootPreset = p_RootPreset;
                LayerIndex = p_Canvas.sortingOrder;
                Canvas = p_Canvas;
                CanvasRect = Canvas.GetComponent<RectTransform>();
                UIxElementCollection = new Dictionary<UIxElementType, UIxElementBase>();
            }

            #endregion

            #region <Callbacks>

            public void OnElementReleased(UIxElementBase pUIxElement)
            {
                if (!ReferenceEquals(null, UIxElementCollection))
                {
                    var uiType = pUIxElement.UICanvasControlType;
                    UIxElementCollection.Remove(uiType);
                }
            }

            /// <summary>
            /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
            /// </summary>
            private void OnDisposeUnmanaged()
            {
                if (!ReferenceEquals(null, UIxElementCollection))
                {
                    
                    var enumerator = UIxElementCollection.Values.ToList();
                    foreach (var uiElement in enumerator)
                    {
                        uiElement.Pooling();
                    }

                    UIxElementCollection.Clear();
                    UIxElementCollection = null;
                }

                if (!ReferenceEquals(null, RootPreset))
                {
                    RootPreset.OnCanvasReleased(this);
                    RootPreset = null;
                }
                
                Object.Destroy(CanvasRect.gameObject);
                CanvasRect = default;
                Canvas = default;
            }

            #endregion

            #region <Methods>

            public async UniTask<UIxElementBase> AddElement(UIxElementType p_ElementType, CancellationToken p_CancellationToken)
            {
                var createParams = await UIPoolManager.GetInstanceUnsafe.GetCreateParamsAsync(p_ElementType, ResourceLifeCycleType.ManualUnload, 0, p_CancellationToken);
                var spawnedUI = UIPoolManager.GetInstanceUnsafe.Pop(createParams, new UIPoolManager.ActivateParams(p_ElementType, this));
                UIxElementCollection.Add(p_ElementType, spawnedUI);

                return spawnedUI;
            }

            #endregion
            
            #region <Disposable>
        
            /// <summary>
            /// dispose 패턴 onceFlag
            /// </summary>
            public bool IsDisposed { get; private set; }
        
            /// <summary>
            /// dispose 플래그를 초기화 시키는 메서드
            /// </summary>
            public void Rejuvenate()
            {
                IsDisposed = false;
            }

            /// <summary>
            /// 인스턴스 파기 메서드
            /// </summary>
            public void Dispose()
            {
                if (IsDisposed)
                {
                    return;
                }
                else
                {
                    IsDisposed = true;
                    OnDisposeUnmanaged();
                }
            }

            #endregion
        }

        #endregion
    }
}

#endif