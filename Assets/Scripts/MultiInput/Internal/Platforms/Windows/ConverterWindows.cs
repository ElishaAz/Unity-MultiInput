using System;
using System.Collections.Generic;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;

namespace MultiInput.Internal.Platforms.Windows
{
    public static class ConverterWindows
    {
        public static void ResolveMouseButtons(RawMouseButtons buttons, Action<MouseEvent> onDown,
           Action<MouseEvent> onUp)
        {
            if (buttons.HasFlag(RawMouseButtons.LeftDown))
            {
                onDown(MouseEvent.LeftMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.LeftUp))
            {
                onUp(MouseEvent.LeftMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.RightDown))
            {
                onDown(MouseEvent.RightMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.RightUp))
            {
                onUp(MouseEvent.RightMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.MiddleDown))
            {
                onDown(MouseEvent.MiddleMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.MiddleUp))
            {
                onUp(MouseEvent.MiddleMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.Button4Down))
            {
                onDown(MouseEvent.ForthMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.Button4Up))
            {
                onUp(MouseEvent.ForthMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.Button5Down))
            {
                onDown(MouseEvent.FifthMouse);
            }

            if (buttons.HasFlag(RawMouseButtons.Button5Up))
            {
                onUp(MouseEvent.FifthMouse);
            }
        }
    }
}