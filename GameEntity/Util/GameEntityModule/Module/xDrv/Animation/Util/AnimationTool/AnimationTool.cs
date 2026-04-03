using System;
using System.Collections.Generic;
using UnityEngine;

namespace k514.Mono.Common
{
    public static partial class AnimationTool
    {
        public static readonly Dictionary<ClipEventType, string> __MotionClipEventNameTable
            = new Dictionary<ClipEventType, string>
            {
                {ClipEventType.StartCue, "OnAnimationMotionStartCue"},
                {ClipEventType.EventCue, "OnAnimationMotionEventCue"},
                {ClipEventType.CancelCue, "OnAnimationMotionCancelCue"},
                {ClipEventType.StopCue, "OnAnimationMotionStopCue"},
                {ClipEventType.SoundCue, "OnAnimationMotionSoundCue"},
                {ClipEventType.EndCue, "OnAnimationMotionEndCue"},
            };

        public static MotionTransitionType[] _MotionTransitionType_Enumerator = EnumFlag.GetEnumEnumerator<MotionTransitionType>(EnumFlag.GetEnumeratorType.ExceptNone);
        public static MotionType[] _MotionTypeEnumerator = EnumFlag.GetEnumEnumerator<MotionType>(EnumFlag.GetEnumeratorType.ExceptNone);

        public const float AttackSpeedStatusAdaptFactor = 1f;
        public const float MoveSpeedStatusAdaptFactor = 1f;
        public const float AnimationSpeedUpperBound = 4f;
        public const float AnimationSpeedLowerBound = 1f / AnimationSpeedUpperBound;
    }
}