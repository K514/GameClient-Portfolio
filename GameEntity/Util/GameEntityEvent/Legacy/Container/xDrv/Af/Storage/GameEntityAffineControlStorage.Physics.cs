using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityAffineControlStorage 
    {
        #region <Callbacks>

        private void OnCreatePhysics()
        {
            _EntityEventTable.Add(2000, new MoveForward());
            _EntityEventTable.Add(2001, new MoveLeftCurve());
            _EntityEventTable.Add(2002, new MoveRightCurve());
            _EntityEventTable.Add(2003, new MoveZigZag());
            _EntityEventTable.Add(2004, new MoveLeftCurveFast());
            _EntityEventTable.Add(2005, new MoveZigZag2());
            _EntityEventTable.Add(2006, new MoveForwardWaitRandomDirection());
            _EntityEventTable.Add(2007, new MoveZigZag3());
            _EntityEventTable.Add(2008, new MoveRightCurveFast());
            _EntityEventTable.Add(2009, new MoveRightCurveFast2());
            _EntityEventTable.Add(2010, new MoveLeftCurveFast2());
        }

        #endregion

        #region <Classess>

        public class MoveForward : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                // caster.ActionModule.MoveTo(caster.GetLookUV());
                
                return false;
            }
        }
        
        public class MoveLeftCurve : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var uv = caster.GetLookUV().RotationVectorByPivot(Vector3.up, -45f * p_DeltaTime);
                // caster.ActionModule.MoveTo(uv);
                
                return false;
            }
        }
        
        public class MoveLeftCurveFast : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var uv = caster.GetLookUV().RotationVectorByPivot(Vector3.up, -360f * p_DeltaTime);
                caster.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 20f * p_DeltaTime);
                // caster.ActionModule.MoveTo(uv);
                
                return false;
            }
        }
        
        public class MoveRightCurve : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var uv = caster.GetLookUV().RotationVectorByPivot(Vector3.up, 45f * p_DeltaTime);
                // caster.ActionModule.MoveTo(uv);
                
                return false;
            }
        }
        
        public class MoveZigZag : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                var caster = p_Handler.Caster;
                caster.RotateSelf(22.5f);

                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                if (p_Handler.Threshold > 0.33f)
                {
                    p_Handler.ResetThreshold();
                    
                    if (p_Handler.ToggleFlag())
                    {
                        caster.RotateSelf(-45f);
                    }
                    else
                    {
                        caster.RotateSelf(45f);
                    }
                }
                else
                {
                    p_Handler.UpdateThreshold(p_DeltaTime);
                }
                
                // caster.ActionModule.MoveTo(caster.GetLookUV());
                
                return false;
            }
        }
        
        public class MoveZigZag2 : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                var caster = p_Handler.Caster;
                caster.RotateSelf(40f);

                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                if (p_Handler.Threshold > 0.7f)
                {
                    p_Handler.ResetThreshold();
                    
                    if (p_Handler.ToggleFlag())
                    {
                        caster.RotateSelf(-80f);
                    }
                    else
                    {
                        caster.RotateSelf(80f);
                    }
                }
                else
                {
                    p_Handler.UpdateThreshold(p_DeltaTime);
                }
                
                // caster.ActionModule.MoveTo(caster.GetLookUV());
                
                return false;
            }
        }
        
        public class MoveForwardWaitRandomDirection : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                p_Handler.ToggleFlag();
                
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var flag = p_Handler.Flag;
                if (p_Handler.Flag)
                {
                    var threshold = p_Handler.Threshold;
                    switch (threshold)
                    {
                        case < 0.2f:
                            p_Handler.UpdateThreshold(p_DeltaTime);
                            // caster.ActionModule.MoveTo(caster.GetLookUV());
                            break;
                        case < 2.5f:
                            p_Handler.UpdateThreshold(p_DeltaTime);
                            break;
                        default:
                            var affine = p_Handler.Affine;
                            p_Handler.UpdateThreshold(p_DeltaTime);
                            p_Handler.ToggleFlag();
                            affine.Rotate(affine.up, Random.Range(0f, 360f), Space.World);
                            caster.AddStatus(StatusTool.BattleStatusGroupType.SimpleMul, BattleStatusTool.BattleStatusType.MoveSpeedRate, -0.5f);
                            // caster.ActionModule.MoveTo(caster.GetLookUV());
                            break;
                    }

                    return false;
                }
                else
                {
                    // caster.ActionModule.MoveTo(caster.GetLookUV());
                    return false;
                }
            }
        }
        
        public class MoveZigZag3 : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                var caster = p_Handler.Caster;
                caster.RotateSelf(70f);

                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                if (p_Handler.Threshold > 0.7f)
                {
                    p_Handler.ResetThreshold();
                    
                    if (p_Handler.ToggleFlag())
                    {
                        caster.RotateSelf(-140f);
                    }
                    else
                    {
                        caster.RotateSelf(140f);
                    }
                }
                else
                {
                    p_Handler.UpdateThreshold(p_DeltaTime);
                }
                
                // caster.ActionModule.MoveTo(caster.GetLookUV());
                
                return false;
            }
        }
        
        public class MoveRightCurveFast : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var uv = caster.GetLookUV().RotationVectorByPivot(Vector3.up, 360f * p_DeltaTime);
                caster.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 10f * p_DeltaTime);
                // caster.ActionModule.MoveTo(uv);
                
                return false;
            }
        }
        
        public class MoveRightCurveFast2 : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var uv = caster.GetLookUV().RotationVectorByPivot(Vector3.up, 360f * p_DeltaTime);
                caster.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 5f * p_DeltaTime);
                // caster.ActionModule.MoveTo(uv);
                
                return false;
            }
        }
        
        public class MoveLeftCurveFast2 : GameEntityAffineControlEventBase
        {
            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var caster = p_Handler.Caster;
                var uv = caster.GetLookUV().RotationVectorByPivot(Vector3.up, -360f * p_DeltaTime);
                caster.AddStatus(StatusTool.BattleStatusGroupType.ExtraAdd, BattleStatusTool.BattleStatusType.MoveSpeedBasis, 50f * p_DeltaTime);
                // caster.ActionModule.MoveTo(uv);
                
                return false;
            }
        }
        
        #endregion
    }
}