using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public partial class PatchScene
    {
        #region <Callbacks>

        protected override void OnCreatePhase(CancellationToken p_CancellationToken)
        {
            _PhaseWeightTable
                = new Dictionary<PatchScenePhase, float>
                {
                    {PatchScenePhase.GetPatchList, 0.25f},
                    {PatchScenePhase.PatchFile, 3f},
                    {PatchScenePhase.PatchTerminate, 0.25f},
                };

            var enumerator = EnumFlag.GetEnumEnumerator<PatchScenePhase>(EnumFlag.GetEnumeratorType.ExceptNone);
            foreach (var progressPhase in enumerator)
            {
                switch (progressPhase)
                {
                    case PatchScenePhase.GetPatchList:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                GetPatchList, p_CancellationToken, 
                                p_Description : "네트워크 프리셋 로드"
                            )
                        );
                        break;
                    }
                    case PatchScenePhase.PatchFile:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                PatchBundle, p_CancellationToken, 
                                p_Description : "네트워크 버전과 현재 클라이언트 버전 비교"
                            )
                        );
                        break;
                    }
                    case PatchScenePhase.PatchTerminate:
                    {
                        var asyncTaskSequence = PopSequence(progressPhase);
                        asyncTaskSequence.AddAsyncTask
                        (
                            new DefaultAsyncTaskParams
                            (
                                PatchTerminate, p_CancellationToken, 
                                p_Description : "패치 종료"
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

            SwitchPhase(PatchScenePhase.GetPatchList);
        }

        #endregion
    }
}