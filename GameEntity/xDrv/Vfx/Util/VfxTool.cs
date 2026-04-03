using System;
using k514.Mono.Common;

namespace k514.Mono.Feature
{
    public static class VfxTool
    {
        #region <Consts>

        public const AnimationModuleDataTableQuery.TableLabel __DefaultAnimationModuleType = AnimationModuleDataTableQuery.TableLabel.None;
        public const ActionModuleDataTableQuery.TableLabel __DefaultActionModuleType = ActionModuleDataTableQuery.TableLabel.None;
        public const PhysicsModuleDataTableQuery.TableLabel __DefaultPhysicsModuleType = PhysicsModuleDataTableQuery.TableLabel.None;
        public const GeometryModuleDataTableQuery.TableLabel __DefaultGeometryModuleType = GeometryModuleDataTableQuery.TableLabel.None;
        public const MindModuleDataTableQuery.TableLabel __DefaultMindModuleType = MindModuleDataTableQuery.TableLabel.None;
        public const RenderModuleDataTableQuery.TableLabel __DefaultRenderModuleType = RenderModuleDataTableQuery.TableLabel.None;
        public const RoleModuleDataTableQuery.TableLabel __DefaultRoleModuleType = RoleModuleDataTableQuery.TableLabel.None;

        public static readonly int[] __VfxIndexList =
        {
            __InstantHealHPVfxIndex, __InstantHealMPVfxIndex,
            
            __BindVfxIndex, __BleedingVfxIndex, __BlessingVfxIndex, __BlindVfxIndex, __BurnVfxIndex,
            __ChilVfxIndex, __ConfuseVfxIndex, __CurseVfxIndex, __GroggyVfxIndex, __HealVfxIndex,
            __ImmortalVfxIndex, __PoisoningVfxIndex, __ShockVfxIndex, __SilenceVfxIndex, __StunVfxIndex,
            __VictoriaVfxIndex, __VictoriaReleaseVfxIndex,
            
            __BlueAuraVfxIndex, __GreenAuraVfxIndex, __RedAuraVfxIndex, __YellowAuraVfxIndex,
            
            __MonsterSpawnVfxIndex,
            
            __PartySkillVfxIndex0, __PartySkillVfxIndex1, __PartySkillVfxIndex2, __PartySkillVfxIndex3
        };
        
        public const int __InstantHealHPVfxIndex = 54;
        public const int __InstantHealMPVfxIndex = 55;

        public const int __BindVfxIndex = 2000;
        public const int __BleedingVfxIndex = 2001;
        public const int __BlessingVfxIndex = 2002;
        public const int __BlindVfxIndex = 2003;
        public const int __BurnVfxIndex = 2004;
        public const int __ChilVfxIndex = 2005;
        public const int __ConfuseVfxIndex = 2006;
        public const int __CurseVfxIndex = 2007;
        public const int __GroggyVfxIndex = 2008;
        public const int __HealVfxIndex = 2009;
        public const int __ImmortalVfxIndex = 2010;
        public const int __PoisoningVfxIndex = 2011;
        public const int __ShockVfxIndex = 2012;
        public const int __SilenceVfxIndex = 2013;
        public const int __StunVfxIndex = 2014;
        public const int __VictoriaVfxIndex = 2015;
        public const int __VictoriaReleaseVfxIndex = 2016;
        
        public const int __BlueAuraVfxIndex = 1;
        public const int __GreenAuraVfxIndex = 2;
        public const int __RedAuraVfxIndex = 3;
        public const int __YellowAuraVfxIndex = 4;
        
        public const int __MonsterSpawnVfxIndex = 104;
        
        public const int __PartySkillVfxIndex0 = 79;
        public const int __PartySkillVfxIndex1 = 80;
        public const int __PartySkillVfxIndex2 = 81;
        public const int __PartySkillVfxIndex3 = 82;

        #endregion
        
        #region <Enums>
                
        [Flags]
        public enum ActivateParamsAttributeType
        {
            None = 0,
            
            DeferredPlayParticle = 1 << 0,
        }

        #endregion

        #region <Methods>

        public static void PreloadVfx()
        {
            foreach (var index in __VfxIndexList)
            {
                var createParams = VfxPoolManager.GetInstanceUnsafe.GetCreateParams(index);
                VfxPoolManager.GetInstanceUnsafe.Preload(createParams, 10);
            }
        }

        #endregion
    }
}