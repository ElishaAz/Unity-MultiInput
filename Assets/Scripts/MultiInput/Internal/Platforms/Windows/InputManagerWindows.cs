using System.Collections.Generic;

namespace MultiInput.Internal.Platforms.Windows
{
    public class InputManagerWindows: IInputManager
    {
        public InputManagerWindows()
        {
            
        }
        
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<IKeyboardInternal> Keyboards { get; }
        public IReadOnlyList<IMouseInternal> Mice { get; }
        public event AnyKeyboardAction OnAnyKeyboardPress;
        public event AnyMouseEvent OnAnyMouseClick;
        public event AnyMouseMovement OnAnyMouseMovement;
    }
}