using System;
using UnityEngine;

namespace k514.Mono.Common
{
    public static class GameEntityAffineControlTool
    {
        #region <Enums>

        [Flags]
        public enum AffineControlAttribute
        {
            None = 0,
            TerminateAfterControl = 1 << 0,
            HasUV = 1 << 1,
        }

        #endregion

        #region <Preset>

        public struct GameEntityAffineControlParams
        {
            #region <Fields>

            public Vector3 UV;
            public AffineControlAttribute AttributeMask;

            #endregion
            
            #region <Constructor>

            public GameEntityAffineControlParams(Vector3 p_UV, AffineControlAttribute p_Mask = default)
            {
                this = default;
                
                AttributeMask = p_Mask;
                UV = p_UV;
                AttributeMask.AddFlag(AffineControlAttribute.HasUV);
            }
  
            #endregion
        }

        #endregion
    }
}