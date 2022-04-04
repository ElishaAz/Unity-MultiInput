using System;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public static class ConverterLinux
    {
        public static KeyCode EventCodeToKeyCode(EventCode eventCode)
        {
            var key = eventCode switch
            {
                EventCode.A => KeyCode.A,
                EventCode.Esc => KeyCode.Escape,
                EventCode.Num1 => KeyCode.Alpha1,
                EventCode.Num2 => KeyCode.Alpha2,
                EventCode.Num3 => KeyCode.Alpha3,
                EventCode.Num4 => KeyCode.Alpha4,
                EventCode.Num5 => KeyCode.Alpha5,
                EventCode.Num6 => KeyCode.Alpha6,
                EventCode.Num7 => KeyCode.Alpha7,
                EventCode.Num8 => KeyCode.Alpha8,
                EventCode.Num9 => KeyCode.Alpha9,
                EventCode.Num0 => KeyCode.Alpha0,
                EventCode.Minus => KeyCode.Minus,
                EventCode.Equal => KeyCode.Equals,
                EventCode.Backspace => KeyCode.Backspace,
                EventCode.Tab => KeyCode.Tab,
                EventCode.Q => KeyCode.Q,
                EventCode.W => KeyCode.Q,
                EventCode.E => KeyCode.E,
                EventCode.R => KeyCode.R,
                EventCode.T => KeyCode.T,
                EventCode.Y => KeyCode.Y,
                EventCode.U => KeyCode.U,
                EventCode.I => KeyCode.I,
                EventCode.O => KeyCode.O,
                EventCode.P => KeyCode.P,
                EventCode.LeftBrace => KeyCode.LeftBracket,
                EventCode.RightBrace => KeyCode.RightBracket,
                EventCode.Enter => KeyCode.Return,
                EventCode.LeftCtrl => KeyCode.LeftControl,
                EventCode.S => KeyCode.S,
                EventCode.D => KeyCode.D,
                EventCode.F => KeyCode.F,
                EventCode.G => KeyCode.G,
                EventCode.H => KeyCode.H,
                EventCode.J => KeyCode.J,
                EventCode.K => KeyCode.K,
                EventCode.L => KeyCode.L,
                EventCode.Semicolon => KeyCode.Semicolon,
                EventCode.Apostrophe => KeyCode.Quote,
                EventCode.Grave => KeyCode.BackQuote,
                EventCode.LeftShift => KeyCode.LeftShift,
                EventCode.Backslash => KeyCode.Backslash,
                EventCode.Z => KeyCode.Z,
                EventCode.X => KeyCode.X,
                EventCode.C => KeyCode.C,
                EventCode.V => KeyCode.V,
                EventCode.B => KeyCode.B,
                EventCode.N => KeyCode.N,
                EventCode.M => KeyCode.M,
                EventCode.Comma => KeyCode.Comma,
                EventCode.Dot => KeyCode.Period,
                EventCode.Slash => KeyCode.Slash,
                EventCode.RightShift => KeyCode.RightShift,
                EventCode.KpAsterisk => KeyCode.KeypadMultiply,
                EventCode.LeftAlt => KeyCode.LeftAlt,
                EventCode.Space => KeyCode.Space,
                EventCode.Capslock => KeyCode.CapsLock,
                EventCode.F1 => KeyCode.F1,
                EventCode.Pf2 => KeyCode.F2,
                EventCode.F3 => KeyCode.F3,
                EventCode.F4 => KeyCode.F4,
                EventCode.F5 => KeyCode.F5,
                EventCode.F6 => KeyCode.F6,
                EventCode.F7 => KeyCode.F7,
                EventCode.F8 => KeyCode.F8,
                EventCode.Pf9 => KeyCode.F9,
                EventCode.F10 => KeyCode.F10,
                EventCode.Numlock => KeyCode.Numlock,
                EventCode.ScrollLock => KeyCode.ScrollLock,
                EventCode.Kp7 => KeyCode.Keypad7,
                EventCode.Kp8 => KeyCode.Keypad8,
                EventCode.Kp9 => KeyCode.Keypad9,
                EventCode.PkpMinus => KeyCode.KeypadMinus,
                EventCode.Kp4 => KeyCode.Keypad5,
                EventCode.Kp5 => KeyCode.Keypad5,
                EventCode.Kp6 => KeyCode.Keypad6,
                EventCode.KpPlus => KeyCode.KeypadPlus,
                EventCode.Kp1 => KeyCode.Keypad1,
                EventCode.Kp2 => KeyCode.Keypad2,
                EventCode.Kp3 => KeyCode.Keypad3,
                EventCode.Kp0 => KeyCode.Keypad0,
                EventCode.KpDot => KeyCode.KeypadPeriod,
                EventCode.Zenkakuhankaku => KeyCode.None,
                EventCode.F11 => KeyCode.F11,
                EventCode.F12 => KeyCode.F12,
                EventCode.KpEnter => KeyCode.KeypadEnter,
                EventCode.RightCtrl => KeyCode.RightControl,
                EventCode.KpSlash => KeyCode.KeypadDivide,
                EventCode.SysRq => KeyCode.SysReq,
                EventCode.RightAlt => KeyCode.RightAlt,
                EventCode.Home => KeyCode.Home,
                EventCode.Up => KeyCode.UpArrow,
                EventCode.Pageup => KeyCode.PageUp,
                EventCode.Left => KeyCode.LeftArrow,
                EventCode.Right => KeyCode.RightArrow,
                EventCode.End => KeyCode.End,
                EventCode.Down => KeyCode.DownArrow,
                EventCode.Pagedown => KeyCode.PageDown,
                EventCode.Insert => KeyCode.Insert,
                EventCode.Delete => KeyCode.Delete,
                EventCode.KpEqual => KeyCode.KeypadEquals,
                // case EventCode.KpPlusMinus:
                //     key = KeyCode.KeypadPlus;
                //     break;
                EventCode.Pause => KeyCode.Pause,
                EventCode.LeftMeta => KeyCode.LeftMeta,
                EventCode.RightMeta => KeyCode.RightMeta,
                EventCode.Help => KeyCode.Help,
                EventCode.Menu => KeyCode.Menu,
                EventCode.Back => KeyCode.Escape,
                EventCode.Homepage => KeyCode.Home,
                EventCode.ScrollUp => KeyCode.PageUp,
                EventCode.ScrollDown => KeyCode.PageDown,
                EventCode.F13 => KeyCode.F13,
                EventCode.F14 => KeyCode.F14,
                EventCode.F15 => KeyCode.F15,
                EventCode.Print => KeyCode.Print,
                EventCode.Question => KeyCode.Question,
                EventCode.LeftMouse => KeyCode.Mouse0,
                EventCode.RightMouse => KeyCode.Mouse1,
                EventCode.MiddleMouse => KeyCode.Mouse2,
                EventCode.MouseBack => KeyCode.Mouse3,
                EventCode.MouseForward => KeyCode.Mouse4,
                _ => KeyCode.None
            };

            return key;
        }

        public static MouseEvent EventCodeToMouseEvent(EventCode eventCode)
        {
            return eventCode switch
            {
                EventCode.LeftMouse => MouseEvent.LeftMouse,
                EventCode.RightMouse => MouseEvent.RightMouse,
                EventCode.MiddleMouse => MouseEvent.MiddleMouse,
                EventCode.MouseBack => MouseEvent.MouseBack,
                EventCode.MouseForward => MouseEvent.MouseForward,
                EventCode.ToolFinger => MouseEvent.SingleFingerTap,
                EventCode.ToolDoubleTap => MouseEvent.DoubleFingerTap,
                EventCode.ToolTripleTap => MouseEvent.TripleFingerTap,
                EventCode.ToolQuadTap => MouseEvent.QuadFingerTap,
                EventCode.ToolQuintTap => MouseEvent.QuintFingerTap,
                EventCode.Touch => MouseEvent.Touch,

                _ => MouseEvent.Other
            };
        }

        public static KeyEventState KeyStateToKeyEvent(KeyState state)
        {
            return state switch
            {
                KeyState.KeyUp => KeyEventState.Up,
                KeyState.KeyDown => KeyEventState.Down,
                KeyState.KeyHold => KeyEventState.Held,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}