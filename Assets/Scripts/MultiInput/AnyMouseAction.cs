using UnityEngine;

namespace MultiInput
{
    public delegate void AnyMouseEvent(MouseEvent code, IMouse mouse);
    public delegate void AnyMouseMovement(MouseMovement movement, IMouse mouse);
}