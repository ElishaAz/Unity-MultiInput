using System;
using System.Collections;
using System.Collections.Generic;
using MultiInput.Internal;
using UnityEngine;

namespace MultiInput
{
    public interface IInputManager: IDisposable
    {
        public IReadOnlyList<IKeyboardInternal> Keyboards { get; }
        public IReadOnlyList<IMouseInternal> Mice { get; }

        public event AnyKeyboardAction OnAnyKeyboardPress;
        public event AnyMouseEvent OnAnyMouseClick;
        public event AnyMouseMovement OnAnyMouseMovement;
    }
}