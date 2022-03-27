namespace MultiInput.Internal.Platforms.Linux
{
    /// <summary>
    /// Like many other event handling systems there are multiple events that happen each time that
    /// the user presses a key. Once when the key is pressed down, another when the key is pressed up,
    /// and another if the user decides to hold the key down.
    /// </summary>
    public enum KeyState
    {
        KeyUp,
        KeyDown,
        KeyHold
    }
}