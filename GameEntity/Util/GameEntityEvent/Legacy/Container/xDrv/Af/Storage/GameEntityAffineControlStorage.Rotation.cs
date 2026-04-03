using UnityEngine;

namespace k514.Mono.Common
{
    public partial class GameEntityAffineControlStorage 
    {
        #region <Callbacks>

        private void OnCreateRotation()
        {
            _EntityEventTable.Add(1000, new DownRizap());
            _EntityEventTable.Add(1001, new LeftSlash());
            _EntityEventTable.Add(1002, new DownRizap2());
            _EntityEventTable.Add(1003, new LeftRotate());
            _EntityEventTable.Add(1004, new RightRotate());
        }

        #endregion

        #region <Classess>

        public class DownRizap : GameEntityAffineControlEventBase
        {
            #region <Consts>

            private const float _TargetDegree = 75f;
            private const float _AngularVel = 120f;

            #endregion

            #region <Methods>

            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                var affine = p_Handler.Affine;
                affine.Rotate(affine.right, 90f, Space.World);

                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var threshold = p_Handler.Threshold;
                if (threshold < _TargetDegree)
                {
                    var affine = p_Handler.Affine;
                    var delta = _AngularVel * p_DeltaTime;
                    var elapsed = _TargetDegree - threshold;
                    if (delta < elapsed)
                    {
                        affine.Rotate(-affine.right, delta, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }
                    else
                    {
                        affine.Rotate(-affine.right, elapsed, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            #endregion
        }
        
        public class DownRizap2 : GameEntityAffineControlEventBase
        {
            #region <Consts>

            private const float _TargetDegree = 75f;
            private const float _AngularVel = 222f;

            #endregion

            #region <Methods>

            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                var affine = p_Handler.Affine;
                affine.Rotate(affine.right, 90f, Space.World);

                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var threshold = p_Handler.Threshold;
                if (threshold < _TargetDegree)
                {
                    var affine = p_Handler.Affine;
                    var delta = _AngularVel * p_DeltaTime;
                    var elapsed = _TargetDegree - threshold;
                    if (delta < elapsed)
                    {
                        affine.Rotate(-affine.right, delta, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }
                    else
                    {
                        affine.Rotate(-affine.right, elapsed, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            #endregion
        }
        
        public class LeftSlash : GameEntityAffineControlEventBase
        {
            #region <Consts>

            private const float _TargetDegree = 90f;
            private const float _AngularVel = 240f;

            #endregion

            #region <Methods>

            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                var affine = p_Handler.Affine;
                affine.Rotate(affine.right, 45f, Space.World);
                affine.Rotate(affine.up, 45f, Space.World);

                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var threshold = p_Handler.Threshold;
                if (threshold < _TargetDegree)
                {
                    var affine = p_Handler.Affine;
                    var degree = _AngularVel * p_DeltaTime;
                    affine.Rotate(-affine.up, degree, Space.World);
                    p_Handler.UpdateThreshold(degree);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            #endregion
        }

        public class LeftRotate : GameEntityAffineControlEventBase
        {
            #region <Consts>

            private const float _TargetDegree = 120f;
            private const float _AngularVel = 240f;

            #endregion

            #region <Methods>

            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var threshold = p_Handler.Threshold;
                if (threshold < _TargetDegree)
                {
                    var affine = p_Handler.Affine;
                    var delta = _AngularVel * p_DeltaTime;
                    var elapsed = _TargetDegree - threshold;
                    if (delta < elapsed)
                    {
                        affine.Rotate(-affine.up, delta, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }
                    else
                    {
                        affine.Rotate(-affine.up, elapsed, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            #endregion
        }
        
        public class RightRotate : GameEntityAffineControlEventBase
        {
            #region <Consts>

            private const float _TargetDegree = 120f;
            private const float _AngularVel = 240f;

            #endregion

            #region <Methods>

            public override bool InitializeAffineControl(GameEntityAffineControlEventContainer p_Handler)
            {
                return true;
            }

            public override bool UpdateAffineControl(GameEntityAffineControlEventContainer p_Handler, float p_DeltaTime)
            {
                var threshold = p_Handler.Threshold;
                if (threshold < _TargetDegree)
                {
                    var affine = p_Handler.Affine;
                    var delta = _AngularVel * p_DeltaTime;
                    var elapsed = _TargetDegree - threshold;
                    if (delta < elapsed)
                    {
                        affine.Rotate(affine.up, delta, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }
                    else
                    {
                        affine.Rotate(affine.up, elapsed, Space.World);
                        p_Handler.UpdateThreshold(delta);
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            }
            
            #endregion
        }
        
        #endregion
    }
}