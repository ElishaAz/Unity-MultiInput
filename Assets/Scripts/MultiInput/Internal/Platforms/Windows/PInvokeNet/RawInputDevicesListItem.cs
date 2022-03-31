using System;
using System.Runtime.InteropServices;

namespace MultiInput.Internal.Platforms.Windows.PInvokeNet
{
    [StructLayout( LayoutKind.Sequential )]
    public struct RawInputDevicesListItem
    {
        public IntPtr hDevice;
        public RawInputDeviceType Type;
    }
}