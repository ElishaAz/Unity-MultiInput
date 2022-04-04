using System.Collections.Generic;
using MultiInput.Internal;
using MultiInput.Keyboard;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput
{
    public static class InputManager
    {
        public static IReadOnlyCollection<IKeyboardInternal> Keyboards =>
            InputManagerImplPicker.Instance.InputManagerImpl.Keyboards;

        public static IReadOnlyCollection<IMouse> Mice =>
            InputManagerImplPicker.Instance.InputManagerImpl.Mice;

        #region any_device

        public static event AnyKeyboardAction OnAnyKeyboardPress
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyKeyboardPress += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyKeyboardPress -= value;
        }

        public static event AnyMouseEvent OnAnyMouseEvent
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseEvent += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseEvent -= value;
        }

        public static event AnyMouseMovement OnAnyMouseMovement
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseMovement += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseMovement -= value;
        }

        public static event AnyMouseWheel OnAnyMouseWheel
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseWheel += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseWheel -= value;
        }

        #endregion

        #region device_changed

        /// <summary>
        /// Called whenever a new keyboard is connected.
        /// </summary>
        public static event KeyboardAddedAction OnKeyboardAdded
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnKeyboardAdded += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnKeyboardAdded -= value;
        }

        /// <summary>
        /// Called whenever a keyboard is disconnected.
        /// </summary>
        public static event KeyboardRemovedAction OnKeyboardRemoved
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnKeyboardRemoved += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnKeyboardRemoved -= value;
        }


        /// <summary>
        /// Called whenever a new mouse is connected.
        /// </summary>
        public static event MouseAddedAction OnMouseAdded
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnMouseAdded += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnMouseAdded -= value;
        }

        /// <summary>
        /// Called whenever a mouse is disconnected.
        /// </summary>
        public static event MouseRemovedAction OnMouseRemoved
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnMouseRemoved += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnMouseRemoved -= value;
        }

        #endregion
    }
}