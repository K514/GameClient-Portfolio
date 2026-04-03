using System;

namespace k514.Mono.Common
{
    public partial class GameEntityBase<Content, CreateParams, ActivateParams>
    {
        #region <Fields>

        public GameEntityTool.GameEntityType GameEntityType { get; protected set; }
        public bool IsInteractableGameEntity { get; private set; }
        public bool IsUnitEntity => GameEntityType == GameEntityTool.GameEntityType.Unit;

        #endregion
        
        #region <Callbacks>

        protected override void OnBindLabel()
        {
            WorldObjectType = WorldObjectTool.WorldObjectType.GameEntity;
        }

        protected void OnBindLabelBubble()
        {
            switch (GameEntityType)
            {
                case GameEntityTool.GameEntityType.Gear:
                case GameEntityTool.GameEntityType.Unit:
                case GameEntityTool.GameEntityType.Projectile:
                case GameEntityTool.GameEntityType.Beam:
                {
                    IsInteractableGameEntity = true;
                    break;
                }
                default:
                case GameEntityTool.GameEntityType.Vfx:
                case GameEntityTool.GameEntityType.Projector:
                case GameEntityTool.GameEntityType.Audio:
                case GameEntityTool.GameEntityType.Video:
                {
                    IsInteractableGameEntity = false;
                    break;
                }
            }
        }
        
        #endregion
    }
}