using System;
using System.Collections.Concurrent;
using MultiInput.Internal.Platforms.Windows;
using MultiInput.Internal.Platforms.Windows.PInvokeNet;
using UnityEngine;

public class WindowsTest : MonoBehaviour
{
    private MyWMListener listener;
    private bool closed;

    private readonly ConcurrentQueue<RawInput> inputQueue = new ConcurrentQueue<RawInput>();

    private void Awake()
    {
        // listener = new MyWMListener(OnInput);
        Debug.Log("@Awake");
        // wmListener = new MyWMListener();
        // wmListener.Start();
    }

    private bool OnInput(RawInput input)
    {
        inputQueue.Enqueue(input);
        return true;
    }

    private void Update()
    {
        // Listener.Update();
        if (inputQueue.TryDequeue(out var val))
        {
            Debug.Log(val);
        }
    }

    // private void OnDisable()
    // {
    //     wmListener.Dispose();
    // }
}