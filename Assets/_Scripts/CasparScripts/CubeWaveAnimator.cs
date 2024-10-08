using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CubeWaveAnimator : MonoBehaviour
{
    public GameObject cubePrefab; // Cube prefab to instantiate
    public int gridSizeX = 10; // Grid size along X
    public int gridSizeZ = 10; // Grid size along Z
    public float waveSpeed = 1f; // Speed of the wave
    public float waveHeight = 2f; // Height of the wave
    public float waveFrequency = 1f; // Frequency of the wave

    private List<GameObject> cubes = new List<GameObject>();

    void Start()
    {
        CreateGrid();
        AnimateCubes();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cubes.Add(cube);
            }
        }
    }

    void AnimateCubes()
    {
        foreach (GameObject cube in cubes)
        {
            Vector3 initialPosition = cube.transform.position;
            float delay = (initialPosition.x + initialPosition.z) * waveFrequency;

            // Apply a sinusoidal animation with a delay for each cube
            cube.transform.DOMoveY(waveHeight, waveSpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetDelay(delay);
        }
    }
}

