namespace k514.Mono.Common
{
    public partial class GameEntityTool
    {
        #region <Methods>
        
        /// <summary>
        /// 해당 Entity의 버전 스냅샷을 리턴하는 메서드
        /// </summary>
        public static GameEntityValidation GetEntityValidation(this IGameEntityBridge p_This)
        {
            return p_This.IsContentValid() ? new GameEntityValidation(p_This) : default;
        }
        
        #endregion
 
        #region <Structs>

        public struct GameEntityValidation
        {
            #region <Fields>

            public readonly IGameEntityBridge Entity;
            public readonly int VersionSnapshot;
            public readonly bool ValidFlag;

            #endregion

            #region <Constructor>

            public GameEntityValidation(IGameEntityBridge p_Entity)
            {
                ValidFlag = !ReferenceEquals(null, p_Entity);
                Entity = ValidFlag ? p_Entity : null;
                VersionSnapshot = ValidFlag ? Entity.GetPooledCount() : 0;
            }

            #endregion

            #region <Methods>
   
            public bool IsValid()
            {
                return ValidFlag 
                       && Entity.IsEntityValid()
                       && VersionSnapshot == Entity.GetPooledCount();
            }
            
            public bool IsValid(IGameEntityBridge p_Entity)
            {
                return ValidFlag 
                       && ReferenceEquals(Entity, p_Entity)
                       && Entity.IsEntityValid()
                       && VersionSnapshot == Entity.GetPooledCount();
            }

            #endregion
        }

        #endregion
    }
}