// #define ABCDE

using AOT;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Windows
{
#if ABCDE
    public class InputRedirector : MonoBehaviour
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        const ushort HID_USAGE_PAGE_GENERIC = 0x01;
        const ushort HID_USAGE_GENERIC_MOUSE = 0x02;
        const ushort HID_USAGE_GENERIC_KEYBOARD = 0x06;
        const uint WM_INPUT = 0x00FF;
        const uint RID_INPUT = 0x10000003;
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        const int RIM_TYPEMOUSE = 0;
        const int RIM_TYPEKEYBOARD = 1;
        const ushort kSpaceScanCode = 0x39;

        struct WNDCLASSEXW
        {
            public int cbSize;
            public int style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;
            public IntPtr hIconSm;
        }



        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandleW([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U2)]
        static extern ushort RegisterClassExW([In] ref WNDCLASSEXW lpwcx);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool UnregisterClassW(IntPtr windowClass, IntPtr hInstance);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowExW(uint dwExStyle, IntPtr windowClass,
            [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
            uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance,
            IntPtr pvParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr DefWindowProcW(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        static extern bool RegisterRawInputDevices(
            [In] [MarshalAs(UnmanagedType.LPArray)] RawInputDevice[] pRawInputDevices, int uiNumDevices, int cbSize);

        [DllImport("User32.dll", SetLastError = true)]
        static extern int GetRegisteredRawInputDevices(
            [In] [Out] [MarshalAs(UnmanagedType.LPArray)] RawInputDevice[] pRawInputDevices, ref int puiNumDevices,
            int cbSize);

        [DllImport("User32.dll", SetLastError = true)]
        static extern int GetRawInputData(IntPtr hRawInput, uint uiCommand,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pData, ref int pcbSize, int cbSizeHeader);

        [DllImport("User32.dll", SetLastError = true)]
        static extern int GetRawInputBuffer([MarshalAs(UnmanagedType.LPArray)] byte[] pData, ref int pcbSize,
            int cbSizeHeader);

        delegate IntPtr WndProcDelegate(IntPtr window, uint message, IntPtr wParam, IntPtr lParam);

        static readonly WndProcDelegate s_WndProc = WndProc;
        static readonly int kRawInputHeaderSize = Marshal.SizeOf<RawInputHeader>();
        static readonly int kRawInputDeviceSize = Marshal.SizeOf<RawInputDevice>();
        static readonly bool kIsWow64 = IsWow64();

        static IntPtr s_HInstance = GetModuleHandleW(null);
        static byte[] s_RawInputBuffer = new byte[8192];
        static bool s_RedirectingInputToUnity = true;

        static byte[] s_RawInputEvents = new byte[8192];
        static int[] s_RawInputHeaderIndices = new int[100];
        static int[] s_RawInputDataIndices = new int[100];

        IntPtr m_WindowClass;
        IntPtr m_Hwnd;
        GUIStyle m_TextStyle;

        const int m_DeviceCount = 2;
        readonly RawInputDevice[] m_Devices;
        RawInputDevice[] m_QueryDevices;

        public InputRedirector()
        {
            m_Devices = new RawInputDevice[m_DeviceCount];
            m_QueryDevices = new RawInputDevice[m_DeviceCount];
        }

        private void RegisterRawInputDevices()
        {
            Debug.Log("Registering raw input devices");

            ref RawInputDevice mouse = ref m_Devices[0];
            mouse.UsagePage = HIDUsagePage.Generic;
            mouse.Usage = HIDUsage.Mouse;
            mouse.WindowHandle = m_Hwnd;

            ref RawInputDevice keyboard = ref m_Devices[1];
            keyboard.UsagePage = HIDUsagePage.Generic;
            keyboard.Usage = HIDUsage.Keyboard;
            keyboard.WindowHandle = m_Hwnd;

            if (!RegisterRawInputDevices(m_Devices, m_DeviceCount, kRawInputDeviceSize))
                throw new Win32Exception(Marshal.GetLastWin32Error(),
                    "Failed to register mouse and keyboard for Raw Input.");
        }

        void Awake()
        {
            m_WindowClass = RegisterWindowClass();
            m_Hwnd = CreateWindowExW(0, m_WindowClass, "Raw Input Redirection Window", 0, 0, 0, 0, 0, IntPtr.Zero,
                IntPtr.Zero, s_HInstance, IntPtr.Zero);
            if (m_Hwnd == null)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to create raw input redirection window.");

            RegisterRawInputDevices();
        }

        void Update()
        {
            int numberOfDevices = 0;
            GetRegisteredRawInputDevices(null, ref numberOfDevices, kRawInputDeviceSize);

            if (m_QueryDevices.Length < numberOfDevices)
                Array.Resize(ref m_QueryDevices, numberOfDevices);

            if (GetRegisteredRawInputDevices(m_QueryDevices, ref numberOfDevices, kRawInputDeviceSize) == -1)
            {
                var error = Marshal.GetLastWin32Error();
                Debug.LogError("GetRegisteredRawInputDevices failed: " + new Win32Exception(error).Message);
            }
            else
            {
                for (int i = 0; i < numberOfDevices; i++)
                {
                    if (m_QueryDevices[i].Usage == HIDUsage.Mouse ||
                        m_QueryDevices[i].Usage == HIDUsage.Keyboard)
                    {
                        if (m_QueryDevices[i].WindowHandle != m_Hwnd)
                        {
                            RegisterRawInputDevices();
                            break;
                        }
                    }
                }
            }
        }

        void OnDestroy()
        {
            if (!DestroyWindow(m_Hwnd))
                throw new Win32Exception(Marshal.GetLastWin32Error(),
                    "Failed to destroy raw input redirection window class.");

            if (!UnregisterClassW(m_WindowClass, s_HInstance))
                throw new Win32Exception(Marshal.GetLastWin32Error(),
                    "Failed to unregister raw input redirection window class.");
        }

        void OnGUI()
        {
            if (m_TextStyle == null)
            {
                m_TextStyle = new GUIStyle(GUI.skin.label);
                m_TextStyle.fontSize = 24;
            }

            GUI.Label(new Rect(100, 10, 500, 30), $"Redirecting input to Unity: {s_RedirectingInputToUnity}",
                m_TextStyle);
        }

        [MonoPInvokeCallback(typeof(WndProcDelegate))]
        static IntPtr WndProc(IntPtr window, uint message, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (message == WM_INPUT)
                    ProcessRawInputMessage(lParam);

                return DefWindowProcW(window, message, wParam, lParam);
            }
            catch (Exception e)
            {
                // Never let exception escape to native code as that will crash the app
                Debug.LogException(e);
                return IntPtr.Zero;
            }
        }

        static void ProcessRawInputMessage(IntPtr lParam)
        {
            int rawInputEventCount = 0;
            int rawInputEventsSize = 0;
            
            var sizeofRawInput = Marshal.SizeOf(typeof(RawInput));

            var result = Win32API.GetRawInputData(lParam, RawInputCommand.Input, out var input, ref sizeofRawInput,
                Marshal.SizeOf(typeof(RawInputHeader)));

            // First, process the message we received
            // int sizeofRawInputBuffer = s_RawInputBuffer.Length;
            // int result = GetRawInputData(lParam, RID_INPUT, s_RawInputBuffer, ref sizeofRawInputBuffer,
            //     kRawInputHeaderSize);
            if (result == -1)
            {
                var errorCode = Marshal.GetLastWin32Error();
                Debug.LogError($"Failed to get raw input data: {new Win32Exception(errorCode).Message}");
                return;
            }

            // CopyEventFromRawInputBuffer(0, kRawInputHeaderSize, ref rawInputEventCount, ref rawInputEventsSize);

            // Next, drain the raw input message queue so they don't keep getting
            // pumped one at a time through PeekMessage/DispatchMessage as that is
            // too slow with high input polling rate devices
            while (true)
            {
                var rawInputCount =
 Win32API.GetRawInputBuffer(out input, ref sizeofRawInput, Marshal.SizeOf(typeof(RawInputHeader)));
                if (rawInputCount == 0)
                    break;

                if (rawInputCount == -1)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    Debug.LogError($"Failed to get raw input buffer: {new Win32Exception(errorCode).Message}");
                    break;
                }
            }

            // Finally, dispatch the events to Unity
            if (s_RedirectingInputToUnity)
            {
                unsafe
                {
                    fixed (int* rawInputHeaderIndices = s_RawInputHeaderIndices)
                    {
                        fixed (int* rawInputDataIndices = s_RawInputDataIndices)
                        {
                            fixed (byte* rawInputData = s_RawInputEvents)
                            {
                                UnityEngine.Windows.Input.ForwardRawInput((uint*) rawInputHeaderIndices,
                                    (uint*) rawInputDataIndices, (uint) rawInputEventCount, rawInputData,
                                    (uint) rawInputEventsSize);
                            }
                        }
                    }
                }
            }
        }

        // private static int CopyEventFromRawInputBuffer(int offset, int dataOffset, ref int rawInputEventCount,
        //     ref int rawInputEventsSize)
        // {
        //     RawInputHeader header;
        //     unsafe
        //     {
        //         fixed (byte* rawInputBufferPtr = s_RawInputBuffer)
        //             header = *(RAWINPUTHEADER*) (rawInputBufferPtr + offset);
        //     }
        //
        //     if (header.Type == RawInputType.Keyboard)
        //     {
        //         unsafe
        //         {
        //             fixed (byte* rawInputBufferPtr = s_RawInputBuffer)
        //             {
        //                 RawKeyboard
        //                 var keyboard = *(RAWKEYBOARD*) (rawInputBufferPtr + dataOffset);
        //
        //                 // Releasing space key will toggle whether we forward events to Unity
        //                 bool keyPressed = (keyboard.Flags & RawKeyboardFlags.RI_KEY_BREAK) == 0;
        //                 if (!keyPressed && keyboard.MakeCode == kSpaceScanCode)
        //                     s_RedirectingInputToUnity = !s_RedirectingInputToUnity;
        //             }
        //         }
        //     }
        //
        //     if (rawInputEventsSize + header.dwSize > s_RawInputEvents.Length)
        //         Array.Resize(ref s_RawInputEvents,
        //             Math.Max(rawInputEventsSize + header.dwSize, 2 * s_RawInputEvents.Length));
        //
        //     if (rawInputEventCount == s_RawInputHeaderIndices.Length)
        //     {
        //         Array.Resize(ref s_RawInputHeaderIndices, 2 * s_RawInputHeaderIndices.Length);
        //         Array.Resize(ref s_RawInputDataIndices, 2 * s_RawInputHeaderIndices.Length);
        //     }
        //
        //     s_RawInputHeaderIndices[rawInputEventCount] = rawInputEventsSize;
        //     s_RawInputDataIndices[rawInputEventCount] = rawInputEventsSize + dataOffset;
        //     Array.Copy(s_RawInputBuffer, offset, s_RawInputEvents, rawInputEventsSize, header.dwSize);
        //
        //     rawInputEventCount++;
        //     rawInputEventsSize += header.dwSize;
        //     return header.dwSize;
        // }

        static IntPtr RegisterWindowClass()
        {
            var wndClass = new WNDCLASSEXW();
            wndClass.cbSize = Marshal.SizeOf<WNDCLASSEXW>();
            wndClass.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(s_WndProc);
            wndClass.hInstance = s_HInstance;
            wndClass.lpszClassName = "RawInputRedirector";

            var registeredClass = RegisterClassExW(ref wndClass);
            if (registeredClass == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to register the window class.");

            return new IntPtr(registeredClass);
        }

        private static bool IsWow64()
        {
            if (IntPtr.Size == 8)
                return false;

            if (!IsWow64Process(GetCurrentProcess(), out bool isWow64))
                throw new Win32Exception(Marshal.GetLastWin32Error(),
                    "Failed to figure out whether we're running under WOW64.");

            return isWow64;
        }

#endif // UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    }
#endif
}