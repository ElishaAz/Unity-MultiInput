using System;
using System.Runtime.CompilerServices;
using MultiInput.Keyboard;

namespace MultiInput.Internal.Platforms.Linux
{
    public class KeyboardLinux : KeyboardAbstract
    {
        private readonly Action<IKeyboardInternal> invokeKeyboardRemoved;
        private bool grab;

        private readonly InputReader inputReader;

        internal KeyboardLinux(string path, Action<IKeyboardInternal> invokeKeyboardRemoved,
            AnyKeyboardAction invokeAnyKeyboardPress) : base(invokeAnyKeyboardPress)
        {
            this.invokeKeyboardRemoved = invokeKeyboardRemoved;
            inputReader = new InputReader(path, HandleEvent, HandleDisconnect);
        }

        private void HandleDisconnect()
        {
            invokeKeyboardRemoved(this);
        }

        private void HandleEvent(EventType type, short code, int value)
        {
            if (type != EventType.EV_KEY) return;
            var eventCode = (EventCode) code;
            var keyCode = ConverterLinux.EventCodeToKeyCode(eventCode);
            keyPressProvider.HandleEvent(keyCode, ConverterLinux.KeyStateToKeyEvent((KeyState) value));
        }

        public void Dispose()
        {
            inputReader.Dispose();
        }

        public override bool Grab
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => grab;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                inputReader.SetGrab(value);
                grab = value;
            }
        }
    }
}