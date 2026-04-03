using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
 {
     public class DefaultRole : RoleBase
     {
         #region <Consts>
 
         public static (bool, RoleModuleDataTableQuery.TableLabel, DefaultRole) CreateModule(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
         {
             return RoleBase.CreateModule(new DefaultRole(p_ModuleRecord, p_Entity));
         }
         
         public static async UniTask<(bool, RoleModuleDataTableQuery.TableLabel, DefaultRole)> CreateModule(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
         {
             return await RoleBase.CreateModule(new DefaultRole(p_ModuleRecord, p_Entity), p_CancellationToken);
         }
 
         #endregion
         
         #region <Fields>
  
         private DefaultRoleModuleDataTable.TableRecord DefaultModuleRecord;
         
         #endregion
 
         #region <Constructor>
 
         private DefaultRole(IRoleModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(RoleModuleDataTableQuery.TableLabel.Default, p_ModuleRecord, p_Entity)
         {
             DefaultModuleRecord = (DefaultRoleModuleDataTable.TableRecord) p_ModuleRecord;
         } 
 
         #endregion
         
         #region <Callbacks>
 
         protected override void _OnAwakeModule()
         {
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