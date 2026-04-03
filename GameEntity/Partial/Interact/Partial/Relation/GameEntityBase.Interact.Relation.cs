using System.Collections.Generic;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        private Dictionary<GameEntityRelationTool.GameEntityRelationType, GameEntityRelation> _GameEntityRelationTable;

        #endregion

        #region <Callbacks>

        private void OnCreateInteractRelation()
        {
            _GameEntityRelationTable = new Dictionary<GameEntityRelationTool.GameEntityRelationType, GameEntityRelation>();
            var enumerator =
                EnumFlag.GetEnumEnumerator<GameEntityRelationTool.GameEntityRelationType>(EnumFlag.GetEnumeratorType
                    .ExceptNone);

            foreach (var gameEntityRelationType in enumerator)
            {
                switch (gameEntityRelationType)
                {
                    case GameEntityRelationTool.GameEntityRelationType.Party:
                        _GameEntityRelationTable.Add(gameEntityRelationType, new GameEntityRelation(gameEntityRelationType, this, OnHandlePartyEvent));
                        break;
                    case GameEntityRelationTool.GameEntityRelationType.Possession:
                        _GameEntityRelationTable.Add(gameEntityRelationType, new GameEntityRelation(gameEntityRelationType, this, OnHandlePossessionEvent));
                        break;
                    case GameEntityRelationTool.GameEntityRelationType.Focus:
                        _GameEntityRelationTable.Add(gameEntityRelationType, new GameEntityRelation(gameEntityRelationType, this, OnHandleFocusEvent));
                        break;
                    case GameEntityRelationTool.GameEntityRelationType.Enemy:
                        _GameEntityRelationTable.Add(gameEntityRelationType, new GameEntityRelation(gameEntityRelationType, this, OnHandleEnemyEvent));
                        break;
                }
            }

            OnCreatePartyInteraction();
            OnCreatePossessionInteraction();
            OnCreateFocusInteraction();
            OnCreateEnemyInteraction();
        }

        private void OnActivateInteractRelation(ActivateParams p_ActivateParams)
        {
            OnActivatePartyInteraction();
            OnActivatePossessionInteraction(p_ActivateParams);
            OnActivateFocusInteraction();
            OnActivateEnemyInteraction();
        }

        private void OnRetrieveInteractRelation()
        {
            OnRetrieveEnemyInteraction();
            OnRetrieveFocusInteraction();
            OnRetrievePossessionInteraction();
            OnRetrievePartyInteraction();
        }

        private void OnDisposeInteractRelation()
        {
            if (!ReferenceEquals(null, _GameEntityRelationTable))
            {
                foreach (var relationKV in _GameEntityRelationTable)
                {
                    var relation = relationKV.Value;
                    relation.Dispose();
                }
                
                _GameEntityRelationTable.Clear();
                _GameEntityRelationTable = default;
            }
        }

        #endregion
    }
}