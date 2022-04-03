using UnityEngine;

namespace MultiInput
{
    public interface IMouse
    {
        public delegate void EventAction(MouseEvent mouseEvent);

        public delegate void MovementAction(MouseMovement movement);

        public delegate void WheelAction(float movement);

        public event MovementAction OnMove;
        public event WheelAction OnWheel;
        public event WheelAction OnHWheel;
        public event EventAction OnEventUp;
        public event EventAction OnEventDown;

        public MouseMovement GetMouseMovement();

        public float GetWheelMovement();
        public float GetHWheelMovement();

        public bool Grab { get; set; }

        public bool GetEvent(MouseEvent code);
        public bool GetEventDown(MouseEvent code);
        public bool GetEventUp(MouseEvent code);
    }
}