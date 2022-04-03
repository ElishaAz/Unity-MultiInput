using System;
using System.Collections.Generic;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;

namespace MultiInput.Internal.Platforms.Windows
{
    public class InputManagerWindows : IInputManager
    {
        private readonly MyWMListener listener;

        private readonly Dictionary<IntPtr, KeyboardWindows> keyboards = new Dictionary<IntPtr, KeyboardWindows>();
        private readonly List<KeyboardWindows> keyboardsList = new List<KeyboardWindows>();
        private readonly Dictionary<IntPtr, MouseWindows> mice = new Dictionary<IntPtr, MouseWindows>();
        private readonly List<MouseWindows> miceList = new List<MouseWindows>();


        public InputManagerWindows()
        {
            listener = new MyWMListener(OnInput, OnDeviceAdded, OnDeviceRemoved);
        }

        private void OnDeviceAdded(RawInputDevicesListItem device)
        {
            switch (device.Type)
            {
                case RawInputDeviceType.MOUSE:
                    var mouse = new MouseWindows();
                    mice.Add(device.hDevice, mouse);
                    miceList.Add(mouse);
                    break;
                case RawInputDeviceType.KEYBOARD:
                    var keyboard = new KeyboardWindows();
                    keyboards.Add(device.hDevice, keyboard);
                    keyboardsList.Add(keyboard);
                    break;
                case RawInputDeviceType.HID:
                default:
                    return;
            }
        }

        private void OnDeviceRemoved(RawInputDevicesListItem device)
        {
            switch (device.Type)
            {
                case RawInputDeviceType.MOUSE:
                    if (mice.Remove(device.hDevice, out var mouse))
                    {
                        miceList.Remove(mouse);
                        OnMouseRemoved?.Invoke(mouse);
                    }

                    break;
                case RawInputDeviceType.KEYBOARD:
                    if (keyboards.Remove(device.hDevice, out var keyboard))
                    {
                        keyboardsList.Remove(keyboard);
                        OnKeyboardRemoved?.Invoke(keyboard);
                    }

                    break;
                case RawInputDeviceType.HID:
                default:
                    return;
            }
        }

        private bool OnInput(RawInput input)
        {
            switch (input.Header.Type)
            {
                case RawInputType.Mouse:
                    return HandleMouse(input.Data.Mouse, input.Header.Device);
                case RawInputType.Keyboard:
                    return HandleKeyboard(input.Data.Keyboard, input.Header.Device);
                case RawInputType.HID:
                case RawInputType.Other:
                default:
                    return false;
            }
        }

        private bool HandleMouse(RawMouse mouse, IntPtr device)
        {
            return mice.TryGetValue(device, out var mouseWindows) && mouseWindows.Process(mouse);
        }

        private bool HandleKeyboard(RawKeyboard keyboard, IntPtr device)
        {
            return false;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<IKeyboardInternal> Keyboards => keyboardsList;
        public IReadOnlyList<IMouseInternal> Mice => miceList;
        public event AnyKeyboardAction OnAnyKeyboardPress;
        public event AnyMouseEvent OnAnyMouseClick;
        public event AnyMouseMovement OnAnyMouseMovement;
        public event KeyboardAddedAction OnKeyboardAdded;
        public event KeyboardRemovedAction OnKeyboardRemoved;
        public event MouseAddedAction OnMouseAdded;
        public event MouseRemovedAction OnMouseRemoved;
    }
}