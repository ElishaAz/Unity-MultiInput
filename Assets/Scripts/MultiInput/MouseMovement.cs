using UnityEngine;

namespace MultiInput
{
    public struct MouseMovement
    {
        public readonly bool IsAbsolute;
        public readonly Vector2 Absolute;
        public readonly Vector2 Relative;

        public MouseMovement(bool isAbsolute, Vector2 absolute, Vector2 relative)
        {
            this.IsAbsolute = isAbsolute;
            this.Absolute = absolute;
            this.Relative = relative;
        }

        public override string ToString()
        {
            return $"{nameof(IsAbsolute)}: {IsAbsolute}, {nameof(Absolute)}: {Absolute}, {nameof(Relative)}: {Relative}";
        }
    }
}