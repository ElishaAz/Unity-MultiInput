using System;

namespace MultiInput.Internal
{
    internal class InputManagerImplPicker : IDisposable
    {
        internal static void CreateInstance()
        {
            Instance = new InputManagerImplPicker();
        }

        public static InputManagerImplPicker Instance { get; private set; }
        public readonly IInputManager InputManagerImpl;

        private InputManagerImplPicker()
        {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            InputManagerImpl = new Platforms.Linux.InputManagerLinux();
#elif UNITY_EDITOR_WINDOWS || UNITY_STANDALONE_WINDOWS
            InputManagerImpl = new Platforms.Windows.InputManagerWindows();
#else
            throw new PlatformNotSupportedException("MultiInput does not support this platform yet!");
#endif
        }

        public void Dispose()
        {
            InputManagerImpl.Dispose();
        }
    }
}