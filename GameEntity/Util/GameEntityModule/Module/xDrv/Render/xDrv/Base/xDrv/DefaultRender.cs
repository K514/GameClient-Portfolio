using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class DefaultRender : RenderBase
    {
        #region <Consts>

        public static (bool, RenderModuleDataTableQuery.TableLabel, DefaultRender) CreateModule(IRenderModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return RenderBase.CreateModule(new DefaultRender(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, RenderModuleDataTableQuery.TableLabel, DefaultRender)> CreateModule(IRenderModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await RenderBase.CreateModule(new DefaultRender(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion

        #region <Constructor>

        private DefaultRender(IRenderModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(RenderModuleDataTableQuery.TableLabel.Default, p_ModuleRecord, p_Entity)
        {
        }

        #endregion
    }
}