using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AOT;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;

namespace MultiInput.Internal.Platforms.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public delegate bool InputAction(RawInput input);

    public delegate void DeviceChangedAction(RawInputDevicesListItem device);

    // public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Some code based on: https://forum.unity.com/threads/recieve-window_commands-in-unity.213741/
    /// And some from pinvoke.net examples.
    /// </summary>
    public class MyWMListener : IDisposable
    {
        private static IntPtr HWND_MESSAGE = new IntPtr(-3);

        private readonly InputAction onInput;
        private readonly DeviceChangedAction onDeviceAdded;
        private readonly DeviceChangedAction onDeviceRemoved;

        // private IntPtr interactionWindow;
        // private IntPtr hMainWindow;
        // private IntPtr oldWndProcPtr;
        // private IntPtr newWndProcPtr;
        // private WndProcDelegate newWndProc;
        private bool isRunning = false;

        private readonly IntPtr hInstance;
        private IntPtr windowClass;
        private IntPtr hRawInputWindow;
        private readonly WndProcDelegate myWndProc;

        private HashSet<RawInputDevicesListItem> rawInputDevices = new HashSet<RawInputDevicesListItem>();

        public MyWMListener(InputAction onInput,
            DeviceChangedAction onDeviceAdded, DeviceChangedAction onDeviceRemoved)
        {
            hInstance = Win32API.GetModuleHandleW(null);
            myWndProc = new WndProcDelegate(WndProc);
            this.onInput = onInput;
            this.onDeviceAdded = onDeviceAdded;
            this.onDeviceRemoved = onDeviceRemoved;
            Start();
        }

        private void Start()
        {
            if (isRunning) return;

            windowClass = RegisterWindowClass(out var wndClass);

            if (windowClass == IntPtr.Zero)
            {
                // TODO: Error
                Debug.LogError("Could Not Register WindowClass");
                return;
            }

            // var hMainWindow = Win32API.GetForegroundWindow();

            // hRawInputWindow = Win32API.CreateWindowExW(0, windowClass, "Unity-MultiInput Raw Input Sink",
            //     0, 0, 0, 100, 100, IntPtr.Zero, 
            //     IntPtr.Zero, hInstance, IntPtr.Zero);
            hRawInputWindow = Win32API.CreateWindowEx(WindowStylesEx.WS_EX_LEFT, wndClass.lpszClassName,
                "Unity-MultiInput Raw Input Sink", 0, 0, 0, 0, 0,
                HWND_MESSAGE, IntPtr.Zero, hInstance, IntPtr.Zero);

            // Win32API.ShowWindow(hRawInputWindow, (int)ShowWindowCommands.Show);

            // newWndProc = new WndProcDelegate(WndProc);
            // newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newWndProc);
            // oldWndProcPtr =
            //     Win32API.SetWindowLongPtr(new HandleRef(this, hMainWindow), Win32API.GWLP_WNDPROC, newWndProcPtr);

            if (!RegisterRawInputDevices())
            {
                // TODO: Error
                Debug.LogError("Could Not Register Raw Input");
                return;
            }

            isRunning = true;
        }

        private bool RegisterRawInputDevices()
        {
            var rid = new RawInputDevice[2];

            rid[0].UsagePage = HIDUsagePage.Generic; // HID_USAGE_PAGE_GENERIC
            rid[0].Usage = HIDUsage.Mouse; // HID_USAGE_GENERIC_MOUSE
            rid[0].Flags = RawInputDeviceFlags.InputSink; // adds mouse
            rid[0].WindowHandle = hRawInputWindow;

            rid[1].UsagePage = HIDUsagePage.Generic; // HID_USAGE_PAGE_GENERIC
            rid[1].Usage = HIDUsage.Keyboard; // HID_USAGE_GENERIC_KEYBOARD
            rid[1].Flags = RawInputDeviceFlags.InputSink; // adds keyboard
            rid[1].WindowHandle = hRawInputWindow;

            var ret = Win32API.RegisterRawInputDevices(rid, rid.Length,
                Marshal.SizeOf(typeof(RawInputDevice)));

            var allRawDevices = GetAllRawDevices();
            if (allRawDevices == null)
            {
                var error = Marshal.GetLastWin32Error();
                Debug.LogError($"GetAllRawDevices failed ({error}): " + new Win32Exception(error).Message);
                return ret;
            }

            var devices = allRawDevices.ToHashSet();

            foreach (var device in rawInputDevices.Except(devices))
            {
                onDeviceAdded?.Invoke(device);
            }

            foreach (var device in devices.Except(rawInputDevices))
            {
                onDeviceAdded?.Invoke(device);
            }

            rawInputDevices = devices;

            return ret;
        }

        private IntPtr RegisterWindowClass(out WndClassExW wndClass)
        {
            wndClass = new WndClassExW
            {
                cbSize = Marshal.SizeOf<WndClassExW>(),
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(myWndProc),
                hInstance = this.hInstance,
                lpszClassName = "Unity-MultiInput_RawInputSink"
            };

            var registeredClass = Win32API.RegisterClassExW(ref wndClass);
            return registeredClass == 0 ? IntPtr.Zero : new IntPtr(registeredClass);
        }

        // A convenient function for getting all raw input devices.
        // This method will get all devices, including virtual devices
        // For remote desktop and any other device driver that's registered
        // as such a device.
        private static RawInputDevicesListItem[] GetAllRawDevices()
        {
            uint deviceCount = 0;
            uint dwSize = (uint) Marshal.SizeOf(typeof(RawInputDevicesListItem));

            // First call the system routine with a null pointer
            // for the array to get the size needed for the list
            var retValue = Win32API.GetRawInputDeviceList(null, ref deviceCount, dwSize);

            if (retValue == -1)
                return null;

            // Now allocate an array of the specified number of entries
            var deviceList = new RawInputDevicesListItem[deviceCount];

            // Now make the call again, using the array
            retValue = Win32API.GetRawInputDeviceList(deviceList, ref deviceCount, dwSize);

            // Finally, return the filled in list
            return -1 == retValue ? null : deviceList;
        }


        private RawInputDevice[] rawInputDeviceList = new RawInputDevice[2];

        public void ScanForNewDevices()
        {
            int numberOfDevices = 0;
            Win32API.GetRegisteredRawInputDevices(null, ref numberOfDevices, Marshal.SizeOf<RawInputDevice>());

            // if (rid.Length < numberOfDevices)
            //     Array.Resize(ref rid, numberOfDevices);
            if (numberOfDevices != rawInputDeviceList.Length)
                rawInputDeviceList = new RawInputDevice[numberOfDevices];

            if (Win32API.GetRegisteredRawInputDevices(rawInputDeviceList, ref numberOfDevices,
                    Marshal.SizeOf<RawInputDevice>()) == -1)
            {
                var error = Marshal.GetLastWin32Error();
                Debug.LogError("GetRegisteredRawInputDevices failed: " + new Win32Exception(error).Message);
            }
            else
            {
                for (int i = 0; i < numberOfDevices; i++)
                {
                    var device = rawInputDeviceList[i];
                    if (device.Usage != HIDUsage.Mouse && device.Usage != HIDUsage.Keyboard) continue;
                    if (device.WindowHandle == hRawInputWindow) continue;
                    RegisterRawInputDevices();
                    break;
                }
            }
        }

        // IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        // {
        //     if (msg == WM_INPUT)
        //     {
        //         onMyEvent(wParam, lParam);
        //     }
        //     return CallWindowProc(oldWndProcPtr, hWnd, msg, wParam, lParam);
        // }
        [MonoPInvokeCallback(typeof(WndProcDelegate))]
        private IntPtr WndProc(IntPtr window, uint message, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (message != (uint) WM.INPUT) return Win32API.DefWindowProcW(window, message, wParam, lParam);

                var size = Marshal.SizeOf(typeof(RawInput));

                var outSize = Win32API.GetRawInputData(lParam, RawInputCommand.Input, out var input, ref size,
                    Marshal.SizeOf(typeof(RawInputHeader)));

                if (outSize == -1)
                {
                    Debug.LogError("outSize == -1");
                    return Win32API.DefWindowProcW(window, message, wParam, lParam);
                }

                Debug.Log(input);
                if (onInput?.Invoke(input) != true)
                {
                    return Win32API.DefWindowProcW(window, message, wParam, lParam);
                }
            }
            catch (Exception e)
            {
                // Never let exception escape to native code as that will crash the app
                Debug.LogException(e);
            }

            return IntPtr.Zero;
        }


        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool manual)
        {
            if (!isRunning) return;

            if (manual)
            {
            }

            if (!Win32API.DestroyWindow(hRawInputWindow))
            {
                // TODO: Error
                Debug.LogError("Could Not Destroy Window");
            }
            // throw new Win32Exception(Marshal.GetLastWin32Error(),
            //     "Failed to destroy raw input redirection window class.");

            if (!Win32API.UnregisterClassW(windowClass, hInstance))
            {
                // TODO: Error
                Debug.LogError("Could Not Unregister Class");
            }

            isRunning = false;

            GC.SuppressFinalize(this);
        }

        ~MyWMListener()
        {
            Dispose(false);
        }
    }
}