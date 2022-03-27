using System.Threading;
using MultiInput.Internal;
using UnityEngine;

namespace MultiInput
{
    public class InputManagerMono : MonoBehaviour
    {
        [SerializeField] private bool setDontDestroyOnLoad = true;

        private static bool instanceExists;

        private InputManagerImplPicker implPicker;
        private bool currentIsActiveInstance;

        private void Awake()
        {
            if (!instanceExists)
            {
                if (setDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }

                instanceExists = true;
                currentIsActiveInstance = true;
                InputManagerImplPicker.CreateInstance();
            }
            else
            {
                Debug.LogWarning("Make sure there is no more than one InputManagerMono instance at a given time!");
                Destroy(gameObject);
            }

            implPicker = InputManagerImplPicker.Instance;
        }

        private void OnEnable()
        {
            if (!currentIsActiveInstance) return;

            foreach (var keyboard in implPicker.InputManagerImpl.Keyboards)
            {
                keyboard.StartMainLoop();
            }
        }

        private void Update()
        {
            if (!currentIsActiveInstance) return;

            foreach (var keyboard in implPicker.InputManagerImpl.Keyboards)
            {
                keyboard.MainLoop();
            }
        }

        private void OnDisable()
        {
            if (!currentIsActiveInstance) return;

            foreach (var keyboard in implPicker.InputManagerImpl.Keyboards)
            {
                keyboard.StopMainLoop();
            }
        }

        private void OnDestroy()
        {
            if (!currentIsActiveInstance) return;

            instanceExists = false;
            InputManagerImplPicker.Instance.Dispose();
        }
    }
}