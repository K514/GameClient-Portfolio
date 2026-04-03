using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Common;
using k514.Mono.Feature;
using xk514;

namespace k514
{
    public partial class SystemBoot
    {
        #region <Singleton/Type/Level0>

        /// <summary>
        /// [시스템 메타 싱글톤] 타입 리스트
        /// 
        /// 시스템 초기화, 부팅에 필요한 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _SystemMetaSingletonTypeSet =
        {
            typeof(SceneChangeEventManager),
        };

        /// <summary>
        /// [EventSenderManager] 타입 리스트
        ///
        /// 특정 이벤트를 전파하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _EventSenderManagerTypeSet =
            typeof(EventSenderManagerBase<,,,,>).GetSubTypeSet()
                .Concat(typeof(EventSenderManagerBase<,,,,,,>).GetSubTypeSet())
                .Concat(typeof(AsyncEventSenderManagerBase<,,,,>).GetSubTypeSet())
                .Concat(typeof(AsyncEventSenderManagerBase<,,,,,,>).GetSubTypeSet())
                .ToArray();
        
#if UNITY_EDITOR
        /// <summary>
        /// [에디터 테이블] 타입 리스트
        ///
        /// 개발 환경을 구성하는 에디터 기능을 기술하는 테이블의 전체 리스트
        /// </summary>
        private static readonly Type[] _EditorTableTypeSet = typeof(EditorOnlyTable<,,,>).GetSubTypeSet();
#endif
      
        /// <summary>
        /// [시스템 기저 싱글톤] 타입 리스트
        /// 
        /// [시스템 메타 싱글톤] + [이벤트 송신자] + [에디터 테이블]
        /// </summary>
        private static readonly Type[] _SystemBasisSingletonTypeSet =
#if UNITY_EDITOR
            _SystemMetaSingletonTypeSet
                .Concat
                (
                    _EventSenderManagerTypeSet
                )
                .Concat
                (
                    _EditorTableTypeSet
                )
                .ToArray();
#else
            _SystemMetaSingletonTypeSet
                .Concat
                (
                    _EventSenderManagerTypeSet
                )
                .ToArray();
#endif

        /// <summary>
        /// 계정매니저 타입 리스트
        /// </summary>
        private static readonly Type[] _AccountManagerTypeSet =
        {
            typeof(DefaultAccountDataTable),
        };
            
        #endregion
 
        #region <Singleton/Type/Level1>

        /// <summary>
        /// 첫번째로 초기화되어야 하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _FirstPassSingletonTypeSet =
        {
            typeof(AssetBundleDownloader), typeof(AssetLoaderManager), typeof(ResourceListTable), typeof(SceneFaderManager),
        };
        
        /// <summary>
        /// 두번째로 초기화되어야 하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _SecondPassSingletonTypeSet = 
            typeof(SystemTable<,,,>)
                .GetSubTypeSet()
                .Except(_FirstPassSingletonTypeSet)
                .Concat(_AccountManagerTypeSet)
                .ToArray();

        
        /// <summary>
        /// 세번째로 초기화되어야 하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _ThirdPassSingletonTypeSet = 
            typeof(ResourceNameMapTable<,,,,>)
                .GetSubTypeSet()
                .ToArray();

        #endregion

        #region <Singleton/Type/Level2>

        /// <summary>
        /// 씬 전이 및 씬 초기화 등을 담당하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _SceneManagerGroup =
        {
            // 씬이 전이된 경우, 해당 씬에 로컬 관리자 오브젝트를 생성하는 매니저
            typeof(SceneEnvironmentManager),
            // 씬을 전이하는 기능을 제공하는 매니저
            typeof(SceneChangeManager),
        };

        /// <summary>
        /// 메인 카메라의 제어 및 카메라 연출을 담당하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _CameraManagerGroup =
        {
            typeof(CameraManager),
        };
        
        /// <summary>
        /// 오디오를 담당하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _AudioManagerGroup =
        {
            typeof(AudioManager),
        };
                     
        /// <summary>
        /// 게임 벨류 테이블 리스트
        /// </summary>
        private static readonly Type[] _GameValueTableGroup = 
            typeof(GameValueTableBase<,,>)
                .GetSubTypeSet()
                .ToArray();

        /// <summary>
        /// 게임 진행시 필요한 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _GameManagerGroup =
        {
            typeof(DefaultGameConfigureManager),
            
            typeof(InputLayerManager), typeof(KeyboardManager),
            
            typeof(GameManager), typeof(PlayerManager), typeof(BossManager),
            typeof(DungeonDataTableQuery), typeof(EnvironmentManager), typeof(InteractManager),
            
            typeof(ItemRankDataTable), typeof(ItemGroupCooldownDataTable), typeof(ItemGroupDataTable),
        };

        /// <summary>
        /// 이벤트 스토리지 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _EventStorageGroup =
        {
            typeof(GameEntityAffineControlStorage), typeof(GameEntityDeployStorage), 
            typeof(GameEntityExtraOptionStorage), typeof(GameEntityItemStorage), 
            typeof(GameEntityEnchantStorage), 
        };
        
        /// <summary>
        /// 모델 테이블 그룹
        /// </summary>
        private static readonly Type[] _ModelTableQueryGroup =
        {
            typeof(PrefabModelDataTableQuery), 
            typeof(WorldObjectModelDataTableQuery), 
            typeof(GameEntityModelDataTableQuery),
            typeof(ParticleModelDataTableQuery), 
            typeof(UnitModelDataTableQuery),
            typeof(MonsterModelDataTableQuery)
        };
        
        /// <summary>
        /// 컴포넌트 테이블 그룹
        /// </summary>
        private static readonly Type[] _ComponentTableQueryGroup =
        {
            typeof(PrefabComponentDataTableQuery), 
            typeof(WorldObjectComponentDataTableQuery), 
            typeof(ProjectorComponentDataTableQuery), 
            typeof(GameEntityComponentDataTableQuery),
            typeof(VfxComponentDataTableQuery), typeof(BeamComponentDataTableQuery), typeof(ProjectileComponentDataTableQuery), 
            typeof(UnitComponentDataTableQuery), 
        };

        /// <summary>
        /// 게임 엔터티 이벤트 매니저 그룹
        /// </summary>
        private static readonly Type[] _GameEntityEventManagerType = typeof(GameEntityEventManagerBase<,,,,>).GetSubTypeSet();
                
        /// <summary>
        /// 오브젝트 풀 타입
        /// </summary>
        private static readonly Type[] _ObjectPoolType = 
            _GameEntityEventManagerType
                .Concat(_ModelTableQueryGroup)
                .Concat(_ComponentTableQueryGroup)
                .Concat(typeof(SpawnTablePoolManagerBase<,,,,,,,,>).GetSubTypeSet())
                .Concat(new []{ typeof(DefaultObjectPoolManager) })
                .ToArray();

#if !SERVER_DRIVE
        /// <summary>
        /// UI 기능을 담당하는 싱글톤 타입 리스트
        /// </summary>
        private static readonly Type[] _UIManagerGroup =
        {
            typeof(UIxControlRoot),
        };
#endif
        
        #endregion
        
        #region <Method/Load>

        public static async UniTask<bool> LoadSystemBasisSingleton(CancellationToken p_CancellationToken)
        {
            var (basisResult, _) = await SingletonTool.CreateSingletonAsync(_SystemBasisSingletonTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
            if (basisResult)
            {
                var (firstResult, _) = await SingletonTool.CreateSingletonAsync(_FirstPassSingletonTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
                if (firstResult)
                {
                    var (secondResult, _) = await SingletonTool.CreateSingletonAsync(_SecondPassSingletonTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
                    if (secondResult)
                    {
                        var (thirdResult, _) = await SingletonTool.CreateSingletonAsync(_ThirdPassSingletonTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
                        return thirdResult;
                    }
                }
            }
            
            return false;
        }

        public static async UniTask<bool> LoadEditorWindowNeededTable(CancellationToken p_CancellationToken)
        {
            var (firstResult, _) = await SingletonTool.CreateSingletonAsync(_FirstPassSingletonTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
            if (firstResult)
            {
                var (secondResult, _) = await SingletonTool.CreateSingletonAsync(_SecondPassSingletonTypeSet, MultiTaskMode.Sequence, p_CancellationToken);
                return secondResult;
            }
            else
            {
                return false;
            }
        }
        
        public static async UniTask<bool> LoadSystemLoop(CancellationToken p_CancellationToken)
        {
            var (result, _) = await SingletonTool.TryCreateSingletonAsync(typeof(SystemLoop), p_CancellationToken);
            return result;
        }
        
        public static async UniTask<bool> LoadSceneManager(CancellationToken p_CancellationToken)
        {
            var (result, _) = await SingletonTool.CreateSingletonAsync(_SceneManagerGroup, MultiTaskMode.Sequence, p_CancellationToken);
            return result;
        }
        
        public static async UniTask<bool> LoadGameManager(CancellationToken p_CancellationToken)
        {
            var (firstResult, _) = await SingletonTool.CreateSingletonAsync(_GameValueTableGroup, MultiTaskMode.Sequence, p_CancellationToken);
            if (firstResult)
            {
                var (secondResult, _) = await SingletonTool.CreateSingletonAsync(_GameManagerGroup, MultiTaskMode.Sequence, p_CancellationToken);
                if (secondResult)
                {
                    var (thirdResult, _) = await SingletonTool.CreateSingletonAsync(_ObjectPoolType, MultiTaskMode.Sequence, p_CancellationToken);
                    if (thirdResult)
                    {
                        var (fourthResult, _) = await SingletonTool.CreateSingletonAsync(_EventStorageGroup, MultiTaskMode.Sequence, p_CancellationToken);
                        if (fourthResult)
                        {
                            _systemState.AddFlag(SystemStateFlag.GameManagerLoaded);
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        public static async UniTask<bool> LoadCameraManager(CancellationToken p_CancellationToken)
        {
#if SERVER_DRIVE
            return false;
#else
            var (result, _) = await SingletonTool.CreateSingletonAsync(_CameraManagerGroup, MultiTaskMode.Sequence, p_CancellationToken);
            return result;
#endif
        }
        
        public static async UniTask<bool> LoadAudioManager(CancellationToken p_CancellationToken)
        {
#if SERVER_DRIVE
            return false;
#else
            var (result, _) = await SingletonTool.CreateSingletonAsync(_AudioManagerGroup, MultiTaskMode.Sequence, p_CancellationToken);
            if (result)
            {
                await AudioManager.GetInstanceUnsafe.LoadEntities(p_CancellationToken);
            }
            return result;
#endif
        }
        
        public static async UniTask<bool> LoadUI(CancellationToken p_CancellationToken)
        {
#if SERVER_DRIVE
            return false;
#else
            var (result, _) = await SingletonTool.CreateSingletonAsync(_UIManagerGroup, MultiTaskMode.Sequence, p_CancellationToken);
            return result;
#endif
        }

#if UNITY_EDITOR && APPLY_TEST_MANAGER
        public static async UniTask<bool> LoadTestManager(CancellationToken p_CancellationToken)
        {
            var (result, _) = await SingletonTool.TryCreateSingletonAsync(typeof(TestManager), p_CancellationToken);
            return result;
        }
#endif
        
        #endregion

        #region <Method/Dispose>

        public static void DisposeGameManager()
        {
            SingletonTool.DisposeSingleton(_EventStorageGroup);
            SingletonTool.DisposeSingleton(_ObjectPoolType);
            SingletonTool.DisposeSingleton(_GameManagerGroup);
            SingletonTool.DisposeSingleton(_GameValueTableGroup);
            
            _systemState.RemoveFlag(SystemStateFlag.GameManagerLoaded);
        }

#if UNITY_EDITOR && APPLY_TEST_MANAGER
        public static void DisposeTestManager()
        {
            SingletonTool.DisposeSingleton(typeof(TestManager));
        }
#endif
        
        #endregion
    }
}