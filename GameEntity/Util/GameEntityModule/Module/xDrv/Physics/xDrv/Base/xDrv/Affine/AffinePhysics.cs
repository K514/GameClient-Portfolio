using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public partial class AffinePhysics : PhysicsBase
    {
        #region <Consts>

        public static (bool, PhysicsModuleDataTableQuery.TableLabel, AffinePhysics) CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return PhysicsBase.CreateModule(new AffinePhysics(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, PhysicsModuleDataTableQuery.TableLabel, AffinePhysics)> CreateModule(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await PhysicsBase.CreateModule(new AffinePhysics(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 물리 모듈 레코드
        /// </summary>
        private AffinePhysicsModuleDataTable.TableRecord _PhysicsRecord;

        #endregion
        
        #region <Constructor>
        
        private AffinePhysics(IPhysicsModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(PhysicsModuleDataTableQuery.TableLabel.Affine, p_ModuleRecord, p_Entity)
        {
            _PhysicsRecord = (AffinePhysicsModuleDataTable.TableRecord) p_ModuleRecord;
        }

        #endregion
    }
}