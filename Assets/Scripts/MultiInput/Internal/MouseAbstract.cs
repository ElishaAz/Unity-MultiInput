using UnityEngine;

namespace MultiInput.Internal
{
    public abstract class MouseAbstract : IMouseInternal
    {
        protected readonly LoopLockVal<Vector2> movement = new LoopLockVal<Vector2>();
        protected readonly LoopLockVal<float> wheel = new LoopLockVal<float>();
        protected readonly LoopLockVal<float> hWheel = new LoopLockVal<float>();

        protected readonly KeyPressProvider<MouseEvent> keyPressProvider;

        protected MouseAbstract()
        {
            keyPressProvider = new KeyPressProvider<MouseEvent>(
                e => OnEventDown?.Invoke(e),
                e => OnEventUp?.Invoke(e));
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
        protected void InvokeOnMove(MouseMovement m) => OnMove?.Invoke(m);
        public MouseMovement GetMouseMovement() => new MouseMovement(movement.GetCommitted());

        public event IMouse.WheelAction OnWheel;
        protected void InvokeOnWheel(float m) => OnWheel?.Invoke(m);
        public float GetWheelMovement() => wheel.GetCommitted();

        public event IMouse.WheelAction OnHWheel;
        protected void InvokeOnHWheel(float m) => OnHWheel?.Invoke(m);
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