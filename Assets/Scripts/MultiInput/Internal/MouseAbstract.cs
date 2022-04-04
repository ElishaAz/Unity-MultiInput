using System;
using MultiInput.Mouse;
using UnityEngine;

namespace MultiInput.Internal
{
    public abstract class MouseAbstract : IMouseInternal
    {
        protected readonly CommitLockVal<Vector2> movement = new CommitLockVal<Vector2>();
        protected readonly CommitLockVal<float> wheel = new CommitLockVal<float>();
        protected readonly CommitLockVal<float> hWheel = new CommitLockVal<float>();

        protected readonly KeyPressProvider<MouseEvent> keyPressProvider;
        private readonly AnyMouseMovement invokeAnyMouseMovement;
        private readonly AnyMouseEvent invokeAnyMouseEvent;
        private readonly AnyMouseWheel invokeAnyMouseWheel;

        protected MouseAbstract(AnyMouseMovement invokeAnyMouseMovement,
            AnyMouseEvent invokeAnyMouseEvent, AnyMouseWheel invokeAnyMouseWheel)
        {
            this.invokeAnyMouseWheel = invokeAnyMouseWheel;
            this.invokeAnyMouseEvent = invokeAnyMouseEvent;
            this.invokeAnyMouseMovement = invokeAnyMouseMovement;

            keyPressProvider = new KeyPressProvider<MouseEvent>(
                OnKeyPressed,
                OnKeyReleased);
        }

        private void OnKeyPressed(MouseEvent e)
        {
            OnEventDown?.Invoke(e);
            invokeAnyMouseEvent(e, this);
        }

        private void OnKeyReleased(MouseEvent e)
        {
            OnEventUp?.Invoke(e);
        }

        #region main_loop

        void IMouseInternal.StartMainLoop()
        {
            keyPressProvider.StartMainLoop();
            StartMainLoop();
            movement.Reset();
            wheel.Reset();
            hWheel.Reset();
        }

        protected virtual void StartMainLoop()
        {
        }

        void IMouseInternal.MainLoop()
        {
            keyPressProvider.MainLoop();
            MainLoop();
            movement.Commit();
            wheel.Commit();
            hWheel.Commit();
        }

        protected virtual void MainLoop()
        {
        }

        void IMouseInternal.StopMainLoop()
        {
            keyPressProvider.StopMainLoop();
            StartMainLoop();
            movement.Reset();
            wheel.Reset();
            hWheel.Reset();
        }

        protected virtual void StopMainLoop()
        {
        }

        #endregion

        public event IMouse.MovementAction OnMove;

        protected void InvokeOnMove(MouseMovement m)
        {
            OnMove?.Invoke(m);
            invokeAnyMouseMovement(m, this);
        }

        public MouseMovement GetMouseMovement() => new MouseMovement(movement.GetCommitted());

        public event IMouse.WheelAction OnWheel;

        protected void InvokeOnWheel(float m)
        {
            OnWheel?.Invoke(m);
            invokeAnyMouseWheel(m, false, this);
        }

        public float GetWheelMovement() => wheel.GetCommitted();

        public event IMouse.WheelAction OnHWheel;

        protected void InvokeOnHWheel(float m)
        {
            OnHWheel?.Invoke(m);
            invokeAnyMouseWheel(m, true, this);
        }

        public float GetHWheelMovement() => hWheel.GetCommitted();


        public bool GetEvent(MouseEvent code) => keyPressProvider.KeysHeld.Contains(code);

        public event IMouse.EventAction OnEventDown;
        protected void InvokeOnEventDown(MouseEvent mouseEvent) => OnEventDown?.Invoke(mouseEvent);
        public bool GetEventDown(MouseEvent code) => keyPressProvider.KeysDown.Contains(code);

        public event IMouse.EventAction OnEventUp;
        protected void InvokeOnEventUp(MouseEvent mouseEvent) => OnEventUp?.Invoke(mouseEvent);
        public bool GetEventUp(MouseEvent code) => keyPressProvider.KeysUp.Contains(code);


        public abstract bool Grab { get; set; }
    }
}