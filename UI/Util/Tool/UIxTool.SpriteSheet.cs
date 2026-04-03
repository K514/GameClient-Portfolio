#if !SERVER_DRIVE

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace k514.Mono.Common
{
    public partial class UIxTool
    {
        #region <Enums>

        public enum AnimationSpriteType
        {
            None,
            Gary,
        }

        #endregion

        #region <Methods>

        public static SpriteAnimationPreset GetSpriteAnimationPreset(this SpriteSheetCarrier p_Carrier,
            Transform p_TargetWrapper, string p_ImageObjectName, int p_StartIndex, int p_EndIndex, 
            float p_AnimationDuration, int p_AnimationLoopCount)
        {
            var result 
                = new SpriteAnimationPreset
                (
                    p_TargetWrapper.FindRecursive<Image>(p_ImageObjectName).Item2,
                    p_Carrier.SelectList(p_StartIndex, p_EndIndex), p_AnimationDuration, p_AnimationLoopCount
                );

            return result;
        }

        #endregion

        #region <Struct>

        public struct SpriteAnimationPreset : _IDisposable
        {
            #region <Fields>

            public Image TargetImage;
            public SpriteIterator SpriteIterator { get; set; }

            #endregion

            #region <Constructor>

            public SpriteAnimationPreset(Image p_Image)
            {
                IsDisposed = false;
                TargetImage = p_Image;
                SpriteIterator = default;
            }

            public SpriteAnimationPreset(Image p_Image, List<Sprite> p_SpriteSet, float p_AnimationDuration, int p_LoopCount)
            {
                IsDisposed = false;
                TargetImage = p_Image;
                SpriteIterator = new SpriteIterator(p_SpriteSet, p_AnimationDuration, p_LoopCount);
            }

            #endregion

            #region <Callbacks>

            public void OnUpdateSpriteIterator(float p_DeltaTime)
            {
                switch (SpriteIterator.OnUpdate(p_DeltaTime))
                {
                    case IteratorTool.IntervalIteratorEvent.ProgressNext:
                    case IteratorTool.IntervalIteratorEvent.ProgressOver:
                        SetImage();
                        break;
                }
            }

            /// <summary>
            /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
            /// </summary>
            private void OnDisposeUnmanaged()
            {
                if (!ReferenceEquals(null, SpriteIterator))
                {
                    SpriteIterator.Dispose();
                    SpriteIterator = null;
                }
            }
            
            #endregion
                
            #region <Methods>

            public void SetSpriteIterator(SpriteIterator p_SpriteIterator)
            {
                if (!ReferenceEquals(null, SpriteIterator))
                {
                    SpriteIterator.Dispose();
                    SpriteIterator = null;
                }

                SpriteIterator = p_SpriteIterator;
            }
                
            public void SetImage()
            {
                TargetImage.sprite = SpriteIterator.GetCurrentSprite();
            }

            public void ClearImage()
            {
                TargetImage.sprite = null;
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
        
        #region <Class>

        public class SpriteSheetCarrier
        {
            ~SpriteSheetCarrier()
            {
                Dispose();
            }
            
            #region <Fields>

            private SpriteSheetLoadType LoadType;
            private string SpriteSheetPath;
            private MultiAssetLoadResult<Sprite> SpriteAssetPreset;
            public Sprite[] SpriteSet { get; private set; }

            #endregion

            #region <Enums>

            private enum SpriteSheetLoadType
            {
                Unity_Resource_LoadAll,
                ResourceNameTable
            }

            #endregion
            
            #region <Constructors>

            public SpriteSheetCarrier(string p_AbsolutePath)
            {
                LoadType = SpriteSheetLoadType.Unity_Resource_LoadAll;
                SpriteSheetPath = p_AbsolutePath;
                SpriteSet = SystemTool.LoadAll<Sprite>(p_AbsolutePath);
            }

            public SpriteSheetCarrier(MultiAssetLoadResult<Sprite> p_SpritePreset)
            {
                LoadType = SpriteSheetLoadType.ResourceNameTable;
                SpriteAssetPreset = p_SpritePreset;
                SpriteSet = SpriteAssetPreset.AssetArray;
            }

            #endregion

            #region <Callbacks>
            
            /// <summary>
            /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
            /// </summary>
            private void OnDisposeUnmanaged()
            {
                switch (LoadType)
                {
                    case SpriteSheetLoadType.Unity_Resource_LoadAll:
                        SpriteSet = null;
                        break;
                    case SpriteSheetLoadType.ResourceNameTable:
                        SpriteSet = null;
                        AssetLoaderManager.GetInstanceUnsafe.UnloadAsset(ref SpriteAssetPreset);
                        SpriteAssetPreset = default;
                        break;
                }
            }

            #endregion
            
            #region <Methods>

            public Sprite GetSprite(int p_Index)
            {
                return SpriteSet.GetElementSafe(p_Index);
            }

            public List<Sprite> SelectList(int p_Start, int p_End)
            {
                var startIndex = Mathf.Max(0, p_Start);
                var endIndex = p_End < 0 ? SpriteSet.Length - 1 :Mathf.Min(p_End, SpriteSet.Length - 1);
                var result = new List<Sprite>();
                for (int i = startIndex; i <= endIndex; i++)
                {
                    result.Add(SpriteSet[i]);
                }

                return result;
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