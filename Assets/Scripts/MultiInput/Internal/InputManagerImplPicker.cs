using System;

namespace MultiInput.Internal
{
    internal class InputManagerImplPicker
    {
        internal static void CreateInstance()
        {
            Instance = new InputManagerImplPicker();
        }

        internal static InputManagerImplPicker Instance { get; private set; }
        internal readonly IInputManager InputManagerImpl;

        private InputManagerImplPicker()
        {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            InputManagerImpl = new Platforms.Linux.InputManagerLinux();
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            InputManagerImpl = new Platforms.Windows.InputManagerWindows();
#else
            throw new PlatformNotSupportedException("MultiInput does not support this platform yet!");
#endif
        }

        internal void Dispose()
        {
            InputManagerImpl.Dispose();
        }

        internal void CallStartMainLoop()
        {
            foreach (var keyboard in InputManagerImpl.Keyboards)
            {
                keyboard.StartMainLoop();
            }

            foreach (var mouse in InputManagerImpl.Mice)
            {
                mouse.StartMainLoop();
            }
        }

        internal void CallMainLoop()
        {
            foreach (var keyboard in InputManagerImpl.Keyboards)
            {
                keyboard.MainLoop();
            }

            foreach (var mouse in InputManagerImpl.Mice)
            {
                mouse.MainLoop();
            }
        }

        internal void CallStopMainLoop()
        {
            foreach (var keyboard in InputManagerImpl.Keyboards)
            {
                keyboard.StopMainLoop();
            }

            foreach (var mouse in InputManagerImpl.Mice)
            {
                mouse.StopMainLoop();
            }
        }

        public void ScanForNewDevices()
        {
            InputManagerImpl.ScanForNewDevices();
        }
    }
}