using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DefaultNamespace;
using MultiInput;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CubeMovement cubePref;

    private readonly Dictionary<IKeyboard, CubeMovement> cubes = new Dictionary<IKeyboard, CubeMovement>();
    private readonly Dictionary<IMouse, CubeMovement> spheres = new Dictionary<IMouse, CubeMovement>();
    private readonly ConcurrentQueue<IKeyboard> activeKeyboardsQueue = new ConcurrentQueue<IKeyboard>();

    private void Awake()
    {
        // InputManager.OnAnyKeyboardPress += CheckForNewKeyboards;

        foreach (var keyboard in InputManager.Keyboards)
        {
            SpawnNewCube(keyboard);
        }
    }

    private void CheckForNewKeyboards(KeyCode code, IKeyboard keyboard)
    {
        if (code == KeyCode.Space)
        {
            activeKeyboardsQueue.Enqueue(keyboard);
        }
    }

    private void Update()
    {
        while (activeKeyboardsQueue.TryDequeue(out var value))
        {
            if (!cubes.ContainsKey(value))
                SpawnNewCube(value);
        }
    }

    private void SpawnNewCube(IKeyboard keyboard)
    {
        var cube = Instantiate(cubePref);
        cube.Setup(keyboard);
        cubes.Add(keyboard, cube);
    }
}