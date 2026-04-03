using System;
using k514.Mono.Feature;

namespace k514.Mono.Common
{
    /// <summary>
    /// 해당 개체가 가지는 속성을 기술하는 부분 클래스
    /// </summary>
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        /// <summary>
        /// 현재 부여된 권한 마스크
        /// </summary>
        private GameEntityTool.GameEntityAttributeType _AttributeMask;
        
        #endregion
        
        #region <Callbacks>

        private void OnCreateAttribute()
        {
            _AttributeMask = GameEntityTool.GameEntityAttributeType.None;
        }

        private void OnActivateAttribute(ActivateParams p_ActivateParams)
        {
            var attributeMask = p_ActivateParams.GameEntityActivateParamsAttributeMask;
            var enumerator = GameEntityTool.ActivateParamsAttributeTypeEnumerator;
            foreach (var tryAttribute in enumerator)
            {
                if (attributeMask.HasAnyFlagExceptNone(tryAttribute))
                {
                    switch (tryAttribute)
                    {
                        case GameEntityTool.ActivateParamsAttributeType.GivePlayer:
                        {
                            PlayerManager.GetInstanceUnsafe?.SetPlayer(this, PlayerTool.PlayerSetType.Overlap);
                            break;
                        }
                        case GameEntityTool.ActivateParamsAttributeType.GiveBoss:
                        {
                            BossManager.GetInstanceUnsafe?.SetBoss(this, BossTool.BossSetType.Overlap);
                            break;
                        }
                        case GameEntityTool.ActivateParamsAttributeType.GivePreserveCorpse:
                        {
                            AddAttribute(GameEntityTool.GameEntityAttributeType.PreserveCorpse);
                            break;
                        }
                        case GameEntityTool.ActivateParamsAttributeType.GiveFollowFallenMaster:
                        {
                            AddAttribute(GameEntityTool.GameEntityAttributeType.FollowFallenMaster);
                            break;
                        }
                    }
                    
                }
            }
        }
        
        private void OnRetrieveAttribute()
        {
            ClearAttribute();
        }

        #endregion
        
        #region <Methods>

        public void AddAttribute(GameEntityTool.GameEntityAttributeType p_Type)
        {
            _AttributeMask.AddFlag(p_Type);
        }

        public void RemoveAttribute(GameEntityTool.GameEntityAttributeType p_Type)
        {
            _AttributeMask.RemoveFlag(p_Type);
        }

        public void TurnAttribute(GameEntityTool.GameEntityAttributeType p_Type)
        {
            _AttributeMask.TurnFlag(p_Type);
        }

        public void ClearAttribute()
        {
            TurnAttribute(GameEntityTool.GameEntityAttributeType.None);
        }

        public bool HasAttribute(GameEntityTool.GameEntityAttributeType p_Type)
        {
            return _AttributeMask.HasAnyFlagExceptNone(p_Type);
        }

        public GameEntityTool.GameEntityAttributeType GetAttribute()
        {
            return _AttributeMask;
        }

        #endregion
    }
}