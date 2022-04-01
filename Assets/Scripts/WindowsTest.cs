using System;
using MultiInput.Internal.Platforms.Windows;
using UnityEngine;

public class WindowsTest : MonoBehaviour
{
    private static MyWMListener wmListener = new MyWMListener();
    private bool closed;

    private void Awake()
    {
        // wmListener = new MyWMListener();
        // wmListener.Start();
    }

    private void OnDisable()
    {
        wmListener.Dispose();
    }
}