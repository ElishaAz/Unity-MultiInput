using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;

namespace MultiInput.Internal.Platforms.Windows
{
    public class InputManagerWindows : InputManagerAbstract
    {
        private readonly MyWMListener listener;

        private readonly Dictionary<IntPtr, KeyboardWindows> keyboards = new Dictionary<IntPtr, KeyboardWindows>();
        private readonly Dictionary<IntPtr, MouseWindows> mice = new Dictionary<IntPtr, MouseWindows>();


        public InputManagerWindows()
        {
            listener = new MyWMListener(OnInput, OnDeviceAdded, OnDeviceRemoved);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void OnDeviceAdded(RawInputDevicesListItem device)
        {
            switch (device.Type)
            {
                case RawInputDeviceType.MOUSE:
                    var mouse = new MouseWindows(InvokeAnyMouseMovement, InvokeAnyMouseEvent, InvokeAnyMouseWheel);
                    mice.Add(device.hDevice, mouse);
                    break;
                case RawInputDeviceType.KEYBOARD:
                    var keyboard = new KeyboardWindows(InvokeAnyKeyboardPress);
                    keyboards.Add(device.hDevice, keyboard);
                    break;
                case RawInputDeviceType.HID:
                default:
                    return;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void OnDeviceRemoved(RawInputDevicesListItem device)
        {
            switch (device.Type)
            {
                case RawInputDeviceType.MOUSE:
                    if (mice.Remove(device.hDevice, out var mouse))
                    {
                        InvokeMouseRemoved(mouse);
                    }

                    break;
                case RawInputDeviceType.KEYBOARD:
                    if (keyboards.Remove(device.hDevice, out var keyboard))
                    {
                        InvokeKeyboardRemoved(keyboard);
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
            return keyboards.TryGetValue(device, out var keyboardWindows) &&
                   keyboardWindows.Process(keyboard);
        }


        public override void Dispose()
        {
        }

        public override void ScanForNewDevices()
        {
            listener.ScanForNewDevices();
        }
    }
}