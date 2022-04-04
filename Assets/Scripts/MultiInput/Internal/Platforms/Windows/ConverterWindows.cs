using System;
using System.Collections.Generic;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;
using MultiInput.Mouse;
using UnityEngine;

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

        public static KeyCode ScanCodeToKeyCode(KeyScanCode scanCode, bool e0On)
        {
            return scanCode switch
            {
                KeyScanCode.Esc => KeyCode.Escape,
                KeyScanCode.One => KeyCode.Alpha1,
                KeyScanCode.Two => KeyCode.Alpha2,
                KeyScanCode.Three => KeyCode.Alpha3,
                KeyScanCode.Four => KeyCode.Alpha4,
                KeyScanCode.Five => KeyCode.Alpha5,
                KeyScanCode.Six => KeyCode.Alpha6,
                KeyScanCode.Seven => KeyCode.Alpha7,
                KeyScanCode.Eight => KeyCode.Alpha8,
                KeyScanCode.Nine => KeyCode.Alpha9,
                KeyScanCode.Zero => KeyCode.Alpha0,
                KeyScanCode.Minus => KeyCode.Minus,
                KeyScanCode.Equals => KeyCode.Equals,
                KeyScanCode.Backspace => KeyCode.Backspace,
                KeyScanCode.Tab => KeyCode.Tab,
                KeyScanCode.Q => KeyCode.Q,
                KeyScanCode.W => KeyCode.W,
                KeyScanCode.E => KeyCode.E,
                KeyScanCode.R => KeyCode.R,
                KeyScanCode.T => KeyCode.T,
                KeyScanCode.Y => KeyCode.Y,
                KeyScanCode.U => KeyCode.U,
                KeyScanCode.I => KeyCode.I,
                KeyScanCode.O => KeyCode.O,
                KeyScanCode.P => KeyCode.P,
                KeyScanCode.OpenBracket => KeyCode.LeftBracket,
                KeyScanCode.CloseBracket => KeyCode.RightBracket,
                KeyScanCode.Enter_NumpadEnter => e0On ? KeyCode.KeypadEnter : KeyCode.Return,
                KeyScanCode.Ctrl => e0On ? KeyCode.RightControl : KeyCode.LeftControl,
                KeyScanCode.A => KeyCode.A,
                KeyScanCode.S => KeyCode.S,
                KeyScanCode.D => KeyCode.D,
                KeyScanCode.F => KeyCode.F,
                KeyScanCode.G => KeyCode.G,
                KeyScanCode.H => KeyCode.H,
                KeyScanCode.J => KeyCode.J,
                KeyScanCode.K => KeyCode.K,
                KeyScanCode.L => KeyCode.L,
                KeyScanCode.Semicolon => KeyCode.Semicolon,
                KeyScanCode.Quote => KeyCode.Quote,
                KeyScanCode.Backtick => KeyCode.BackQuote,
                KeyScanCode.LeftShift => KeyCode.LeftShift,
                KeyScanCode.Backslash => KeyCode.Backslash,
                KeyScanCode.Z => KeyCode.Z,
                KeyScanCode.X => KeyCode.X,
                KeyScanCode.C => KeyCode.C,
                KeyScanCode.V => KeyCode.V,
                KeyScanCode.B => KeyCode.B,
                KeyScanCode.N => KeyCode.N,
                KeyScanCode.M => KeyCode.M,
                KeyScanCode.Comma => KeyCode.Comma,
                KeyScanCode.Period => KeyCode.Period,
                KeyScanCode.Slash_NumpadDivide => e0On ? KeyCode.KeypadDivide : KeyCode.Slash,
                KeyScanCode.RightShift => KeyCode.RightShift,
                KeyScanCode.NumpadTimes_PrintScreen => e0On ? KeyCode.Print : KeyCode.KeypadMultiply,
                KeyScanCode.Alt => e0On ? KeyCode.RightAlt : KeyCode.LeftAlt,
                KeyScanCode.Space => KeyCode.Space,
                KeyScanCode.CapsLock => KeyCode.CapsLock,
                KeyScanCode.F1 => KeyCode.F1,
                KeyScanCode.F2 => KeyCode.F2,
                KeyScanCode.F3 => KeyCode.F3,
                KeyScanCode.F4 => KeyCode.F4,
                KeyScanCode.F5 => KeyCode.F5,
                KeyScanCode.F6 => KeyCode.F6,
                KeyScanCode.F7 => KeyCode.F7,
                KeyScanCode.F8 => KeyCode.F8,
                KeyScanCode.F9 => KeyCode.F9,
                KeyScanCode.F10 => KeyCode.F10,
                KeyScanCode.F11 => KeyCode.F11,
                KeyScanCode.F12 => KeyCode.F12,
                KeyScanCode.NumLock => KeyCode.Numlock,
                KeyScanCode.ScrollLock => KeyCode.ScrollLock,
                KeyScanCode.Numpad7_Home => e0On ? KeyCode.Home : KeyCode.Keypad7,
                KeyScanCode.Numpad8_Up => e0On ? KeyCode.UpArrow : KeyCode.Keypad8,
                KeyScanCode.Numpad9_PageUp => e0On ? KeyCode.PageUp : KeyCode.Keypad9,
                KeyScanCode.NumpadMinus => KeyCode.KeypadMinus,
                KeyScanCode.Numpad4_Left => e0On ? KeyCode.LeftArrow : KeyCode.Keypad4,
                KeyScanCode.Numpad5 => KeyCode.Keypad5,
                KeyScanCode.Numpad6_Right => e0On ? KeyCode.RightArrow : KeyCode.Keypad6,
                KeyScanCode.NumpadPlus => KeyCode.KeypadPlus,
                KeyScanCode.Numpad1_End => e0On ? KeyCode.End : KeyCode.Keypad1,
                KeyScanCode.Numpad2_Down => e0On ? KeyCode.DownArrow : KeyCode.Keypad2,
                KeyScanCode.Nupad3_PageDown => e0On ? KeyCode.PageDown : KeyCode.Keypad3,
                KeyScanCode.Numpad0_Insert => e0On ? KeyCode.Insert : KeyCode.Keypad0,
                KeyScanCode.NumpadDot_Delete => e0On ? KeyCode.Delete : KeyCode.KeypadPeriod,
                KeyScanCode._LeftWin => KeyCode.LeftWindows,
                KeyScanCode._RightWin => KeyCode.RightWindows,
                // KeyScanCode._Application => KeyCode.App,
                _ => KeyCode.None
            };
        }
    }
}