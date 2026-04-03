using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public partial class PatchScene
    {
        #region <Callbacks>

        protected override void _OnEntryPhaseLoop()
        {
        }

        protected override async UniTask _OnTerminatePhaseLoop(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;

            SystemBoot.OnPatchSuccess();
        }

        #endregion

        #region <Methods>

        private async UniTask GetPatchList(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
            var requestUrl = NetworkNodeTable.GetInstanceUnsafe.GetRecord(NetworkTool.PatchNode).Value + SystemMaintenance.BundlePatchListTableMetaPath;
            Debug.LogError(requestUrl);
            var (success, result) = await AssetBundleDownloader.GetInstanceUnsafe.TryGet(requestUrl, GetCancellationToken());
            if (success)
            {
                var text = result.downloadHandler.text;
                if (await BundlePatchListTable.GetInstanceUnsafe.LoadRawText(text, p_CancellationToken))
                {
                    if (BundlePatchListTable.GetInstanceUnsafe.VerifySignature())
                    {
#if APPLY_PRINT_LOG
                        if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
                        {
                            CustomDebug.LogWarning((this, "서버 서명 인증 성공"));
                        }
#endif
                    }
                    else
                    {
#if APPLY_PRINT_LOG
                        if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
                        {
                            CustomDebug.LogError((this, "서버 서명 인증 실패"));
                        }
#endif
                        throw new Exception("서버 서명 인증 실패");
                    }
                }
                else
                {
#if APPLY_PRINT_LOG
                    if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
                    {
                        CustomDebug.LogError((this, "패치 테이블을 읽는데 실패했습니다."));
                    }
#endif
                    throw new Exception("패치 테이블을 읽는데 실패했습니다.");
                }
            }
            else
            {
#if APPLY_PRINT_LOG
                if (CustomDebug.CustomDebugLogFlag.PrintSceneControlLog.HasOpen())
                {
                    CustomDebug.LogError((this, "서버로부터 패치 리스트를 다운로드 받는데 실패했습니다."));
                }
#endif
                throw new Exception("서버로부터 패치 리스트를 다운로드 받는데 실패했습니다.");
            }
        }

        private void DownloadBundleRequest(string p_ReqeustUrl, string p_BundleName, ref List<NetworkTool.UnityWebRequestParams> r_Result)
        {
            var downloadPath = (SystemMaintenance.GetAssetBundleAbsolutePath() + p_BundleName).GetUpperPath();
            var requestBundleUrl = p_ReqeustUrl + p_BundleName;
            r_Result.Add(new NetworkTool.UnityWebRequestParams(requestBundleUrl, NetworkTool.DefaultTimeOutSecond, downloadPath));
        }

        private async UniTask PatchBundle(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
            var patchBundleTable = BundlePatchListTable.GetInstanceUnsafe.GetTable();
            var requestUrlHeader = NetworkNodeTable.GetInstanceUnsafe.GetRecord(NetworkTool.PatchNode).Value;
            var requestParamsList = new List<NetworkTool.UnityWebRequestParams>();

            foreach (var recordKV in patchBundleTable)
            {
                var bundleName = recordKV.Key;
                var bundlePath = SystemMaintenance.GetAssetBundleAbsolutePath() + bundleName;

                switch (bundleName)
                {
                    case var _ when !File.Exists(bundlePath):
                    {
                        CustomDebug.LogError((this, $"다운로드 : {bundleName} 파일이 존재하지 않습니다."));
                        DownloadBundleRequest(requestUrlHeader, bundleName, ref requestParamsList);
                        break;
                    }
                    case var _ when File.Exists(bundlePath):
                    {
                        var record = recordKV.Value;
                        var localHash = DataIOTool.GetSHA256FromFile(bundlePath);
                        if (localHash != record.Hash)
                        {
                            CustomDebug.LogError((this, $"다운로드 : {bundleName} 해시값이 일치하지 않습니다."));
                            DownloadBundleRequest(requestUrlHeader, bundleName, ref requestParamsList);
                        }
                        break;
                    }
                }
            }

            await AssetBundleDownloader.GetInstanceUnsafe.DownloadThrottled(requestParamsList, GetCancellationToken());
        }

        private async UniTask PatchTerminate(IAsyncTaskHandler p_AsyncTaskRequest, CancellationToken p_CancellationToken)
        {
            await UniTask.DelayFrame(1);
        }

        #endregion
    }
}