using System.Runtime.InteropServices;

namespace MultiInput.Internal.Platforms.Windows.PInvokeNet
{
    /// <summary>
    /// Value type for raw input from a keyboard.
    /// </summary>    
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        /// <summary>Scan code for key depression.</summary>
        public short MakeCode;
        /// <summary>Scan code information.</summary>
        public RawKeyboardFlags Flags;
        /// <summary>Reserved.</summary>
        public short Reserved;
        /// <summary>Virtual key code.</summary>
        public VirtualKeys VirtualKey;
        /// <summary>Corresponding window message.</summary>
        public WM Message;
        /// <summary>Extra information.</summary>
        public int ExtraInformation;

        public override string ToString()
        {
            return $"{nameof(MakeCode)}: {MakeCode}, {nameof(Flags)}: {Flags}, {nameof(Reserved)}: {Reserved}, {nameof(VirtualKey)}: {VirtualKey}, {nameof(Message)}: {Message}, {nameof(ExtraInformation)}: {ExtraInformation}";
        }
    }

}