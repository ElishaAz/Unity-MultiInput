using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public class KeyboardLinux : IKeyboardInternal
    {
        private readonly InputReader inputReader;
        private readonly Action<KeyCode, KeyboardLinux> invokeAnyKeyboardPress;

        private readonly KeyPressProvider<KeyCode> keyPressProvider;

        internal KeyboardLinux(string path, Action<KeyCode, KeyboardLinux> invokeAnyKeyboardPress)
        {
            this.invokeAnyKeyboardPress = invokeAnyKeyboardPress;
            keyPressProvider = new KeyPressProvider<KeyCode>(
                code => OnKeyPressed?.Invoke(code),
                code => OnKeyReleased?.Invoke(code));

            inputReader = new InputReader(path,HandleEvent);
        }

        private void HandleEvent(EventType type, short code, int value)
        {
            if (type != EventType.EV_KEY) return;

            var keyCode = ConverterLinux.EventCodeToKeyCode((EventCode) code);
            keyPressProvider.HandleEvent(keyCode, ConverterLinux.KeyStateToKeyEvent((KeyState) value));
            invokeAnyKeyboardPress?.Invoke(keyCode, this);
        }

        void IKeyboardInternal.StartMainLoop()
        {
            keyPressProvider.StartMainLoop();
        }

        void IKeyboardInternal.MainLoop()
        {
            keyPressProvider.MainLoop();
        }

        void IKeyboardInternal.StopMainLoop()
        {
            keyPressProvider.StopMainLoop();
        }

        public void Dispose()
        {
            inputReader.Dispose();
        }


        public event IKeyboard.Action OnKeyPressed;
        public event IKeyboard.Action OnKeyReleased;

        public bool GetKey(KeyCode code)
        {
            return keyPressProvider.KeysHeld.Contains(code);
        }

        public bool GetKeyDown(KeyCode code)
        {
            return keyPressProvider.KeysDown.Contains(code);
        }

        public bool GetKeyUp(KeyCode code)
        {
            return keyPressProvider.KeysUp.Contains(code);
        }

        public bool Grab { get; set; }
    }
}