using System.Threading;
using Cysharp.Threading.Tasks;

namespace k514.Mono.Common
{
    public class AnimationModuleCluster : GameEntityModuleClusterBase<AnimationModuleCluster, AnimationModuleDataTableQuery.TableLabel, IAnimationModuleDataTableBridge, IAnimationModuleDataTableRecordBridge, IAnimationModule>
    {
        #region <Consts>

        static AnimationModuleCluster()
        {
            _NullModule = new NullAnimation();
        }
        
        #endregion
        
        #region <Constructor>

        public AnimationModuleCluster(IGameEntityBridge p_Entity) : base(p_Entity, GameEntityModuleTool.ModuleType.Animation, AnimationModuleDataTableQuery.GetInstanceUnsafe)
        {
        }

        #endregion
        
        #region <Methods>

        protected override (bool, AnimationModuleDataTableQuery.TableLabel, IAnimationModule) SpawnModule(int p_Index)
        {
            if (AnimationModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case AnimationModuleDataTableQuery.TableLabel.AnimatorAnimation:
                        return AnimatorAnimation.CreateModule(o_Record, _GameEntity);
                }
            }

            return default;
        }

        protected override async UniTask<(bool, AnimationModuleDataTableQuery.TableLabel, IAnimationModule)> SpawnModule(int p_Index, CancellationToken p_CancellationToken)
        {
            if (AnimationModuleDataTableQuery.GetInstanceUnsafe.TryGetLabelContext(p_Index, out var o_Label, out var o_Table, out var o_Record))
            {
                switch (o_Label)
                {
                    case AnimationModuleDataTableQuery.TableLabel.AnimatorAnimation:
                        return await AnimatorAnimation.CreateModule(o_Record, _GameEntity, p_CancellationToken);
                }
            }

            return default;
        }
        
        #endregion
    }
}