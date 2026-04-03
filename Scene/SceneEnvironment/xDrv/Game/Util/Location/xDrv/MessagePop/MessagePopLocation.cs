using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public class MessagePopLocation : LocationBase
    {
        protected override void PreloadLocation(List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
        }

        protected override async void ActivateLocation(SceneTool.SceneLocationWaveType p_Type, List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
            if (p_MetaSet.CheckCollectionSafe())
            {
                foreach (var meta in p_MetaSet)
                {
                    var indexList = meta.SpawnEntityList;
                    foreach (var index in indexList)
                    {
                        //UIxControlRoot.GetInstanceUnsafe.UIxMainMenu.PopMessage(UIxAddGameItem.eMessageType.None, lang.Text);
                        //UIxControlRoot.GetInstanceUnsafe.UIxMainMenu.ChatMessage(lang.Text);
                        // await UniTask.Delay(1500);

                        /*var nextText = lang.NextText;
                        var list = new List<string>() { lang.Text };
                        Debug.LogError(lang.Text);
                        list.AddRange(nextText);*/

                        /*
                        var nextText = lang.NextText;
                        if (nextText.CheckCollectionSafe())
                        {
                            foreach (var text in nextText)
                            {
                                // UIxControlRoot.GetInstanceUnsafe.UIxMainMenu.PopMessage(UIxAddGameItem.eMessageType.None, text);
                                UIxControlRoot.GetInstanceUnsafe.UIxMainMenu.ChatMessage(text);
                                await UniTask.Delay(3500);
                            }
                        }*/
                    }
                }
            }
        }
        
        public override void SetSceneLocationMeta(GameSceneEnvironmentBase p_Env, SceneTool.SceneLocationType p_Type, SceneTool.SceneLocationMeta p_Meta)
        {
            base.SetSceneLocationMeta(p_Env, p_Type, p_Meta);
  
            var index = SceneLocationPivotMetaTable[SceneTool.SceneLocationWaveType.Default][0].SpawnEntityList[0];

            /*var assetLoadResult = PrefabNameTable.GetInstanceUnsafe.GetResource(lang.CheckPointPrefab, ResourceLifeCycleType.SceneUnload);
            if (assetLoadResult)
            {
                var affine = transform;
                var affinePos = new AffineCorrectionPreset(AffineTool.CorrectPositionType.ForceSurface, new AffinePreset(affine.position), GameConst.Terrain_LayerMask);
                var correctedAffine = affinePos.CorrectPosition();
                var checkPoint = Instantiate(assetLoadResult.Asset, correctedAffine.Position, Quaternion.identity, affine);
            }*/
        }
    }
}