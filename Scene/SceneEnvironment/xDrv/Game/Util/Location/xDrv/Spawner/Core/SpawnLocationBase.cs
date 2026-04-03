using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public abstract class SpawnLocationBase : LocationBase
    {
        #region <Fields>

        public List<IGameEntityBridge> SpawnEntityList { get; private set; }
        protected GameEntityBaseEventReceiver _GameEntityEventReceiver;
        public Subject<IGameEntityBridge> _SpawnEntity { get; private set; }
        public Subject<SpawnLocationBase> _TriggerLocationBase { get; private set; }
        public int SpawnedEntityCount { get; private set; }
        public int DeadEntityCount { get; private set; }
        
        #endregion
        
        #region <Callbacks>

        protected override void Awake()
        {
            base.Awake();
             
            SpawnEntityList = new List<IGameEntityBridge>(GameConst.__CAPACITY_SPAWNER);
            _GameEntityEventReceiver = new GameEntityBaseEventReceiver(GameEntityTool.GameEntityBaseEventType.Dead, OnHandleGameEntityBaseEvent);
            _SpawnEntity = new Subject<IGameEntityBridge>();
            _TriggerLocationBase = new Subject<SpawnLocationBase>();
        }

        private void OnHandleGameEntityBaseEvent(GameEntityTool.GameEntityBaseEventType p_Type, GameEntityBaseEventParams p_Params)
        {
            switch (p_Type)
            {
                case GameEntityTool.GameEntityBaseEventType.Dead:
                {
                    var deadEntity = p_Params.Trigger;

                    DeadEntityCount++;

                    SpawnEntityList.Remove(deadEntity);
                    _SpawnEntity.OnNext(deadEntity);
           
                    CheckEliminate();
                    break;
                }
            }
        }
        
        protected override void OnDisposeUnmanaged()
        {
            base.OnDisposeUnmanaged();

            if (!ReferenceEquals(null, _GameEntityEventReceiver))
            {
                _GameEntityEventReceiver.Dispose();
                _GameEntityEventReceiver = null;
            }
        }
        
        #endregion

        #region <Methods>

        protected override void PreloadLocation(List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
            foreach (var sceneLocationPivotMeta in p_MetaSet)
            {
                var spawnEntityIndexList = sceneLocationPivotMeta.SpawnEntityList;
                if (spawnEntityIndexList.TryGetRandomElement(out var o_Index))
                {
                    PreloadEntity(o_Index);
                }
            }
        }
        
        protected override void ActivateLocation(SceneTool.SceneLocationWaveType p_Type, List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
            switch (p_Type)
            {
                case SceneTool.SceneLocationWaveType.Default:
                {
                    SpawnWave(p_MetaSet);
                    CheckEliminate();
                    break;
                }
                default:
                case SceneTool.SceneLocationWaveType.FirstOnce:
                {
                    SpawnWave(p_MetaSet);
                    break;
                }
            }
        }
        
        private void CheckEliminate()
        {
            if (!IsEliminate() && SpawnEntityList.Count < 1)
            {
                SetPhase(LocationPhase.Eliminated);

                if (SceneEnvironmentManager.GetInstanceUnsafe.TryGetGamePlaySceneEnvironment(out var o_SceneEnv))
                {
                    o_SceneEnv.CheckStageClear();
                }
            }
        }
        
        private void SpawnWave(List<SceneTool.SceneLocationPivotMeta> p_MetaSet)
        {
            if (p_MetaSet.CheckCollectionSafe())
            {
                _TriggerLocationBase.OnNext(this);
                
                foreach (var sceneLocationPivotMeta in p_MetaSet)
                {
                    var spawnEntityIndexList = sceneLocationPivotMeta.SpawnEntityList;
                    if (spawnEntityIndexList.TryGetRandomElement(out var o_Index))
                    {
                        var spawned = SpawnEntity(o_Index, sceneLocationPivotMeta.GetAffinePreset());
                        
                        SpawnedEntityCount++;
                        
                        SpawnEntityList.Add(spawned);
                        _SpawnEntity.OnNext(spawned);
                    }
                }
            }
            else
            {
                SetPhase(LocationPhase.Eliminated);
            }
        }

        public void SetSpawnEntity(Action<IGameEntityBridge> p_CallBack)
        {
            _SpawnEntity.Subscribe(p_CallBack);
            for(int i = 0; i < SpawnEntityList.Count; i++) 
            {
                p_CallBack?.Invoke(SpawnEntityList[i]);
            }
        }

        protected abstract void PreloadEntity(int p_Index);
        protected abstract IGameEntityBridge SpawnEntity(int p_Index, AffinePreset p_AffinePreset);

        #endregion
    }
}