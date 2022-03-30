using System.Collections.Generic;
using MultiInput.Internal;
using UnityEngine;

namespace MultiInput
{
    public static class InputManager
    {
        public static IReadOnlyList<IKeyboard> Keyboards =>
            InputManagerImplPicker.Instance.InputManagerImpl.Keyboards;
        public static IReadOnlyList<IMouse> Mice =>
            InputManagerImplPicker.Instance.InputManagerImpl.Mice;


        public static event AnyKeyboardAction OnAnyKeyboardPress
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyKeyboardPress += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyKeyboardPress -= value;
        }
        public static event AnyMouseEvent OnAnyMouseEvent
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseClick += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseClick -= value;
        }
        public static event AnyMouseMovement OnAnyMouseMovement
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseMovement += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyMouseMovement -= value;
        }
    }
}