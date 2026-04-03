using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract class LocationBase : MonoBehaviour, _IDisposable
    {
        #region <Fields>

        private BoxCollider _BoxCollider;
        protected LocationPhase _CurrentPhase;
        public Dictionary<SceneTool.SceneLocationWaveType, List<SceneTool.SceneLocationPivotMeta>> SceneLocationPivotMetaTable { get; private set; }
        protected GameSceneEnvironmentBase _GameSceneEnvironment;
        public SceneTool.SceneLocationType LocationType { get; private set; }
        
        #endregion

        #region <Enums>

        public enum LocationPhase
        {
            None,
            Ready,
            LocationActivate,
            Eliminated,
        }

        #endregion
        
        #region <Callbacks>

        protected virtual void Awake()
        {
            _BoxCollider = gameObject.AddComponent<BoxCollider>();
            _BoxCollider.isTrigger = true;
            SceneLocationPivotMetaTable = new Dictionary<SceneTool.SceneLocationWaveType, List<SceneTool.SceneLocationPivotMeta>>();

            var enumerator = EnumFlag.GetEnumEnumerator<SceneTool.SceneLocationWaveType>(EnumFlag.GetEnumeratorType.GetAll);
            foreach (var sceneLocationWaveType in enumerator)
            {
                SceneLocationPivotMetaTable.Add(sceneLocationWaveType, new List<SceneTool.SceneLocationPivotMeta>());
            }
            
            SetPhase(LocationPhase.None);
        }

        private void OnTriggerStay(Collider other)
        {
            if (_CurrentPhase == LocationPhase.Ready)
            {
                if (InteractManager.GetInstanceUnsafe?.IsPlayer(other) ?? false)
                {
                    SetPhase(LocationPhase.LocationActivate);
                }
            }
        }

        public void OnLocationPreload()
        {
            foreach (var waveTypeKV in SceneLocationPivotMetaTable)
            {
                var waveType = waveTypeKV.Key;
                PreloadLocation(SceneLocationPivotMetaTable[waveType]);
            }
        }
        
        private void OnLocationActivate(SceneTool.SceneLocationWaveType p_Type)
        {
            ActivateLocation(p_Type, SceneLocationPivotMetaTable[p_Type]);
            _GameSceneEnvironment.OnLocationActivated(this);
        }
        
        private void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected virtual void OnDisposeUnmanaged()
        {
        }

        #endregion

        #region <Methods>

        public bool IsEliminate() => _CurrentPhase == LocationPhase.Eliminated;

        protected abstract void PreloadLocation(List<SceneTool.SceneLocationPivotMeta> p_MetaSet);
        
        protected abstract void ActivateLocation(SceneTool.SceneLocationWaveType p_Type, List<SceneTool.SceneLocationPivotMeta> p_MetaSet);
        
        public void SetPhase(LocationPhase p_Type)
        {
            _CurrentPhase = p_Type;
            
            switch (_CurrentPhase)
            {
                case LocationPhase.Ready:
                    _BoxCollider.enabled = true;
                    break;
                case LocationPhase.None:
                case LocationPhase.Eliminated:
                    _BoxCollider.enabled = false;
                    break;
                case LocationPhase.LocationActivate:
                    _BoxCollider.enabled = false;
                    OnLocationActivate(SceneTool.SceneLocationWaveType.Default);
                    break;
            }
        }

        public virtual void SetSceneLocationMeta(GameSceneEnvironmentBase p_Env, SceneTool.SceneLocationType p_Type, SceneTool.SceneLocationMeta p_Meta)
        {
            _GameSceneEnvironment = p_Env;
            LocationType = p_Type;
            
            transform.position = p_Meta.Position;
            transform.rotation = Quaternion.Euler(p_Meta.Rotation);
            transform.localScale = p_Meta.Scale;

            _BoxCollider.center = p_Meta.Center;
            _BoxCollider.size = p_Meta.Size;

            var metaSet = p_Meta.SceneLocationPivotMetaSet;
            foreach (var meta in metaSet)
            {
                var waveType = meta.SceneLocationWaveType;
                SceneLocationPivotMetaTable[waveType].Add(meta);
            }
            
            OnLocationActivate(SceneTool.SceneLocationWaveType.FirstOnce);
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
}