using UnityEngine;

namespace k514.Mono.Common
{
    public static class GameEntityDeployTool
    {
        #region <Structs>
        
        public struct GameEntityDeployParams
        {
            #region <Fields>

            public readonly IGameEntityBridge Caster;
            public readonly bool IsCasterAction;
            public readonly bool IsTriggered;
            public readonly Vector3 ActionTriggerUV;

            #endregion

            #region <Constructor>

            public GameEntityDeployParams(IGameEntityBridge p_Caster, Vector3 p_ActionTriggerUV)
            {
                Caster = p_Caster;
                IsCasterAction = false;
                
                IsTriggered = !p_ActionTriggerUV.IsReachedZero();
                ActionTriggerUV = p_ActionTriggerUV;
            }

            #endregion
                
            #region <Methods>

            public ProjectilePoolManager.CreateParams GetShotCreateParams(int p_Index)
            {
                switch (this)
                {
                    /*case var _ when Caster.TryGetSubProjectileCreateParams(p_Index, out var o_Preset):
                    {
                        return o_Preset;
                    }
                    case var _ when Caster.TryGetMaster(out var o_Master) && o_Master.TryGetSubProjectileCreateParams(p_Index, out var o_Preset):
                    {
                        return o_Preset;
                    }*/
                    default:
                    {
                        return ProjectilePoolManager.GetInstanceUnsafe.GetCreateParams(p_Index);
                    }
                }
            }
                      
            public Vector3 GetActionTriggerUV()
            {
                if (IsTriggered)
                {
                    return ActionTriggerUV;
                }
                else
                {
                    return Caster.GetLookUV();
                }
            }

            public bool TryGetAutoAimDirection(EntityQueryTool.FilterResultType p_Type, out Vector3 o_UV)
            {
                return TryGetAutoAimDirection(p_Type, out o_UV, out var o_Enemy);
            }

            public bool TryGetAutoAimDirection(EntityQueryTool.FilterResultType p_Type, out Vector3 o_UV, out IGameEntityBridge o_Enemy)
            {
                o_UV = default;
                o_Enemy = default;
                
                return false;
                
                /*if (IsTriggered)
                {
                    Caster.OnActionTriggerDirectionUpdate(ActionTriggerUV);
                    o_UV = Caster.GetLookUV();
                    o_Enemy = default;
                    
                    return false;
                }
                else
                {
                    if (Caster.TryUpdateAndGetCurrentEnemy(out o_Enemy))
                    {
                        Caster.OnActionTriggerDirectionUpdate(Caster.GetDirectionXZUnitVectorTo(o_Enemy));
                        o_UV = Caster.GetLookUV();

                        return true;
                    }
                    else
                    {
                        Caster.OnActionTriggerDirectionUpdate(Caster.GetLookUV());
                        o_UV = Caster.GetLookUV();
                        o_Enemy = default;
                        
                        return false;
                    }
                }*/
            }

            #endregion
        }

        #endregion
    }
}