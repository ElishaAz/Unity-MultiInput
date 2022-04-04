using System;
using MultiInput;
using MultiInput.Keyboard;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class CubeMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10;

        private IKeyboard keyboard;
        private bool initialized = false;

        public void Setup(IKeyboard keyboard)
        {
            this.keyboard = keyboard;
            GetComponent<MeshRenderer>().material.color = Random.ColorHSV();

            // keyboard.Grab = true;

            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;

            if (keyboard.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * (speed * Time.deltaTime));
            }

            if (keyboard.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * (speed * Time.deltaTime));
            }

            if (keyboard.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * (speed * Time.deltaTime));
            }

            if (keyboard.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * (speed * Time.deltaTime));
            }
        }
    }
}