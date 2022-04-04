using System;
using MultiInput.Internal.Platforms.Linux;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;
using MultiInput.Keyboard;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Windows
{
    public class KeyboardWindows : KeyboardAbstract
    {
        public KeyboardWindows(AnyKeyboardAction invokeAnyKeyboardPress): base(invokeAnyKeyboardPress)
        {
        }

        internal bool Process(RawKeyboard rawKeyboard)
        {
            var scanCode = rawKeyboard.MakeCode;
            var state = (rawKeyboard.Flags & RawKeyboardFlags.KeyBreak) != 0 ? KeyEventState.Up : KeyEventState.Down;

            // Key break scan codes are supposed to be 0x80 more than the regular scan code
            if (scanCode >= 0x80)
            {
                state = KeyEventState.Up;
                scanCode -= 0x80;
            }

            var code = ConverterWindows.ScanCodeToKeyCode((KeyScanCode) scanCode,
                (rawKeyboard.Flags & RawKeyboardFlags.KeyE0) != 0);

            keyPressProvider.HandleEvent(code, state);

            return Grab;
        }


        public override bool Grab { get; set; }
    }
}