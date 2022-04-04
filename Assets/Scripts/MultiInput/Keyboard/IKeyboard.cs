using UnityEngine;

namespace MultiInput.Keyboard
{
    public interface IKeyboard : IInput
    {
        public delegate void Action(KeyCode code);

        public event Action OnKeyPressed;
        public event Action OnKeyReleased;

        public bool GetKey(KeyCode code);
        public bool GetKeyDown(KeyCode code);
        public bool GetKeyUp(KeyCode code);
    }
}