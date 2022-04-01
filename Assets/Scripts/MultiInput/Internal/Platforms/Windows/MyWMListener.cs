using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;

namespace MultiInput.Internal.Platforms.Windows
{
    public delegate void InputAction(RawInput input);

    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Some code based on: https://forum.unity.com/threads/recieve-window_commands-in-unity.213741/
    /// And some from pinvoke.net examples.
    /// </summary>
    public class MyWMListener : IDisposable
    {
        // private IntPtr interactionWindow;
        private IntPtr hMainWindow;
        private IntPtr oldWndProcPtr;
        private IntPtr newWndProcPtr;
        private WndProcDelegate newWndProc;
        private bool isRunning = false;

        public MyWMListener()
        {
            Start();
        }

        private void Start()
        {
            if (isRunning) return;

            hMainWindow = Win32API.GetForegroundWindow();
            newWndProc = new WndProcDelegate(WndProc);
            newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newWndProc);
            oldWndProcPtr = Win32API.SetWindowLongPtr(new HandleRef(this, hMainWindow), Win32API.GWLP_WNDPROC, newWndProcPtr);

            // Debug.Log(string.Join(",",GetAllRawDevices().Select(a => a.ToString())));

            var rid = new RawInputDevice[2];

            rid[0].UsagePage = HIDUsagePage.Generic; // HID_USAGE_PAGE_GENERIC
            rid[0].Usage = HIDUsage.Mouse; // HID_USAGE_GENERIC_MOUSE
            rid[0].Flags = RawInputDeviceFlags.None; // adds mouse
            rid[0].WindowHandle = hMainWindow;

            rid[1].UsagePage = HIDUsagePage.Generic; // HID_USAGE_PAGE_GENERIC
            rid[1].Usage = HIDUsage.Keyboard; // HID_USAGE_GENERIC_MOUSE
            rid[1].Flags = RawInputDeviceFlags.None; // adds mouse
            rid[1].WindowHandle = hMainWindow;

            Win32API.RegisterRawInputDevices(rid, rid.Length,
                Marshal.SizeOf(typeof(RawInputDevicesListItem)));

            isRunning = true;
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
            uint retValue = Win32API.GetRawInputDeviceList(null, ref deviceCount, dwSize);

            // If anything but zero is returned, the call failed, so return a null list
            if (0 != retValue)
                return null;

            // Now allocate an array of the specified number of entries
            var deviceList = new RawInputDevicesListItem[deviceCount];

            // Now make the call again, using the array
            retValue = Win32API.GetRawInputDeviceList(deviceList, ref deviceCount, dwSize);

            // Free up the memory we first got the information into as
            // it is no longer needed, since the structures have been
            // copied to the deviceList array.
            //IntPtr pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
            //Marshal.FreeHGlobal(pRawInputDeviceList);

            // Finally, return the filled in list
            return 0 != retValue ? null : deviceList;
        }

        private static IntPtr StructToPtr(object obj)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        // IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        // {
        //     if (msg == WM_INPUT)
        //     {
        //         onMyEvent(wParam, lParam);
        //     }
        //     return CallWindowProc(oldWndProcPtr, hWnd, msg, wParam, lParam);
        // }
        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg != (int) WM.INPUT)
                return Win32API.CallWindowProc(oldWndProcPtr, hWnd, msg, wParam, lParam);

            var size = Marshal.SizeOf(typeof(RawInput));

            var outSize = Win32API.GetRawInputData(lParam, RawInputCommand.Input, out var input, ref size,
                Marshal.SizeOf(typeof(RawInputHeader)));
            if (outSize != -1)
            {
                OnInput?.Invoke(input);
                Debug.Log(input);
            }

            return Win32API.CallWindowProc(oldWndProcPtr, hWnd, msg, wParam, lParam);
        }


        public void Dispose()
        {
            if (!isRunning) return;
            isRunning = false;
            var rid = new RawInputDevice[2];

            rid[0].UsagePage = HIDUsagePage.Generic; // HID_USAGE_PAGE_GENERIC
            rid[0].Usage = HIDUsage.Mouse; // HID_USAGE_GENERIC_MOUSE
            rid[0].Flags = RawInputDeviceFlags.Remove; // adds mouse
            rid[0].WindowHandle = hMainWindow;

            rid[1].UsagePage = HIDUsagePage.Generic; // HID_USAGE_PAGE_GENERIC
            rid[1].Usage = HIDUsage.Keyboard; // HID_USAGE_GENERIC_KEYBOARD
            rid[1].Flags = RawInputDeviceFlags.Remove; // adds keyboard
            rid[1].WindowHandle = hMainWindow;

            Win32API.RegisterRawInputDevices(rid, rid.Length,
                Marshal.SizeOf(typeof(RawInputDevicesListItem)));
            
            Win32API.SetWindowLongPtr(new HandleRef(this, hMainWindow), Win32API.GWLP_WNDPROC, default);

            hMainWindow = IntPtr.Zero;
            oldWndProcPtr = IntPtr.Zero;
            newWndProcPtr = IntPtr.Zero;
            newWndProc = null;
            isRunning = false;

            // GC.SuppressFinalize(this);
        }

        // ~MyWMListener()
        // {
        //     Dispose();
        // }

        public event InputAction OnInput;
    }
}