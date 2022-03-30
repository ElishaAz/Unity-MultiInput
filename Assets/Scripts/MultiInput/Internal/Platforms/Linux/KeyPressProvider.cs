using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public class KeyPressProvider<T>
    {
        private struct MyKeyEvent
        {
            public MyKeyEvent(T code, KeyState state)
            {
                Code = code;
                State = state;
            }

            public readonly T Code;
            public readonly KeyState State;
        }
        
        private readonly ConcurrentQueue<MyKeyEvent> actions = new ConcurrentQueue<MyKeyEvent>();
        public readonly HashSet<T> KeysDown = new HashSet<T>();
        public readonly HashSet<T> KeysUp = new HashSet<T>();
        public readonly HashSet<T> KeysHeld = new HashSet<T>();
        
        private readonly Action<T> onKeyPressed;
        private readonly Action<T> onKeyReleased;

        public KeyPressProvider(Action<T> onKeyPressed, Action<T> onKeyReleased)
        {
            this.onKeyPressed = onKeyPressed;
            this.onKeyReleased = onKeyReleased;
        }
        
        public void HandleEvent(T key, KeyState state)
        {
            switch (state)
            {
                case KeyState.KeyDown:
                    KeysHeld.Add(key);
                    onKeyPressed?.Invoke(key);
                    break;
                case KeyState.KeyHold:
                    KeysHeld.Add(key);
                    break;
                case KeyState.KeyUp:
                    KeysHeld.Remove(key);
                    onKeyReleased?.Invoke(key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (state == KeyState.KeyHold) return;
            var myEvent = new MyKeyEvent(key, state);
            actions.Enqueue(myEvent);
        }

        public void StartMainLoop()
        {
            KeysDown.Clear();
            KeysUp.Clear();
            KeysHeld.Clear();
        }

        public void MainLoop()
        {
            KeysDown.Clear();
            KeysUp.Clear();

            while (actions.TryDequeue(out var action))
            {
                switch (action.State)
                {
                    case KeyState.KeyDown:
                        KeysDown.Add(action.Code);
                        break;
                    case KeyState.KeyUp:
                        KeysUp.Add(action.Code);
                        break;
                }
            }
        }

        public void StopMainLoop()
        {
            KeysDown.Clear();
            KeysUp.Clear();
            KeysHeld.Clear();
        }
    }
}