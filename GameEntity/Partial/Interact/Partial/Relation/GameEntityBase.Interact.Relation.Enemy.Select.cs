using UnityEngine;
using Random = System.Random;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private GameEntityTool.EnemySelectType _EnemySelectType;

        #endregion
        
        #region <Methods>

        public void SetEnemySelectType(GameEntityTool.EnemySelectType p_Type)
        {
            _EnemySelectType = p_Type;
        }
        
        public void UpdateEnemy()
        {
            ClearEnemy();
            FindEnemyFromFocus();
            SelectEnemy();
        }

        /// <summary>
        /// 현재 시야에 들어온 개체 중에 관계 타입이 적인 유닛을 추가하는 메서드
        /// </summary>
        private void FindEnemyFromFocus()
        {
            var focusGroup = GetFocusGroup();
            foreach (var focusEntity in focusGroup)
            {
                if (focusEntity.IsEngageable())
                {
                    var entityRelate = GetGroupRelate(focusEntity);
                    if (entityRelate == GameEntityTool.GameEntityGroupRelateType.Enemy)
                    {
                        AddEnemy(focusEntity);
                    }
                    else
                    {
                        RemoveEnemy(focusEntity);
                    }
                }
            }
        }

        private void SelectEnemy()
        {
            switch (_EnemySelectType)
            {
                default:
                    break;
                case GameEntityTool.EnemySelectType.None:
                case GameEntityTool.EnemySelectType.NearestPosition:
                {
                    var enemyGroup = GetEnemyGroup();
                    var result = default(IGameEntityBridge);
                    
                    if (enemyGroup.Count > 0)
                    {
                        var minDistance = float.MaxValue;
                        foreach (var enemy in enemyGroup)
                        {
                            if (InteractManager.GetInstanceUnsafe.TryGetSqrDistance(this, enemy, out var o_SqrDistance) && o_SqrDistance < minDistance)
                            {
                                minDistance = o_SqrDistance;
                                result = enemy;
                            }
                        }
                    }

                    SetEnemy(result);
                    break;
                }
                case GameEntityTool.EnemySelectType.NearestAngle:
                {
                    var enemyGroup = GetEnemyGroup();
                    var result = default(IGameEntityBridge);
                    
                    if (enemyGroup.Count > 0)
                    {
                        var pivotUV = Affine.forward;
                        var maxDot = float.MinValue;
                        foreach (var enemy in enemyGroup)
                        {
                            var enemyUV = Affine.GetDirectionUnitVectorTo(enemy);
                            var tryDot = Vector3.Dot(pivotUV, enemyUV);
                            if (tryDot > maxDot)
                            {
                                maxDot = tryDot;
                                result = enemy;
                            }
                        }
                    }

                    SetEnemy(result);
                    break;
                }
                case GameEntityTool.EnemySelectType.Random:
                {
                    var enemyGroup = GetEnemyGroup();
                    var result = enemyGroup.GetRandomElement();
    
                    SetEnemy(result);
                    break;
                }
            }
        }

        #endregion
    }
}