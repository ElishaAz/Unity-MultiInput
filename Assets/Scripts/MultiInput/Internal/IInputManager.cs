using System;
using System.Collections;
using System.Collections.Generic;
using MultiInput.Internal;
using MultiInput.Keyboard;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput
{
    public interface IInputManager : IDisposable
    {
        internal virtual void StartMainLoop()
        {
        }

        internal virtual void MainLoop()
        {
        }

        internal virtual void StopMainLoop()
        {
        }

        public void ScanForNewDevices();

        public IReadOnlyCollection<IKeyboardInternal> Keyboards { get; }
        public IReadOnlyCollection<IMouseInternal> Mice { get; }

        public event AnyKeyboardAction OnAnyKeyboardPress;
        public event AnyMouseEvent OnAnyMouseEvent;
        public event AnyMouseMovement OnAnyMouseMovement;
        public event AnyMouseWheel OnAnyMouseWheel;

        public event KeyboardAddedAction OnKeyboardAdded;
        public event KeyboardRemovedAction OnKeyboardRemoved;
        public event MouseAddedAction OnMouseAdded;
        public event MouseRemovedAction OnMouseRemoved;
    }
}