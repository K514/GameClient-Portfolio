using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;
// using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;

namespace k514.Mono.Common
{
    public class AStarGeometry : GeometryBase
    {
        #region <Consts>

        public static (bool, GeometryModuleDataTableQuery.TableLabel, AStarGeometry) CreateModule(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return GeometryBase.CreateModule(new AStarGeometry(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, GeometryModuleDataTableQuery.TableLabel, AStarGeometry)> CreateModule(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await GeometryBase.CreateModule(new AStarGeometry(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Fields>

        /// <summary>
        /// AStar 길찾기 모듈
        /// </summary>
        // private AIPath _AStarGeometry;
        
        /// <summary>
        /// AStar 목적지 모듈
        /// </summary>
        // private AIDestinationSetter _AStarDestinationPreset;

        /// <summary>
        /// AStar 목적지 Affine
        /// </summary>
        private Transform _AStarDestinationAffine;
        
        #endregion
        
        #region <Constructor>

        private AStarGeometry(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GeometryModuleDataTableQuery.TableLabel.AStar, p_ModuleRecord, p_Entity)
        {
            _AStarDestinationAffine = new GameObject("AffineDestination").transform;
            _AStarDestinationAffine.DontDestroyOnLoadSafe();
#if UNITY_EDITOR
            _AStarDestinationAffine.hideFlags = HideFlags.HideInHierarchy;
#endif
            CheckAStarPath();
        }

        private void CheckAStarPath()
        {
            /*if (_Affine.GetSafeComponent(ref _AStarGeometry))
            {
                _AStarGeometry.canMove = false;
                var autoRepath = _AStarGeometry.autoRepath;
                autoRepath.maximumPeriod = 0f;
                _AStarGeometry.autoRepath = autoRepath;
                
                OnUpdateVolume();
            }
            if (_Affine.GetSafeComponent(ref _AStarDestinationPreset))
            {
                _AStarDestinationPreset.target = null;
            }*/
        }
        
        #endregion
        
        #region <Callbacks>

        protected override void _OnAwakeModule()
        {
            base._OnAwakeModule();
            
            CheckAStarPath();
        }
        
        private void OnUpdateVolume()
        {
            /*_AStarGeometry.radius = _Entity.Radius.DefaultValue;
            _AStarGeometry.height = _Entity.Height.DefaultValue;*/
        }

        /*protected override void OnDestinationUpdate()
        {
            base.OnDestinationUpdate();

            var destinationType = _CurrentDestinationType;
            switch (destinationType)
            {
                case GeometryTool.NavigateDestinationType.Position:
                    _AStarDestinationAffine.position = _CurrentDestination.DestinationPosition;
                    _AStarDestinationPreset.target = _AStarDestinationAffine;
                    break;
                case GeometryTool.NavigateDestinationType.Entity:
                    _AStarDestinationPreset.target = _CurrentDestination.DestinationEntity.Affine;
                    break;
            }
        }*/

        /*protected override void _OnNavigateEnd(GeometryTool.NavigationResultPreset p_Preset)
        {
            base._OnNavigateEnd(p_Preset);
            
            _AStarDestinationPreset.target = null;
        }*/

        protected override void OnDisposeUnmanaged()
        {
            if (_AStarDestinationAffine != null)
            {
                Object.DestroyImmediate(_AStarDestinationAffine.gameObject);
                _AStarDestinationAffine = null; 
            }
            
            base.OnDisposeUnmanaged();
        }

        #endregion

        #region <Methods>
        
        protected override bool IsEnterable(GeometryTool.NavigateDestinationPreset p_Preset)
        {
            if (base.IsEnterable(p_Preset))
            {
                var destinationType = p_Preset.DestinationType;
                switch (destinationType)
                {
                    default:
                    {
                        return true;
                    }
                    case GeometryTool.NavigateDestinationType.Position:
                    {
                        /*if (SceneEnvironmentManager.GetInstanceUnsafe.TryGetGamePlaySceneEnvironment(out var o_GameSceneEnv))
                        {
                            if (o_GameSceneEnv.IsOnAStarPath(p_Preset.DestinationPosition))
                            {
                                return true;
                            }
                        }*/
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        
        protected override GeometryTool.NavigationStatePreset GetNavigationState()
        {
            return default;

            /*var destinationType = _CurrentDestination.DestinationType;
            switch (destinationType)
            {
                default:
                case GeometryTool.NavigateDestinationType.None:
                    return default;
                case GeometryTool.NavigateDestinationType.Position:
                {
                    var uv = _Entity.GetDirectionXZUnitVectorTo(_AStarGeometry.steeringTarget);
                    var sqrDistance = _Entity.GetSqrXZDistanceTo(_CurrentDestination.DestinationPosition);

                    return new GeometryTool.NavigationStatePreset(uv, sqrDistance, _AStarGeometry.reachedDestination);
                }
                case GeometryTool.NavigateDestinationType.Entity:
                {
                    var destinationEntity = _CurrentDestination.DestinationEntity;
                    if (destinationEntity.IsValid())
                    {
                        var uv = _Entity.GetDirectionXZUnitVectorTo(_AStarGeometry.steeringTarget);
                        var sqrDistance = InteractManager.GetInstanceUnsafe.GetSqrXZDistanceBetween(_Entity, destinationEntity);

                        return new GeometryTool.NavigationStatePreset(uv, sqrDistance, _AStarGeometry.reachedDestination);
                    }
                    else
                    {
                        _CurrentDestination.DestinationType = GeometryTool.NavigateDestinationType.Position;

                        var uv = _Entity.GetDirectionXZUnitVectorTo(_AStarGeometry.steeringTarget);
                        var sqrDistance = _Entity.GetSqrXZDistanceTo(_CurrentDestination.DestinationPosition);

                        return new GeometryTool.NavigationStatePreset(uv, sqrDistance, _AStarGeometry.reachedDestination);
                    }
                }
            }*/
        }

        #endregion
    }
}