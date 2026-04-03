using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using xk514;

namespace k514.Mono.Common
{
    public class AnimationManager : AsyncSingleton<AnimationManager>
    {
        #region <Fields>
        
        private Dictionary<int, AnimatorPreset> _AnimatorTable;

        #endregion
        
        #region <Callbacks>

        protected override void TryInitializeDependency()
        {
            base.TryInitializeDependency();

            _Dependencies.Add(typeof(MotionClipDataTableQuery));
        }

        protected override async UniTask OnCreated(CancellationToken p_Cancellation)
        {
            await UniTask.CompletedTask;

            _AnimatorTable = new Dictionary<int, AnimatorPreset>();
        }

        protected override async UniTask OnInitiate(CancellationToken p_Cancellation)
        {
            await UniTask.CompletedTask;
        }
        
        protected override void OnDisposeSingleton()
        {
            if (!ReferenceEquals(null, _AnimatorTable) && AssetLoaderManager.GetInstanceUnsafe is not null)
            {
                foreach (var animatorPresetKV in _AnimatorTable)
                {
                    var animatorPreset = animatorPresetKV.Value;
                    var assetLoadResult = animatorPreset.Animator;
                    AssetLoaderManager.GetInstanceUnsafe.UnloadAsset(ref assetLoadResult);
                }
                _AnimatorTable.Clear();
                _AnimatorTable = null;
            }
            
            base.OnDisposeSingleton();
        }

        #endregion
        
        #region <Methods>

        public AnimatorPreset LoadAnimationController(int p_Index)
        {
            if (AnimationControllerNameTable.GetInstanceUnsafe.HasKey(p_Index))
            {
                if (_AnimatorTable.TryGetValue(p_Index, out var o_Preset))
                {
                    return o_Preset;
                }
                else
                {
                    var assetLoadResult
                        = AnimationControllerNameTable
                            .GetInstanceUnsafe
                            .GetResource
                            (
                                p_Index, ResourceLifeCycleType.ManualUnload
                            );

                    if (assetLoadResult.ValidFlag)
                    {
                        var einIdleClip = default(ClipPreset);
                        var einMoveClip = default(ClipPreset);
                        var einAttackClip = default(ClipPreset);
                        var einDeadClip = default(ClipPreset);

                        var targetClipGroup = assetLoadResult.Asset.animationClips;
                        var motionTable = new Dictionary<AnimationTool.MotionType, List<ClipPreset>>();
                        var indexPermutationList = new List<int>();
       
                        foreach (var motionType in AnimationTool._MotionTypeEnumerator)
                        {
                            var motionName = motionType.ToString();
                            var clipPresetList = new List<ClipPreset>();
                            
                            foreach (var animationClip in targetClipGroup)
                            {
                                var targetClip = animationClip;
                                var targetClipName = targetClip.name;
                   
                                if (targetClipName.Contains(motionName))
                                {
                                    var recordValid = MotionClipDataTableQuery.GetInstanceUnsafe.TryGetRecordBridge(targetClipName, out var o_Record);
                                    var motionPlaceType = recordValid ? o_Record.MotionPlaceType : AnimationTool.MotionPlaceType.None;
                                    var parsedIndex = int.Parse(targetClipName.CutString(motionName, false, false));
                                    indexPermutationList.Add(parsedIndex);
                                    clipPresetList.Add(new ClipPreset(motionType, parsedIndex, motionPlaceType, targetClip));
                                }
                            }
                            
                            // 애니메이터에 추가된 순으로 clip들이 정렬되기 때문에, index 서순으로 재배치해줘야 한다.
                            var assembledClipCount = clipPresetList.Count;
                            for (var i = 0; i < assembledClipCount; i++)
                            {
                                var targetPermutateIndex = indexPermutationList[i];
                                if (targetPermutateIndex != i)
                                {
                                    indexPermutationList[targetPermutateIndex] = targetPermutateIndex;
                                    indexPermutationList[i] = i;
                                    
                                    (clipPresetList[targetPermutateIndex], clipPresetList[i]) = (clipPresetList[i], clipPresetList[targetPermutateIndex]);
                                }
                            }
                            
                            indexPermutationList.Clear();
                            motionTable.Add(motionType, clipPresetList);

                            if (clipPresetList.Count > 0)
                            {
                                switch (motionType)
                                {
                                    default:
                                    case AnimationTool.MotionType.IdleRelax:
                                    case AnimationTool.MotionType.IdleCombat:
                                    case AnimationTool.MotionType.IdleAerial:
                                    case AnimationTool.MotionType.Groggy:
                                        if (!einIdleClip.ValidFlag) einIdleClip = clipPresetList[0];
                                        break;
                                    case AnimationTool.MotionType.MoveWalk:
                                    case AnimationTool.MotionType.MoveRun:
                                        if (!einMoveClip.ValidFlag) einMoveClip = clipPresetList[0];
                                        break;
                                    case AnimationTool.MotionType.Punch:
                                    case AnimationTool.MotionType.Kick:
                                    case AnimationTool.MotionType.Cast:
                                    case AnimationTool.MotionType.Dash:
                                    case AnimationTool.MotionType.JumpUp:
                                    case AnimationTool.MotionType.JumpDown:
                                    case AnimationTool.MotionType.Interact:
                                        if (!einAttackClip.ValidFlag) einAttackClip = clipPresetList[0];
                                        break;
                                    case AnimationTool.MotionType.Hit:
                                    case AnimationTool.MotionType.Dead:
                                        if (!einDeadClip.ValidFlag) einDeadClip = clipPresetList[0];
                                        break;
                                }
                            }
                        }

                        foreach (var clipListKV in motionTable)
                        {
                            var motionType = clipListKV.Key;
                            var clipList = clipListKV.Value;
                            if (clipList.Count < 1)
                            {
                                switch (motionType)
                                {
                                    case AnimationTool.MotionType.IdleRelax:
                                    case AnimationTool.MotionType.IdleCombat:
                                    case AnimationTool.MotionType.IdleAerial:
                                    case AnimationTool.MotionType.Groggy:
                                        if(einIdleClip.ValidFlag) clipList.Add(einIdleClip);
                                        break;
                                    case AnimationTool.MotionType.MoveWalk:
                                    case AnimationTool.MotionType.MoveRun:
                                        if(einMoveClip.ValidFlag) clipList.Add(einMoveClip);
                                        break;
                                    case AnimationTool.MotionType.Punch:
                                    case AnimationTool.MotionType.Kick:
                                    case AnimationTool.MotionType.Cast:
                                    case AnimationTool.MotionType.Dash:
                                    case AnimationTool.MotionType.JumpUp:
                                    case AnimationTool.MotionType.JumpDown:
                                    case AnimationTool.MotionType.Interact:
                                        if(einAttackClip.ValidFlag) clipList.Add(einAttackClip);
                                        break;
                                    case AnimationTool.MotionType.Hit:
                                    case AnimationTool.MotionType.Dead:
                                        if(einDeadClip.ValidFlag) clipList.Add(einDeadClip);
                                        break;
                                }
                            }

#if UNITY_EDITOR
                            if (clipList.Count < 1)
                            {
                                Debug.LogError($"[AnimationManager] [idx : {p_Index}] [Motion : {motionType}] Fallback Failed");
                            }
#endif
                        }
                        
                        var animationParamsRecord = new AnimatorPreset(p_Index, assetLoadResult, motionTable);
                        _AnimatorTable.Add(p_Index, animationParamsRecord);
                        
                        return animationParamsRecord;
                    }
                    else
                    {
#if APPLY_PRINT_LOG
                        CustomDebug.LogError(($"Animator 로드에 실패했습니다. : [{AnimationControllerNameTable.GetInstanceUnsafe.GetRecord(p_Index).ResourceName}]", Color.red));
#endif
                        return default;
                        
                    }
                }
            }
            else
            {
#if APPLY_PRINT_LOG
                CustomDebug.LogError(($"Animator Key 가 유효하지 않습니다. : [{p_Index}]", Color.red));
#endif
                return default;
            }
        }

        #endregion
    }
}