using System;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public class MouseLinux : InputReader, IMouseInternal
    {
        private readonly Action<MouseMovement, MouseLinux> invokeAnyMouseMovement;
        private readonly Action<MouseEvent, MouseLinux> invokeAnyMouseEvent;

        private Vector2 absPos;

        private Vector2 currentMovement;
        private Vector2 movement;
        private bool isAbsolute;

        private KeyPressProvider<MouseEvent> keyPressProvider;

        internal MouseLinux(string path, Action<MouseMovement, MouseLinux> invokeAnyMouseMovement,
            Action<MouseEvent, MouseLinux> invokeAnyMouseEvent) : base(path)
        {
            this.invokeAnyMouseMovement = invokeAnyMouseMovement;
            this.invokeAnyMouseEvent = invokeAnyMouseEvent;
            keyPressProvider = new KeyPressProvider<MouseEvent>(
                e => OnEventDown?.Invoke(e),
                e => OnEventUp?.Invoke(e));
        }

        protected override void HandleEvent(EventType type, short code, int value)
        {
            switch (type)
            {
                case EventType.EV_KEY:
                    var mouseEvent = InputConverter.EventCodeToMouseEvent((EventCode) code);
                    var state = (KeyState) value;
                    keyPressProvider.HandleEvent(mouseEvent, state);
                    invokeAnyMouseEvent?.Invoke(mouseEvent, this);
                    break;

                case EventType.EV_REL:
                    var axis = (RelativeMovementAxis) code;
                    RelativeMoveEventHandler(axis, value);
                    break;
                case EventType.EV_ABS:
                    var axisAbs = (AbsoluteMovementAxis) code;
                    AbsoluteMoveEventHandler(axisAbs, value);
                    Debug.Log($"Mouse moved abs {code}, {value}");
                    // OnMouseMove?.Invoke(e);
                    break;
            }
        }

        private void RelativeMoveEventHandler(RelativeMovementAxis axis, int value)
        {
            var movementNow = axis switch
            {
                RelativeMovementAxis.X => new Vector2(value, 0),
                RelativeMovementAxis.Y => new Vector2(0, value),
                _ => Vector2.zero
            };

            lock (this)
            {
                absPos += movementNow;
                currentMovement += movementNow;
                isAbsolute = false;
            }

            Debug.Log($"Mouse moved {movementNow}");

            var mouseMovement = new MouseMovement(false, absPos, movementNow);

            OnMove?.Invoke(mouseMovement);
            invokeAnyMouseMovement.Invoke(mouseMovement, this);
        }

        private void AbsoluteMoveEventHandler(AbsoluteMovementAxis axis, int value)
        {
            var posNow = axis switch
            {
                AbsoluteMovementAxis.MT_POSITION_X => new Vector2(value, 0),
                AbsoluteMovementAxis.MT_POSITION_Y => new Vector2(0, value),
                _ => Vector2.zero
            };

            lock (this)
            {
                currentMovement += posNow - absPos;
                absPos = posNow;
                isAbsolute = true;
            }

            var mouseMovement = new MouseMovement(true, absPos, posNow);

            OnMove?.Invoke(mouseMovement);
            invokeAnyMouseMovement.Invoke(mouseMovement, this);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        void IMouseInternal.StartMainLoop()
        {
            keyPressProvider.StartMainLoop();

            lock (this)
            {
                movement = currentMovement = Vector2.zero;
            }
        }

        void IMouseInternal.MainLoop()
        {
            keyPressProvider.MainLoop();

            lock (this)
            {
                movement = currentMovement;
                currentMovement = Vector2.zero;
            }
        }

        void IMouseInternal.StopMainLoop()
        {
            keyPressProvider.StopMainLoop();

            lock (this)
            {
                movement = currentMovement = Vector2.zero;
            }
        }

        public event IMouse.Movement OnMove;
        public event IMouse.Event OnEventUp;
        public event IMouse.Event OnEventDown;

        public MouseMovement GetMouseMovement() => new MouseMovement(isAbsolute, absPos, movement);

        public bool GetEvent(MouseEvent code) => keyPressProvider.KeysHeld.Contains(code);

        public bool GetEventDown(MouseEvent code) => keyPressProvider.KeysDown.Contains(code);

        public bool GetEventUp(MouseEvent code) => keyPressProvider.KeysUp.Contains(code);
    }
}