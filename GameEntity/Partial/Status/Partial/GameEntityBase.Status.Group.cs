using k514.Mono.Feature;
using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 해당 개체가 소속된 그룹 프리셋
        /// </summary>
        private GameEntityGroupPreset _GroupPreset;

        /// <summary>
        /// 해당 개체가 소속된 그룹 프리셋을 리턴하는 프로퍼티
        /// </summary>
        public GameEntityGroupPreset GroupPreset => _GroupPreset;
        
        #endregion

        #region <Callbacks>

        private void OnCreateGroup()
        {
        }
        
        private void OnActivateGroup()
        {
            ResetGroupMask();
        }
        
        private void OnRetrieveGroup()
        {
        }
        
        public void OnGroupMaskChanged()
        {
        }

        #endregion
        
        #region <Methods>

        public void SetGroupMask(GameEntityGroupPreset p_GroupPreset)
        {
            _GroupPreset = p_GroupPreset;
            
            OnGroupMaskChanged();
        }

        public void SetGroupMask(int p_Index)
        {
            var record = GameEntityGroupTable.GetInstanceUnsafe.GetRecord(p_Index);
            SetGroupMask(record.GameEntityGroupPreset);
        }

        public void ResetGroupMask()
        {
            SetGroupMask(ComponentDataRecord.GetGroupPreset());
        }

        public void SetGroupMask(GameEntityTool.GameEntityGroupType p_AllyTypeMask, GameEntityTool.GameEntityGroupType p_EnemyTypeMask)
        {
            SetGroupMask(new GameEntityGroupPreset(GetMainGroup(), p_AllyTypeMask, p_EnemyTypeMask));
        }
                
        public void SetAllyMask(GameEntityTool.GameEntityGroupType p_AllyTypeMask)
        {
            SetGroupMask(new GameEntityGroupPreset(GetMainGroup(), p_AllyTypeMask, GetEnemyMask()));
        }

        public void SetEnemyMask(GameEntityTool.GameEntityGroupType p_EnemyTypeMask)
        {
            SetGroupMask(new GameEntityGroupPreset(GetMainGroup(), GetAllyMask(), p_EnemyTypeMask));
        }
        
        public void SetFollowGroupMask(IGameEntityBridge p_Entity)
        {
            SetGroupMask(p_Entity.GetAllyMask(), p_Entity.GetEnemyMask());
        }
        
        public void SetCounterGroupMask(IGameEntityBridge p_Entity)
        {
            SetGroupMask(p_Entity.GetEnemyMask(), p_Entity.GetAllyMask());
        }

        public GameEntityTool.GameEntityGroupType GetMainGroup()
        {
            return _GroupPreset.MainGroupType;
        }

        public GameEntityTool.GameEntityGroupType GetAllyMask()
        {
            if (TryGetMaster(out var o_Master))
            {
                return o_Master.GetAllyMask();
            }
            else
            {
                return _GroupPreset.AllyMask;
            }
        }
        
        public GameEntityTool.GameEntityGroupType GetEnemyMask()
        {
            if (TryGetMaster(out var o_Master))
            {
                return o_Master.GetEnemyMask();
            }
            else
            {
                return _GroupPreset.EnemyMask;
            }
        }
        
        /// <summary>
        /// 지정한 유닛과 해당 유닛의 피아 관계를 리턴한다.
        /// Enemy 마스크와 Ally 마스크에 중복된 세력 타입이 포함되는 경우, Enemy 마스크를 우선하여 해당 세력을 적으로 취급한다.
        /// </summary>
        public GameEntityTool.GameEntityGroupRelateType GetGroupRelate(IGameEntityBridge p_TargetUnit)
        {
            // 타겟이 유효한 경우, 관계는 (적, 아군, 중립) 중에 하나로 정의된다.
            if (p_TargetUnit.IsEntityValid())
            {
                var targetAllyMask = p_TargetUnit.GetAllyMask();
                
                /* SE Cond */
                // 1. 해당 개체의 적 플래그가 타겟의 동맹 플래그와 겹친다면 타겟은 적이다.
                if ((GetEnemyMask() & targetAllyMask) != GameEntityTool.GameEntityGroupType.None)
                {
                    return GameEntityTool.GameEntityGroupRelateType.Enemy;
                }
                else
                {
                    /* SE Cond */
                    // 1. 해당 개체의 동맹 플래그가 타겟의 동맹 플래그와 겹친다면 타겟은 아군이다.
                    if ((GetAllyMask() & targetAllyMask) != GameEntityTool.GameEntityGroupType.None)
                    {
                        return GameEntityTool.GameEntityGroupRelateType.Ally;
                    }
                    /* SE Cond */
                    // 1. 타겟이 적도, 동맹도 아니라면 타겟은 중립이다.
                    else
                    {
                        return GameEntityTool.GameEntityGroupRelateType.Neutral;
                    }
                }
            }
            // 타겟이 유효하지 않는 경우, 관계를 정의할 수 없다.
            else
            {
                return GameEntityTool.GameEntityGroupRelateType.None;
            }
        }

        
        #endregion
    }
}