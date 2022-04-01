using System.Runtime.InteropServices;

namespace MultiInput.Internal.Platforms.Windows.PInvokeNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RawHID
    {
        public int Size;
        public int Count;

        public override string ToString()
        {
            return $"{nameof(Size)}: {Size}, {nameof(Count)}: {Count}";
        }
    }
}