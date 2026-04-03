using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public abstract partial class RenderBase : GameEntityModuleBase, IRenderModule
    {
        #region <Consts>

        /// <summary>
        /// 텍스처 소거 선딜레이
        /// </summary>
        private const int _Dead_Dissolve_PreDelay_Msec = 2000;
        
        /// <summary>
        /// 텍스처 소거 전체 시간
        /// </summary>
        private const int _Dead_Dissolve_Msec = 1500;

        protected static (bool, RenderModuleDataTableQuery.TableLabel, Module) CreateModule<Module>(Module p_Module)
            where Module : RenderBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, RenderModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._RenderModuleType, p_Module);
            }
        }
        
        protected static async UniTask<(bool, RenderModuleDataTableQuery.TableLabel, Module)> CreateModule<Module>(Module p_Module, CancellationToken p_CancellationToken)
            where Module : RenderBase
        {
            if (ReferenceEquals(null, p_Module))
            {
                return (false, RenderModuleDataTableQuery.TableLabel.None, default);
            }
            else
            {
                return (true, p_Module._RenderModuleType, p_Module);
            }
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// 해당 랜더 모듈의 타입
        /// </summary>
        private RenderModuleDataTableQuery.TableLabel _RenderModuleType;

        /// <summary>
        /// 해당 모듈을 기술하는 테이블 레코드
        /// </summary>
        private IRenderModuleDataTableRecordBridge _RenderModuleRecord;

#if !SERVER_DRIVE
        /// <summary>
        /// 해당 모듈의 초기화 시점에서 마스터 노드가 가지고 있었던 랜더러 프리셋
        /// </summary>
        private RenderableTool.RendererControlPreset _DefaultModelRendererControlPreset;

        /// <summary>
        /// 초기화 이후 새로이 등록된 랜더러 프리셋
        /// </summary>
        private RenderableTool.RendererControlPreset _AddedModelRendererControlPreset;
#endif
        
        #endregion

        #region <Constructor>

        protected RenderBase(RenderModuleDataTableQuery.TableLabel p_ModuleType, IRenderModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GameEntityModuleTool.ModuleType.Render, p_ModuleRecord, p_Entity)
        {
            /*_RenderType = p_ModuleType;
            _ModuleRecord = p_ModuleRecord;

#if !SERVER_DRIVE
            _DefaultModelRendererControlPreset = new RenderableTool.RendererControlPreset(_Entity.gameObject);
            _AddedModelRendererControlPreset = new RenderableTool.RendererControlPreset();

            // 각 프리팹은 초기화 시, 원본 머티리얼을 직접 참조하여 공유하기 때문에
            // 각종 랜더링 연출을 위해 머티리얼 값을 수정하는 경우 해당 머티리얼을 공유하는 모든 오브젝트에 영향을 준다.
            // 따라서 해당 모델이 보유한 모든 머티리얼을 복사하여 참조시켜야 하며 해당 함수는 _DefaultModelRendererControlPreset 내부의 SwitchMaterial를 활용할 것.
            // 아래의 SetShader함수는 셰이더를 교체하면서 SwitchMaterial과 동일한 기능을 수행한다.
            SetShader(RenderableTool.RenderGroupType.OriginGroup, p_ModuleRecord.ShaderTypeMask);
            
            // OnInitializeAttachVfx();
            OnInitializeStackTimer();
#endif*/
        }

        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
/*#if !SERVER_DRIVE
            _Entity.ReserveUpdateCameraInteract();
#endif*/
        }

        protected override void _OnSleepModule()
        {
/*#if !SERVER_DRIVE
            _DissolveTimerHandler.TerminateEventHandler();
            ResetRenderEffect();
            _Entity.ReserveUpdateCameraInteract();
#endif*/
        }

        protected override void _OnResetModule()
        {
        }

        public override void OnModule_Hit(IGameEntityBridge p_Trigger, DamageCalculator.HitResult p_HitResult)
        {
#if !SERVER_DRIVE
            SetRenderEffect(RenderableTool.ShaderControlType.Intensity, 1);
#endif
        }

        public override void OnModule_HitMotion_Start()
        {
#if !SERVER_DRIVE
            SetRenderEffect(RenderableTool.ShaderControlType.Intensity, 1);
#endif
        }

        public override void OnModule_HitMotion_Over()
        {
#if !SERVER_DRIVE
            ClearRenderEffect(RenderableTool.ShaderControlType.Intensity);
#endif
        }
    
        #endregion

        #region <Methods>

        public RenderModuleDataTableQuery.TableLabel GetRenderModuleType()
        {
            return _RenderModuleType;
        }

        #endregion
    }
}