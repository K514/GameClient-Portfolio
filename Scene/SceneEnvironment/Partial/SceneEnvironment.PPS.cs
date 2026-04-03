#if !SERVER_DRIVE && APPLY_PPS

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public partial class SceneEnvironment
    {
        #region <Fields>

        private List<PpsObjectBase> _PpsObjectList;
        public PpsObjectBase BasisPpsObject { get; private set; }
        
        #endregion

        #region <Callbacks>

        private async UniTask OnCreatePPS(CancellationToken p_Token)
        {
            _PpsObjectList = new List<PpsObjectBase>();
            await LoadPpsObject(p_Token);

            BasisPpsObject = _PpsObjectList[0];
        }

        private void OnUpdatePPS(float p_DeltaTime)
        {
            if (_PpsObjectList.CheckCollectionSafe())
            {
                foreach (var pps in _PpsObjectList)
                {
                    pps.OnUpdate(p_DeltaTime);
                }
            }
        }
        
        public void OnPpsReleased(PpsObjectBase p_Pps)
        {
            _PpsObjectList.Remove(p_Pps);
        }
        
        private void OnDisposePPS()
        {
            if (!ReferenceEquals(null, _PpsObjectList))
            {
                var enumerator = _PpsObjectList.ToList();
                foreach (var ppsObject in enumerator)
                {
                    ppsObject.Pooling();
                }
 
                _PpsObjectList.Clear();
                _PpsObjectList = null;
            }
        }

        #endregion

        #region <Methods>
        
        /// <summary>
        /// PPS 기능을 활성화시키는 메서드
        /// </summary>
        public void SetPpsEnable(bool p_Flag)
        {
            foreach (var ppsObject in _PpsObjectList)
            {
                ppsObject.SetVolumeEnable(p_Flag);
            }
        }
        
        /// <summary>
        /// 현재 씬 세팅으로부터 PPS 세팅을 적용하는 메서드
        /// </summary>
        private async UniTask LoadPpsObject(CancellationToken p_CancellationToken)
        {
            await ApplyPostProcessVolume(new PpsObjectPoolManager.CreateParams(), p_CancellationToken);
            
            if (PpsObjectCreateParamsList.CheckCollectionSafe())
            {
                foreach (var createParams in PpsObjectCreateParamsList)
                {
                    await ApplyPostProcessVolume(createParams, p_CancellationToken);
                }
            }
        }

        /// <summary>
        /// 씬 단위 수명을 가지는 PPS 볼륨 오브젝트를 생성하는 메서드
        /// </summary>
        private async UniTask ApplyPostProcessVolume(PpsObjectPoolManager.CreateParams p_CreateParams, CancellationToken p_CancellationToken)
        {
            await PpsObjectPoolManager.GetInstanceUnsafe.AddPool(p_CreateParams, 0, p_CancellationToken);
            var spawned = PpsObjectPoolManager.GetInstanceUnsafe.Pop(p_CreateParams, new PpsObjectPoolManager.ActivateParams(this, new AffineCorrectionPreset(AffineTool.CorrectPositionType.None, transform)));
            _PpsObjectList.Add(spawned);
        }
        
        #endregion
    }
}

#endif