using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DefaultNamespace;
using MultiInput;
using MultiInput.Keyboard;
using MultiInput.Mouse;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CubeMovement cubePref;
    [SerializeField] private SphereMovement spherePref;

    private readonly Dictionary<IKeyboard, CubeMovement> cubes = new Dictionary<IKeyboard, CubeMovement>();
    private readonly Dictionary<IMouse, SphereMovement> spheres = new Dictionary<IMouse, SphereMovement>();

    private readonly ConcurrentQueue<IKeyboard> activeKeyboardsQueue = new ConcurrentQueue<IKeyboard>();
    private readonly ConcurrentQueue<IMouse> activeMiceQueue = new ConcurrentQueue<IMouse>();

    private void Awake()
    {
        InputManager.OnAnyKeyboardPress += CheckForNewKeyboards;
        InputManager.OnAnyMouseEvent += CheckForNewMice;
    }

    private void CheckForNewKeyboards(KeyCode code, IKeyboard keyboard)
    {
        if (code == KeyCode.Space)
        {
            activeKeyboardsQueue.Enqueue(keyboard);
        }
    }

    private void CheckForNewMice(MouseEvent e, IMouse mouse)
    {
        if (e == MouseEvent.LeftMouse)
        {
            activeMiceQueue.Enqueue(mouse);
        }
    }

    private void Update()
    {
        while (activeKeyboardsQueue.TryDequeue(out var value))
        {
            if (!cubes.ContainsKey(value))
                SpawnNewCube(value);
        }

        while (activeMiceQueue.TryDequeue(out var value))
        {
            if (!spheres.ContainsKey(value))
                SpawnNewSphere(value);
        }
    }

    private void SpawnNewCube(IKeyboard keyboard)
    {
        var cube = Instantiate(cubePref);
        cube.Setup(keyboard);
        cubes.Add(keyboard, cube);
    }

    private void SpawnNewSphere(IMouse mouse)
    {
        var sphere = Instantiate(spherePref);
        sphere.Setup(mouse);
        spheres.Add(mouse, sphere);
    }
}