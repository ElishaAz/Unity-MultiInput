using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MultiInput.Internal.Platforms.Linux.IOCtlPInvoke;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    public delegate void HandleEventAction(EventType type, short code, int value);

    /// <summary>
    /// This is where the bulk of the work happens. Here we have a class, where you provide the
    /// path to one of the event files and it publishes updates whenever it comes in.
    /// An example file that does this would be "/dev/input/event0".
    /// <br/><br/>
    /// More research would be needed to support more events types, but I was only interested in
    /// keyboard and mouse input so it serves my purposes. I also opted to drop the timestamp that
    /// is included with each button event, but if you're interested, you can find it on the first
    /// 16 bits on the buffer.
    /// </summary>
    public class InputReader : IDisposable
    {
        private readonly string path;

        private readonly HandleEventAction handleEvent;
        private readonly Action onDisconnect;

        private const int BufferLength = 24;

        private readonly byte[] buffer = new byte[BufferLength];

        private bool disposing;

        private SafeUnixHandle handle;
        private Task task;

        public InputReader(string path, HandleEventAction handleEvent, Action onDisconnect)
        {
            this.path = path;
            this.handleEvent = handleEvent;
            this.onDisconnect = onDisconnect;

            handle = UnsafeNativeMethods.Open(path, 0);

            task = Task.Run(Run);
        }

        private void Run()
        {
            try
            {
                while (true)
                {
                    if (disposing)
                        break;

                    var response = UnsafeNativeMethods.Read(handle, buffer, BufferLength);

                    if (response == -1)
                    {
                        var error = Marshal.GetLastWin32Error();

                        if (error == 19) // Device not found = device disconnected
                        {
                            onDisconnect();
                        }
                        else
                        {
                            Debug.LogException(new UnixIOException(error));
                        }

                        break;
                    }

                    if (disposing)
                        break;

                    var type = BitConverter.ToInt16(new[] {buffer[16], buffer[17]}, 0);
                    var code = BitConverter.ToInt16(new[] {buffer[18], buffer[19]}, 0);
                    var value = BitConverter.ToInt32(new[] {buffer[20], buffer[21], buffer[22], buffer[23]}, 0);

                    var eventType = (EventType) type;

                    handleEvent(eventType, code, value);
                }

                handle.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Dispose()
        {
            disposing = true;
        }

        ~InputReader()
        {
            Dispose();
            Debug.Log($"Disposing {path}");
            task.Wait(100);
        }

        public void SetGrab(bool grab)
        {
            if (disposing) return;
            if (UnsafeNativeMethods.Ioctl(handle, UnsafeNativeMethods.EVIOCGRAB, grab) == -1)
            {
                Debug.LogException(new UnixIOException());
            }
        }
    }
}