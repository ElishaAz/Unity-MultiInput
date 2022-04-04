using MultiInput.Keyboard;

namespace MultiInput.Internal
{
    public interface IKeyboardInternal : IKeyboard
    {
        internal virtual void StartMainLoop()
        {
        }

        internal virtual void MainLoop()
        {
        }

        internal virtual void StopMainLoop()
        {
        }
    }
}