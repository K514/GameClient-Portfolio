using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514
{
    /// <summary>
    /// 씬 단위로 수명을 가지는 게임 데이터 테이블 싱글톤들을 제어하는 싱글톤 클래스
    /// 기본적으로 싱글톤들은 게임오브젝트나 프리팹과 달리 생명주기를 제어해주는 객체가 없으므로
    /// 해당 싱글톤 클래스에서 제어해준다.
    /// </summary>
    public class TableManager : SceneChangeEventReceiveAsyncSingleton<TableManager>
    {
        #region <Fields>

        /// <summary>
        /// 씬 단위 수명을 가지는 현재 활성화된 싱글톤 그룹
        /// </summary>
        private List<ITable> _SceneGameDataSingletonGroups;
        
        #endregion
        
        #region <Callbacks>

        protected override async UniTask OnCreated(CancellationToken p_CancellationToken)
        {
            await base.OnCreated(p_CancellationToken);

            _SceneGameDataSingletonGroups = new List<ITable>();
        }

        protected override async UniTask OnInitiate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnScenePreload(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneStart(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTerminate(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        public override async UniTask OnSceneTransition(CancellationToken p_CancellationToken)
        {
            await UniTask.CompletedTask;
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// 씬 단위 테이블을 등록하는 메서드
        /// </summary>
        public void AddSceneLifeCycleTable(ITable p_GameDataTable)
        {
            if (!_SceneGameDataSingletonGroups.Contains(p_GameDataTable))
            {
                _SceneGameDataSingletonGroups.Add(p_GameDataTable);
            }
        }
        
        /// <summary>
        /// 등록된 테이블을 제거하는 메서드
        /// </summary>
        public void RemoveSceneLifeCycleTable(ITable p_GameDataTable)
        {
            _SceneGameDataSingletonGroups.Remove(p_GameDataTable);
        }
        
        /// <summary>
        /// 현재 활성화 중인 씬 단위 테이블 데이터를 릴리스하는 메서드
        /// </summary>
        public async UniTask Clear_SceneLifeCycle_TableSingleton()
        {
            var singletonCount = _SceneGameDataSingletonGroups.Count;
            for (int i = singletonCount - 1; i > -1; i--)
            {
                var targetSingleton = _SceneGameDataSingletonGroups[i];
                targetSingleton.Dispose();
            }
        }
        
        #endregion
    }
}