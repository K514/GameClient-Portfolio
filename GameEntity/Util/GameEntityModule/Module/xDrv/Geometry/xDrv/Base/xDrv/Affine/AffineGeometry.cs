using System.Threading;
using Cysharp.Threading.Tasks;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    public class AffineGeometry : GeometryBase
    {
        #region <Consts>

        public static (bool, GeometryModuleDataTableQuery.TableLabel, AffineGeometry) CreateModule(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity)
        {
            return GeometryBase.CreateModule(new AffineGeometry(p_ModuleRecord, p_Entity));
        }
        
        public static async UniTask<(bool, GeometryModuleDataTableQuery.TableLabel, AffineGeometry)> CreateModule(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity, CancellationToken p_CancellationToken)
        {
            return await GeometryBase.CreateModule(new AffineGeometry(p_ModuleRecord, p_Entity), p_CancellationToken);
        }

        #endregion
        
        #region <Constructor>

        private AffineGeometry(IGeometryModuleDataTableRecordBridge p_ModuleRecord, IGameEntityBridge p_Entity) : base(GeometryModuleDataTableQuery.TableLabel.Affine, p_ModuleRecord, p_Entity)
        {
        }

        #endregion

        #region <Methods>

        protected override GeometryTool.NavigationStatePreset GetNavigationState()
        {
            var destinationType = _CurrentDestination.DestinationType;
            switch (destinationType)
            {
                default:
                case GeometryTool.NavigateDestinationType.None:
                    return default;
                case GeometryTool.NavigateDestinationType.Position:
                {
                    var direction = Entity.GetDirectionXZVectorTo(_CurrentDestination.DestinationPosition);
                    var sqrDistance = direction.sqrMagnitude;
               
                    return new GeometryTool.NavigationStatePreset(direction.normalized, sqrDistance, false);
                }
                case GeometryTool.NavigateDestinationType.Entity:
                {
                    var destinationEntity = _CurrentDestination.DestinationEntity;
                    if (destinationEntity.IsEntityValid())
                    {
                        var direction = Entity.GetDirectionXZVectorTo(_CurrentDestination.DestinationEntity);
                        if (InteractManager.GetInstanceUnsafe.TryGetSqrXZDistance(Entity, destinationEntity, out var o_SqrXZDistance))
                        {
                            return new GeometryTool.NavigationStatePreset(direction.normalized, o_SqrXZDistance, false);
                        }
                        else
                        {
                            _CurrentDestination.DestinationType = GeometryTool.NavigateDestinationType.Position;
                        
                            direction = Entity.GetDirectionXZVectorTo(_CurrentDestination.DestinationPosition);
                            var sqrDistance = direction.sqrMagnitude;
                
                            return new GeometryTool.NavigationStatePreset(direction.normalized, sqrDistance, false);
                        }
                    }
                    else
                    {
                        _CurrentDestination.DestinationType = GeometryTool.NavigateDestinationType.Position;
                        
                        var direction = Entity.GetDirectionXZVectorTo(_CurrentDestination.DestinationPosition);
                        var sqrDistance = direction.sqrMagnitude;
                
                        return new GeometryTool.NavigationStatePreset(direction.normalized, sqrDistance, false);
                    }
                }
            }
        }
        
        #endregion
    }
}