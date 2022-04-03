using UnityEngine;

namespace MultiInput.Internal.Platforms.Windows
{
    public class KeyboardWindows: IKeyboardInternal
    {
        public event IKeyboard.Action OnKeyPressed;
        public event IKeyboard.Action OnKeyReleased;
        public bool GetKey(KeyCode code)
        {
            throw new System.NotImplementedException();
        }

        public bool GetKeyDown(KeyCode code)
        {
            throw new System.NotImplementedException();
        }

        public bool GetKeyUp(KeyCode code)
        {
            throw new System.NotImplementedException();
        }

        public bool Grab { get; set; }
    }
}