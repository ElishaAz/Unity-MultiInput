using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MultiInput.Internal.Platforms.Linux;
using UnityEngine;

namespace MultiInput.Internal
{
    public class KeyPressProvider<T>
    {
        private struct MyKeyEvent
        {
            public MyKeyEvent(T code, KeyEventState state)
            {
                Code = code;
                State = state;
            }

            public readonly T Code;
            public readonly KeyEventState State;
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

        public void HandleEvent(T key, KeyEventState state)
        {
            switch (state)
            {
                case KeyEventState.Down:
                    KeysHeld.Add(key);
                    onKeyPressed?.Invoke(key);
                    break;
                case KeyEventState.Held:
                    KeysHeld.Add(key);
                    break;
                case KeyEventState.Up:
                    KeysHeld.Remove(key);
                    onKeyReleased?.Invoke(key);
                    break;
                case KeyEventState.Released:
                    KeysHeld.Remove(key);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            var myEvent = new MyKeyEvent(key, state);
            actions.Enqueue(myEvent);
        }

        public void StartMainLoop()
        {
            KeysDown.Clear();
            KeysUp.Clear();
        }

        public void MainLoop()
        {
            KeysDown.Clear();
            KeysUp.Clear();

            while (actions.TryDequeue(out var action))
            {
                switch (action.State)
                {
                    case KeyEventState.Down:
                        KeysDown.Add(action.Code);
                        break;
                    case KeyEventState.Up:
                        KeysUp.Add(action.Code);
                        break;
                }
            }
        }

        public void StopMainLoop()
        {
            KeysDown.Clear();
            KeysUp.Clear();
        }
    }
}