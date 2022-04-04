using System;
using System.Runtime.CompilerServices;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public class MouseLinux : MouseAbstract
    {
        private readonly Action<IMouseInternal> invokeMouseRemoved;
        private readonly InputReader inputReader;
        private bool grab;

        internal MouseLinux(string path, Action<IMouseInternal> invokeMouseRemoved,
            AnyMouseMovement invokeAnyMouseMovement,
            AnyMouseEvent invokeAnyMouseEvent, AnyMouseWheel invokeAnyMouseWheel) : base(invokeAnyMouseMovement,
            invokeAnyMouseEvent, invokeAnyMouseWheel)
        {
            this.invokeMouseRemoved = invokeMouseRemoved;
            inputReader = new InputReader(path, HandleEvent, HandleDisconnect);
        }

        private void HandleDisconnect()
        {
            invokeMouseRemoved(this);
        }

        private void HandleEvent(EventType type, short code, int value)
        {
            switch (type)
            {
                case EventType.EV_KEY:
                    var mouseEvent = ConverterLinux.EventCodeToMouseEvent((EventCode) code);
                    var state = (KeyState) value;
                    keyPressProvider.HandleEvent(mouseEvent, ConverterLinux.KeyStateToKeyEvent(state));
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
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => grab;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                grab = value;
                inputReader.SetGrab(value);
            }
        }
    }
}