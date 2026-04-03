using UnityEngine;

namespace k514.Mono.Common
{
    public struct CameraShakePreset
    {
        public readonly Vector3 Direction;
        public readonly float Distance;
        public readonly float PingPongBound;
        public readonly float PingPongBoundHalves;
        public bool ValidFlag;

        public CameraShakePreset(Vector3 p_Direction, float p_Distance, float p_PingPongBound, float p_PingPongBoundHalves)
        {
            Direction = p_Direction;
            Distance = p_Distance;
            PingPongBound = p_PingPongBound;
            PingPongBoundHalves = p_PingPongBoundHalves;
            ValidFlag = true;
        }
    }

}