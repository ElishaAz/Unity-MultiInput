using System;

namespace MultiInput.Internal.Platforms.Linux
{
    /// <summary>
    /// Here is the event that I use to process key press events.
    /// </summary>
    public class KeyPressEvent : EventArgs
    {
        public KeyPressEvent(EventCode code, KeyState state)
        {
            Code = code;
            State = state;
        }

        public EventCode Code { get; }
    
        public KeyState State { get; }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(State)}: {State}";
        }
    }
}