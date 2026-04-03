using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class TitleScene
    {
        #region <Callbacks>

        protected override void OnCreatePhase(CancellationToken p_CancellationToken)
        {
            _PhaseWeightTable 
                = new Dictionary<TitleScenePhase, float>
                {
                    {TitleScenePhase.TitleOpen, 1f},
                    {TitleScenePhase.StartButton, 1f},
                    {TitleScenePhase.OptionButton, 1f},
                    {TitleScenePhase.ReplayButton, 1f},
                    {TitleScenePhase.ExitButton, 1f},
                };

            var enumerator = EnumFlag.GetEnumEnumerator<TitleScenePhase>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var progressPhase in enumerator)
            {
                switch (progressPhase)
                {
                    case TitleScenePhase.TitleOpen:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams 
                            (
                                Test, p_CancellationToken, 
                                p_Description : "타이틀 오프닝 연출"
                            )
                        );
                        break;
                    }
                    case TitleScenePhase.StartButton:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams 
                            (
                                StartButtonProgress, p_CancellationToken, 
                                p_Description : "타이틀 종료 연출"
                            )
                        );
                        break;
                    }
                    case TitleScenePhase.OptionButton:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams 
                            (
                                Test, p_CancellationToken, 
                                p_Description : "타이틀 종료 연출"
                            )
                        );
                        break;
                    }
                    case TitleScenePhase.ReplayButton:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams 
                            (
                                Test, p_CancellationToken, 
                                p_Description : "타이틀 종료 연출"
                            )
                        );
                        break;
                    }
                    case TitleScenePhase.ExitButton:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams 
                            (
                                Test, p_CancellationToken, 
                                p_Description : "타이틀 종료 연출"
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

            SwitchPhase(TitleScenePhase.TitleOpen);
        }

        #endregion
    }
}