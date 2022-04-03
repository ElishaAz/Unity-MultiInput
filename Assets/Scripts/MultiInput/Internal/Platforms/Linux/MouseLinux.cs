using System;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public class MouseLinux : MouseAbstract
    {
        private readonly Action<MouseMovement, MouseLinux> invokeAnyMouseMovement;
        private readonly Action<MouseEvent, MouseLinux> invokeAnyMouseEvent;

        private readonly InputReader inputReader;
        private bool grab;

        internal MouseLinux(string path, Action<MouseMovement, MouseLinux> invokeAnyMouseMovement,
            Action<MouseEvent, MouseLinux> invokeAnyMouseEvent)
        {
            this.invokeAnyMouseMovement = invokeAnyMouseMovement;
            this.invokeAnyMouseEvent = invokeAnyMouseEvent;

            inputReader = new InputReader(path, HandleEvent);
        }

        private void HandleEvent(EventType type, short code, int value)
        {
            switch (type)
            {
                case EventType.EV_KEY:
                    var mouseEvent = ConverterLinux.EventCodeToMouseEvent((EventCode) code);
                    var state = (KeyState) value;
                    keyPressProvider.HandleEvent(mouseEvent, ConverterLinux.KeyStateToKeyEvent(state));
                    invokeAnyMouseEvent?.Invoke(mouseEvent, this);
                    break;

                case EventType.EV_REL:
                    var axis = (RelativeMovementAxis) code;
                    RelativeMoveEventHandler(axis, value);
                    break;
            }
        }

        private void RelativeMoveEventHandler(RelativeMovementAxis axis, int value)
        {
            switch (axis)
            {
                case RelativeMovementAxis.X or RelativeMovementAxis.Y:
                {
                    var movementNow = axis == RelativeMovementAxis.X ? new Vector2(value, 0) : new Vector2(0, -value);

                    movement.SetCurrent(movement.GetCurrent() + movementNow);

                    // Debug.Log($"Mouse moved {movementNow}");

                    var mouseMovement = new MouseMovement(movementNow);

                    InvokeOnMove(mouseMovement);
                    invokeAnyMouseMovement.Invoke(mouseMovement, this);
                    break;
                }
                case RelativeMovementAxis.Wheel:
                    wheel.SetCurrent(wheel.GetCurrent() + value);
                    break;
                case RelativeMovementAxis.HWheel:
                    hWheel.SetCurrent(hWheel.GetCurrent() + value);
                    break;
            }
        }

        public void Dispose()
        {
            inputReader.Dispose();
        }

        public override bool Grab
        {
            get => grab;
            set
            {
                grab = value;
                inputReader.ReOpen(value);
            }
        }
    }
}