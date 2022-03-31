using System;

namespace MultiInput.Internal.Platforms.Windows.PInvokeNet
{
    /// <summary>
    /// Enumeration containing flags for raw keyboard input.
    /// </summary>
    [Flags]
    public enum RawKeyboardFlags : ushort
    {
        /// <summary></summary>
        KeyMake = 0,
        /// <summary></summary>
        KeyBreak = 1,
        /// <summary></summary>
        KeyE0 = 2,
        /// <summary></summary>
        KeyE1 = 4,
        /// <summary></summary>
        TerminalServerSetLED = 8,
        /// <summary></summary>
        TerminalServerShadow = 0x10,
        /// <summary></summary>
        TerminalServerVKPACKET = 0x20
    }
}