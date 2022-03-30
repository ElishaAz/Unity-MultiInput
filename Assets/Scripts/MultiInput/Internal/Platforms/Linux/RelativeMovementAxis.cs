using Unity.VisualScripting;

namespace MultiInput.Internal.Platforms.Linux
{
    /// <summary>
    /// Mouse movements are expressed in an amount moved and an axis associated with that change.
    /// 0 represents movements on the X axis and 1 represents movements on the Y axis.
    /// </summary>
    public enum RelativeMovementAxis
    {
        X = 0,
        Y = 1,
        Z = 2,
        RX = 3,
        RY = 4,
        RZ = 5,
        HWheel = 6,
        Dial = 7,
        Wheel = 8,
        Misc = 9,
        Max = 10,
        CNT = 11
    }
}