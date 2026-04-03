using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class GeometryBase : GameEntityModuleBase, IGeometryModule
    {
        #region <Consts>

        protected static (bool, GeometryModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : GeometryBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, GeometryModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._GeometryModuleType, p_Module);
            }
        }

        protected static async UniTask<(bool, GeometryModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : GeometryBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, GeometryModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._GeometryModuleType, p_Module);
            }
        }

        #endregion
        
        #region <Fields>

        private GeometryModuleDataTableQuery.TableLabel _GeometryModuleType;
        private IGeometryModuleDataTableRecordBridge _GeometryModuleRecord;
        
        #endregion

        #region <Constructor>

        protected GeometryBase(GeometryModuleDataTableQuery.TableLabel p_ModuleType, IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Geometry, p_ModuleRecord, p_Entity)
        {
            _GeometryModuleType = p_ModuleType;
            _GeometryModuleRecord = p_ModuleRecord;

            OnCreateNavigate();
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            OnAwakeNavigate();
        }
        
        protected override void _OnSleepModule()
        {
            OnSleepNavigate();
        }

        protected override void _OnResetModule()
        {
        }

        public override void OnModule_Update(float p_DeltaTime)
        {
            base.OnModule_Update(p_DeltaTime);

            OnUpdateNavigate(p_DeltaTime);
        }

        #endregion

        #region <Methods>

        public GeometryModuleDataTableQuery.TableLabel GetGeometryModuleType()
        {
            return _GeometryModuleType;
        }
        
        #endregion
    }
}