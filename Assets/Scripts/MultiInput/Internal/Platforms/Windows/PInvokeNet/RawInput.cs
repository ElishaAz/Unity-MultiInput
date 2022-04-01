using System.Runtime.InteropServices;

namespace MultiInput.Internal.Platforms.Windows.PInvokeNet
{
    /// <summary>
    /// Contains the raw input from a device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInput
    {
        /// <summary>
        /// Header for the data.
        /// </summary>
        public RawInputHeader Header;
        public Union Data;
        [StructLayout(LayoutKind.Explicit)]
        public struct Union
        {
            /// <summary>
            /// Mouse raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawMouse Mouse;
            /// <summary>
            /// Keyboard raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawKeyboard Keyboard;
            /// <summary>
            /// HID raw input data.
            /// </summary>
            [FieldOffset(0)]
            public RawHID HID;

            public override string ToString()
            {
                return $"{nameof(Mouse)}: {Mouse}, {nameof(Keyboard)}: {Keyboard}, {nameof(HID)}: {HID}";
            }
        }

        public override string ToString()
        {
            return $"{nameof(Header)}: {Header}, {nameof(Data)}: {Data}";
        }
    }
}