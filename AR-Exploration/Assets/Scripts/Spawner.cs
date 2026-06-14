using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] GameObject _spawnedObject;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _maxHeight;
    [SerializeField] private int _maxSpawnedObjects;

    private float _cooldown;

    private void Awake()
    {
        _cooldown = 0;
    }

    private void Update()
    {
        _cooldown += Time.deltaTime;

        if (_cooldown > _spawnDelay)
        {
            SpawnObject();
        }
    }
    
    // Spawns an object
    private void SpawnObject()
    {
        _cooldown = 0;

        ARPlane plane = GetRandomPlane();
        Vector3 newPos = GetRandomPlanePosition(plane);

        Instantiate(_spawnedObject, newPos, Quaternion.identity);
    }

    // Gets a random horizontal plane
    private ARPlane GetRandomPlane()
    {
        List<ARPlane>  planes = new List<ARPlane>();

        foreach (ARPlane plane in _planeManager.trackables)
        {
            // Only takes size of horizonal planes
            if (plane.alignment == UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            {
                planes.Add(plane);
            }
        }

        // Gets random plane
        int randomPlane = Random.Range(0, planes.Count);
        return planes[randomPlane];
    }

    // Gets a random position on the plane
    private Vector3 GetRandomPlanePosition(ARPlane plane)
    {
        Vector3 centerPos = plane.center;

        Vector2 half = plane.size / 2;

        float xRandomOffset = Random.Range(-half.x, half.x);
        float yRandomOffset = Random.Range(0, _maxHeight);
        float zRandomOffset = Random.Range(-half.y, half.y);

        return new Vector3(xRandomOffset + centerPos.x, centerPos.y + yRandomOffset, centerPos.z + zRandomOffset);
    }
}
