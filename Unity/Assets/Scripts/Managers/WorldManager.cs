using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    public float WorldScale => worldScale;
    public Vector2 LocationOffset { get; private set; } = Vector2.zero;
    public Vector2 CurrentGpsLocation { get; private set; } = Vector2.zero;
    public Vector2 Current5gLocation { get; private set; } = Vector2.zero;

    [SerializeField] private float worldScale = 0.1f;
    [SerializeField] private float spawnRadius = 50f;
    [SerializeField] private int gemCount = 100;
    [SerializeField] private List<GameObject> gemPrefabs = new List<GameObject>();
    [SerializeField] public UnityEvent<Vector2> OnGpsLocationUpdate = new UnityEvent<Vector2>();
    [SerializeField] public UnityEvent<Vector2> On5gLocationUpdate = new UnityEvent<Vector2>();


    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(gameObject); }
    }
    private void Start()
    {
        SpawnGems();
    }

    public void SetLocationOffset(Vector2 realWorldLocationInMeters)
    { LocationOffset = realWorldLocationInMeters; }

    public void UpdateGpsLocation(Vector2 realWorldLocationInMeters)
    {
        CurrentGpsLocation = realWorldLocationInMeters.IngameLocation();
        OnGpsLocationUpdate.Invoke(CurrentGpsLocation);
    }

    public void Update5GLocation(Vector2 realWorldLocationInMeters)
    {
        Current5gLocation = realWorldLocationInMeters.IngameLocation();
        On5gLocationUpdate.Invoke(Current5gLocation);
    }

    public void SpawnGems()
    {
        for (int i = 0; i < gemCount; i++)
        {
            Instantiate(gemPrefabs[Random.Range(0, gemPrefabs.Count)], new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
        }
    }
}

public static class WorldManagerExtensions
{
    public static Vector2 IngameLocation(this Vector2 realWorldLocation)
    {
        return (realWorldLocation - WorldManager.Instance.LocationOffset) * WorldManager.Instance.WorldScale;
    }
}
