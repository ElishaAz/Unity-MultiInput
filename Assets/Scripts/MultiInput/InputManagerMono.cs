using System.Collections;
using System.Threading;
using MultiInput.Internal;
using UnityEngine;

namespace MultiInput
{
    public class InputManagerMono : MonoBehaviour
    {
        [SerializeField] private bool setDontDestroyOnLoad = true;
        [SerializeField] public bool scanForNewDevices = false;
        [SerializeField] public float scanInterval = 1f;

        private static bool instanceExists;

        private InputManagerImplPicker implPicker;
        private bool currentIsActiveInstance;
        private bool scanForNewDevicesRunning = false;
        private float currentScanInterval;

        private WaitForSeconds scanWait;

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
                return;
            }

            implPicker = InputManagerImplPicker.Instance;

            scanWait = new WaitForSeconds(scanInterval);
            currentScanInterval = scanInterval;
        }

        private IEnumerator ScanCoroutine()
        {
            scanForNewDevicesRunning = true;
            while (scanForNewDevicesRunning && scanForNewDevices)
            {
                yield return scanWait;
                implPicker.ScanForNewDevices();
            }

            scanForNewDevicesRunning = false;
        }

        private void OnEnable()
        {
            if (!currentIsActiveInstance) return;

            implPicker.CallStartMainLoop();

            if (scanForNewDevices && !scanForNewDevicesRunning)
                StartCoroutine(ScanCoroutine());
        }

        private void Update()
        {
            if (!currentIsActiveInstance) return;

            implPicker.CallMainLoop();

            // ReSharper disable once CompareOfFloatsByEqualityOperator as they should be exactly the same
            if (scanInterval != currentScanInterval)
            {
                scanWait = new WaitForSeconds(scanInterval);
                scanInterval = currentScanInterval;
            }

            if (!scanForNewDevicesRunning && !scanForNewDevices)
            {
                StartCoroutine(ScanCoroutine());
            }
        }

        private void OnDisable()
        {
            if (!currentIsActiveInstance) return;

            implPicker.CallStopMainLoop();

            scanForNewDevicesRunning = false;
        }

        private void OnDestroy()
        {
            if (!currentIsActiveInstance) return;

            instanceExists = false;
            implPicker.Dispose();
        }
    }
}