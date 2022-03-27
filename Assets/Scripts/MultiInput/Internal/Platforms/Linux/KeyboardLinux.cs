using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public class KeyboardLinux : IKeyboardInternal, IDisposable
    {
        private struct MyKeyEvent
        {
            public MyKeyEvent(KeyCode code, KeyState state)
            {
                Code = code;
                State = state;
            }

            public readonly KeyCode Code;
            public readonly KeyState State;
        }

        private readonly InputReader inputReader;
        private readonly Action<KeyCode, KeyboardLinux> invokeAnyKeyboardPress;
        private readonly ConcurrentQueue<MyKeyEvent> actions = new ConcurrentQueue<MyKeyEvent>();
        private readonly HashSet<KeyCode> keysDown = new HashSet<KeyCode>();
        private readonly HashSet<KeyCode> keysUp = new HashSet<KeyCode>();
        private readonly HashSet<KeyCode> keysHeld = new HashSet<KeyCode>();

        internal KeyboardLinux(InputReader inputReader, Action<KeyCode, KeyboardLinux> invokeAnyKeyboardPress)
        {
            this.inputReader = inputReader;
            this.invokeAnyKeyboardPress = invokeAnyKeyboardPress;
            inputReader.OnKeyPress += EventHandler;
        }

        private void EventHandler(KeyPressEvent pressEvent)
        {
            var key = InputConverter.EventCodeToKeyCode(pressEvent.Code);
            switch (pressEvent.State)
            {
                case KeyState.KeyDown:
                    keysHeld.Add(key);
                    OnKeyPressed?.Invoke(key);
                    break;
                case KeyState.KeyHold:
                    keysHeld.Add(key);
                    break;
                case KeyState.KeyUp:
                    keysHeld.Remove(key);
                    OnKeyReleased?.Invoke(key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (pressEvent.State == KeyState.KeyHold) return;
            var myEvent = new MyKeyEvent(key, pressEvent.State);
            actions.Enqueue(myEvent);
        }

        void IKeyboardInternal.StartMainLoop()
        {
            keysDown.Clear();
            keysUp.Clear();
            keysHeld.Clear();
        }

        void IKeyboardInternal.MainLoop()
        {
            keysDown.Clear();
            keysUp.Clear();

            while (actions.TryDequeue(out var action))
            {
                switch (action.State)
                {
                    case KeyState.KeyDown:
                        keysDown.Add(action.Code);
                        invokeAnyKeyboardPress?.Invoke(action.Code, this);
                        break;
                    case KeyState.KeyUp:
                        keysUp.Add(action.Code);
                        break;
                }
            }
        }

        void IKeyboardInternal.StopMainLoop()
        {
            keysDown.Clear();
            keysUp.Clear();
            keysHeld.Clear();
        }

        public void Dispose()
        {
            inputReader.OnKeyPress -= EventHandler;
            inputReader.Dispose();
        }


        public event IKeyboard.Action OnKeyPressed;
        public event IKeyboard.Action OnKeyReleased;

        public bool GetKey(KeyCode code)
        {
            return keysHeld.Contains(code);
        }

        public bool GetKeyDown(KeyCode code)
        {
            return keysDown.Contains(code);
        }

        public bool GetKeyUp(KeyCode code)
        {
            return keysUp.Contains(code);
        }
    }
}