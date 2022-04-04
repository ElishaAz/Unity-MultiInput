using System;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Windows
{
    public class MouseWindows : MouseAbstract
    {
        public MouseWindows(AnyMouseMovement invokeAnyMouseMovement,
            AnyMouseEvent invokeAnyMouseEvent,
            AnyMouseWheel invokeAnyMouseWheel) : base(invokeAnyMouseMovement, invokeAnyMouseEvent, invokeAnyMouseWheel)
        {
        }

        #region process_input

        internal bool Process(RawMouse rawMouse)
        {
            if ((rawMouse.Flags & RawMouseFlags.MoveAbsolute) != 0)
            {
                var movementNow = new Vector2(rawMouse.LastX, rawMouse.LastY);
                movement.SetCurrent(movement.GetCurrent() + movementNow);

                InvokeOnMove(new MouseMovement(movementNow));
            }

            if ((rawMouse.ButtonFlags & RawMouseButtons.MouseWheel) != 0)
            {
                wheel.SetCurrent(wheel.GetCurrent() + rawMouse.ButtonData);
            }

            if ((rawMouse.ButtonFlags & RawMouseButtons.MouseHWheel) != 0)
            {
                hWheel.SetCurrent(hWheel.GetCurrent() + rawMouse.ButtonData);
            }

            ConverterWindows.ResolveMouseButtons(rawMouse.ButtonFlags,
                HandleKeyDown, HandleKeyUp);

            return Grab;
        }

        private void HandleKeyDown(MouseEvent e)
        {
            keyPressProvider.HandleEvent(e, KeyEventState.Down);
        }

        private void HandleKeyUp(MouseEvent e)
        {
            keyPressProvider.HandleEvent(e, KeyEventState.Up);
        }

        #endregion

        public override bool Grab { get; set; } = false;
    }
}