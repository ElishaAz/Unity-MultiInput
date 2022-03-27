using System.Collections.Generic;
using MultiInput.Internal;
using UnityEngine;

namespace MultiInput
{
    public static class InputManager
    {
        public static IReadOnlyList<IKeyboard> Keyboards =>
            InputManagerImplPicker.Instance.InputManagerImpl.Keyboards;


        public static event AnyKeyboardAction OnAnyKeyboardPress
        {
            add => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyKeyboardPress += value;
            remove => InputManagerImplPicker.Instance.InputManagerImpl.OnAnyKeyboardPress -= value;
        }
    }
}