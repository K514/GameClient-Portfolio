using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
 {
     public class SlaveRole : RoleBase
     {
         #region <Consts>
 
         public static (bool, RoleModuleDataTableQuery.TableLabel, SlaveRole) CreateModule(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
         {
             return RoleBase.CreateModule(new SlaveRole(p_ModuleRecord, p_Entity));
         }
         
         public static async UniTask<(bool, RoleModuleDataTableQuery.TableLabel, SlaveRole)> CreateModule(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
         {
             return await RoleBase.CreateModule(new SlaveRole(p_ModuleRecord, p_Entity), p_CancellationToken);
         }
 
         #endregion
         
         #region <Fields>
  
         private SlaveModuleDataTable.TableRecord SlaveModuleRecord;
         
         #endregion
 
         #region <Constructor>
 
         private SlaveRole(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(RoleModuleDataTableQuery.TableLabel.Slave, p_ModuleRecord, p_Entity)
         {
             SlaveModuleRecord = (SlaveModuleDataTable.TableRecord) p_ModuleRecord;
         } 
 
         #endregion
         
         #region <Callbacks>
 
         protected override void _OnAwakeModule()
         {
             /*base._OnAwakeModule();

             _Entity.UpdateScaleFactor(ModuleRecord.ExtraScale);
             switch (ModuleRecord.MasterContractType)
             {
                 case SlaveModuleTable.MasterContractType.None:
                 case SlaveModuleTable.MasterContractType.FirstEncounter:
                 case SlaveModuleTable.MasterContractType.Player:
#if !SERVER_DRIVE
                     _Entity.MindModule.SetSlaveMasterUnit(PlayerManager.GetInstanceUnsafe.Player);
#endif
                     break;
             }*/
         }
  
         protected override void _OnSleepModule()
         {
         }
 
         protected override void _OnResetModule()
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