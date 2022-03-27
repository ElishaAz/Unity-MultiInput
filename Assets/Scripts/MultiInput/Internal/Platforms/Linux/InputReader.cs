using System;
using System.IO;
using System.Threading.Tasks;

namespace MultiInput.Internal.Platforms.Linux
{
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
        public delegate void RaiseKeyPress(KeyPressEvent e);

        public delegate void RaiseMouseMove(MouseMoveEvent e);

        public event RaiseKeyPress OnKeyPress;
        public event RaiseMouseMove OnMouseMove;

        private const int BufferLength = 24;
    
        private readonly byte[] _buffer = new byte[BufferLength];
    
        private FileStream _stream;
        private bool _disposing;

        public InputReader(string path)
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Task.Run(Run);
        }

        private void Run()
        {
            while (true)
            {
                if (_disposing)
                    break;

                _stream.Read(_buffer, 0, BufferLength);

                var type = BitConverter.ToInt16(new[] {_buffer[16], _buffer[17]}, 0);
                var code = BitConverter.ToInt16(new[] {_buffer[18], _buffer[19]}, 0);
                var value = BitConverter.ToInt32(new[] {_buffer[20], _buffer[21], _buffer[22], _buffer[23]}, 0);

                var eventType = (EventType) type;

                switch (eventType)
                {
                    case EventType.EV_KEY:
                        HandleKeyPressEvent(code, value);
                        break;
                    case EventType.EV_REL:
                        var axis = (MouseAxis) code;
                        var e = new MouseMoveEvent(axis, value);
                        OnMouseMove?.Invoke(e);
                        break;
                }
            }
        }

        private void HandleKeyPressEvent(short code, int value)
        {
            var c = (EventCode) code;
            var s = (KeyState) value;
            var e = new KeyPressEvent(c, s);
            OnKeyPress?.Invoke(e);
        }

        public void Dispose()
        {
            _disposing = true;
            _stream.Dispose();
            _stream = null;
        }
    }
}