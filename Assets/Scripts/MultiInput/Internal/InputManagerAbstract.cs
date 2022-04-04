using System.Collections.Generic;
using MultiInput.Keyboard;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput.Internal
{
    public abstract class InputManagerAbstract : IInputManager
    {
        private readonly HashSet<IKeyboardInternal> keyboards = new HashSet<IKeyboardInternal>();
        private readonly HashSet<IMouseInternal> mice = new HashSet<IMouseInternal>();

        public abstract void ScanForNewDevices();

        public abstract void Dispose();

        #region any_device

        public virtual event AnyKeyboardAction OnAnyKeyboardPress;

        protected void InvokeAnyKeyboardPress(KeyCode code, IKeyboard keyboard) =>
            OnAnyKeyboardPress?.Invoke(code, keyboard);

        public virtual event AnyMouseEvent OnAnyMouseEvent;

        protected void InvokeAnyMouseEvent(MouseEvent e, IMouse mouse) =>
            OnAnyMouseEvent?.Invoke(e, mouse);

        public virtual event AnyMouseMovement OnAnyMouseMovement;

        protected void InvokeAnyMouseMovement(MouseMovement movement, IMouse mouse) =>
            OnAnyMouseMovement?.Invoke(movement, mouse);

        public virtual event AnyMouseWheel OnAnyMouseWheel;

        protected void InvokeAnyMouseWheel(float movement, bool horizontal, IMouse mouse) =>
            OnAnyMouseWheel?.Invoke(movement, horizontal, mouse);

        #endregion

        #region device_change

        public IReadOnlyCollection<IKeyboardInternal> Keyboards => keyboards;
        public event KeyboardAddedAction OnKeyboardAdded;
        public virtual event KeyboardRemovedAction OnKeyboardRemoved;

        protected void InvokeKeyboardAdded(IKeyboardInternal keyboard)
        {
            keyboards.Add(keyboard);
            OnKeyboardAdded?.Invoke(keyboard);
        }

        protected void InvokeKeyboardRemoved(IKeyboardInternal keyboard)
        {
            keyboards.Remove(keyboard);
            OnKeyboardRemoved?.Invoke(keyboard);
        }


        public IReadOnlyCollection<IMouseInternal> Mice => mice;
        public event MouseAddedAction OnMouseAdded;
        public virtual event MouseRemovedAction OnMouseRemoved;

        protected void InvokeMouseAdded(IMouseInternal mouse)
        {
            mice.Add(mouse);
            OnMouseAdded?.Invoke(mouse);
        }

        protected void InvokeMouseRemoved(IMouseInternal mouse)
        {
            mice.Remove(mouse);
            OnMouseRemoved?.Invoke(mouse);
        }

        #endregion
    }
}