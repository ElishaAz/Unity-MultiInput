using MultiInput.Mouse;

namespace MultiInput.Internal
{
    public interface IMouseInternal : IMouse
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