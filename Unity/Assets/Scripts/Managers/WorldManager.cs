using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Threading;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    public float WorldScale => worldScale;
    public Vector2 LocationOffset { get; private set; } = Vector2.zero;
    public Vector2 CurrentGpsLocation { get; private set; } = Vector2.zero;
    public Vector2 Current5gLocation { get; private set; } = Vector2.zero;
    public float Current5gRadius { get; private set; } = 0f;

    [SerializeField] private float worldScale = 0.1f;
    [SerializeField] private float spawnRadius = 50f;
    [SerializeField] private int gemCount = 100;
    [SerializeField] private List<GameObject> gemPrefabs = new List<GameObject>();
    [SerializeField] public UnityEvent<Vector2> OnGpsLocationUpdate = new UnityEvent<Vector2>();
    [SerializeField] public UnityEvent<Vector2> On5gLocationUpdate = new UnityEvent<Vector2>();
    [SerializeField] public UnityEvent<float> On5gRadiusUpdate = new UnityEvent<float>();


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }
    }
    private void Start()
    {
        SpawnGems();
        Input.location.Start();
    }

    public void SetLocationOffset(Vector2 realWorldLocationInMeters)
    { LocationOffset = realWorldLocationInMeters; }

    public void UpdateGpsLocation(Vector2 realWorldLocationInMeters)
    {
        if (LocationOffset == Vector2.zero) { LocationOffset = realWorldLocationInMeters; }
        CurrentGpsLocation = realWorldLocationInMeters.IngameLocation();
        OnGpsLocationUpdate.Invoke(CurrentGpsLocation);
    }

    public void Update5GLocation(Vector2 realWorldLocationInMeters)
    {
        if(LocationOffset == Vector2.zero) { LocationOffset = realWorldLocationInMeters; }
        Current5gLocation = realWorldLocationInMeters.IngameLocation();
        On5gLocationUpdate.Invoke(Current5gLocation);
    }

    public void Update5GRadius(float radiusInMeters)
    {
        Current5gRadius = radiusInMeters * WorldScale;
        On5gRadiusUpdate.Invoke(Current5gRadius);
    }

    public void SpawnGems()
    {
        for (int i = 0; i < gemCount; i++)
        {
            Instantiate(gemPrefabs[Random.Range(0, gemPrefabs.Count)], new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
        }
    }

    public bool New5GLocationData = false;
    public Vector2 NewLocation = Vector2.zero;
    public float NewRadius = 0;

    private readonly object locationLock5g = new();
    public void Queue5gLocation(Vector2 location, float radius)
    {
        lock(locationLock5g)
        {
            NewLocation = location;
            NewRadius = radius;
            New5GLocationData = true;
        }
    }

    private void FixedUpdate()
    {
        lock (locationLock5g)
        {
            if (New5GLocationData)
            {
                Update5GLocation(NewLocation);
                Update5GRadius(NewRadius);
                New5GLocationData = false;
            }
        }
        Debug.Log(Input.location.lastData.longitude);
    }
}

public static class WorldManagerExtensions
{
    public static Vector2 IngameLocation(this Vector2 realWorldLocation)
    {
        return (realWorldLocation - WorldManager.Instance.LocationOffset) * WorldManager.Instance.WorldScale;
    }
}
