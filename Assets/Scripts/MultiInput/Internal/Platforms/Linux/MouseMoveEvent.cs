using System;

namespace MultiInput.Internal.Platforms.Linux
{
    /// <summary>
    /// Here is the event that I use process mouse movement change updates.
    /// </summary>
    public class MouseMoveEvent : EventArgs
    {
        public MouseMoveEvent(MouseAxis axis, int amount)
        {
            Axis = axis;
            Amount = amount;
        }
    
        public MouseAxis Axis { get; }
    
        public int Amount { get; set; }
    }
}