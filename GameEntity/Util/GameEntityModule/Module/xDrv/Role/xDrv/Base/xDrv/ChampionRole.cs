using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class ChampionRole : RoleBase
    {
        #region <Consts>

        public static (bool, RoleModuleDataTableQuery.TableLabel, ChampionRole) CreateModule(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return RoleBase.CreateModule(new ChampionRole(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, RoleModuleDataTableQuery.TableLabel, ChampionRole)> CreateModule(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await RoleBase.CreateModule(new ChampionRole(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>
 
        private EliteModuleDataTable.TableRecord EliteModuleRecord;
    
        #endregion

        #region <Constructor>

        private ChampionRole(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(RoleModuleDataTableQuery.TableLabel.Champion, p_ModuleRecord, p_Entity)
        {
            EliteModuleRecord = (EliteModuleDataTable.TableRecord) p_ModuleRecord;
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            /*base._OnAwakeModule();
            
            _Entity.AddElement(ModuleRecord.ElementType);
            _Entity.UpdateScaleFactor(ModuleRecord.ExtraScale);*/
        }
 
        protected override void _OnSleepModule()
        {
        }

        protected override void _OnResetModule()
        {
        }

        /// <summary>
        /// 인스턴스가 파기될 때 수행할 작업을 기술한다.
        /// </summary>
        protected override void OnDisposeUnmanaged()
        {
        }
 
        #endregion

        #region <Methods>
                
#if !SERVER_DRIVE
        protected override string GetPrefix()
        {
            return string.Empty;
        }
#endif

        #endregion
    }
}