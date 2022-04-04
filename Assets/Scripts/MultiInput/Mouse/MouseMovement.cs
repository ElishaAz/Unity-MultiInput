using UnityEngine;

namespace MultiInput.Mouse
{
    /// <summary>
    /// Represents a mouse's movement.
    /// </summary>
    public readonly struct MouseMovement
    {
        /// <summary>
        /// The relative movement.
        /// </summary>
        public readonly Vector2 Movement;

        public MouseMovement(Vector2 movement)
        {
            this.Movement = movement;
        }

        public override string ToString()
        {
            return $"{nameof(Movement)}: {Movement}";
        }
    }
}