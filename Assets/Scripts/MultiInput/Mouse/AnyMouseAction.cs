namespace MultiInput.Mouse
{
    public delegate void AnyMouseEvent(MouseEvent code, IMouse mouse);

    public delegate void AnyMouseMovement(MouseMovement movement, IMouse mouse);

    public delegate void AnyMouseWheel(float movement, bool horizontal, IMouse mouse);
}