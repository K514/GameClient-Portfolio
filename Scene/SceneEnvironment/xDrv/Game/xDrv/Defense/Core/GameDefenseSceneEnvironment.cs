
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using UniRx;
using UnityEngine;

namespace k514.Mono.Feature
{
    public abstract class GameDefenseSceneEnvironment : GameSceneEnvironmentBase
    {
        #region <Consts>

        private const float __FirstWaveInterval = 5f;
        private const float __WaveInterval = 30f;
        private const int __WaveCount = 5;

        #endregion
        
        #region <Fields>

        private ProgressTimer _Counter;
        private int _CurrentWave;
        protected Subject<object> _Subject = new Subject<object>();

        public Subject<object> GetSubject => _Subject;
        public ProgressTimer GetTimer => _Counter;
        public (int, int) CurrentWave => (_CurrentWave, __WaveCount);
        #endregion

        #region <Callbacks>

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await base.OnScenePreload(p_CancellationToken);
            
            GameManager.GetInstanceUnsafe.SetGameControl(GameManagerTool.GameControl.Dungeon);
            GameManager.GetInstanceUnsafe.SetStageType(GameManagerTool.StageType.Normal);
            GameManager.GetInstanceUnsafe.AddGold(5000);
        }
        
        protected override void OnCreateGameSceneEnvironment()
        {
            base.OnCreateGameSceneEnvironment();

            _Counter = __WaveInterval;
            _Counter.Progress(__WaveInterval - __FirstWaveInterval);
        }

        protected override void OnUpdateSceneStart(float p_DeltaTime)
        {
            base.OnUpdateSceneStart(p_DeltaTime);

            if (!IsWaveOver())
            {    
                if (_Counter.IsOver())
                {
                    _Counter.Reset();
                    _CurrentWave++;
                    SetLocationPhase(LocationBase.LocationPhase.LocationActivate);
                    _Subject.OnNext(_CurrentWave);
                }
                else
                {
                    _Counter.Progress(p_DeltaTime);
                }
            }
        }

        #endregion

        #region <Methods>

        public bool IsWaveOver() => _CurrentWave >= __WaveCount;
        
        public override void CheckStageClear()
        {
            if (IsWaveOver())
            {
                base.CheckStageClear();
            }
            else
            {
                if (IsLocationEliminate(SceneTool.SceneLocationType.ZakoSpawner)
                    && IsLocationEliminate(SceneTool.SceneLocationType.ChampSpawner)
                    && IsLocationEliminate(SceneTool.SceneLocationType.BossSpawner))
                {
                    _Counter = __WaveInterval;
                    _Counter.Progress(__WaveInterval - __FirstWaveInterval);
                }
            }
        }
        
        #endregion
    }
}