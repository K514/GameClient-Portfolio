using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class LoadingScene
    {
        #region <Callbacks>

        protected override void OnCreatePhase(CancellationToken p_CancellationToken)
        {
            _PhaseWeightTable 
                = new Dictionary<LoadingScenePhase, float>
                {
                    {LoadingScenePhase.UnloadingResource, 0.5f},
                    {LoadingScenePhase.LoadingResource, 0.5f},
                    {LoadingScenePhase.LoadingScene, 0.5f},
                    {LoadingScenePhase.LoadingSceneStageOn, 0.5f},
                    {LoadingScenePhase.MergeScene, 0.5f},
                    {LoadingScenePhase.AsyncLoadTerminate, 0.5f},
                };

            var enumerator = EnumFlag.GetEnumEnumerator<LoadingScenePhase>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var progressPhase in enumerator)
            {
                switch (progressPhase)
                {
                    case LoadingScenePhase.UnloadingResource:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                Release_LifeCycle_Asset, p_CancellationToken, 
                                p_Description : "에셋 언로드"
                            )
                        );
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                Release_LifeCycle_Singleton, p_CancellationToken, 
                                p_Description : "싱글톤 언로드"
                            )
                        );
                        break;
                    }
                    case LoadingScenePhase.LoadingResource:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (      
                            new DefaultAsyncTaskParams
                            (
                                CheckSceneBundle, p_CancellationToken, 
                                p_Description : "현재 씬의 번들을 로딩"
                            )
                        );
                        break;
                    }
                    case LoadingScenePhase.LoadingScene:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (                       
                            new DefaultAsyncTaskParams
                            (
                                AsyncLoadScene, p_CancellationToken, 
                                p_Description : "씬 로딩"
                            )
                        );
                        break;
                    }
                    case LoadingScenePhase.LoadingSceneStageOn:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams
                            (
                                PreloadScene, p_CancellationToken, 
                                p_Description : "프리로드"
                            )
                        );
                        break;
                    }
                    case LoadingScenePhase.MergeScene:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (   
                            new DefaultAsyncTaskParams
                            (
                                MergeScene, p_CancellationToken, 
                                p_Description : "씬 병합"
                            )
                        );
                        break;
                    }
                    case LoadingScenePhase.AsyncLoadTerminate:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                SceneLoadOver, p_CancellationToken, 
                                p_Description : "씬 로드 완료"
                            )
                        );
                        break;
                    }
                }
            }
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await base.OnInitiate(p_CancellationToken);

            SwitchPhase(LoadingScenePhase.UnloadingResource);
        }

        #endregion
    }
}