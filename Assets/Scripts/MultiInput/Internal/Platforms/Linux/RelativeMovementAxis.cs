using Unity.VisualScripting;

namespace MultiInput.Internal.Platforms.Linux
{
    /// <summary>
    /// Mouse movements are expressed in an amount moved and an axis associated with that change.
    /// 0 represents movements on the X axis and 1 represents movements on the Y axis.
    /// </summary>
    public enum MouseAxis
    {
        X = 0,
        Y = 1,
        Z = 2,
        RX = 3,
        RY = 4,
        RZ = 5,
        HWHEEL = 6,
        DIAL = 7,
        WHEEL = 8,
        MISC = 9,
        MAX = 10,
        CNT = 11
    }
}