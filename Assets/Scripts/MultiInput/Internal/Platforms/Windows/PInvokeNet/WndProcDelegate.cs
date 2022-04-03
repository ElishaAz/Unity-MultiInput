using System;

namespace MultiInput.Internal.Platforms.Windows.PInvokeNet
{
    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}