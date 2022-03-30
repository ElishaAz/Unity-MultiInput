using UnityEngine;

namespace MultiInput
{
    public interface IMouse
    {
        public delegate void Event(MouseEvent mouseEvent);

        public delegate void Movement(MouseMovement movement);

        public event Movement OnMove;
        public event Event OnEventUp;
        public event Event OnEventDown;

        public MouseMovement GetMouseMovement();

        public bool GetEvent(MouseEvent code);
        public bool GetEventDown(MouseEvent code);
        public bool GetEventUp(MouseEvent code);
    }
}