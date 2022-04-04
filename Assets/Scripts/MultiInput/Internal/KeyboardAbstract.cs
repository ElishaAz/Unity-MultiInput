using System;
using MultiInput.Keyboard;
using UnityEngine;

namespace MultiInput.Internal
{
    public abstract class KeyboardAbstract : IKeyboardInternal
    {
        protected readonly KeyPressProvider<KeyCode> keyPressProvider;
        private readonly AnyKeyboardAction invokeAnyKeyboardPress;

        protected KeyboardAbstract(AnyKeyboardAction invokeAnyKeyboardPress)
        {
            this.invokeAnyKeyboardPress = invokeAnyKeyboardPress;

            keyPressProvider = new KeyPressProvider<KeyCode>(
                KeyPressed,
                KeyReleased);
        }

        private void KeyPressed(KeyCode code)
        {
            OnKeyPressed?.Invoke(code);
            invokeAnyKeyboardPress(code, this);
        }

        private void KeyReleased(KeyCode code)
        {
            OnKeyReleased?.Invoke(code);
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

        public bool GetKey(KeyCode code) => keyPressProvider.KeysHeld.Contains(code);
        public event IKeyboard.Action OnKeyPressed;
        public bool GetKeyDown(KeyCode code) => keyPressProvider.KeysDown.Contains(code);
        public event IKeyboard.Action OnKeyReleased;
        public bool GetKeyUp(KeyCode code) => keyPressProvider.KeysUp.Contains(code);


        public abstract bool Grab { get; set; }
    }
}