using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class InitScene
    {
        #region <Callbacks>

        protected override void OnCreatePhase(CancellationToken p_CancellationToken)
        {
            _PhaseWeightTable 
                = new Dictionary<InitScenePhase, float>
                {
                    {InitScenePhase.InitSceneStart, 1f},
                    {InitScenePhase.InitSceneProcess, 1f},
                    {InitScenePhase.InitSceneProcess2, 1f},
                    {InitScenePhase.InitSceneTerminate, 1f},
                };

            var enumerator = EnumFlag.GetEnumEnumerator<InitScenePhase>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var progressPhase in enumerator)
            {
                switch (progressPhase)
                {
                    case InitScenePhase.InitSceneStart:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams 
                            (
                                BootingStart, p_CancellationToken, 
                                p_Description : "부팅 연출 및 시스템 테이블을 미리 로드"
                            )
                        );
                        break;
                    }
                    case InitScenePhase.InitSceneProcess:
                    {
                        #region <InitSceneProcess>

                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams
                            (
                                LoadGameManager, p_CancellationToken, 
                                p_Description : "게임 매니저 로딩"
                            )
                        );
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams
                            (
                                LoadSceneManager, p_CancellationToken, 
                                p_Description : "씬 매니저 로딩"
                            )
                        );
#if !SERVER_DRIVE
                        asyncTaskSequence.AddAsyncTask
                        (  
                            new DefaultAsyncTaskParams
                            (
                                LoadExtraManager, p_CancellationToken, 
                                p_Description : "기타 매니저 로딩"
                            )
                        );
#endif
                        break;
                        
                        #endregion
                    }
                    case InitScenePhase.InitSceneProcess2:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
#if UNITY_EDITOR
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                BootEditorOnly, p_CancellationToken, 
                                p_Description : "에디터 모드에서만 동작하는 시스템 초기화"
                            )
                        );
#endif
                        break;
                    }
                    case InitScenePhase.InitSceneTerminate:
                    {
                        PopSequence(progressPhase);
                        break;
                    }
                }
            }
            
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await base.OnInitiate(p_CancellationToken);

            SwitchPhase(InitScenePhase.InitSceneStart);
        }
        
        #endregion
    }
}